using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjManager : Singleton<ObjManager> {

	public GameObject model;
	private float movespeed = 2.0f;
	private float scale = 0.05f;
	private Dictionary<int,GameObject> point_ids;


	// Use this for initialization
	void Start () {
		var filepath = "Assets/Models/pikachu.obj";
		point_ids = new Dictionary<int, GameObject> ();
		LoadModel (filepath);
	}
		
	public void LoadModel(string filepath) {
		foreach (KeyValuePair<int, GameObject> entry in point_ids) {
			Destroy (entry.Value);
		}
		point_ids.Clear ();
		Destroy (model);
		model = OBJLoader.LoadOBJFile (filepath);
		model.transform.parent = transform;
		model.transform.position += transform.position;
		model.transform.position = new Vector3(0,transform.localScale.y,0);
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

	void UpdatePoints(Quaternion rotation, Vector3 p_scale) {
		foreach(KeyValuePair<int, GameObject> entry in point_ids)
		{
			var pos = entry.Value.transform.position;
			pos = rotation * pos;
			pos = new Vector3(pos.x*p_scale.x,pos.y*p_scale.y,pos.z*p_scale.z);
			point_ids [entry.Key].transform.position = pos;
		}

	}

	// Update is called once per frame
	void Update () {
		var quat = Quaternion.identity;
		var model_scale = Vector3.one;
		var rotate = new Vector3 (0, movespeed, 0);
		if (model != null) {
			if (Input.GetKey ("left")) {
				model.transform.Rotate(rotate);
				quat = Quaternion.Euler (rotate);
			}
			if (Input.GetKey ("right")) {
				model.transform.Rotate(-1*rotate);
				quat = Quaternion.Euler (-1*rotate);
			}
			if (Input.GetKeyDown ("r")) {	
				Vector3 rot = model.transform.rotation.eulerAngles;
				rot = new Vector3(rot.x,rot.y+180,rot.z);
				quat = Quaternion.Euler (rot);
				model.transform.rotation = Quaternion.Euler(rot);
			}

			if (Input.GetKey ("up")) {
				model.transform.localScale *= (1 + scale);
				model_scale *= (1 + scale); 
			}
			if (Input.GetKey ("down")) {
				model.transform.localScale *= (1 - scale);
				model_scale *= (1 - scale); 

			}
			UpdatePoints (quat,model_scale);
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

	

		//updating vertices
		for(int i = 0; i < vertices.Count; i++)
		{
			vertices[i] += new Vector3(0,transform.localScale.y,0);
		}

		return vertices;
	}

	public Dictionary<int,GameObject> GetVerticesWithIDs() {

		return point_ids;
	}
}
