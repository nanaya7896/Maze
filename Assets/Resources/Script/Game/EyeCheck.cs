using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EyeCheck : MonoBehaviour {

	/// <summary>
	/// 視力検査用のオブジェクト
	/// </summary>
	[SerializeField]
	List<GameObject> eyeCheckObj = new List<GameObject>();

	SentenceManager sentenceMgr=null;
	[SerializeField]
	SentenceManager m_SentenceMgr
	{
		get
		{
			if (sentenceMgr == null) 
			{
				sentenceMgr = GameObject.FindWithTag ("Sentence").GetComponent<SentenceManager>();
			}
			return sentenceMgr;
		}
	}
	KeyCode key;
	int eyeNumber=0;
	bool isOnce=false;
	// Use this for initialization
	void Start (){
		isOnce = false;
		//子オブジェクトを全て走査する
		foreach (Transform tra in gameObject.transform) 
		{
			eyeCheckObj.Add (tra.gameObject);

		}
		//
		for (int i = 0; i < eyeCheckObj.Count; i++)
		{
			eyeCheckObj [i].SetActive (false);
		}


	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_SentenceMgr.m_isSentenceEnd)
		{
			if (!isOnce) {
				SetEyeNumber ();
				isOnce = true;
			}
			if (Input.anyKeyDown) {
				key = GetInputKeyCode ();
				if (key == KeyCode.UpArrow || key == KeyCode.DownArrow || key == KeyCode.RightArrow || key == KeyCode.LeftArrow)
				{
					if (isCheckSuccess (eyeNumber, key)) {
						Debug.Log ("正解");
					} else {
						Debug.Log ("不正解");
					}
					//次の問題に変更
					SetEyeNumber ();
				}
			}
		}
	}

	bool isCheckSuccess(int questionNum,KeyCode InputKeyCode)
	{
		switch (questionNum) 
		{
		//上が正解のとき
		case 0:
			{
				if (InputKeyCode == KeyCode.UpArrow) {
					return true;
				}
			}
			break;
		case 1:
		//下が正解のとき
			{
				if (InputKeyCode == KeyCode.DownArrow)
				{
					return true;
				}
			}
			break;
		//右が正解のとき
		case 2:
			{
				if (InputKeyCode == KeyCode.RightArrow) 
				{
					return true;
				}
			}
			break;
		case 3:
			{
				if (InputKeyCode == KeyCode.LeftArrow) 
				{
					return true;
				}
			}
			break;
		default:
			break;
		}
		return false;
	}

	void SetEyeNumber()
	{
		eyeNumber = Random.Range (0, 4);

		for (int i = 0; i < 4; i++) {
			if (eyeNumber == i) {
				eyeCheckObj [i].SetActive (true);
			} else {
				eyeCheckObj [i].SetActive (false);
			}
		}
	}


	KeyCode GetInputKeyCode()
	{
		KeyCode keyVal = KeyCode.None;
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			keyVal = KeyCode.UpArrow;
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			keyVal =  KeyCode.DownArrow;
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			keyVal =  KeyCode.RightArrow;
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			keyVal =  KeyCode.LeftArrow;
		}

		return keyVal;
	}
}
