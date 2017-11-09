using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class FileUpload : MonoBehaviour {

	private GameObject model;
	public Button fileUploadButton;
	string path;

	// Use this for initialization
	void Start () {
		Button btn = fileUploadButton.GetComponent<Button>();
		btn.onClick.AddListener(OpenExplorer);
	}

	public void OpenExplorer() {
		path = EditorUtility.OpenFilePanel ("Object Upload", "", "obj");
		if(path.Length != 0)
			ObjManager.Instance.LoadModel (path);

	}
}
