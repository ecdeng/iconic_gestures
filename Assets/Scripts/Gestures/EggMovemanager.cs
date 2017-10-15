using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EggMoveManager : MoveManager {


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
		//get all y
		var allVertices = this.GetVertices ();
		List<Vector3> answer = new List<Vector3>();

		//get point with highest y value
		Vector3 max = allVertices[0];
		foreach (var ver in allVertices) {
			if (ver.y > max.y) {
				max = ver;
			}
		}

		float targetZ = max.z;

		// get all points that have the target Z value
		foreach (var ver in allVertices) {
			if (Math.Abs(ver.z - targetZ) <= 0.005) {
				answer.Add (ver);
			}
		}

		return answer;

		//		divide up the points
		//		points = new List<Vector3>(points.Where((x, i) => i % (5) == 0));

		//		//divide up points
		//		uppoints = new List<Vector3>(uppoints.Where((x, i) => i % (5) == 0));

		//		//add them between hands
		//		for(var i = 0; i < points.Count; i++) {
		//			answer.Add (points [i]);
		//			answer.Add (points [(i + points.Count / 2) % points.Count]);
		//		}
		//
		//		//add them between hands
		//		for(var i = 0; i < uppoints.Count; i++) {
		//			answer.Add (uppoints [i]);
		//			answer.Add (uppoints [(i + uppoints.Count / 2) % uppoints.Count]);
		//		}

		//return answer;
	}
}
