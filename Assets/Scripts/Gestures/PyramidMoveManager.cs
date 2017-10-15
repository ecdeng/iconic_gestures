using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
