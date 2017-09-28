using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour {

	private GameObject gameObject;
	Vector3[] vertices;
	private bool togglepoints;
	private List<GameObject> points;


	void UpdateMesh(GameObject obj) {
		Mesh mesh = null;
		var filter = obj.GetComponent<MeshFilter>();
		if (filter == null) {
			mesh = obj.GetComponentInChildren<MeshFilter> ().mesh;
		}
		else {
			mesh = obj.GetComponent<MeshFilter> ().mesh;
		}
		var vertices = mesh.vertices;
		var qangle = obj.transform.rotation;

		//updating qangle
		for(int i = 0; i < vertices.Length; i++)
		{
			vertices[i] = qangle * vertices[i];
		}

		//THIS IS FOR YOU ERIC
		for(int i = 0; i < vertices.Length; i++)
		{
			vertices[i] = obj.transform.localScale.x * vertices[i];
			//vertices [i] = 0.1f * vertices[i];
		}
			



		mesh.vertices = vertices;

	}




	// Use this for initialization
	void Start () {
		points = new List<GameObject> ();



	}

	public void ChangeObject(string name) {
		Destroy (gameObject);

		if (name=="sphere") {
			gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.transform.localScale = 0.1f * Vector3.one;
		}
		else {
			gameObject = (GameObject)Instantiate(Resources.Load(name + "/" + name));
		}
		gameObject.transform.position = this.transform.position;
		//gameObject.transform.localScale *= 3;

		UpdateMesh (gameObject);

		Mesh mesh = gameObject.GetComponentInChildren<MeshFilter> ().mesh;
		vertices = mesh.vertices;

		togglepoints = false;

		foreach (var point in points) {
			Destroy (point);
		}
			points.Clear ();

		foreach (var vertex in vertices) {
//			var v = vertex * 0.1f;
			var gameObj = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			gameObj.transform.localScale = 0.01f * Vector3.one;
			gameObj.transform.position = vertex;
			gameObj.SetActive (true);
			points.Add (gameObj);
		}

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

		if (Input.GetKey("p")) {

			//0 means no points 

			togglepoints = !togglepoints;
			foreach (var point in points) {
				point.SetActive(togglepoints);
			}

			//1 means create


			//2 means alive but dont create
		}
	}
}
