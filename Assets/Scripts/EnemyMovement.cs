using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//巡逻
public class EnemyMovement : MonoBehaviour {
	Transform player;//获取角色Transform组件

	UnityEngine.AI.NavMeshAgent nav;//巡逻组件

	PlayerHealth playerHealth;
	EnemyHealth enemyHealth;
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();//获取组件

		playerHealth = player.GetComponent<PlayerHealth>();
		enemyHealth = GetComponent <EnemyHealth> ();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0) {
			nav.SetDestination (player.position);//始终靠近目标位置
		} else {
			nav.enabled = false;
		}
	}
}
