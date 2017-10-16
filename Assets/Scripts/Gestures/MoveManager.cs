using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// Class is in charge of finding points for gesture path and then talks to hands to move
/// </summary>
public class MoveManager : MonoBehaviour {

	//object properties
	protected int numVerts = 10;
	public bool showObject = true;

	//points
	protected List<Vector3> vertices;
	private float scale = 0.02f;
	private List<GameObject> mesh_points = new List<GameObject>();

	//hands
	protected GameObject hand1;
	protected GameObject hand2;
	protected float speed = 10;

	//hand positions
	protected Vector3 hand1_pos = Vector3.zero;
	protected Vector3 hand2_pos = Vector3.zero;


	//override to set vertices
	public virtual List<Vector3> GetObjectVertices (int n) {

		//get all y
		var allVertices = this.GetVertices ();
		List<Vector3> answer = new List<Vector3>();

		//get point with highest y value
		Vector3 max = allVertices[0];
		foreach (var ver in allVertices) {
			if (ver.y > max.y) {
				max = ver;
			}
		}

		float targetZ = max.z;

		// get all points that have the target Z value
		foreach (var ver in allVertices) {
			if (Math.Abs(ver.z - targetZ) <= 0.005) {
				answer.Add (ver);
			}
		}

		return answer;

		//return this.GetVertices ();
	}

	// Use this for initialization
	protected virtual void Start () {
		//grab hand objects
		hand1 = GameObject.Find ("Hand1");
		hand2 = GameObject.Find ("Hand2");
		UpdateShowObject ();
		vertices = GetObjectVertices(numVerts);
		ShowPoints ();

	}


	/// <summary>
	/// Begins the gesture.
	/// </summary>
	public void BeginGesture() {
		hand1.SendMessage ("StartMoving", vertices);
		hand2.SendMessage ("StartMoving", vertices);

	}


	/// <summary>
	/// Destroies all points and self.
	/// </summary>
	public void DestroyAll() {
		foreach(var obj in mesh_points) {
			Destroy (obj);
		}
		Destroy (gameObject);
	}


	/// <summary>
	/// Updates the show object.
	/// </summary>
	public void UpdateShowObject() {
		var renderer = GetComponent<Renderer> ();
		renderer.enabled = showObject;

	}

	/// <summary>
	/// Shows the points.
	/// </summary>
	protected void ShowPoints() {
		foreach (var vertex in vertices) {
			var gameObj = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			gameObj.transform.position = vertex;
			gameObj.transform.localScale = Vector3.one * scale;
			mesh_points.Add (gameObj);
		}
	}

	/// <summary>
	/// Gets the minimum max vertices.
	/// </summary>
	/// <returns>The minimum max vertices.</returns>
	protected Vector3[] GetMinMaxVertices() {

		var oldrotation = transform.rotation;
		transform.rotation = Quaternion.identity;

		var renderer = GetComponent<Collider> ();
		var min = renderer.bounds.min;
		var max = renderer.bounds.max;
	

		var min_max = new Vector3[2] { min, max };

		transform.rotation = oldrotation;

		return min_max;



	}


	/// <summary>
	/// Gets the vertices on the mesh
	/// </summary>
	/// <returns>The vertices.</returns>
	protected List<Vector3> GetVertices() {
		//grab vertices
		List<Vector3> vertices = new List<Vector3>();

		var meshFilter = GetComponent<MeshFilter> ();
		vertices = new List<Vector3>(meshFilter.mesh.vertices);


		//grab tranform
		var angle = transform.rotation;
		var scale = transform.localScale;
		var position = transform.position;

		//updating vertices
		for(int i = 0; i < vertices.Count; i++)
		{
			vertices[i] = angle * vertices[i];
			vertices[i] = new Vector3(vertices[i].x*scale.x,vertices[i].y*scale.y,vertices[i].z*scale.z);
			vertices[i] += position;
		}

		return vertices;
	}


	/// <summary>
	/// Gets vertices specifically for box like objects
	/// </summary>
	/// <returns>The boxy vertices.</returns>
	protected List<Vector3> GetBoxyVertices() {
		List<Vector3> new_vertices = new List<Vector3>();

		var minmax = this.GetMinMaxVertices ();


		var boundPoint1 = minmax[0];
		var boundPoint2 = minmax[1];

		new_vertices.Add(boundPoint1);
		new_vertices.Add(boundPoint2);
		new_vertices.Add(new Vector3(boundPoint1.x, boundPoint1.y, boundPoint2.z));
		new_vertices.Add(new Vector3(boundPoint1.x, boundPoint2.y, boundPoint1.z));
		new_vertices.Add(new Vector3(boundPoint2.x, boundPoint1.y, boundPoint1.z));
		new_vertices.Add(new Vector3(boundPoint1.x, boundPoint2.y, boundPoint2.z));
		new_vertices.Add(new Vector3(boundPoint2.x, boundPoint1.y, boundPoint2.z));
		new_vertices.Add(new Vector3(boundPoint2.x, boundPoint2.y, boundPoint1.z));




		//var allVertices = this.GetVertices (0);
		float minX = new_vertices [0].x;
		float minY = new_vertices [0].y;
		float minZ = new_vertices [0].z;
		float maxX = new_vertices [0].x;
		float maxY = new_vertices [0].y;
		float maxZ = new_vertices [0].z;

		List<Vector3> points = new List<Vector3>();

		// get the min/max x/y/z values
		foreach (var ver in new_vertices) {
			if (ver.x < minX) {
				minX = ver.x;
			}
			if (ver.x > maxX) {
				maxX = ver.x;
			} 
			if (ver.y < minY) {
				minY = ver.y;
			}
			if (ver.y > maxY) {
				maxY = ver.y;
			}
			if (ver.z < minZ) {
				minZ = ver.z;
			}
			if (ver.z > maxZ) {
				maxZ = ver.z;
			}
			//points.Add (ver);
		}

		float midX = (float) (maxX + minX) / (float) 2;
		float midY = (float) (maxY + minY) / (float) 2;
		float midZ = (float) (maxZ + minZ) / (float) 2;


		points.Add (new Vector3(midX, midY, minZ)); //front face
		points.Add (new Vector3(midX, minY, midZ)); //bottom face
		points.Add (new Vector3(minX, midY, midZ)); //left face
		points.Add (new Vector3(midX, midY, maxZ)); //back face
		points.Add (new Vector3(midX, maxY, midZ)); // top face
		points.Add (new Vector3(maxX, midY, midZ)); //right face

		var angle = transform.rotation;
		var scale = transform.localScale;
		var position = transform.position;

		for (var i = 0; i < points.Count; i++) {
			points[i] = angle * points[i];
			points [i] += position;
		}
		return points;
	}

    public List<Vector3> GetCrossSectionVertices(int n, char axis, int precision)
    {
        List<Vector3> vertices = this.GetVertices();
        SortedList<int, List<Vector3>> crossSection = new SortedList<int, List<Vector3>>();
        foreach (var vert in vertices)
        {
            float axisValue;
            if (axis == 'x') axisValue = vert.x;
            else if (axis == 'y') axisValue = vert.y;
            else axisValue = vert.z;

            int key = (int)(axisValue * precision);
            if (!crossSection.ContainsKey(key))
            {
                crossSection.Add(key, new List<Vector3>());
            }
            crossSection[key].Add(new Vector3(vert.x, vert.y, vert.z));
        }

        List<Vector3> new_vertices = new List<Vector3>(new HashSet<Vector3>(crossSection.Values[crossSection.Count / 2]));
        new_vertices.Sort(new Vector3Comparer());
        List<Vector3> left = new List<Vector3>();
        List<Vector3> right = new List<Vector3>();
        foreach (Vector3 vert in new_vertices)
        {
            if (vert.x >= 0) right.Add(vert);
            if (vert.x <= 0) left.Add(vert);
        }

        List<Vector3> leftHalf = new List<Vector3>();
        List<Vector3> rightHalf = new List<Vector3>();
        List<Vector3> points = new List<Vector3>();
        int space = Math.Min(left.Count, right.Count) / ((n / 2) - 1);
        print(space + " " + Math.Min(left.Count, right.Count));
        for (int i = 0; i < Math.Min(left.Count, right.Count); i += space)
        {
            leftHalf.Add(left[i]);
            rightHalf.Add(right[i]);
        }

        if (leftHalf.Count + rightHalf.Count == n - 2)
        {
            leftHalf.Add(left[left.Count - 1]);
        }
        if (leftHalf.Count + rightHalf.Count == n - 1)
        {
            rightHalf.Add(right[right.Count - 1]);
        }

        points.AddRange(leftHalf);
        points.AddRange(rightHalf);

        return points;
    }
}

/// <summary>
/// Comparator for sorting the cross-section points in a CCW parametric order
/// </summary>
class Vector3Comparer : Comparer<Vector3>
{
    public override int Compare(Vector3 a, Vector3 b)
    {
        if (a.y == b.y)
        {
            return (a.x < b.x) ? 1 : -1;
        }
        return (a.y < b.y) ? 1 : -1;
    }
}