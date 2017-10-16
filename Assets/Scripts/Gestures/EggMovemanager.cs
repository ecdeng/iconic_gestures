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
        return this.GetCrossSectionVertices(n, 'z', 1000);

		List<Vector3> vertices = this.GetVertices();
		SortedList<int, List<Vector3>> crossSection = new SortedList<int, List<Vector3>>();
		foreach (var vert in vertices)
		{
			int key = (int) (vert.z * 1000);
			if (!crossSection.ContainsKey(key))
			{
				crossSection.Add(key, new List<Vector3>());
			}
			crossSection[key].Add(new Vector3(vert.x, vert.y, vert.z));
		}

		List<Vector3> new_vertices = new List<Vector3>(new HashSet<Vector3>(crossSection.Values[crossSection.Count / 2]));
		new_vertices.Sort(new Vector3Comparer());
		List<Vector3> left = new List<Vector3>();
		List<Vector3> right = new List<Vector3>();
		foreach (Vector3 vert in new_vertices)
		{
			if (vert.x > 0) right.Add(vert);
			else if (vert.x < 0) left.Add(vert);
		}

		List<Vector3> leftHalf = new List<Vector3> ();
		List<Vector3> rightHalf = new List<Vector3> ();
		List<Vector3> points = new List<Vector3>();
		int space = Math.Min(left.Count, right.Count) / ((n / 2) - 1);
		print(space + " " + Math.Min(left.Count, right.Count));
		for (int i = 0; i < Math.Min(left.Count, right.Count); i += space)
		{
			leftHalf.Add(left[i]);
			rightHalf.Add(right[i]);
		}

		if (leftHalf.Count + rightHalf.Count == n - 2)
		{
			leftHalf.Add(left[left.Count - 1]);
		}
		if (leftHalf.Count + rightHalf.Count == n - 1)
		{
			rightHalf.Add(right[right.Count - 1]);
		}

		points.AddRange (leftHalf);
		points.AddRange (rightHalf);

		return points;

		//get all y
//		var allVertices = this.GetVertices ();
//		List<Vector3> answer = new List<Vector3>();
//
//		//get point with highest y value
//		Vector3 max = allVertices[0];
//		foreach (var ver in allVertices) {
//			if (ver.y > max.y) {
//				max = ver;
//			}
//		}
//
//		float targetZ = max.z;
//
//		// get all points that have the target Z value
//		foreach (var ver in allVertices) {
//			if (Math.Abs(ver.z - targetZ) <= 0.005) {
//				answer.Add (ver);
//			}
//		}
//
//		return answer;
	}
}
