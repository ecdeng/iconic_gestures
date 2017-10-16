using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BookMoveManager : MoveManager {

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
		return base.GetObjectVertices (n);

		return this.GetBoxyVertices ();
	}
}
