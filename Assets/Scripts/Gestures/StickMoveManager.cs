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
			else left.Add(vert);
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


//		var allVertices = this.GetVertices ();
//		Vector3 minY = allVertices [0]; // lower endpoint
//		Vector3 maxY = allVertices [0]; // upper endpoint
//
//		// get the min and max x and y values
//		foreach (var ver in allVertices) {
//			if (ver.y < minY.y) {
//				minY = ver;
//			} else if (ver.y > maxY.y) {
//				maxY = ver;
//			}
//		}
//		float middleYValue = ((maxY.y + minY.y) / 2);
//
//		Vector3 minX = allVertices [0]; // lower endpoint
//		Vector3 maxX = allVertices [0]; // upper endpoint
//
//		// get the points with max/min x values around the middle Y value
//		foreach (var ver in allVertices) {
//			if (Math.Abs (ver.y - middleYValue) <= .09) {
//				if (ver.x < minX.x) {
//					minX = ver;
//				} else if (ver.x > maxX.x) {
//					maxX = ver;
//				}
//			}
//		}
//
//		//		Vector3 avg1 = new Vector3 (min.x - 0.05f, (min.y + max.y) / 2, min.z);
//		//		Vector3 avg2 = new Vector3 (min.x + 0.05f, (min.y + max.y) / 2, min.z);
//		//
//		List<Vector3> points = new List<Vector3>();
//		//		points.Add(min);
//		//		points.Add(avg1);
//		//		points.Add (max);
//		//		points.Add (avg2);
//		points.Add(minY);
//		points.Add (maxY);
//		points.Add (minX);
//		points.Add (maxX);
//		return points;
	}
		
}
