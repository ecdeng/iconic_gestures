using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {

	private bool moving = false;
	private float speed = 0.5f;

	void OnCollisionEnter(Collision collision)
	{
		print ("collide");
		moving = false; 
	}

	void OnTriggerEnter(Collider collider)
	{
		print ("trigger");
		moving = false; 
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey ("space")) {
			moving = true;
		}

		Vector3 direction = Vector3.zero - transform.position;

		if (moving) {
			transform.Translate(direction * speed * Time.deltaTime);
		}
		
	}
}
