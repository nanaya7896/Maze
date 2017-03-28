using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
public class TimeChangeScript : MonoBehaviour {

	[SerializeField]
	private float LimitTime=999f;
	//数字を保存するリスト
	[SerializeField]
	List<Sprite> sp = new List<Sprite>();

	//時間を描画するためのSpriteRendererコンポーネントを持つGameObject
	[SerializeField]
	List<GameObject> one = new List<GameObject>();



	void Start () 
	{
		foreach(Sprite spr in Resources.LoadAll<Sprite>("Image/Number"))
		{
			sp.Add(spr);
		}

		int num = 10;
		//子オブジェクトの数だけ数字用GameObjectを取得
		for (int i = 0; i < transform.childCount;i++)
		{
			num = (int)Mathf.Pow (10, i);
			one.Add(GameObject.Find("/UI/Canvas/Time/" + num));

		}
	}

	public void TimeUpdate()
	{
		LimitTime -= Time.deltaTime;
		changeTimeSprite(LimitTime);
	}


	/// <summary>
	/// 時間用スプライトを変更するところ
	/// </summary>
	/// <param name="time">Time.</param>
	void changeTimeSprite(float time)
	{
		
		for (int i = 0; i < transform.childCount;i++) 
		{
			float val =time / Mathf.Pow (10, i);
			one [i].GetComponent<Image> ().sprite = sp [(int)val % 10];
		}
	}

	public void SetLimitTime(float val)
	{
		LimitTime = val;
	}

	/// <summary>
	/// 制限時間の現在の時間を取得
	/// </summary>
	/// <returns>The limit time.</returns>
	public int GetCurrentLimitTime()
	{
		return (int)LimitTime;
	}

	public void Reset()
	{
		LimitTime = 999.0f;
	}

	public bool isFinishTime()
	{
		return LimitTime < 0f;
	}
}
