using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjManager : Singleton<ObjManager> {

	public GameObject parent;
	public GameObject model;
	private float movespeed = 2.0f;
	private float scale = 0.05f;
	private Dictionary<int,GameObject> point_ids;

	Vector2 scrollPosition = Vector2.zero;


	// Use this for initialization
	void Start () {
		parent = new GameObject ("parent");
		parent.transform.parent = transform;
		var filepath = "Assets/Models/flask.obj";
		point_ids = new Dictionary<int, GameObject> ();
		LoadModel (filepath);


	}

	public float GetMinVertex(GameObject obj) {
		var min = obj.transform.position.y;
		var renderers = obj.GetComponentsInChildren<Renderer> ();
		foreach (var renderer in renderers) {
			min = Mathf.Min (min, renderer.bounds.min.y);
		}
		return min;
	}
		
	public void LoadModel(string filepath) {
		foreach (KeyValuePair<int, GameObject> entry in point_ids) {
			Destroy (entry.Value);
		}
		point_ids.Clear ();
		Destroy (model);
		model = OBJLoader.LoadOBJFile (filepath);

		var min = GetMinVertex (model);
		var offset = model.transform.position.y - min;
		var vec = new Vector3(0,offset,0);
		model.transform.position += vec;
	
		model.transform.parent = parent.transform;
		CreatePoints (vec);
	}

	void CreatePoints(Vector3 offset) {
		var points = GetVertices ();

		int id = 0;
		foreach (var vertex in points) {
			var gameObj = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			gameObj.transform.position = vertex;
			gameObj.transform.position += offset;
			gameObj.transform.localScale = Vector3.one * scale * 5;
			point_ids.Add (id++, gameObj);
		}
	}

	public void Select(GameObject sphere) {
		Renderer renderer = sphere.GetComponent<Renderer>();
		renderer.material.color = Color.green;
	}

	public void Highlight(GameObject sphere) {
		Renderer renderer = sphere.GetComponent<Renderer>();
		renderer.material.color = Color.red;
	}

	public void Unhighlight(GameObject sphere) {
		Renderer renderer = sphere.GetComponent<Renderer>();
		renderer.material.color = Color.white;
	}
		
	// returns gameobject for the sphere given id
	public GameObject GetGameObject(int id) {
		return point_ids [id];
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
				parent.transform.Rotate(rotate);
				quat = Quaternion.Euler (rotate);
			}
			if (Input.GetKey ("right")) {
				parent.transform.Rotate(-1*rotate);
				quat = Quaternion.Euler (-1*rotate);
			}
			if (Input.GetKeyDown ("r")) {	
				var rot = new Vector3(0,180,0);
				parent.transform.Rotate(rot);
				quat = Quaternion.Euler (rot);
			}

			if (Input.GetKey ("up")) {
				parent.transform.localScale *= (1 + scale);
				model_scale *= (1 + scale); 
			}
			if (Input.GetKey ("down")) {
				parent.transform.localScale *= (1 - scale);
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
