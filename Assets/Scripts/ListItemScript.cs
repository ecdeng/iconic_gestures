using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ListItemScript : MonoBehaviour , IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
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

	public void OnPointerClick (PointerEventData evd)
	{
		Debug.Log ("OnPointerClick: " + id);
	}

	public void OnPointerEnter (PointerEventData evd)
	{
		Debug.Log ("OnPointerEntered: " + id);
	}

	public void OnPointerExit (PointerEventData evd)
	{
		Debug.Log ("OnPointerExited: " + id);
	}
}
