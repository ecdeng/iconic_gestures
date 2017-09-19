using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour {

	private GameObject obj;

	[SerializeField]
	public GameObject hand1;

	[SerializeField]
	public GameObject hand2;

	private bool up = true;
	private bool down = true;
	private bool right = true;
	private bool left = true;
	private bool space = true;

	private bool moving = true;
	private float speed = 0.5f;

	// Use this for initialization
	void Start () {


	}

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


	
	// Update is called once per frame
	void Update () {








		/*if (Input.GetKey("space") && space){
			space = false;
			print ("attempting to collapse");
			Ray ray; // the ray that will be shot
			RaycastHit hit; // variable to hold the object that is hit


			if (Physics.Raycast (hand2.transform.position, transform.position - hand2.transform.position, out hit, 10)) {
				if (hit.collider.name != "Hand1") {
					print ("There is something in front of the object!");
					print (hit.point);
					hand2.transform.position = hit.point;
				
				}
			}


			if (Physics.Raycast (hand1.transform.position, transform.position - hand1.transform.position, out hit, 10)) {
				if (hit.collider.name != "Hand2") {
					print ("There is something in front of the object!");
					print (hit.point);
					hand1.transform.position = hit.point;
				}
			}
				
		}*/
			
		if (Input.GetKey ("up")) {
			Destroy (obj);
			up = false;
			obj = (GameObject)Instantiate(Resources.Load("basketball"));
			obj.transform.position = this.transform.position;
		}

		if (Input.GetKey ("right")) {
			Destroy (obj);
			right = false;
			obj = (GameObject)Instantiate(Resources.Load("sword"));
			obj.transform.position = this.transform.position;
		}
			

		if (Input.GetKey("down")){
			Destroy (obj);
			down = false;
			obj = (GameObject)Instantiate(Resources.Load("bowl"));
			obj.transform.position = this.transform.position;
		}

		if (Input.GetKey("left")){
			Destroy (obj);
			left = false;
			obj = (GameObject)Instantiate(Resources.Load("pencil"));
			obj.transform.position = this.transform.position;
		}
	}
}
