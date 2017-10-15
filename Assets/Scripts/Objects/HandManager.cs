using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {

	//hand specific info
	private bool moving = false;
	private float speed = 10f;

	//original postion
	private Vector3 originalPos;
	private Quaternion originalRotation;

	//gesture points
	private List<Vector3> vertices;
	private Vector3 hand1_pos;
	private Vector3 hand2_pos;

	//network
	private GameObject NetworkManager;
	private List<Vector3> transformPointsToSend;
	private List<Vector3> normalPointsToSend;


	//name of hand
	private bool isHand1;


	/// <summary>
	/// Resets the states
	/// </summary>
	void ResetState() {
		hand1_pos = originalPos;
		hand2_pos = originalPos;
		transform.rotation = originalRotation;
	}


	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerEnter(Collider collider) {
		/*print ("trigger");
		moving = false; 
		object[] message = new object[2];
		message [0] = transformPointsToSend;
		message [1] = normalPointsToSend;
		NetworkManager.SendMessage ("sendPoints",message);
		transformPointsToSend.Clear ();
		normalPointsToSend.Clear ();*/
	}

	/// <summary>
	/// Moves until a collision.
	/// </summary>
	void MoveUntilCollision() {
		Vector3 direction = Vector3.zero - originalPos;

		if (moving) {
			transform.Translate(direction.normalized * speed * Time.deltaTime,Space.World);
			transformPointsToSend.Add (transform.position);
			normalPointsToSend.Add (transform.right);
		}
	}

	/// <summary>
	/// Starts the moving of hands.
	/// </summary>
	public void StartMoving(List<Vector3> v) {
		vertices = v;
		StartCoroutine (Move());
	}
		


	// Use this for initialization
	void Start () {
		NetworkManager = GameObject.Find ("NetworkManager");
		originalPos = transform.position;
		originalRotation = transform.rotation;
		transformPointsToSend = new List<Vector3> ();
		normalPointsToSend = new List<Vector3> ();
		isHand1 = name == "Hand1";

	}
		
	// Update is called once per frame
	void Update () {

		if (Input.GetKey ("space")) {
			ResetState ();
		}
		
	}

	/// <summary>
	/// Sets ups the hand position .
	/// </summary>
	protected IEnumerator Move()
	{
		for(int i = 0; i < vertices.Count/2; i ++)
		{

			hand1_pos = vertices [i];
			hand2_pos = vertices [(i + vertices.Count / 2) % vertices.Count];

			yield return new WaitForSeconds(1f);
		}
	}

	/// <summary>
	/// Updates the position of hands.
	/// </summary>
	protected void UpdateHandPosition()
	{

		if (vertices != null) {

			var normal = (hand1_pos - hand2_pos).normalized;
			if (isHand1)
				MoveHand (hand1_pos, normal);
			else
				MoveHand (hand2_pos, normal);
		}
	}

	/// <summary>
	/// Moves the hand.
	/// </summary>
	/// <param name="hand">Hand.</param>
	/// <param name="position">Position.</param>
	/// <param name="normal">Normal.</param>
	private void MoveHand(Vector3 position, Vector3 normal) {

		transform.position = Vector3.Lerp (transform.position, position,  speed*Time.deltaTime);
		transform.up = Vector3.Lerp (transform.up, normal,  speed*Time.deltaTime);	
	}

	/// <summary>
	/// Calls update hand position every frame
	/// </summary>
	void FixedUpdate() {

		UpdateHandPosition ();
	}


}
