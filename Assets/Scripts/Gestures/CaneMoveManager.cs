using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class CaneMoveManager : MoveManager {


	/// <summary>
	/// Gets the object vertices - override base class.
	/// </summary>
	/// <returns>The object vertices.</returns>
	public override List<Vector3> GetObjectVertices (int n) {

		n -= 4;

		List<Vector3> vertices = this.GetVertices();
		SortedList<int, List<Vector3>> crossSection_y = new SortedList<int, List<Vector3>>();

		//get all y
		var allVertices = this.GetVertices ();
		List<Vector3> answer = new List<Vector3>();

		//get point with highest y value
		//get point with highest y value
		Vector3 max = allVertices[0];
		foreach (var ver in allVertices) {
			int key = (int)(ver.y * 1000);
			if (ver.y > max.y) {
				max = ver;
			}
			if (!crossSection_y.ContainsKey(key))
			{
				crossSection_y.Add(key, new List<Vector3>());
			}
			crossSection_y[key].Add(ver);

		}

		float targetZ = max.z;

		// get all points that have the target Z value and below certain y
		foreach (var ver in allVertices) {
			if (Math.Abs(ver.z - targetZ) <= 0.005 && ver.y < 0.12f) {
				answer.Add (ver);
			}
		}

		answer.Sort(new Vector3Comparer());

		//add all left points
		List<Vector3> left = new List<Vector3>();
		foreach (Vector3 vert in answer)
		{
			if (vert.x <= 0.03f) left.Add(vert);
		}

		List<Vector3> leftHalf = new List<Vector3>();
		List<Vector3> rightHalf = new List<Vector3>();
		List<Vector3> final = new List<Vector3>();
		int space = left.Count / ((n / 2));

		//add specifics to left half
		for (int i = 0; i < left.Count; i += space) {
			leftHalf.Add (left [i]);
		}

		leftHalf.Reverse ();

		//get corresponding right half sections
		for (int i = 0; i < leftHalf.Count; i++) {
			int key = (int)(leftHalf [i].y * 1000);
			var y_values = crossSection_y [key];

			//get all values on the same y and same z
			foreach (var right_vec in y_values) {
				if (right_vec.x > 0.03f && Math.Abs(right_vec.z - leftHalf[i].z) <= 0.005) {
					rightHalf.Add (right_vec);
					break;
				}
			}
		}
			
		Vector3 canetop = allVertices [0];
		Vector3 hookleft = allVertices [0];

		//get polebottom right, middle, hookleft and top
		foreach (var ver in allVertices) {
			
			if (ver.y <= hookleft.y && ver.x <= hookleft.x) {
				hookleft = ver;
			} 

			else if (ver.y > canetop.y) {
				canetop = ver;
			}
		}



		//get canebottom points
		Vector3 canebottom = canetop;
		double diff = 0.001;
		foreach (var ver in allVertices) {
			if (ver.x < canetop.x + 0.01 && ver.x > canetop.x - 0.01) {
				canebottom = ver;
			}
		}

		//get final hook points
		Vector3 hookright = hookleft;
		foreach(var ver in allVertices) {
			if (ver.y <  hookleft.y + 0.01 && ver.y > hookleft.y - 0.01 && ver.x > hookright.x && ver.x < canebottom.x) {
				hookright = ver;
			}
		}

		//add special points
		leftHalf.Add (canebottom);
		leftHalf.Add (hookright);
		rightHalf.Add (canetop);
		rightHalf.Add (hookleft);
		final.AddRange(leftHalf);
		final.AddRange(rightHalf);



		return final;

	}
}

