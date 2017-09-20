using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {

	private bool moving = false;
	private float speed = 0.5f;
	private Vector3 originalPos;



	void OnTriggerEnter(Collider collider)
	{
		print ("trigger");
		moving = false; 
	}


	// Use this for initialization
	void Start () {
		originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey ("space")) {
			transform.position = originalPos;
			moving = true;
		}

		Vector3 direction = Vector3.zero - transform.position;

		if (moving) {
			transform.Translate(direction * speed * Time.deltaTime);
		}
		
	}
}
