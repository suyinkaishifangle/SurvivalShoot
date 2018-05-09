using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour {
	public float speed = 5;
	private Animator _animator;
	
    // Use this for initialization  
    void Start()  
    {
        _animator = this.GetComponent<Animator>();
		
    }  
	
	
	// Update is called once per frame
	void Update () {
		float v = Input.GetAxis("Vertical");  
  
        transform.Translate(new Vector3(0, 0, v) * speed * Time.deltaTime); 
		 if(Input.GetKeyDown(KeyCode.W))
         {
			
             _animator.SetBool("IsWalking", true);
			 
         }
        if(Input.GetKeyUp(KeyCode.W))
         {
             _animator.SetBool("IsWalking", false);
         }
         
    }
}
	

