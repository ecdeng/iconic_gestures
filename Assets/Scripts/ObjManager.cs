using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PositionNormals
{
	public Vector3 pos;
	public Vector3 norm;
	public PositionNormals(Vector3 pos, Vector3 norm){
		this.pos = pos;
		this.norm = norm;
	}
}

public class ObjManager : Singleton<ObjManager> {

	public GameObject parent;
	public GameObject model;
	private float movespeed = 2.0f;
	private float camspeed = 0.5f;
	private float scale = 0.05f;
	private Dictionary<int,GameObject> point_ids;
	private Dictionary<int,PositionNormals> point_normals;
	public int counter = 0;
	private GameObject counterText;
	private HashSet<float> y_set;
	private bool followMode = false;
	public bool isInSelectionMode; // in selection mode is first stage
	private List<int> selected_point_ids;

	Vector2 scrollPosition = Vector2.zero;


	// Use this for initialization
	void Start () {
		parent = new GameObject ("parent");
		parent.transform.parent = transform;
		var filepath = "Assets/Models/pikachu.obj";
		point_ids = new Dictionary<int, GameObject> ();
		point_normals = new Dictionary<int, PositionNormals> ();
		y_set = new HashSet<float> ();
		LoadModel (filepath);
		counter = 0;
		counterText = GameObject.Find ("Counter");
		isInSelectionMode = true;
		selected_point_ids = new List<int> ();

	}

	public void setSelectedPoints(List<int>selected)
	{
		selected_point_ids = selected;
	}

	public void setInSelectionMode(bool inSelectionMode)
	{
		isInSelectionMode = inSelectionMode;
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
		point_normals.Clear ();
		y_set.Clear ();
		Destroy (model);
		model = OBJLoader.LoadOBJFile (filepath);
		//model = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		var min = GetMinVertex (model);
		var offset = model.transform.position.y - min;
		var vec = new Vector3(0,offset,0);
		model.transform.position += vec;
	
		model.transform.parent = parent.transform;
		CreatePoints (vec);
	}

	void CreatePoints(Vector3 offset) {
		var points = GetVertices ();

		points = SortPoints (points);

		int id = 0;
		foreach (var vertex in points) {
			var pos = vertex.pos;
			var gameObj = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			gameObj.transform.position = pos;
			gameObj.transform.position += offset;
			gameObj.transform.localScale = Vector3.one * scale * 5;
			point_ids.Add (id, gameObj);
			point_normals.Add (id, vertex);
			id++;
		}
	}

	Vector2 CartesianToPolar(Vector3 point )
	{
		Vector2 polar;

		//calc longitude angle
		polar.y = Mathf.Atan2(point.x,point.z);

		//this is easier to write and read than sqrt(pow(x,2), pow(y,2))!
		var xzLen = new Vector2(point.x,point.z).magnitude; 
		//atan2 does the magic
		polar.x = Mathf.Atan2(-point.y,xzLen);

		//convert to deg
		polar *= Mathf.Rad2Deg;

		return polar;
	}

	List<PositionNormals> SortPoints(List<PositionNormals> points) {

		//var new_vertices = vertices.Where ((x, i) => i % 1 == 0).ToList ();

		y_set = new HashSet<float> (y_set.Where ((x, i) => i % 2 == 0)); 
		//points = points.Where ((x, i) => i % 1 == 0).ToList();
		points = points.Where ((x, i) => y_set.Contains (x.pos.y)).ToList();

		//points = points.OrderBy (obj => obj.pos.y).ThenBy(obj => obj.pos.x).ThenBy(obj => obj.pos.z).ToList();

		points = points.OrderBy(obj => AngleToCamera(obj.pos)).ThenBy(obj => obj.pos.y).ToList();
		return points;
	}

	public void Select(GameObject sphere) {
		Renderer renderer = sphere.GetComponent<Renderer>();
		renderer.material.color = Color.green;
		counter++;
		counterText.GetComponent<Text> ().text = counter.ToString ();
	}

	public void Highlight(GameObject sphere) {
		Renderer renderer = sphere.GetComponent<Renderer>();
		renderer.material.color = Color.red;

	}

	float AngleToCamera(Vector3 pos) {
		var vec1 = new Vector2(pos.x,pos.z);
		var vec2 = new Vector2 (Camera.main.transform.position.x, Camera.main.transform.position.z);
		return -1 * AngleBetweenVector2 (vec1, vec2);
	}

	float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
	{
		Vector2 vec1Rotated90 = new Vector2(-vec1.y, vec1.x);
		float sign = (Vector2.Dot(vec1Rotated90, vec2) < 0) ? -1.0f : 1.0f;
		return Vector2.Angle(vec1, vec2) * sign;
	}


	public void FollowCamera(GameObject sphere) {

		if (!followMode)
			return;
		var angle = AngleToCamera(sphere.transform.position);
		UpdatePoints(Quaternion.Euler(new Vector3(0,angle,0)),Vector3.one);
		Debug.Log (angle);
	}

	public void Unhighlight(GameObject sphere, bool unSelect) {
		if (unSelect) {
			counter--;
			counterText.GetComponent<Text> ().text = counter.ToString ();
		}
		Renderer renderer = sphere.GetComponent<Renderer>();
		renderer.material.color = Color.white;
	}
		
	// returns gameobject for the sphere given id
	public GameObject GetGameObject(int id) {
		return point_ids [id];
	}

	void UpdatePoints(Quaternion rotation, Vector3 p_scale) {
		parent.transform.Rotate(rotation.eulerAngles);

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
		var cam = Camera.main;
		var quat = Quaternion.identity;
		var model_scale = Vector3.one;
		var rotate = new Vector3 (0, movespeed, 0);
		if (model != null) {
			if (Input.GetKey ("left")) {
				quat = Quaternion.Euler (-1*rotate);
			}

			if (Input.GetKey ("right")) {
				quat = Quaternion.Euler (rotate);
			}

			if (Input.GetKeyDown ("r")) {	
				var rot = new Vector3(0,180,0);
				quat = Quaternion.Euler (rot);
			}

			if (Input.GetKey ("w")) {
				cam.transform.position += new Vector3 (0, camspeed, 0);
			}
			if (Input.GetKey ("a")) {
				cam.transform.position += new Vector3 (-1*camspeed, 0, 0);

			}
			if (Input.GetKey ("s")) {
				cam.transform.position += new Vector3 (0, -1*camspeed,0);

			}
			if (Input.GetKey ("d")) {
				cam.transform.position += new Vector3 (1*camspeed, 0, 0);

			}

			if (Input.GetKey ("up")) {
				parent.transform.localScale *= (1 + scale);
				model_scale *= (1 + scale);

			}
			if (Input.GetKey ("down")) {
				parent.transform.localScale *= (1 - scale);
				model_scale *= (1 - scale);;

			}
			if (Input.GetKeyDown ("f")) {
				followMode = !followMode;
			}
			UpdatePoints (quat,model_scale);
		}
	}

	/// <summary>
	/// Gets the vertices on the mesh
	/// </summary>
	/// <returns>The vertices.</returns>
	public List<PositionNormals> GetVertices() {
		//grab vertices
		List<PositionNormals> posnormals = new List<PositionNormals>();
		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3> ();

		var meshFilter = model.GetComponent<MeshFilter> ();
		if (meshFilter == null) {
			var filters = model.GetComponentsInChildren<MeshFilter> ();
			foreach (var filter in filters) {
				vertices.AddRange (filter.mesh.vertices);
				normals.AddRange (filter.mesh.normals);
			}
		} else {
			vertices = new List<Vector3> (meshFilter.mesh.vertices);
			normals = new List<Vector3> (meshFilter.mesh.normals);
		}


		//updating vertices
		for(int i = 0; i < vertices.Count; i++)
		{
			vertices[i] += new Vector3(0,transform.localScale.y,0);
			posnormals.Add (new PositionNormals(vertices [i], normals [i]));
			y_set.Add (vertices [i].y);
		}

		return posnormals;
	}

	public Dictionary<int,GameObject> GetVerticesWithIDs() {

		return point_ids;
	}
}
