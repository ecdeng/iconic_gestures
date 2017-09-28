using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMoveManager : MonoBehaviour {

	List<Vector3> vertices;
	int currentPosition = 0;
	float speed = 5.0f;
	private bool startMoving = true;
	private Vector3 oldPosition;
	private Vector3 oldRight;

	// Use this for initialization
	void Start () {
		float scaling = 1.5f;
		List<Vector3> pts = PointsOnSphere(10);
		List<GameObject> uspheres = new List<GameObject>();
		int i = 0;
		oldPosition = transform.position;
		oldRight = transform.right;

		foreach (Vector3 value in pts)
		{
			uspheres.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
			uspheres[i].transform.localScale *= 0.1f;
			uspheres[i].transform.position = value * scaling;
			i++;
		}
		
	}

	void Update() {
		if (vertices == null) {
			//vertices = PointManager.new_vertices;
			vertices = PointsOnSphere(10);
		}
		else if (startMoving) {
			StartCoroutine (Move ());
			startMoving = false;
		}

	}

	List<Vector3> PointsOnSphere(int n)
	{
		List<Vector3> upts = new List<Vector3>();
		float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
		float off = 2.0f / n;
		float x = 0;
		float y = 0;
		float z = 0;
		float r = 0;
		float phi = 0;

		for (var k = 0; k < n; k++){
			y = k * off - 1 + (off /2);
			r = Mathf.Sqrt(1 - y * y);
			phi = k * inc;
			x = Mathf.Cos(phi) * r;
			z = Mathf.Sin(phi) * r;

			upts.Add(new Vector3(x, y, z));
		}
		return upts;
	}


	IEnumerator Move()
	{
		for(int i = 0; i < vertices.Count; i ++)
		{
			Debug.Log (currentPosition);
			if (gameObject.name == "Hand1")
				currentPosition = i;
			else
				currentPosition = (i + 1) % vertices.Count;
			yield return new WaitForSeconds(1f);
		}
	}

	void FixedUpdate()
	{
		if (vertices != null) {
			var normal = (vertices [currentPosition]).normalized;
			transform.position = Vector3.Lerp (transform.position, vertices [currentPosition] * 1.5f, 10 * Time.deltaTime);
			transform.right = Vector3.Lerp (transform.right, normal, 10 * Time.deltaTime);
		}
	}

}
