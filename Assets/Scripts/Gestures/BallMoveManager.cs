using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMoveManager : MoveManager {

	private float scale = 0.12f;

	void Start () {
		base.Start ();
		vertices = PointsOnSphere(10);
		ShowPoints ();
		StartCoroutine (base.Move(vertices.Count/2));
	}

	void Update() {

	}

	List<Vector3> PointsOnSphere(int n)
	{
		List<Vector3> upts = new List<Vector3>();
		float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
		float off = 2.0f / n;
		float x = 0;
		float y = 0;
		float z = 0;
		float r = 0;
		float phi = 0;

		for (var k = 0; k < n; k++){
			y = k * off - 1 + (off /2);
			r = Mathf.Sqrt(1 - y * y);
			phi = k * inc;
			x = Mathf.Cos(phi) * r;
			z = Mathf.Sin(phi) * r;

			upts.Add(new Vector3(x, y, z)*scale - new Vector3(0,0.1f,0));
		}
		return upts;
	}

	void FixedUpdate() {
		
		UpdatePosition ();
	}
}
