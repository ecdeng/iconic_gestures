using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Vector3Comparer : Comparer<Vector3>
{
    public override int Compare(Vector3 a, Vector3 b)
    {
        if (a.y == b.y)
        {
            return (a.x < b.x) ? 1 : -1;
        }
        return (a.y < b.y) ? 1 : -1;
    }
}

public class FlaskMoveManager : MoveManager {


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
	}
}
