using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Instance;
	void Awake() {
		Instance = (T) FindObjectOfType(typeof(T));
	}
}