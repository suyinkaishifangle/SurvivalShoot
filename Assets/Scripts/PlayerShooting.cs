using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;



    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;//存储碰撞体
    int shootableMask;//层的掩码

    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunParticles = GetComponent<ParticleSystem>();//获取线性渲染的掩码
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();

    }
    void Start () {
		
	}


    void Update()
    {
        timer += Time.deltaTime;//两贞之间的时间间隔

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot();

        }
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();  //关闭射击特效
        }
    }


    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;

    }

    void Shoot()  //射击逻辑、特效
    {
        timer = 0f; //每次射击之前清零，正式开始计算此次时间
        gunAudio.Play(); //射击枪声播放
        gunLight.enabled = true;  //射击灯光

        gunParticles.Stop();  //粒子效果暂停
        gunParticles.Play();  //粒子效果开始

        gunLine.enabled = true;  //线性渲染打开，产生射线，表示子弹轨迹
        gunLine.SetPosition(0, transform.position);  //参数0代表直线第一个顶点，position就是主角当前位置

        shootRay.origin = transform.position;  //为了定义第二个顶点的设置
          //shootray是光线类型，这是定义光线的起点
        shootRay.direction = transform.forward;  //光线的方向

        if(Physics.Raycast(shootRay,out shootHit, range, shootableMask)) 
         //raycast射线检测    光线       碰撞体信息    光线长度    过滤层
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth!=null)
            {
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            }
            gunLine.SetPosition(1, shootHit.point);  //1代表第二个顶点，参数2代表与敌人碰撞的顶点
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }  //1代表第二个顶点

    }
}
