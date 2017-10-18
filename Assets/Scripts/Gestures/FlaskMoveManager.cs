using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskMoveManager : MoveManager {



	/// <summary>
	/// Gets the object vertices - override base class.
	/// </summary>
	/// <returns>The object vertices.</returns>
	public override List<Vector3> GetObjectVertices (int n) {
        return this.GetCrossSectionVertices(n, 'z', 1000);
	}
}
