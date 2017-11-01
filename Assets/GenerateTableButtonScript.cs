using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateTableButtonScript : MonoBehaviour {

	public Button generateTableButton;
	public GameObject TablePanel;
	public GameObject ListItemPrefab;
	private GameObject model;

	// Use this for initialization
	void Start () {
		Button btn = generateTableButton.GetComponent<Button>();
		btn.onClick.AddListener(delegate{GenerateTable(2,2);});
		TablePanel = GameObject.Find ("TablePanel");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GenerateTable(int rows, int cols) {
		for (int i = 0; i < 2; i++) {
			for (int j = 0; j < 2; j++) {
				GameObject newListItem = Instantiate(ListItemPrefab) as GameObject;
				ListItemScript listItemScript = newListItem.GetComponent<ListItemScript>();
				listItemScript.id = i * 2 + j;
				listItemScript.text.text = "Hello";
				listItemScript.selected = false;

				newListItem.transform.SetParent (TablePanel.transform);
				newListItem.transform.localScale = Vector3.one;
			}
		}

//		listItemScript.id = point.Key;
//		listItemScript.x = point.Value.transform.position.x;
//		listItemScript.y = point.Value.transform.position.y;
//		listItemScript.z = point.Value.transform.position.z;
//		//			listItemScript.text.text = listItemScript.id.ToString () + ": (" +
//		//				listItemScript.x.ToString ("0.0000") + "," +
//		//				listItemScript.y.ToString ("0.0000") + "," +
//		//				listItemScript.z.ToString ("0.0000") + ")";
//		listItemScript.text.text = listItemScript.id.ToString () + ": (" +
//			System.Math.Round(listItemScript.x,3).ToString() + "," +
//			System.Math.Round(listItemScript.y,3).ToString() + "," +
//			System.Math.Round(listItemScript.z,3).ToString() + ")";
//		listItemScript.selected = false;
//
//		newListItem.transform.SetParent (ContentPanel.transform);
//
//		//			newListItem.transform.parent = ContentPanel.transform;
//		newListItem.transform.localScale = Vector3.one;
	}
}
