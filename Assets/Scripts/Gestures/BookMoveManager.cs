using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BookMoveManager : MoveManager {


	/// <summary>
	/// Gets the object vertices - override base class.
	/// </summary>
	/// <returns>The object vertices.</returns>
	public override List<Vector3> GetObjectVertices (int n) {
		return this.GetBoxyVertices ();
	}
}
