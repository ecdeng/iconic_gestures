using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Behavior script for the list of listItemScript elements
/// handles destroying and creating lists for the current model
/// </summary>
public class ListControllerScript : Singleton<ListControllerScript> {
		public GameObject ContentPanel; // what each list element is rendered on
		public GameObject ListItemPrefab; // prefab template for each list element

		List<GameObject> listItemGameObjects; // list of game objects for each list element 
		List<ListItemScript> listItems; // the scripts for each element, holds the data for each one
		public List<int> selectedListItemIDs; // id of each selected point

	void Start () {
	}

	/// <summary>
	/// destroy all list elements if any
	/// </summary>
	public void DestroyListIfAny() {
		if (listItemGameObjects != null && listItemGameObjects.Count > 0) {
			foreach (GameObject gameObject in listItemGameObjects) {
				Destroy (gameObject);
			}
		}
	}

	/// <summary>
	/// create list elements based on points for the current model
	/// </summary>
	public void CreateListForModel () {
		listItems = new List<ListItemScript> ();
		listItemGameObjects = new List<GameObject> ();
		selectedListItemIDs = new List<int> ();

		DestroyListIfAny ();

		Dictionary<int,GameObject> point_ids = ObjManager.Instance.GetVerticesWithIDs();
		foreach(KeyValuePair<int, GameObject> point in point_ids)
		{
			GameObject newListItem = Instantiate(ListItemPrefab) as GameObject;
			ListItemScript listItemScript = newListItem.GetComponent<ListItemScript>();
			listItemScript.id = point.Key;
			listItemScript.displayID = point.Key;
			listItemScript.x = point.Value.transform.position.x;
			listItemScript.y = point.Value.transform.position.y;
			listItemScript.z = point.Value.transform.position.z;
			listItemScript.text.text = listItemScript.id.ToString () + ": (" + // rounding so that coordinate text isn't too long
				System.Math.Round(listItemScript.x,3).ToString() + "," +
				System.Math.Round(listItemScript.y,3).ToString() + "," +
				System.Math.Round(listItemScript.z,3).ToString() + ")";
			listItemScript.selected = false;

			listItems.Add (listItemScript); // store the script for this item
			listItemGameObjects.Add (newListItem); // store the game object for this item

			newListItem.transform.SetParent (ContentPanel.transform);
			newListItem.transform.localScale = Vector3.one;

		}
	}

	/// <summary>
	/// Destroy all list elements and remake the list for only points selected from the selection state
	/// </summary>
	public void RemoveUnselectedPoints () {
		Debug.Log ("Removing");

		// create list of selected points
		selectedListItemIDs.Clear ();
		int id = 0;
		foreach (ListItemScript item in listItems) {
			if (item.selected == true) {
				selectedListItemIDs.Add (item.id);
				ObjManager.Instance.AddToVirtualMemory (id++, item.id);
			}
		}

		// store them in OBJManager in case
		ObjManager.Instance.setSelectedPoints (selectedListItemIDs);

		// go through and destroy all game objects to reset the list
		DestroyListIfAny ();

		listItems.Clear ();
		listItemGameObjects.Clear ();


		// go through and remake the objects for the selected IDs
		Dictionary<int,GameObject> point_ids = ObjManager.Instance.GetVerticesWithIDs();
		Dictionary<int,int> original_to_new_id = ObjManager.Instance.GetPhysicalMemory ();
		foreach(KeyValuePair<int, GameObject> point in point_ids)
		{
			if (selectedListItemIDs.Contains (point.Key)) {
				GameObject newListItem = Instantiate (ListItemPrefab) as GameObject;
				ListItemScript listItemScript = newListItem.GetComponent<ListItemScript> ();
				listItemScript.id = point.Key;
				listItemScript.displayID = original_to_new_id[point.Key];
				listItemScript.x = point.Value.transform.position.x;
				listItemScript.y = point.Value.transform.position.y;
				listItemScript.z = point.Value.transform.position.z;

				listItemScript.text.text = listItemScript.displayID.ToString () + ": (" +
				System.Math.Round (listItemScript.x, 3).ToString () + "," +
				System.Math.Round (listItemScript.y, 3).ToString () + "," +
				System.Math.Round (listItemScript.z, 3).ToString () + ")";
				listItemScript.selected = false;

				listItems.Add (listItemScript);
				listItemGameObjects.Add (newListItem);

				newListItem.transform.SetParent (ContentPanel.transform);
				newListItem.transform.localScale = Vector3.one;
			}
		}

		Canvas.ForceUpdateCanvases ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
