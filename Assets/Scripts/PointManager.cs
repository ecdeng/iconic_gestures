using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PointManager : MonoBehaviour {

	public static List<Vector3> new_vertices;

	void SelectPoints(int n) {
		Mesh mesh = GetComponentInChildren<MeshFilter> ().mesh;
		Vector3[] vertices = mesh.vertices;
		var lvertices = new List<Vector3> (vertices);
		new_vertices = new List<Vector3>(lvertices.Where((x, i) => i % (lvertices.Count/n) == 0));
		foreach (var vertex in new_vertices) {
			var gameObj = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			gameObj.transform.position = vertex;
			gameObj.transform.localScale = Vector3.one * 0.01f;
		}
	}

	// Use this for initialization
	void Start () {
		SelectPoints (100);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
