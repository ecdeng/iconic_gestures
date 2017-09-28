using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ObjectManager : MonoBehaviour {

	//current game object
	private GameObject currObject;
	//show mesh
	private bool togglepoints;
	//points for mesh
	private List<GameObject> points;


	List<Vector3> UpdateMesh(GameObject obj) {

		//grab vertices
		List<Vector3> vertices = new List<Vector3>();
		var meshFilter = obj.GetComponent<MeshFilter> ();
		if (meshFilter == null) {

			var filters = obj.GetComponentsInChildren<MeshFilter> ();
			foreach (var filter in filters) {
				if (filter != null && filter.mesh != null)
					vertices.AddRange (filter.mesh.vertices);
			}
		} else {
			vertices = new List<Vector3>(meshFilter.mesh.vertices);
		}
			
		//grab tranform
		var angle = obj.transform.rotation;
		var scale = obj.transform.localScale;
		var position = obj.transform.position;

		//updating vertices
		for(int i = 0; i < vertices.Count; i++)
		{
			vertices[i] = angle * vertices[i];
			vertices[i] =new Vector3(vertices[i].x*scale.x,vertices[i].y*scale.y,vertices[i].z*scale.z);
			vertices [i] += position;
		}
			
		return vertices;

	}




	// Use this for initialization
	void Start () {
		points = new List<GameObject> ();
		togglepoints = false;
	}

	public void ChangeObject(string name) {

		//destory previous settings
		Destroy (currObject);

		foreach (var point in points) {
			Destroy (point);
		}
		points.Clear ();


		//create new objects
		currObject = (GameObject)Instantiate(Resources.Load(name + "/" + name));

		//grab vertices
		var vertices = UpdateMesh (currObject);


		foreach (var vertex in vertices) {

			//create spheres
			var gameObj = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			gameObj.transform.localScale = 0.01f * Vector3.one;
			gameObj.transform.position = vertex;
			gameObj.SetActive (togglepoints);
			points.Add (gameObj);
		}

	}
		
		
	
	// Update is called once per frame
	void Update () {
		
		//toggle show points
		if (Input.GetKeyDown("p")) {
			togglepoints = !togglepoints;
			foreach (var point in points) {
				point.SetActive(togglepoints);
			}
		}
	}
}
