using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PositionNormals
{
	public Vector3 pos;
	public Quaternion norm;
	public PositionNormals(Vector3 pos, Quaternion norm){
		this.pos = pos;
		this.norm = norm;
	}
}

public class ObjManager : Singleton<ObjManager> {


	//model and point data
	public GameObject parent;
	public GameObject model;
	private float scale = 0.05f;
	private HashSet<float> y_set;
	private HashSet<float> radial_set;

	//user movement
	private float movespeed = 2.0f;
	private float camspeed = 0.5f;

	//id maps
	private Dictionary<int,GameObject> point_ids;
	private Dictionary<int,PositionNormals> point_normals;
	private Dictionary<int,int> virtual_memory;
	private Dictionary<int,int> physical_memory;

	//counters
	public int counter;
	private GameObject counterText;

	//selection and follow mode
	private bool followMode;
	public bool isInSelectionMode; // in selection mode is first stage
	private HashSet<int> selected_point_ids;

	//scrolling
	Vector2 scrollPosition = Vector2.zero;

	// Use this for initialization
	void Start () {
		InitObjects ();
		InitModel ();
		LoadModel ("Assets/Models/mug.obj");
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
				model_scale *= (1 + scale);

			}
			if (Input.GetKey ("down")) {
				model_scale *= (1 - scale);
			}
			if (Input.GetKeyDown ("f")) {
				followMode = !followMode;
			}
			UpdatePoints (quat,model_scale);
		}
	}

	/// <summary>
	/// Inits the Objects
	/// </summary>
	void InitObjects() {
		scale = 0.05f;

		counter = 0;
		counterText = GameObject.Find ("PointsSelectedCounter");

		isInSelectionMode = true;
		selected_point_ids = new HashSet<int> ();

		followMode = false;

	}

	/// <summary>
	/// Inits model information
	/// </summary>
	void InitModel() {
		parent = new GameObject ("parent");
		parent.transform.parent = transform;

		point_ids = new Dictionary<int, GameObject> ();
		point_normals = new Dictionary<int, PositionNormals> ();
		virtual_memory = new Dictionary<int, int> ();
		physical_memory = new Dictionary<int,int> ();

		y_set = new HashSet<float> ();
		radial_set = new HashSet<float> ();
	}

	/// <summary>
	/// Loads the entire model
	/// </summary>
	/// <param name="filepath">Filepath.</param>
	public void LoadModel(string filepath) {
		DestroyModel ();
		model = OBJLoader.LoadOBJFile (filepath);

		var minmax = GetMinMaxVertex (model);
		var min = minmax [0];
		var max = minmax [1];
		var offset = model.transform.position.y - min;
		var vec = new Vector3(0,offset,0);
		model.transform.position += vec;

		model.transform.parent = parent.transform;
		CreatePoints (vec);
		ListControllerScript.Instance.CreateListForModel ();
		Debug.Log (max);

		//change scale to bounding box
		while (GetMinMaxVertex (model) [1] > 5) {
			UpdatePoints (Quaternion.identity, Vector3.one * (1-scale));
		}
	}

	/// <summary>
	/// Destroy previous Model
	/// </summary>
	public void DestroyModel() {
		foreach (KeyValuePair<int, GameObject> entry in point_ids) {
			Destroy (entry.Value);
		}
		point_ids.Clear ();
		point_normals.Clear ();
		y_set.Clear ();
		radial_set.Clear ();
		Destroy (model);
	}

	/// <summary>
	/// Creates the vertex mesh
	/// </summary>
	/// <param name="offset">Offset.</param>
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

	/// <summary>
	/// Update the model and the points
	/// </summary>
	/// <param name="rotation">Rotation.</param>
	/// <param name="p_scale">P scale.</param>
	void UpdatePoints(Quaternion rotation, Vector3 p_scale) {

		//Debug.Log (GetMinMaxVertex (model) [1]);
		parent.transform.Rotate(rotation.eulerAngles);
		parent.transform.localScale *= p_scale.x;

		foreach(KeyValuePair<int, GameObject> entry in point_ids)
		{
			var pos = entry.Value.transform.position;
			pos = rotation * pos;
			pos = new Vector3(pos.x*p_scale.x,pos.y*p_scale.y,pos.z*p_scale.z);
			point_ids [entry.Key].transform.position = pos;
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
			posnormals.Add (new PositionNormals(vertices [i], Quaternion.Euler(normals [i])));
			y_set.Add (vertices [i].y);
			radial_set.Add(AngleToCamera(vertices[i]));

		}
		//Debug.Log (vertices [0]);
		//Debug.Log (Quaternion.Euler (normals [0]));

		return posnormals;
	}

	/// <summary>
	/// Sort the order of the points
	/// </summary>
	/// <returns>The points.</returns>
	/// <param name="points">Points.</param>
	List<PositionNormals> SortPoints(List<PositionNormals> points) {

		//restrict current points by height and radians
		var total_points = points.Count;
		int y_distro = 1;
		int rad_distro = 1;
		Debug.Log (total_points); 
		if (total_points > 100) {
			y_distro = 10;
			rad_distro = Mathf.Max (total_points / 3000, 1);
		}
			y_set = new HashSet<float> (y_set.Where ((x, i) => i % y_distro == 0));
			radial_set = new HashSet<float> (radial_set.Where ((x, i) => i % rad_distro == 0));


		//update points
		points = points.Where ((x, i) => y_set.Contains (x.pos.y)).ToList();
		points = points.Where((x,i) => radial_set.Contains(AngleToCamera(x.pos))).ToList();

		//order points
		points = points.OrderBy(obj => obj.pos.y).ThenBy(obj => AngleToCamera(obj.pos)).ToList();

		return points;
	}


	/// <summary>
	/// Get minimum vertex of model for placement
	/// </summary>
	/// <returns>The minimum vertex.</returns>
	/// <param name="obj">Object.</param>
	public float[] GetMinMaxVertex(GameObject obj) {
		var min = obj.transform.position.y;
		var max = obj.transform.position.y;
		var renderers = obj.GetComponentsInChildren<Renderer> ();
		foreach (var renderer in renderers) {
			min = Mathf.Min (min, renderer.bounds.min.y);
			max = Mathf.Max (max, renderer.bounds.max.y);
		}
		return new float[]{min,max};
	}
		

	/// <summary>
	/// helper functions for getting and sorting
	/// </summary>
	/// <returns>The to camera.</returns>
	/// <param name="pos">Position.</param>
	private float AngleToCamera(Vector3 pos) {
		var vec1 = new Vector2(pos.x,pos.z);
		var vec2 = new Vector2 (Camera.main.transform.position.x, Camera.main.transform.position.z);
		return -1 * AngleBetweenVector2 (vec1, vec2);
	}

	/// <summary>
	/// Finding angle between 2 vectors for ordering points
	/// </summary>
	/// <returns>The between vector2.</returns>
	/// <param name="vec1">Vec1.</param>
	/// <param name="vec2">Vec2.</param>
	private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
	{
		Vector2 vec1Rotated90 = new Vector2(-vec1.y, vec1.x);
		float sign = (Vector2.Dot(vec1Rotated90, vec2) < 0) ? -1.0f : 1.0f;
		return Vector2.Angle(vec1, vec2) * sign;
	}
		


	/// <summary>
	/// Show the point relative to the camera
	/// </summary>
	/// <param name="sphere">Sphere.</param>
	public void FollowCamera(GameObject sphere) {

		if (!followMode)
			return;
		var angle = AngleToCamera(sphere.transform.position);
		UpdatePoints(Quaternion.Euler(new Vector3(0,angle,0)),Vector3.one);
	}
		

	/// <summary>
	/// Getter for dictionary where key is point ID id and value are the game objects for the spheres/vertices on the mesh
	/// </summary>
	/// <returns>The vertices with I ds.</returns>
	public Dictionary<int,GameObject> GetVerticesWithIDs() {

		return point_ids;
	}

	/// <summary>
	/// Adds to virtual memory map where key is new simplified id to display in grouping state and value is the original point id
	/// </summary>
	/// <param name="virtual_id">Virtual identifier.</param>
	/// <param name="physical_id">Physical identifier.</param>
	public void AddToVirtualMemory(int virtual_id, int physical_id) {
		virtual_memory.Add (virtual_id, physical_id);
		physical_memory.Add (physical_id, virtual_id);
	}

	/// <summary>
	/// Returns dictionary where key is new simplified id to display in grouping state and value is the original point id
	/// </summary>
	/// <returns>The virtual memory.</returns>
	public Dictionary<int,int> GetVirtualMemory() {
		return virtual_memory;
	}

	/// <summary>
	/// Returns dictionary where key is original point id and value is the new simplified id to display in grouping state
	/// </summary>
	/// <returns>The physical memory.</returns>
	public Dictionary<int,int> GetPhysicalMemory() {
		return physical_memory;
	}

	/// <summary>
	/// returns gameobject for the sphere given id
	/// </summary>
	/// <returns>The game object.</returns>
	/// <param name="id">Identifier.</param>
	public GameObject GetGameObject(int id) {
		return point_ids [id];
	}
		
	/// <summary>
	/// remove unselected spheres in grouping state
	/// </summary>
	/// <param name="selected">Selected.</param>
	public void setSelectedPoints(List<int>selected)
	{
		selected_point_ids = new HashSet<int>(selected);
		foreach(var entry in point_ids) {
			if (!selected_point_ids.Contains (entry.Key)) {
				Destroy (entry.Value);
			}
		}
			
		point_ids = point_ids.Where (entry => selected_point_ids.Contains (entry.Key)).ToDictionary(entry => entry.Key, entry => entry.Value);

	}

	/// <summary>
	/// change state of application from selection to grouping state
	/// </summary>
	/// <param name="inSelectionMode">If set to <c>true</c> in selection mode.</param>
	public void setInSelectionMode(bool inSelectionMode)
	{
		isInSelectionMode = inSelectionMode;
	}

	// select the point to be used in grouping stage of the application
	public void Select(GameObject sphere) {
		Renderer renderer = sphere.GetComponent<Renderer>();
		renderer.material.color = Color.green;
		counter++;
		counterText.GetComponent<Text> ().text = counter.ToString ();
	}

	/// <summary>
	/// highlight the selected sphere to a red color, used when hovering over the corresponding point in the list 
	// in selection mode
	/// </summary>
	/// <param name="sphere">Sphere.</param>
	public void Highlight(GameObject sphere) {
		Renderer renderer = sphere.GetComponent<Renderer>();
		renderer.material.color = Color.red;

	}

	/// <summary>
	/// unhighlight the selected sphere to its original color
	/// </summary>
	/// <param name="sphere">Sphere.</param>
	/// <param name="unSelect">If set to <c>true</c> un select.</param>
	public void Unhighlight(GameObject sphere, bool unSelect) {
		if (unSelect) {
			counter--;
			counterText.GetComponent<Text> ().text = counter.ToString ();
		}
		Renderer renderer = sphere.GetComponent<Renderer>();
		renderer.material.color = Color.white;
	}
}
