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
		if (!selected) { // if point was previously not selected, toggle
			selected = true;
			ObjManager.Instance.Select (ObjManager.Instance.GetGameObject (id));
			gameObject.GetComponent<Image>().color = UnityEngine.Color.green;

		} else {
			selected = false;
			ObjManager.Instance.Unhighlight (ObjManager.Instance.GetGameObject (id));
			gameObject.GetComponent<Image>().color = UnityEngine.Color.clear;
		}
	}

	public void OnPointerEnter (PointerEventData evd)
	{
		Debug.Log ("OnPointerEntered: " + id);
		if (!selected) {
			gameObject.GetComponent<Image> ().color = UnityEngine.Color.red;
			ObjManager.Instance.Highlight (ObjManager.Instance.GetGameObject (id));
		}
	}

	public void OnPointerExit (PointerEventData evd)
	{
		if (!selected) {
			gameObject.GetComponent<Image>().color = UnityEngine.Color.clear;
			ObjManager.Instance.Unhighlight (ObjManager.Instance.GetGameObject (id));
		}

	}
}
