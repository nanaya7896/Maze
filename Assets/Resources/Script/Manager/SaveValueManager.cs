using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 何かしらの値を保存して使い回すクラス
/// シーン変更で消さずに生き継いで使用する
/// </summary>
public class SaveValueManager : SingletonMonoBehaviour<SaveValueManager> {


	static int correct;
	//視力検査の正解数を保存する
	public int m_Correct
	{
		get
		{
			return correct;
		}
		set
		{
			correct = value;
		}
	}

	float MaxLightDistance=11.0f;
	//ライトの最大の長さを保存
	public float m_MaxLightDistance
	{
		get
		{ 
			return MaxLightDistance;
		}
		set
		{
			MaxLightDistance = value;
		}
	}

	public void Awake()
	{

		if(this != Instance){
			Destroy(this);
			return;
		}
		DontDestroyOnLoad (this.gameObject);


		AllReset ();
	}

	public void AddScore(int val)
	{
		correct+=val;
	}



	/// <summary>
	/// リセットしたいものを選択いてリセットをかける
	/// </summary>
	/// <param name="ResetName">Reset name.</param>
	public void AtReset(string ResetName)
	{
		
	}


	/// <summary>
	/// このクラスで使う値を全て初期値に戻す
	/// </summary>
	public void AllReset()
	{
		//正解数の初期化
		correct = 0;
		//ライトの長さを保管
		m_MaxLightDistance = 11f;
	}

	public int GetLightDistance()
	{
		//正解すればするほど短くなる
		return ((int)m_MaxLightDistance - correct);
	}
}
