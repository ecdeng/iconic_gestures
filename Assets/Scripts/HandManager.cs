using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {

	private bool moving = false;
	private float speed = 0.5f;
	private Vector3 originalPos;
	private Quaternion originalRotation;
	private List<Vector3> vertices;
	private int numPoints = 50;
	private GameObject NetworkManager;


	void Push()
	{
		transform.position = originalPos;
		moving = true;
	}

	void ResetState()
	{
		transform.position = originalPos;
		transform.rotation = originalRotation;
	}


	void OnTriggerEnter(Collider collider)
	{
		print ("trigger");
		moving = false; 
	}
		


	// Use this for initialization
	void Start () {
		NetworkManager = GameObject.Find ("NetworkManager");
		originalPos = transform.position;
		originalRotation = transform.rotation;

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey ("space")) {
			transform.position = originalPos;
			moving = true;
		}

	
		Vector3 direction = Vector3.zero - originalPos;

		if (moving) {
			transform.Translate(direction.normalized * speed * Time.deltaTime,Space.World);
			object[] message = new object[2];
			message [0] = new List<Vector3>() {transform.position};
			message [1] = new List<Vector3>() {transform.right};
			NetworkManager.SendMessage ("sendPoints",message);
		}
		
	}

}
