using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class FileUpload : Singleton<FileUpload> {

	private GameObject model;
	public Button fileUploadButton;
	public string path;

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

	public void Disable () {
		Button btn = fileUploadButton.GetComponent<Button>();
		btn.gameObject.SetActive (false);
	}
}
