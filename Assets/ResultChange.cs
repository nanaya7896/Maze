using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultChange : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			FadeManager.Instance.LoadLevel (SceneManage.SceneName.TITLE, 2f, false);
		}
	}
}
