using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour {

	private GameObject gameObject;



	// Use this for initialization
	void Start () {

	}

	void ChangeObject(string name) {
		Destroy (gameObject);
		gameObject = (GameObject)Instantiate(Resources.Load(name));
		gameObject.transform.position = this.transform.position;
	}

	void ApplyPhysics() {
		
	}
		
	
	// Update is called once per frame
	void Update () {
			
		if (Input.GetKey ("up")) {
			ChangeObject ("basketball");
		}

		if (Input.GetKey ("right")) {
			ChangeObject ("sword");
		}
			
		if (Input.GetKey("down")){
			ChangeObject ("bowl");
		}

		if (Input.GetKey("left")){
			ChangeObject ("pencil");
		}
	}
}
