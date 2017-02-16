using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {


	PlayerController playerCon =null;
	/// <summary>
	/// プレイヤー管理しているところ
	/// </summary>
	/// <value>The m player con.</value>
	PlayerController m_PlayerCon
	{
		get
		{
			if (playerCon == null) {
				playerCon = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
			}
			return playerCon;
		}
	}

	GameUIManager UIManager=null;
	/// <summary>
	/// UIを管理しているところ
	/// </summary>
	/// <value>The m user interface manager.</value>
	GameUIManager m_UIManager
	{
		get
		{
			if (UIManager == null) 
			{
				UIManager = GameObject.Find ("/UI").GetComponent<GameUIManager> ();
			}
			return UIManager;
		}
	}

	MapManager mapManager=null;
	//mapを管理しているところ
	MapManager m_MapManager
	{
		get
		{
			if (mapManager == null) {
				mapManager = GameObject.Find ("/GameManager/MapManager").GetComponent<MapManager> ();
			}
			return mapManager;
		}
	}

	EffectManager effectManager =null;
	EffectManager m_EffectManager
	{
		get
		{
			if (effectManager == null) {
				effectManager = GameObject.FindWithTag ("Effect").GetComponent<EffectManager> ();
			}
			return effectManager;
		}
	}

	bool[] isOnce = new bool[4];
	// Use this for initialization
	void Start () {


		for (int i = 0; i < 4; i++) {
			isOnce [i] = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		SetMainKeyItem ();
		SetActiveMap ();
	}

	void SetMainKeyItem()
	{
		
		if (m_PlayerCon.isItemGet [0]) {
			m_UIManager.SetItemMaterial (0);
		}

		if (m_PlayerCon.isItemGet [1]) {
			m_UIManager.SetItemMaterial (1);
		}
		
			
		if (m_PlayerCon.isItemGet [2]) {
			m_UIManager.SetItemMaterial (2);
		}
		
			
		if (m_PlayerCon.isItemGet [3]) {
			m_UIManager.SetItemMaterial (3);
		}
		
	}

	public void SetActiveMap()
	{		
		if (m_PlayerCon.isWarp [0]) {
			//m_MapManager.ChangeStageActive (0, true);
			SetAllMapActive(0);
			return;
		}
		else if (m_PlayerCon.isWarp [1]) {
			//m_MapManager.ChangeStageActive (1, true);
			SetAllMapActive(1);
			return;
		}
		else if (m_PlayerCon.isWarp [2]) {
			//m_MapManager.ChangeStageActive (2, true);
			SetAllMapActive (2);
			return;
		}
		else if (m_PlayerCon.isWarp [3]) {
			SetAllMapActive (3);
			return;
			//m_MapManager.ChangeStageActive (3, true);
		}

		SetAllMapActive (5);
	}

	void SetAllMapActive(int stagenum)
	{
		for (int i = 0; i < 4; i++) {
			if (i == stagenum) {
				m_MapManager.ChangeStageActive (i, true);
			} else {
				m_MapManager.ChangeStageActive (i, false);
			}
		}
	}
}
