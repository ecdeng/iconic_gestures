using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaneMoveManager : MoveManager {

	// Use this for initialization
	void Start () {
		base.Start ();
		vertices = this.GetCanePoints();
		ShowPoints ();
		StartCoroutine (base.Move(2));
	}

	List<Vector3> GetCanePoints()
	{

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

		Vector3 avg1 = new Vector3 (min.x - 0.05f, (min.y + max.y) / 2, min.z);
		Vector3 avg2 = new Vector3 (min.x + 0.05f, (min.y + max.y) / 2, min.z);

			
		List<Vector3> points = new List<Vector3>();
		points.Add(min);
		points.Add(avg1);
		points.Add (max);
		points.Add (avg2);

		return points;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {

		UpdatePosition ();
	}
}
