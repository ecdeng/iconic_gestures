using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaneMoveManager : MoveManager {

	// Use this for initialization
	void Start () {
		base.Start ();
		vertices = this.GetVertices(0);
		ShowPoints ();
		StartCoroutine (base.Move(1));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {

		UpdatePosition ();
	}
}
