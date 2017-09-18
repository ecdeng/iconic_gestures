using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour {

	[SerializeField]
	public GameObject hand1;

	[SerializeField]
	public GameObject hand2;


	// Use this for initialization
	void Start () {
		print ("HI");
		Ray ray; // the ray that will be shot
		RaycastHit hit; // variable to hold the object that is hit

		var fwd = hand2.transform.forward;

		print (hand2.gameObject.GetComponent<Renderer> ().bounds.center);
		print (fwd);


		if (Physics.Raycast (hand2.transform.position, transform.position - hand2.transform.position, out hit, 10)) {
			print("There is something in front of the object!");
			print (hit.point);
		}
		/*var cam = Camera.main.transform;

		if (Physics.Raycast (cam.position, cam.forward, 500))
			print ("HIIIII");*/

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}




}
