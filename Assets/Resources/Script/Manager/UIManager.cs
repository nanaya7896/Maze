using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class UIManager : MonoBehaviour {


	[SerializeField]
	private SceneManage.SceneName nextSceneName;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}




	/// <summary>
	/// ボタンがクリックされたときの処理
	/// </summary>
	public void ClickChangeScene()
	{
		FadeManager.Instance.LoadLevel (SceneManage.SceneName.EYECHECK, 2.0f, false);
		//SceneManage.Instance.SetNewScene (SceneManage.SceneName.EYECHECK, false);
	}
}
