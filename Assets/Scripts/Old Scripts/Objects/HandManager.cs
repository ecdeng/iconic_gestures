using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {

	//hand specific info
	private bool moving = false;
	private float speed = 5f;

	//original postion
	private Vector3 originalPos;
	private Quaternion originalRotation;

	//gesture points
	private List<Vector3> vertices;
	private Vector3 hand1_pos;
	private Vector3 hand2_pos;
	private Vector3 hand1_norm;
	private Vector3 hand2_norm;
	private Vector3 hand_pos;
	private Vector3 hand_norm;

	//network
	private GameObject NetworkManager;
	private List<Vector3> transformPointsToSend;
	private List<Vector3> normalPointsToSend;

	//name of hand
	private bool isLeftHand;


	/// <summary>
	/// Resets the states
	/// </summary>
	void ResetState() {
		transform.position = originalPos;
		transform.rotation = originalRotation;
		moving = false;
	}


	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerEnter(Collider c) {
		/*object[] message = new object[2];
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
		isLeftHand = name == "Hand1";

	}
		
	// Update is called once per frame
	void Update () {

		if (Input.GetKey ("space")) {
			ResetState ();
		}
		
	}

	/// <summary>
	/// Get Normals of surface
	/// </summary>
	protected void CalculateRoundNormals() {

		//get model
		var model = ObjectManager.Instance.currObject;

		//get horizontal norm
		var norm1 = (hand1_pos - hand2_pos).normalized;

		//inverse it for right hand
		if (!isLeftHand)
			norm1 *= -1;

		//get norm of point to center
		var norm2 = (hand_pos).normalized;
		var norm = norm1.magnitude > 0 && model.name.Contains("Flask") ? norm1 : norm2;
		var outside_point = hand_pos + norm;

		//perform ray cast to object
		Ray ray = new Ray (outside_point, hand_pos - outside_point);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100, 1<< 1)) {

			//store normals and hand_pos
			Debug.DrawRay(hit.point,hit.normal,Color.green,100);
			hand_norm = hit.normal;
			hand_pos = hand_pos + hand_norm * 0.03f;
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
			hand2_pos = vertices [(i + 4) % vertices.Count];
			hand_pos = isLeftHand ? hand1_pos : hand2_pos;
			CalculateRoundNormals ();
			moving = true;

			yield return new WaitForSeconds(1f);
		}
		ResetState ();
	}

	/// <summary>
	/// Updates position of hands
	/// </summary>
	void FixedUpdate() {

		if (vertices != null && moving) {
			transform.position = Vector3.Lerp (transform.position, hand_pos,  speed*Time.deltaTime);
			var actual_norm = isLeftHand ? hand_norm : -1 * hand_norm; 
			var quat = Quaternion.LookRotation (transform.forward, actual_norm);
			transform.rotation = Quaternion.Lerp (transform.rotation, quat, speed * Time.deltaTime);
			//transform.up = Vector3.Lerp (transform.up, hand_norm,  speed*Time.deltaTime);	
		}
	}


}
