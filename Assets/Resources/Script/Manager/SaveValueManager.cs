using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 何かしらの値を保存して使い回すクラス
/// シーン変更で消さずに生き継いで使用する
/// </summary>
public class SaveValueManager : SingletonMonoBehaviour<SaveValueManager> {


	//視力検査の正解数を保存する
	public int correct
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

	float MaxLightDistance=10.0f;
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
		
	}
}
