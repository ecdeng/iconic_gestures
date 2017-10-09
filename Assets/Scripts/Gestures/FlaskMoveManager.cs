using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskMoveManager : MoveManager {

	// Use this for initialization
	void Start () {
		base.Start ();
//		vertices = this.GetFlaskPoints();
		ShowPoints ();
		StartCoroutine (base.Move(1));
	}

	void GetFlaskPoints() {
//	List<Vector3> GetFlaskPoints() {

	}

	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		UpdatePosition ();
	}

}
