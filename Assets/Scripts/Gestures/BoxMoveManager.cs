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

		return new_vertices;

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
