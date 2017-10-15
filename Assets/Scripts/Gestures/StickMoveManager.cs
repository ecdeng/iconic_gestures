using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StickMoveManager : MoveManager {

	void Start () {
		base.Start ();
	}

	void Update() {

	}

	/// <summary>
	/// Gets the object vertices - override base class.
	/// </summary>
	/// <returns>The object vertices.</returns>
	public override List<Vector3> GetObjectVertices (int n) {
		var allVertices = this.GetVertices ();
		Vector3 minY = allVertices [0]; // lower endpoint
		Vector3 maxY = allVertices [0]; // upper endpoint

		// get the min and max x and y values
		foreach (var ver in allVertices) {
			if (ver.y < minY.y) {
				minY = ver;
			} else if (ver.y > maxY.y) {
				maxY = ver;
			}
		}
		float middleYValue = ((maxY.y + minY.y) / 2);

		Vector3 minX = allVertices [0]; // lower endpoint
		Vector3 maxX = allVertices [0]; // upper endpoint

		// get the points with max/min x values around the middle Y value
		foreach (var ver in allVertices) {
			if (Math.Abs (ver.y - middleYValue) <= .09) {
				if (ver.x < minX.x) {
					minX = ver;
				} else if (ver.x > maxX.x) {
					maxX = ver;
				}
			}
		}

		//		Vector3 avg1 = new Vector3 (min.x - 0.05f, (min.y + max.y) / 2, min.z);
		//		Vector3 avg2 = new Vector3 (min.x + 0.05f, (min.y + max.y) / 2, min.z);
		//
		List<Vector3> points = new List<Vector3>();
		//		points.Add(min);
		//		points.Add(avg1);
		//		points.Add (max);
		//		points.Add (avg2);
		points.Add(minY);
		points.Add (maxY);
		points.Add (minX);
		points.Add (maxX);
		return points;
	}
		
}
