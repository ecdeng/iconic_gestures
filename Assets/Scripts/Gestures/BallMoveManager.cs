using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BallMoveManager : MoveManager {


	void Start () {
		base.Start ();

	}

	void Update() {

	}

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
		return base.GetObjectVertices (n);
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
