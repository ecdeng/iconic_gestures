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
		Vector3 max = allVertices[0];
		foreach (var ver in allVertices) {
			if (ver.y > max.y) {
				max = ver;
			}
			var key = (int)(ver.y * 1000);
			if (!crossSection_y.ContainsKey(key))
			{
				crossSection_y.Add(key, new List<Vector3>());
			}
			crossSection_y[key].Add(ver);

		}

		float targetZ = max.z;

		// get all points that have the target Z value
		foreach (var ver in allVertices) {
			if (Math.Abs(ver.z - targetZ) <= 0.005 && ver.y < 0.12f) {
				answer.Add (ver);
			}
		}

		answer.Sort(new Vector3Comparer());


		List<Vector3> left = new List<Vector3>();
		foreach (Vector3 vert in answer)
		{
			if (vert.x <= 0.03f) left.Add(vert);
		}

		List<Vector3> leftHalf = new List<Vector3>();
		List<Vector3> rightHalf = new List<Vector3>();
		List<Vector3> final = new List<Vector3>();
		int space = left.Count / ((n / 2));
		for (int i = 0; i < left.Count; i += space) {
			leftHalf.Add (left [i]);
		}

		for (int i = 0; i < leftHalf.Count; i++) {
			int key = (int)(leftHalf [i].y * 1000);
			var y_values = crossSection_y [key];


			foreach (var right_vec in y_values) {
				print(right_vec);
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


		leftHalf.Add (canebottom);
		leftHalf.Add (hookright);
		rightHalf.Add (canetop);
		rightHalf.Add (hookleft);
		final.AddRange(leftHalf);
		final.AddRange(rightHalf);



		return final;




//		//return this.GetCrossSectionVertices(n, 'z', 1000, -100f, 0.12f, 0.03f);
//
//		//get all y
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
//			if (Math.Abs(ver.z - targetZ) <= 0.005 && ver.y < 0.12f) {
//				answer.Add (ver);
//			}
//		}
//
//		answer.Sort(new Vector3Comparer());
//
//		List<Vector3> left = new List<Vector3>();
//		List<Vector3> right = new List<Vector3>();
//
//		foreach (Vector3 vert in answer)
//		{
//			if (vert.x >= 0.03f) right.Add(vert);
//			if (vert.x <= 0.03f) left.Add(vert);
//		}
//
//		List<Vector3> leftHalf = new List<Vector3>();
//		List<Vector3> rightHalf = new List<Vector3>();
//		List<Vector3> final = new List<Vector3>();
//		int space = Math.Min(left.Count, right.Count) / ((n / 2) - 1);
//		print(space + " " + Math.Min(left.Count, right.Count));
//		for (int i = 0; i < Math.Min(left.Count, right.Count); i += space)
//		{
//			leftHalf.Add(left[i]);
//			rightHalf.Add(right[i]);
//		}
//
//		if (leftHalf.Count + rightHalf.Count == n - 2)
//		{
//			leftHalf.Add(left[left.Count - 1]);
//		}
//		if (leftHalf.Count + rightHalf.Count == n - 1)
//		{
//			rightHalf.Add(right[right.Count - 1]);
//		}
//
//		final.AddRange(leftHalf);
//		final.AddRange(rightHalf);
//
//
//		return final;

		//return answer.Where((x, i) => i % answer.Count/10 == 0).ToList();

		//polebottomright
		//polebottomleft
		//polemiddleright
		//polemiddleleft
		//canetop
		//canebottom
		//hookright
		//hookleft

//		var allverts = this.GetVertices ();
//		Vector3 polebottomright = allverts[0];
//		Vector3 polemiddleright = allverts [0];
//		Vector3 canetop = allverts [0];
//		Vector3 hookleft = allverts [0];
//
//		//get polebottom right, middle, hookleft and top
//		foreach (var ver in allverts) {
//			if (ver.y <= polebottomright.y && ver.x >= polebottomright.x) {
//				polebottomright = ver;
//			} 
//			if (ver.y <= polemiddleright.y && ver.y > 0 && ver.x >= polebottomright.x) {
//				polemiddleright = ver;
//			}
//			if (ver.y <= hookleft.y && ver.x <= hookleft.x) {
//				hookleft = ver;
//			} 
//
//			else if (ver.y > canetop.y) {
//				canetop = ver;
//			}
//		}
//
//		//get both pole points
//		Vector3 polebottomleft = polebottomright;
//		Vector3 polemiddleleft = polemiddleright;
//		foreach (var ver in allverts) {
//			if (ver.y == polebottomleft.y && ver.x < polebottomleft.x) {
//				polebottomleft = ver;
//			}
//			if (ver.y > 0 && ver.y <= 0.1 && ver.x < polemiddleleft.x && ver.x > canetop.x) {
//				polemiddleleft = ver;
//			}
//		}
//
//		//get canebottom points
//		Vector3 canebottom = canetop;
//		double diff = 0.001;
//		foreach (var ver in allverts) {
//			if (ver.x < canetop.x + 0.01 && ver.x > canetop.x - 0.01) {
//				canebottom = ver;
//			}
//		}
//
//		//get final hook points
//		Vector3 hookright = hookleft;
//		foreach(var ver in allverts) {
//			if (ver.y <  hookleft.y + 0.01 && ver.y > hookleft.y - 0.01 && ver.x > hookright.x && ver.x < canebottom.x) {
//				hookright = ver;
//			}
//		}
//
//
//
//		/*Vector3 min = allverts[0];
//		Vector3 max = allverts [1];
//		foreach (var ver in allverts) {
//			if (ver.y < min.y) {
//				min = ver;
//			} else if (ver.y > max.y) {
//				max = ver;
//			}
//		}
//
//		Vector3 avg1 = new Vector3 (min.x - 0.05f, (min.y + max.y) / 2, min.z);
//		Vector3 avg2 = new Vector3 (min.x + 0.05f, (min.y + max.y) / 2, min.z);
//
//		*/
//		List<Vector3> points = new List<Vector3>();
//		points.Add(polebottomleft);
//		points.Add (polemiddleleft);
//		points.Add(canebottom);
//		points.Add(hookright);
//		points.Add(polebottomright);
//		points.Add(polemiddleright);
//		points.Add(canetop);
//		points.Add(hookleft);
//
//		return points;
	}
}

