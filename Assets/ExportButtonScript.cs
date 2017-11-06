using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
		List<string> unformatted = GenerateTableButtonScript.Instance.inputVals;
		int numCols = GenerateTableButtonScript.Instance.numCols;
		List<int> unformattedToInt = new List<int>(unformatted.Count);
		for (int i = 0; i < unformattedToInt.Count; i++) {
			unformattedToInt [i] = int.Parse(unformatted [i]);
			Debug.Log (unformattedToInt [i] + ", ");
		}
		List<List<int>> result = new List<List<int>>(numCols);
		for (int j = 0; j < numCols; j++) {
			result.Add(new List<int>());
			for (int i = 0; i < unformattedToInt.Count; i++) {
				if (i%j == 0) {
					result [j][i] = unformattedToInt [i];
				}
			}
		}
		return result;
	}
}
