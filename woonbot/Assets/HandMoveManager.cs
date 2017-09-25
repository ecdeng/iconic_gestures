using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMoveManager : MonoBehaviour {

	List<Vector3> vertices;
	int currentPosition = 0;
	float speed = 5.0f;
	private bool startMoving = true;

	// Use this for initialization
	void Start () {
		var gameobj = GameObject.Find ("basketball");

		
	}

	void Update() {
		if (vertices == null) {
			vertices = PointManager.new_vertices;
		}
		else if (startMoving) {
			StartCoroutine (Move ());
			startMoving = false;
		}

	}


	IEnumerator Move()
	{
		for(int i = 0; i < vertices.Count; i ++)
		{
			Debug.Log (currentPosition);
			currentPosition = i;
			yield return new WaitForSeconds(1f);
		}
	}

	void FixedUpdate()
	{
		if (vertices != null) {
			transform.position = Vector3.Lerp(transform.position, vertices[currentPosition], Time.deltaTime * 10);
			var normal = transform.position - vertices [currentPosition];

			transform.right = Vector3.Lerp(transform.right, normal, Time.deltaTime * 10);

		}
	}
}
