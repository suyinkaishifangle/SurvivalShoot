using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {
	public Transform target;//跟随的角色
	public float smoothing = 5f;
	Vector3 offset;
	// Use this for initialization
	void Start () {
		offset = transform.position - target.position;
	}
	
	void FixedUpdate()
	{
		Vector3 camPos = target.position + offset;
		transform.position = Vector3.Lerp(transform.position,camPos,smoothing*Time.deltaTime);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
