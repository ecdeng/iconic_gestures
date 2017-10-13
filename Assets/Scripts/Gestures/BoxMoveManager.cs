using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoxMoveManager : MoveManager {

	List<Vector3> GetBoxVertices() {
		List<Vector3> new_vertices = new List<Vector3>();

		var collider = model.GetComponent<Renderer> ();

		var boundPoint1 = collider.bounds.min;
		var boundPoint2 = collider.bounds.max;

		new_vertices.Add(collider.bounds.min);
		new_vertices.Add(collider.bounds.max);
		new_vertices.Add(new Vector3(boundPoint1.x, boundPoint1.y, boundPoint2.z));
		new_vertices.Add(new Vector3(boundPoint1.x, boundPoint2.y, boundPoint1.z));
		new_vertices.Add(new Vector3(boundPoint2.x, boundPoint1.y, boundPoint1.z));
		new_vertices.Add(new Vector3(boundPoint1.x, boundPoint2.y, boundPoint2.z));
		new_vertices.Add(new Vector3(boundPoint2.x, boundPoint1.y, boundPoint2.z));
		new_vertices.Add(new Vector3(boundPoint2.x, boundPoint2.y, boundPoint1.z));

		var angle = transform.rotation;
		var scale = transform.localScale;
		var position = transform.position;

		for (var i = 0; i < new_vertices.Count; i++) {
//			new_vertices[i] = angle * new_vertices[i];
//			new_vertices[i] = new Vector3(new_vertices[i].x*scale.x,new_vertices[i].y*scale.y,new_vertices[i].z*scale.z);
			new_vertices [i] += position;

		}
		//return new_vertices;

		//var allVertices = this.GetVertices (0);
		float minX = new_vertices [0].x;
		float minY = new_vertices [0].y;
		float minZ = new_vertices [0].z;
		float maxX = new_vertices [0].x;
		float maxY = new_vertices [0].y;
		float maxZ = new_vertices [0].z;

		List<Vector3> points = new List<Vector3>();

		// get the min/max x/y/z values
		foreach (var ver in new_vertices) {
			if (ver.x < minX) {
				minX = ver.x;
			}
			if (ver.x > maxX) {
				maxX = ver.x;
			} 
			if (ver.y < minY) {
				minY = ver.y;
			}
			if (ver.y > maxY) {
				maxY = ver.y;
			}
			if (ver.z < minZ) {
				minZ = ver.z;
			}
			if (ver.z > maxZ) {
				maxZ = ver.z;
			}
			//points.Add (ver);
		}

		float midX = (float) (maxX + minX) / (float) 2;
		float midY = (float) (maxY + minY) / (float) 2;
		float midZ = (float) (maxZ + minZ) / (float) 2;


		points.Add (new Vector3(midX, midY, minZ)); //front face
		points.Add (new Vector3(midX, midY, maxZ)); //back face
		points.Add (new Vector3(midX, minY, midZ)); //bottom face
		points.Add (new Vector3(midX, maxY, midZ)); // top face
		points.Add (new Vector3(minX, midY, midZ)); //left face
		points.Add (new Vector3(maxX, midY, midZ)); //right face
		return points;

	}

	// Use this for initialization
	void Start () {
		base.Start ();
		vertices = GetBoxVertices();
		ShowPoints ();
		StartCoroutine (base.Move(2));
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {

		UpdatePosition ();
	}
}
