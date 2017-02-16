using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

	//Stageを全て格納しておく
	[SerializeField]
	List<GameObject> stage =new List<GameObject>();
	//現在のステージ番号
	int CurrentStageNum=0;
	int Max_StageNum=4;
	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < Max_StageNum; i++) 
		{
			stage.Add (GameObject.Find ("/GameManager/MapManager/Stage_" + i));
			//はじめにステージを描画しないようにしておく
			ChangeStageActive(i,false);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int GetCurrentStageNum()
	{
		return CurrentStageNum;
	}

	public void ChangeStageActive(int stageNum,bool isActive)
	{
		if (stage [stageNum].active == isActive) {
			return;
		}
		stage [stageNum].SetActive (isActive);
	}
}
