using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityEngine
{
public class PlayerMovement : MonoBehaviour {
	public float speed = 6f;
	Vector3 movement;
	Animator anim;
	Rigidbody rb;
	int floorMask;//Floor层对应的掩码
	float camRayLength = 100f;//射线长度
		
	void Awake()
	{
		//获取Floor层的掩码
		floorMask = LayerMask.GetMask("Floor");
		//获取动画控制器
		anim = GetComponent<Animator>();
		//获取刚体组件
		rb = GetComponent<Rigidbody>();
	}
	void FixedUpdate() {
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		Move(h,v);//角色运动
		Turning();//实现角色旋转
		Animating(h,v);

	}
	void Move(float h,float v){
		movement.Set(h,0f,v);//更新x v分量
		movement = movement.normalized * speed * Time.deltaTime;
		//movement.normalized归一化向量：与原来向量方向一致长度为一
		rb.MovePosition(transform.position + movement);
	}
	void Turning(){
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);//发出一条射线，从镜头到鼠标的位置
		RaycastHit floorHit;
		if(Physics.Raycast(camRay,out floorHit,camRayLength,floorMask)){//判断射线是否与floorMask层碰撞
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;
			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);//Quaternion旋转
			rb.MoveRotation (newRotation);
		}
	}
	void Animating(float h,float v){
		bool walking = h != 0f||v != 0f;
		anim.SetBool("IsWalking",walking);

	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
}