using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListItemScript : MonoBehaviour {
	public Text text;
	public int id;
	public float x;
	public float y;
	public float z;
	public bool selected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseEnter()
	{
		//If your mouse hovers over the GameObject with the script attached, output this message
		Debug.Log("Mouse is over point " + id);
	}
}
