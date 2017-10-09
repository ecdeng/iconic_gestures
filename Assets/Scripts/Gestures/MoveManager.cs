using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour {

	public int numVerts = 0;
	public bool showObject = true;
	protected GameObject model;
	protected int currentPosition = 0;
	protected List<Vector3> vertices;
	protected float speed = 10;
	protected Vector3[] minMaxVertices;

	// Use this for initialization
	protected virtual void Start () {
		model = TestObjectManager.Instance.GetObject();
		minMaxVertices = GetMinMaxVertices ();
		UpdateShow ();

	}

	public void UpdateShow() {
		var renderer = model.GetComponent<Renderer> ();
		if (renderer == null) {
			var renderers = model.GetComponentsInChildren<Renderer> ();
			foreach (var childRenderer in renderers) {
				childRenderer.enabled = showObject;
			}
		} else {
			renderer.enabled = showObject;
		}	
	}

	protected void ShowPoints() {
		foreach (var vertex in vertices) {
			var gameObj = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			gameObj.transform.position = vertex;
			gameObj.transform.localScale = Vector3.one * 0.01f;
//			print (vertex.x + "," + vertex.y + "," + vertex.z);

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected Vector3[] GetMinMaxVertices() {
		var allverts = this.GetVertices (0);
		Vector3 min = allverts[0];
		Vector3 max = allverts [1];
		foreach (var ver in allverts) {
			if (ver.y < min.y) {
				min = ver;
			} else if (ver.y > max.y) {
				max = ver;
			}
		}

		return new Vector3[2] { min, max };
	}

	protected List<Vector3> GetVertices(int n) {
		//grab vertices
		List<Vector3> vertices = new List<Vector3>();
		var meshFilter = model.GetComponent<MeshFilter> ();
		if (meshFilter == null) {

			var filters = model.GetComponentsInChildren<MeshFilter> ();
			foreach (var filter in filters) {
				if (filter != null && filter.mesh != null)
					vertices.AddRange (filter.mesh.vertices);
			}
		} else {
			vertices = new List<Vector3>(meshFilter.mesh.vertices);
		}

		//grab tranform
		var angle = model.transform.rotation;
		var scale = model.transform.localScale;
		var position = model.transform.position;

		//updating vertices
		for(int i = 0; i < vertices.Count; i++)
		{
			vertices[i] = angle * vertices[i];
			vertices[i] = new Vector3(vertices[i].x*scale.x,vertices[i].y*scale.y,vertices[i].z*scale.z);
			vertices [i] += position;
		}

		//vertices = new List<Vector3>(vertices.Where((x, i) => i % (vertices.Count/n) == 0));


		return vertices;
	}

		
	protected IEnumerator Move(int offset)
	{
		for(int i = 0; i < vertices.Count; i ++)
		{
			//Debug.Log (currentPosition);
			if (gameObject.name == "Hand1") {
				currentPosition = i;
				//print ("Hand1: " + currentPosition);

			} else {
				currentPosition = (i + offset) % vertices.Count;
				//print ("Hand2: " + currentPosition);

			}
			yield return new WaitForSeconds(1f);
		}
	}

	protected void UpdatePosition()
	{
		var center = new Vector3 ((minMaxVertices [0].x + minMaxVertices [1].x) / 2, (minMaxVertices [0].y + minMaxVertices [1].y) / 2, (minMaxVertices [0].z + minMaxVertices [1].z) / 2);
		if (vertices != null) {
			var normal = (vertices [currentPosition] - center).normalized;
			transform.position = Vector3.Lerp (transform.position, vertices [currentPosition],  speed*Time.deltaTime);
			transform.right = Vector3.Lerp (transform.right, normal,  speed*Time.deltaTime);
		}
	}
}
