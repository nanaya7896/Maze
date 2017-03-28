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

	/// <summary>
	/// 敵を管理するところ
	/// </summary>
	EnemyManager enemyManager =null;
	EnemyManager m_EnemyManager
	{
		get
		{
			if (enemyManager == null) {
				enemyManager = GameObject.FindWithTag ("Enemy").GetComponent<EnemyManager> ();
			}
			return enemyManager;
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

	//エフェクトをかんりするところ
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

	//時間を管理するところ
	TimeChangeScript timeManager=null;
	TimeChangeScript m_TimeManager
	{
		get {
			if (timeManager == null) {
				timeManager = GameObject.Find ("/UI/Canvas/Time").GetComponent<TimeChangeScript> ();
			}
			return timeManager;
		}
	}
	float limitTime =999f;

	enum GameState
	{
		INIT,
		PLAY,
		STOP,
		GAMEOVER,
		CLEAR
	}

	private readonly StateMachine<GameState> state = new StateMachine<GameState> ();

	void Awake()
	{
		
		state.Add (GameState.INIT, InitInit, InitUpdate, InitEnd);
		state.Add (GameState.PLAY, PlayInit, PlayUpdate, PlayEnd);
		state.Add (GameState.STOP, StopInit, StopUpdate, StopEnd);
		state.Add (GameState.GAMEOVER, GameOverInit, null, null);
		state.Add(GameState.CLEAR,ClearInit,null,null);

		state.SetState (GameState.INIT);

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
		//ステートマシンのUpdate
		state.Update ();

	}


	//================================ステートマシン======================//

	void InitInit()
	{
		m_UIManager.ResetItem ();
		//移動できない状態にする
		m_PlayerCon.m_isMove = false;
		//ポーズ画面のテクスチャの表示をオフにする
		m_UIManager.isSetchangePoseTExtureActive (false);

	}
	void InitUpdate()
	{
		state.SetState (GameState.PLAY);
	}
	void InitEnd()
	{
		//制限時間を設定する
		m_TimeManager.SetLimitTime(limitTime);
	}

	void PlayInit()
	{
		m_PlayerCon.m_isMove = true;
		AudioManager.Instance.PlayBGM ("GameBGM_1");
	}


	void PlayUpdate()
	{

		//アイテムを全て取得していたら
		if (m_UIManager.isGetAllItem ()) {
			state.SetState (GameState.CLEAR);
		} else {
			//制限時間が０になったら
			if (m_TimeManager.isFinishTime ()) {
				state.SetState (GameState.GAMEOVER);
			}
		}
		if (m_PlayerCon.GetisHit ()) {
			state.SetState (GameState.GAMEOVER);
		}
		//一時停止
		if (Input.GetKeyDown (KeyCode.Escape)) {
			state.SetState (GameState.STOP);
		}

		Debug.Log (m_PlayerCon.stageNum);
		//Debug.Log (m_PlayerCon.transform.position.z);
		if (m_PlayerCon.stageNum == 3) {
			if (m_PlayerCon.transform.position.z > 10f) {
				m_EnemyManager.EnemyMove ();
				m_EnemyManager.setisRun (true);
			}
		}

		SetMainKeyItem ();
		SetActiveMap ();
		//制限時間の更新
		m_TimeManager.TimeUpdate ();
	}

	void PlayEnd()
	{
		
	}

	void StopInit()
	{
		m_PlayerCon.m_isMove = false;
		m_UIManager.isSetchangePoseTExtureActive (true);
	}

	void StopUpdate()
	{
		
		if (Input.GetKeyDown (KeyCode.Escape)) {
			state.SetState (GameState.PLAY);
		}
	}

	void StopEnd()
	{
		m_UIManager.isSetchangePoseTExtureActive (false);
	}

	void GameOverInit()
	{
		FadeManager.Instance.LoadLevel (SceneManage.SceneName.GAMEOVER,2f,false);	
	}

	void ClearInit()
	{
		FadeManager.Instance.LoadLevel (SceneManage.SceneName.RESULT,2f,false);
	}

	//================================ここまで======================//


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

	/// <summary>
	/// 現在のステートの名前が返ってくる
	/// </summary>
	/// <returns>The current state.</returns>
	public string GetCurrentState()
	{
		return state.GetCurrentStateName ();
	}
}
