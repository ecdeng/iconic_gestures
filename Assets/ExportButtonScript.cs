using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ExportButtonScript : MonoBehaviour {
	public Button exportButton;
	public GameObject tableInfo;

	// Use this for initialization
	void Start () {
		Button btn = exportButton.GetComponent<Button>();
		btn.onClick.AddListener(delegate{Export();});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public List<List<int>> Export () {
		//Debug.Log ("Attempting to export.");

		List<string> unformatted = GenerateTableButtonScript.Instance.inputVals;
		//Debug.Log ("unformatted size: " + unformatted.Count);
		int numCols = GenerateTableButtonScript.Instance.numCols;
		//int numRows = GenerateTableButtonScript.Instance.numRows;
		List<int> unformattedToInt = new List<int>(unformatted.Count);
		unformattedToInt =  (Enumerable.Repeat (0, unformatted.Count)).ToList();

		//Debug.Log ("unformatted to int size: " + unformattedToInt.Count);
		for (int i = 0; i < unformattedToInt.Count; i++) {
			unformattedToInt [i] = int.Parse(unformatted [i]);
			//Debug.Log (unformattedToInt [i] + ", ");
		}

		List<int> unformattedToOrigKeys = new List<int> (unformatted.Count);
		unformattedToOrigKeys =  (Enumerable.Repeat (0, unformatted.Count)).ToList();
		for (int i = 0; i < unformattedToOrigKeys.Count; i++) {
			unformattedToOrigKeys[i] = ObjManager.Instance.GetVirtualMemory() [unformattedToInt [i]];
		}
		Debug.Log ("unformatted to original keys: " + unformattedToOrigKeys.Count);
	
		//convert from row-wise to column-wise
		List<List<int>> result = new List<List<int>>(numCols);
		for (int j = 0; j < numCols + 1; j++) {
			result.Add(new List<int>());
			for (int i = 0; i < unformattedToOrigKeys.Count; i++) {
				if (i%numCols == j) {
					result [j].Add (unformattedToOrigKeys [i]);
					//result [j-1][i] = unformattedToInt [i];
				}
			}
		}
//		Debug.Log ("firstcol0: " + result[0].Count);
//		Debug.Log ("secondcol1: " + result[1].Count);
		Debug.Log (result[0][0] +  "," + result[0][1] + "," + result[0][2]);
		return result;
	}
}
