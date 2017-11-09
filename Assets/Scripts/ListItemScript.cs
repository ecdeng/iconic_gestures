using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ListItemScript : MonoBehaviour , IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
	public Text text;
	public int displayID;
	public int id;
	public float x;
	public float y;
	public float z;
	public bool selected;
	private float scale = 0.05f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// handler for clicking on list element. If in selection mode, show the element and point in green.
	/// </summary>
	/// <param name="evd">Evd.</param>
	public void OnPointerClick (PointerEventData evd)
	{
		// only do something if in first stage/selection mode for now
		if (ObjManager.Instance.isInSelectionMode) {
			if (!selected) { // if point was previously not selected, toggle
				selected = true;
				ObjManager.Instance.Select (ObjManager.Instance.GetGameObject (id));
				gameObject.GetComponent<Image> ().color = UnityEngine.Color.green;

			} else {
				selected = false;
				ObjManager.Instance.Unhighlight (ObjManager.Instance.GetGameObject (id), true);
				gameObject.GetComponent<Image> ().color = UnityEngine.Color.clear;
			}
		}
	}

	/// <summary>
	/// handler for hovering over list element. if in selection mode, hovering (not selection) will show the element in red and also highlight the point red
	/// if in grouping state, element will highlight in green and point will enlarge
	/// </summary>
	/// <param name="evd">Evd.</param>
	public void OnPointerEnter (PointerEventData evd)
	{
		var obj = ObjManager.Instance.GetGameObject (id);
		if (!selected && ObjManager.Instance.isInSelectionMode) { // stage one
			gameObject.GetComponent<Image> ().color = UnityEngine.Color.red;
			ObjManager.Instance.Highlight (ObjManager.Instance.GetGameObject (id));
		} else if (!ObjManager.Instance.isInSelectionMode) { // stage 2
			GameObject sphere = ObjManager.Instance.GetGameObject (id);
			sphere.transform.localScale = Vector3.one * scale * 10;
			gameObject.GetComponent<Image> ().color = UnityEngine.Color.green;
		}
		ObjManager.Instance.FollowCamera (obj);

	}

	/// <summary>
	/// handler for exiting (aka finish hovering) over list element. undo the UI changes made in OnPointerEnter
	/// </summary>
	/// <param name="evd">Evd.</param>
	public void OnPointerExit (PointerEventData evd)
	{
		if (!selected && ObjManager.Instance.isInSelectionMode) {
			gameObject.GetComponent<Image> ().color = UnityEngine.Color.clear;
			ObjManager.Instance.Unhighlight (ObjManager.Instance.GetGameObject (id), false);
		} else if (!ObjManager.Instance.isInSelectionMode) {
			GameObject sphere = ObjManager.Instance.GetGameObject (id);
			sphere.transform.localScale = Vector3.one * scale * 5;
			gameObject.GetComponent<Image> ().color = UnityEngine.Color.clear;
		}

	}
}
