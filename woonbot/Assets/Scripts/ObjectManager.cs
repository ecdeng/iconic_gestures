using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour {

	private GameObject obj;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {

		var up = true;
		var down = true;
		var right = true;
		var left = true;



		if (Input.GetKey ("up") && up) {
			Destroy (obj);
			up = false;
			obj = (GameObject)Instantiate(Resources.Load("basketball"));
			obj.transform.position = this.transform.position;
		}

		if (Input.GetKey ("right") && right) {
			Destroy (obj);
			right = false;
			obj = (GameObject)Instantiate(Resources.Load("sword"));
			obj.transform.position = this.transform.position;
		}
			

		if (Input.GetKey("down") && down){
			Destroy (obj);
			down = false;
			obj = (GameObject)Instantiate(Resources.Load("bowl"));
			obj.transform.position = this.transform.position;
		}

		if (Input.GetKey("left") && left){
			Destroy (obj);
			left = false;
			obj = (GameObject)Instantiate(Resources.Load("pencil"));
			obj.transform.position = this.transform.position;
		}
	}
}
