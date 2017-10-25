using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListControllerScript : MonoBehaviour {
		public GameObject ContentPanel;
		public GameObject ListItemPrefab;
		private GameObject model;

	void Start () {

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

			newListItem.transform.SetParent (ContentPanel.transform);

//			newListItem.transform.parent = ContentPanel.transform;
			newListItem.transform.localScale = Vector3.one;

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
