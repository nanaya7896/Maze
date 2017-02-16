using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

/// <summary>
/// Playerを制御するところ
/// </summary>
public class PlayerController : MonoBehaviour {

	[SerializeField,Header("プレイヤーの移動速度")]
	float walkSpeed=1f;
	[SerializeField,Header("プレイヤーの走る速度")]
	float runSpeed=2f;
	[SerializeField,Header("プレイヤーの速度")]
	Vector3 velocity;
	[SerializeField,Header("カメラの回転速度")]
	float cameraRotateSpeed = 1f;

	[SerializeField,Header("Player視点のカメラ")]
	Transform MainCamera;
	[SerializeField,Header("ワープで移動する場所")]
	List<GameObject> targetObj = new List<GameObject> ();
	private GameObject startObj;
	//前Fの座標を保存する
	Vector3 prevPos;
	//ぶつかったオブジェクトのタグ
	string hitTag;
	//デバッグ用ヒットタグ
	public string debugHitTag;
	//アイテムを取得したかどうかをあらわす
	[Header("アイテムを取得したかどうか")]
	public bool[] isItemGet= new bool[4];
	//ワープしたかどうかを判定
	[Header("ワープ判定に入ったかどうか")]
	public bool[] isWarp =new bool[4];
	//Playerの状態遷移
	public enum PlayerState
	{
		IDEL,
		WALK,
		JUMP
	}

	private readonly StateMachine<PlayerState> state = new StateMachine<PlayerState> ();
	/// <summary>
	/// プレイヤーのステート
	/// </summary>
	public PlayerState ps=PlayerState.IDEL;

	Animator anim =null;
	Animator m_Anim
	{
		get
		{ 
			if (anim == null) {
				anim = GetComponent<Animator> ();
			}
			return anim;
		}
	}


	Light playerLight=null;
	Light m_PlayerLight
	{
		get
		{
			if (playerLight == null) {
				playerLight = GameObject.FindWithTag ("Light").GetComponent<Light> ();
			}
			return playerLight;
		}
	}


	void Awake()
	{
		state.Add (PlayerState.IDEL, IdelInit, IdelUpdate, IdelEnd);
		state.Add (PlayerState.WALK, WalkInit, WalkUpdate, WalkEnd);
		state.Add (PlayerState.JUMP, JumpInit, JumpUpdate, JumpEnd);
		ps = PlayerState.IDEL;

		for (int i = 0; i < 4; i++) {
			targetObj.Add (GameObject.FindWithTag ("Target_"+i));
			isItemGet [i] = false;
		}

		//正解数に応じてライトのレンジを変更
		m_PlayerLight.range = SaveValueManager.Instance.GetLightDistance ();
	}
		
	void Start () {
		//MainCameraの取得
		MainCamera = GameObject.FindWithTag ("MainCamera").transform;
		CameraUpdate ();

		startObj = GameObject.FindWithTag ("StartTarget");
	}

	void Update () {
		PlayerRotate ();
		if (HitAction (hitTag)) {
			hitTag = string.Empty;
		}
		//stateのアップデートをよぶ
		state.Update ();


		//ステートを変更するところ
		ChangeState(ps);

		if (WrapAction (hitTag))
		{
			hitTag = string.Empty;
			ps = PlayerState.IDEL;

			return;
		}
	}

	/// <summary>
	/// プレイヤーの移動
	/// </summary>
	void PlayerMove()
	{
		//移動値前後を求める
		Vector3 forward = MainCamera.transform.TransformDirection (Vector3.forward);
		//移動値左右を求める
		Vector3 right = MainCamera.transform.TransformDirection (Vector3.right);
		//移動速度を求める
		velocity = Input.GetAxis ("Horizontal") * right + Input.GetAxis ("Vertical") * forward;

		float speed = walkSpeed;
		//ダッシュ判定
		if (Input.GetKey (KeyCode.LeftShift))
		{
			SetAnimState ("isRun", true);
			speed = runSpeed;
		}
		else
		{
			SetAnimState ("isRun", false);
		}
		//座標を変更
		transform.position += velocity * speed * Time.deltaTime;
	}

	/// <summary>
	/// 軸の回転
	/// </summary>
	void PlayerRotate()
	{
		
		transform.Rotate (0f, Input.GetAxis("RightAndLeft")*cameraRotateSpeed *Time.deltaTime, 0f);

	}

	/// <summary>
	/// FPS視点のカメラの更新
	/// </summary>
	void CameraUpdate()
	{
		//カメラはプレイヤーと同じ位置
		MainCamera.position = new Vector3(transform.position.x,transform.position.y +0.25f,transform.position.z);

	}

	/// <summary>
	/// Stateを変更する
	/// </summary>
	/// <param name="_ps">Ps.</param>
	void ChangeState(PlayerState _ps)
	{
		if (_ps.ToString () == state.GetCurrentStateName ()) {
			return;
		}
		state.SetState (_ps);
	}

	void SetAnimState(string changeParam , bool changeVal)
	{
		m_Anim.SetBool (changeParam, changeVal);
	}
	//以下ステートマシン
	void IdelInit()
	{
		SetAnimState ("isWalk", false);
		SetAnimState ("isRun", false);
		SetAnimState ("isJump", false);
	}

	void IdelUpdate()
	{
		
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
			SetAnimState ("isWalk", true);
			ps = PlayerState.WALK;
		}

		Ray ray=new Ray(this.transform.position,transform.forward);
		if (Physics.Raycast (ray, out hit, 0.3f)) {
			debugHitTag = hit.collider.tag;
			if (Input.GetKeyDown (KeyCode.Return)) {
				hitTag = hit.collider.tag;
			}
		}

		/*if (Input.GetKeyDown (KeyCode.Return)) {
			if (WrapAction (hitTag)) {
				ps = PlayerState.IDEL;

				return;
			}
		}*/


	}

	void IdelEnd()
	{
		
	}

	void WalkInit()
	{
		
	}

	RaycastHit hit;
	void WalkUpdate()
	{


		prevPos = transform.position;

		Ray ray=new Ray(this.transform.position,transform.forward);
		if (Physics.Raycast (ray, out hit, 0.1f)) 
		{
			if (Input.GetKeyDown (KeyCode.Return)) {
				hitTag = hit.collider.tag;
			}

		}
		/*if (Input.GetKeyDown (KeyCode.Return)) {
			if (WrapAction (hitTag)) {
				ps = PlayerState.IDEL;

				return;
			}
		}*/
		PlayerMove ();

		if (prevPos == transform.position) {
			ps = PlayerState.IDEL;
		}

	}

	void WalkEnd()
	{
	
	}

	void JumpInit()
	{
		
	}

	void JumpUpdate()
	{
		
	}

	void JumpEnd()
	{
		
	}
	//ステートここまで

	/// <summary>
	/// ワープするアクション
	/// </summary>
	/// <returns><c>true</c>, if action was wraped, <c>false</c> otherwise.</returns>
	/// <param name="gameObjectTag">Game object tag.</param>
	public bool WrapAction(string gameObjectTag)
	{
		
		switch (gameObjectTag) {
		case "Wrap_0":
			this.transform.position = targetObj [0].transform.position;
			isWarp[0] = true;
			Debug.Log ("行き先はWrap_0");
			return true;
		case "Wrap_1":
			this.transform.position = targetObj [1].transform.position;
			isWarp[1] = true;
			Debug.Log ("行き先はWrap_1");
			return true;

		case "Wrap_2":
			this.transform.position = targetObj [2].transform.position;
			isWarp[2] = true;
			Debug.Log ("行き先はWrap_2");
			return true;

		case "Wrap_3":
			this.transform.position = targetObj [3].transform.position;
			isWarp[3] = true;
			Debug.Log ("行き先はWrap_3");
			return true;

		case "Target_0":
			this.transform.position = startObj.transform.position;
			isWarp [0] = false;
			return true;
		case "Target_1":
			this.transform.position = startObj.transform.position;
			isWarp [1] = false;
			return true;

		case "Target_2":
			this.transform.position = startObj.transform.position;
			isWarp [2] = false;
			return true;

		case "Target_3":
			this.transform.position = startObj.transform.position;
			isWarp [3] = false;
			return true;

		default:
			break;
		}

		return false;
	}


	public bool HitAction(string gameObjectTag)
	{
		switch (gameObjectTag) {
		case "switch":
			{
				//if (Input.GetKeyDown (KeyCode.Return)) {
				WallAction.isKeyCheck = true;
				//}
			}
			return true;
			break;
		case "Item_0":
			{
				isItemGet [0] = true;
				return true;
				break;
			}
		case "Item_1":
			{
				isItemGet [1] = true;
				return true;
				break;
			}
		case "Item_2":
			{
				isItemGet [2] = true;
				return true;
				break;
			}
		case "Item_3":
			{
				isItemGet [3] = true;
				return true;
				break;
			}
		}

		return false;
	}



}
