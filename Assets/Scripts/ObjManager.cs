using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjManager : Singleton<ObjManager> {

	public GameObject model;
	private float movespeed = 1.0f;
	private float scale = 0.05f;

	// Use this for initialization
	void Start () {
		var filepath = "Assets/Models/pikachu.obj";
		LoadModel (filepath);
	}

	public void LoadModel(string filepath) {
		model = OBJLoader.LoadOBJFile (filepath);
		model.transform.parent = transform;
		model.transform.position = transform.position + new Vector3(0,transform.localScale.y,0);
	}

	// Update is called once per frame
	void Update () {
		if (model != null) {
			if (Input.GetKey ("left")) {
				model.transform.Rotate(0, movespeed, 0);
			}
			if (Input.GetKey ("right")) {
				model.transform.Rotate(0, -1*movespeed, 0);
			}
			if (Input.GetKeyDown ("r")) {	
				Vector3 rot = model.transform.rotation.eulerAngles;
				rot = new Vector3(rot.x,rot.y+180,rot.z);
				model.transform.rotation = Quaternion.Euler(rot);
			}

			if (Input.GetKey ("up")) {
				model.transform.localScale *= (1 + scale);
			}
			if (Input.GetKey ("down")) {
				model.transform.localScale *= (1 - scale);

			}
		}
	}
}
