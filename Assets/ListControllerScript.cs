using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListControllerScript : Singleton<ListControllerScript> {
		public GameObject ContentPanel;
		public GameObject ListItemPrefab;
		private GameObject model;

		List<GameObject> listItemGameObjects;
		List<ListItemScript> listItems;
		public List<int> selectedListItemIDs;

	void Start () {
	}

	void DestroyListIfAny() {
		if (listItemGameObjects.Count > 0) {
			foreach (GameObject gameObject in listItemGameObjects) {
				Destroy (gameObject);
			}
		}
	}

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
			listItemScript.x = point.Value.transform.position.x;
			listItemScript.y = point.Value.transform.position.y;
			listItemScript.z = point.Value.transform.position.z;
//			listItemScript.text.text = listItemScript.id.ToString () + ": (" +
//				listItemScript.x.ToString ("0.0000") + "," +
//				listItemScript.y.ToString ("0.0000") + "," +
//				listItemScript.z.ToString ("0.0000") + ")";
			listItemScript.text.text = listItemScript.id.ToString () + ": (" +
				System.Math.Round(listItemScript.x,3).ToString() + "," +
				System.Math.Round(listItemScript.y,3).ToString() + "," +
				System.Math.Round(listItemScript.z,3).ToString() + ")";
			listItemScript.selected = false;

			listItems.Add (listItemScript); // store the script for this item
			listItemGameObjects.Add (newListItem); // store the game object for this item

			newListItem.transform.SetParent (ContentPanel.transform);

//			newListItem.transform.parent = ContentPanel.transform;
			newListItem.transform.localScale = Vector3.one;

		}
	}

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
				listItemScript.id = original_to_new_id[point.Key];
				listItemScript.x = point.Value.transform.position.x;
				listItemScript.y = point.Value.transform.position.y;
				listItemScript.z = point.Value.transform.position.z;

				listItemScript.text.text = listItemScript.id.ToString () + ": (" +
				System.Math.Round (listItemScript.x, 3).ToString () + "," +
				System.Math.Round (listItemScript.y, 3).ToString () + "," +
				System.Math.Round (listItemScript.z, 3).ToString () + ")";
				listItemScript.selected = false;

				listItems.Add (listItemScript);
				listItemGameObjects.Add (newListItem);

				newListItem.transform.SetParent (ContentPanel.transform);

				//			newListItem.transform.parent = ContentPanel.transform;
				newListItem.transform.localScale = Vector3.one;
			}
		}

		Canvas.ForceUpdateCanvases ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
