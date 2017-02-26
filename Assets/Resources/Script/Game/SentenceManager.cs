using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SentenceManager : MonoBehaviour {

	[SerializeField,Header("文章")]
	List<string> word =new List<string>();
	[SerializeField,Header("子オブジェクトのテキストを取得")]
	List<GameObject> childObj =new List<GameObject>();
	/// <summary>
	/// 文字を送る時間
	/// </summary>
	float time;
	/// <summary>
	/// 現在のwordの番号
	/// </summary>
	int wordNumber;
	/// <summary>
	/// 今死闘している文章の番号
	/// </summary>
	int sentenceNumber;
	/// <summary>
	/// 文章が終わったかどうか
	/// </summary>
	bool isEnd=false;
	/// <summary>
	/// 表示するかどうか
	/// </summary>
	bool isActive=false;
	/// <summary>
	/// 文章全てを表示したか
	/// </summary>
	bool isSentenceEnd=false;
	public bool m_isSentenceEnd
	{
		get{ 
			return isSentenceEnd;
		}
		set{ 
			isSentenceEnd = value;
		}
	}
	/// <summary>
	/// 点滅する感覚の時間
	/// </summary>
	float blinkTime=0f;

	// Use this for initialization
	void Start () {
		//timeは0で初期化
		time = 0f;
		//文字番号は1から
		wordNumber = 0;
		//文章の番号は0から
		sentenceNumber=0;
	/*	
	 * for (int i = 0; i < 2; i++) 
		{
			childObj.Add (GameObject.FindGameObjectsWithTag ("Wrod") [i]);
		}
		*/
		childObj [1].SetActive (isActive);
		isSentenceEnd = false;
		//SubStringで最初の文字から何番目あで描画するか決定する
		//childObj [1].GetComponent<Text> ().text = word [sentenceNumber].Substring (0, wordNumber);
		//Debug.Log (childObj [1].GetComponent<Text> ().text);
	}
	
	// Update is called once per frame
	void Update () {


		if (!isSentenceEnd) {
			if (!isEnd) {
			
				time += Time.deltaTime;
				NextWord (time);
				//Debug.Log (word [0].Length);
				if (childObj [0].GetComponent<Text> ().text.Length >= word [sentenceNumber].Length) {
					isEnd = true;
					return;
				}
				DrawWord (sentenceNumber, wordNumber);
			} else {
				if (Input.GetKeyDown (KeyCode.Return)) {
					NextSentence ();
					return;
				}
				blinkTime += Time.deltaTime;
				if (blinkTime > 0.5f) {
					isActive = !isActive;
					blinkTime = 0f;
				}
			}
		}

		childObj [1].SetActive (isActive);
	}

	void NextWord(float _time)
	{
		if (_time > 0.1f) 
		{
			time = 0f;
			wordNumber++;
		}
		if (Input.GetKeyDown (KeyCode.Return)) {
			wordNumber = word [sentenceNumber].Length;
		}
	}

	/// <summary>
	/// 文字を描画します
	/// </summary>
	/// <param name="_sentenceNum">Sentence number.</param>
	/// <param name="_wordNum">Word number.</param>
	void DrawWord(int _sentenceNum , int _wordNum)
	{
		childObj [0].GetComponent<Text> ().text = word [_sentenceNum].Substring (0, _wordNum);	
	}


	/// <summary>
	/// 文章を次の文に変更します
	/// </summary>
	void NextSentence()
	{
		if (sentenceNumber < childObj.Count) {
			sentenceNumber++;
			wordNumber = 0;
			isEnd = false;
			isActive = false;
			blinkTime = 0f;
			childObj [1].SetActive (isActive);
		} else {
			m_isSentenceEnd = true;
			for (int i = 0; i < childObj.Count; i++) {
				childObj [i].SetActive (false);
				isActive = false;
			}
		}
	}


	public int GetSentenceNumber()
	{
		return sentenceNumber;
	}

	public bool GetisSentenceEnd()
	{
		return m_isSentenceEnd;
	}
}
