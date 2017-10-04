using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EggMovemanager : MoveManager {

	void Start () {
		base.Start ();
		vertices = this.GetEggPoints();
		ShowPoints ();
		StartCoroutine (base.Move(1));
	}

	List<Vector3> GetEggPoints()
	{

		//get all y
		var allverts = this.GetVertices (0);

		List<Vector3> answer = new List<Vector3>();


		//get all points on the bototm ring
		List<Vector3> points = new List<Vector3>();
		for(var i = 0; i < allverts.Count - 1; i++) {
			if (allverts [i].y == allverts[100].y) {
				points.Add(allverts[i]);
			}
		}

		//divide up the points
		points = new List<Vector3>(points.Where((x, i) => i % (5) == 0));


		//get all points on top ring
		List<Vector3> uppoints = new List<Vector3>();

		for(var i = 0; i < allverts.Count - 1; i++) {
			if (allverts [i].y == allverts[allverts.Count - 100].y) {
				uppoints.Add(allverts[i]);
			}
		}

		//divide up points
		uppoints = new List<Vector3>(uppoints.Where((x, i) => i % (5) == 0));




		//add them between hands
		for(var i = 0; i < points.Count; i++) {
			answer.Add (points [i]);
			answer.Add (points [(i + points.Count / 2) % points.Count]);
		}

		//add them between hands
		for(var i = 0; i < uppoints.Count; i++) {
			answer.Add (uppoints [i]);
			answer.Add (uppoints [(i + uppoints.Count / 2) % uppoints.Count]);
		}




			

		//points = new List<Vector3>(points.Where((x, i) => i % (5) == 0));


		return answer;
		/*Vector3 min = allverts[0];
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

		return points;*/
	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {

		UpdatePosition ();
	}
}
