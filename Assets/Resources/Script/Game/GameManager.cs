using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	PlayerController playerCon =null;
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
	}

	void SetMainKeyItem()
	{
		if (!isOnce [0]) {
			if (m_PlayerCon.isItemGet [0]) {
				m_UIManager.SetItemMaterial (0);
				isOnce [0] = true;
			}

		}

		if (!isOnce [1]) {
			if (m_PlayerCon.isItemGet [1]) {
				m_UIManager.SetItemMaterial (1);
				isOnce [1] = true;
			}
		}

		if (!isOnce [2]) {
			if (m_PlayerCon.isItemGet [2]) {
				m_UIManager.SetItemMaterial (2);
				isOnce [2] = true;
			}
		}

		if (!isOnce [3]) {
			if (m_PlayerCon.isItemGet [3]) {
				m_UIManager.SetItemMaterial (3);
				isOnce [3] = true;
			}
		}
	}
}
