using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {

	private bool moving = false;
	private float speed = 0.5f;
	private Vector3 originalPos;
	private Quaternion originalRotation;
	private List<Vector3> vertices;
	private GameObject NetworkManager;
	private List<Vector3> transformPointsToSend;
	private List<Vector3> normalPointsToSend;


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
		object[] message = new object[2];
		message [0] = transformPointsToSend;
		message [1] = normalPointsToSend;
		NetworkManager.SendMessage ("sendPoints",message);
		transformPointsToSend.Clear ();
		normalPointsToSend.Clear ();
	}
		


	// Use this for initialization
	void Start () {
		NetworkManager = GameObject.Find ("NetworkManager");
		originalPos = transform.position;
		originalRotation = transform.rotation;
		transformPointsToSend = new List<Vector3> ();
		normalPointsToSend = new List<Vector3> ();

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
			transformPointsToSend.Add (transform.position);
			normalPointsToSend.Add (transform.right);
		}
		
	}

}
