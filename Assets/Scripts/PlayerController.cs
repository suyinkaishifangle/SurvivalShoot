using UnityEngine;
using System.Collections;

public delegate void MyJumpDelegate ();
public class PlayerController : MonoBehaviour 
{
	public Rigidbody target;//刚体
	public float speed = 1.0f;
	public float walkSpeedDownscale = 2.0f;
	public float turnSpeed = 2.0f;//键盘操作时的旋转速度
	public float mouseTurnSpeed = 0.3f;//鼠标操作旋转速度
	public float jumpSpeed = 1.0f;//跳跃速度
    //参与射线检测的层码
	public LayerMask groundLayers = -1;
    //检测主角是否着地时的偏移量
	public float groundedCheckOffset = 0.7f;
	public bool showGizmos = true;
	public bool requireLock = true;
	public bool controlLock = false;
	public MyJumpDelegate onJump = null;
	private const float inputThreshold = 0.01f;
    //着地时的阻力系数
	private const float groundDrag = 5.0f;
	private const float directionalJumpFactor = 0.7f;
	private const float groundedDistance = 0.5f;
	private bool grounded;//是否着地
	private bool walking;
	void Start () 
	{
        //初始为空，则获取刚体组件
		if (target == null)
			target = GetComponent<Rigidbody> ();
        //若target仍未空，输出提示
		if (target == null)
		{
			Debug.LogError ("变量target未赋值");
			enabled = false;
			return;
		}
        //冻结旋转,角色受外力作用时无法旋转
		target.freezeRotation = true;
		walking = false;
	}
	
	void Update ()
	{
		float rotationAmount;
if (Input.GetMouseButton (1) && 
    (!requireLock || controlLock || Cursor.lockState == CursorLockMode.Locked))
		{
			if (controlLock)
	Cursor.lockState = CursorLockMode.Locked;
	rotationAmount = Input.GetAxis ("Mouse X") * mouseTurnSpeed * Time.deltaTime;
		}
		else
		{
			if (controlLock)
		Cursor.lockState = CursorLockMode.None;
		rotationAmount = Input.GetAxis ("Horizontal") *  turnSpeed * Time.deltaTime;
		}
		target.transform.RotateAround(target.transform.up, rotationAmount);
		if (Input.GetButtonDown ("ToggleWalk"))
			walking = !walking;
	}
    //侧移量的计算获取
	float SidestepAxisInput
	{
		get
		{
			if (Input.GetMouseButton (1))
			{
		float sidestep = Input.GetAxis ("Sidestep"), horizontal = Input.GetAxis ("Horizontal");
	return Mathf.Abs (sidestep) > Mathf.Abs (horizontal)
        ? sidestep : horizontal;
			}
			else
				return Input.GetAxis ("Sidestep");
		}
	}

	public bool Grounded
	{
		get { return grounded; }//get set用法
	}
	void FixedUpdate ()
	{
        //射线检测是否着地与groundLayers层碰撞
	grounded = Physics.Raycast (target.transform.position + target.transform.up * -groundedCheckOffset,target.transform.up * -1,groundedDistance,groundLayers);
		if (grounded)
		{
		target.drag = groundDrag;//阻力系数
		if (Input.GetButton ("Jump"))
			{
			target.AddForce (jumpSpeed * target.transform.up + target.velocity.normalized * directionalJumpFactor,ForceMode.VelocityChange);
				if (onJump != null)
					onJump ();
			}
			else
			{
	Vector3 movement = Input.GetAxis ("Vertical") * 
        target.transform.forward +
	SidestepAxisInput * target.transform.right;
	float appliedSpeed = walking ? speed / walkSpeedDownscale : speed;
				if (Input.GetAxis ("Vertical") < 0.0f)
					appliedSpeed /= walkSpeedDownscale;
				if (movement.magnitude > inputThreshold)
					target.AddForce (movement.normalized * appliedSpeed, ForceMode.VelocityChange);
				else
				{
					target.velocity = new Vector3 (0.0f, target.velocity.y, 0.0f);
					return;
				}
			}
		}
		else
			target.drag = 0.0f;
	}

	void OnDrawGizmos ()
	{
		if (!showGizmos || target == null)
			return;
		Gizmos.color = grounded ? Color.blue : Color.red;
		Vector3 p  = target.transform.position;
		Vector3 a = p + target.transform.up * -groundedCheckOffset;
		Gizmos.DrawLine(a, a + target.transform.up * -groundedDistance);
	}
}
