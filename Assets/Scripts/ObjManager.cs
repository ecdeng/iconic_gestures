using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjManager : Singleton<ObjManager> {

	public GameObject model;
	private float movespeed = 1.0f;
	private float scale = 0.05f;
	private Dictionary<int,GameObject> point_ids;

	// Use this for initialization
	void Start () {
		var filepath = "Assets/Models/pikachu.obj";
		point_ids = new Dictionary<int, GameObject> ();
		LoadModel (filepath);


	}

	void LoadModel(string filepath) {
		point_ids.Clear ();
		model = OBJLoader.LoadOBJFile (filepath);
		model.transform.parent = transform;
		model.transform.position = transform.position + new Vector3(0,transform.localScale.y,0);
		CreatePoints ();
	}

	void CreatePoints() {
		var points = GetVertices ();

		int id = 0;
		foreach (var vertex in points) {
			var gameObj = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			gameObj.transform.position = vertex;
			gameObj.transform.localScale = Vector3.one * scale;
			point_ids.Add (id++, gameObj);
		}
	}

	void UpdatePoints(Quaternion rot, Vector3 scale) {
		/*foreach (var obj in point_ids.Values) {
			obj.transform.position = rot * obj.transform.position;
			var pos = obj.transform.position;
			obj.transform.position = new Vector3(pos.x*scale.x,pos.y*scale.y,pos*scale.z);
		}*/
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
			//UpdatePoints (model.transform.localRotation,model.transform.localScale);
		}
	}

	/// <summary>
	/// Gets the vertices on the mesh
	/// </summary>
	/// <returns>The vertices.</returns>
	protected List<Vector3> GetVertices() {
		//grab vertices
		List<Vector3> vertices = new List<Vector3>();

		var meshFilter = model.GetComponent<MeshFilter> ();
		if (meshFilter == null) {
			var filters = model.GetComponentsInChildren<MeshFilter> ();
			foreach (var filter in filters) {
				vertices.AddRange (filter.mesh.vertices);
			}
		}
		else
			vertices = new List<Vector3>(meshFilter.mesh.vertices);


		//grab tranform
		/*var angle = model.transform.rotation;
		var scale = model.transform.localScale;*/
		var position = model.transform.position;

		//updating vertices
		for(int i = 0; i < vertices.Count; i++)
		{
			//vertices[i] = angle * vertices[i];
			//vertices[i] = new Vector3(vertices[i].x*scale.x,vertices[i].y*scale.y,vertices[i].z*scale.z);
			//vertices[i] += position;
			vertices[i] += new Vector3(0,transform.localScale.y,0);
			vertices [i] += position;
		}

		return vertices;
	}
}
