using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMoveManager : MoveManager {


	void Start () {
		base.Start ();

	}

	void Update() {

	}

	public override List<Vector3> GetObjectVertices (int n) {
		float scale = 0.12f;	
		List<Vector3> upts = new List<Vector3>();
		float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
		float off = 2.0f / n;
		float x = 0;
		float y = 0;
		float z = 0;
		float r = 0;
		float phi = 0;

		// change k to n during generalization
		for (var k = 0; k < n; k++){
			y = k * off - 1 + (off /2);
			r = Mathf.Sqrt(1 - y * y);
			phi = k * inc;
			x = Mathf.Cos(phi) * r;
			z = Mathf.Sin(phi) * r;

			upts.Add(new Vector3(x, y, z)*scale);
		}
		return upts;
	}
}
