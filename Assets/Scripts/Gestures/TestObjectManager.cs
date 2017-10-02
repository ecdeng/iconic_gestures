using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectManager : MonoBehaviour {

	public string ObjectName;
	private GameObject currObject;
	public static TestObjectManager Instance = null;
	private GameObject hand1;
	private GameObject hand2;


	public void SetName(string name) {
		ObjectName = name;
	}
		

	void Awake() {

		if (Instance == null) {
			Instance = this;
		}
		currObject = (GameObject)Instantiate(Resources.Load(ObjectName + "/" + ObjectName));
		currObject.transform.parent = gameObject.transform;
		hand1 = GameObject.Find ("Hand1");
		hand2 = GameObject.Find ("Hand2");

		var scripts = hand1.GetComponents<MonoBehaviour>();
		for(int i = 0; i < scripts.Length; i++){
			var data = scripts[i];
			if (data.GetType().FullName.Contains (ObjectName)) {
				data.enabled = true;
				break;
			}
		}

		scripts = hand2.GetComponents<MonoBehaviour>();
		for(int i = 0; i < scripts.Length; i++){
			var data = scripts[i];
			if (data.GetType().FullName.Contains (ObjectName)) {
				data.enabled = true;
				break;
			}
		}

	}

	// Use this for initialization
	void Start () {


		
	}

	public GameObject GetObject () {
		return currObject;
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
