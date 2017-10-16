using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PyramidMoveManager : MoveManager {


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

		List<Vector3> vlist = new List<Vector3>();

		var masterlist = this.GetVertices();

		// Get min/max x, y, and z directions
		float minX = masterlist[0].x, minY = masterlist[0].y, minZ = masterlist[0].z;
		float maxX = masterlist[0].x, maxY = masterlist[0].y, maxZ = masterlist[0].z;

		//		Vector3 min = masterlist[0];
		//		Vector3 max = masterlist [1];
		foreach (var ver in masterlist) {
			if (ver.x < minX) {
				minX = ver.x;
			} else if (ver.x > maxX) {
				maxX = ver.x;
			}

			if (ver.y < minY) {
				minY = ver.y;
			} else if (ver.y > maxY) {
				maxY = ver.y;
			}

			if (ver.z < minZ) {
				minZ = ver.z;
			} else if (ver.z > maxZ) {
				maxZ = ver.z;
			}
		}

		// get midpoint x&z
		float midX = (minX + maxX) / 2;
		float midZ = (minZ + maxZ) / 2;

		// get height of half of y
		float chunkY = (minY + maxY) / 2;

		// top&bottom points
		vlist.Add(new Vector3(midX, maxY + 0.1f, midZ));
		vlist.Add(new Vector3(midX, minY - 0.1f, midZ));

		// points down sides
		for (int i = 1; i <= 2; i++) {
			// add 
			float lengthX = (i*chunkY)*(maxX-midX)/(maxY-minY);
			float lengthZ = (i*chunkY)*(maxZ-midZ)/(maxY-minY);

			vlist.Add(new Vector3(midX + lengthX, maxY - i*chunkY, midZ));
			vlist.Add(new Vector3(midX - lengthX, maxY - i*chunkY, midZ));
			//
			//			vlist.Add(new Vector3(midX, maxY - i*chunkY, midZ + lengthZ));
			//			vlist.Add(new Vector3(midX, maxY - i*chunkY, midZ - lengthZ));
		}

		return vlist;
	}
}
