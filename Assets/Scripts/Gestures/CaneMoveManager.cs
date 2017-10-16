using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaneMoveManager : MoveManager {


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


		//polebottomright
		//polebottomleft
		//polemiddleright
		//polemiddleleft
		//canetop
		//canebottom
		//hookright
		//hookleft

		var allverts = this.GetVertices ();
		Vector3 polebottomright = allverts[0];
		Vector3 polemiddleright = allverts [0];
		Vector3 canetop = allverts [0];
		Vector3 hookleft = allverts [0];

		//get polebottom right, middle, hookleft and top
		foreach (var ver in allverts) {
			if (ver.y <= polebottomright.y && ver.x >= polebottomright.x) {
				polebottomright = ver;
			} 
			if (ver.y <= polemiddleright.y && ver.y > 0 && ver.x >= polebottomright.x) {
				polemiddleright = ver;
			}
			if (ver.y <= hookleft.y && ver.x <= hookleft.x) {
				hookleft = ver;
			} 

			else if (ver.y > canetop.y) {
				canetop = ver;
			}
		}

		//get both pole points
		Vector3 polebottomleft = polebottomright;
		Vector3 polemiddleleft = polemiddleright;
		foreach (var ver in allverts) {
			if (ver.y == polebottomleft.y && ver.x < polebottomleft.x) {
				polebottomleft = ver;
			}
			if (ver.y > 0 && ver.y <= 0.1 && ver.x < polemiddleleft.x && ver.x > canetop.x) {
				polemiddleleft = ver;
			}
		}

		//get canebottom points
		Vector3 canebottom = canetop;
		double diff = 0.001;
		foreach (var ver in allverts) {
			if (ver.x < canetop.x + 0.01 && ver.x > canetop.x - 0.01) {
				canebottom = ver;
			}
		}

		//get final hook points
		Vector3 hookright = hookleft;
		foreach(var ver in allverts) {
			if (ver.y <  hookleft.y + 0.01 && ver.y > hookleft.y - 0.01 && ver.x > hookright.x && ver.x < canebottom.x) {
				hookright = ver;
			}
		}



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

		*/
		List<Vector3> points = new List<Vector3>();
		points.Add(polebottomleft);
		points.Add (polemiddleleft);
		points.Add(canebottom);
		points.Add(hookright);
		points.Add(polebottomright);
		points.Add(polemiddleright);
		points.Add(canetop);
		points.Add(hookleft);

		return points;
	}
}
