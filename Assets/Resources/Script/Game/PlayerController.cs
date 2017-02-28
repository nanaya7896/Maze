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
	bool isMove=false;
	public bool m_isMove
	{
		get
		{
			return isMove;
		}
		set
		{
			isMove = value;
		}
	}

	//ワープするまでの時間
	float time=4f;
	//プレイヤーのワープ時点でのポジション
	Vector3 playerPos;
	//ワープするときの浮遊地点
	Vector3 WarpPosition;
	//速度
	Vector3 vel;

	public int stageNum=0;
	//Playerの状態遷移
	public enum PlayerState
	{
		IDLE,
		WALK,
		JUMP,
		WARP
	}

	private readonly StateMachine<PlayerState> state = new StateMachine<PlayerState> ();
	/// <summary>
	/// プレイヤーのステート
	/// </summary>
	public PlayerState ps=PlayerState.IDLE;

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


	public Light playerLight=null;
	Light m_PlayerLight
	{
		get
		{
			if (playerLight == null) {
				//playerLight = GameObject.FindWithTag ("Light").GetComponent<Light> ();
				playerLight =GameObject.Find("Spotlight").GetComponent<Light>();
			}
			return playerLight;
		}
	}

	EffectManager effectManager=null;
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

	Rigidbody rigid=null;
	Rigidbody m_Rigid
	{
		get
		{
			if (rigid == null) {
				rigid = GetComponent<Rigidbody> ();
			}
			return rigid;
		}
	}

	ButtonText bText=null;
	ButtonText m_bText
	{
		get
		{
			if (bText == null) {
				bText = GameObject.Find ("ButtonText").GetComponent<ButtonText>();
			}
			return bText;
		}
	}


	void Awake()
	{
		state.Add (PlayerState.IDLE, IdelInit, IdelUpdate, IdelEnd);
		state.Add (PlayerState.WALK, WalkInit, WalkUpdate, WalkEnd);
		state.Add (PlayerState.JUMP, JumpInit, JumpUpdate, JumpEnd);
		state.Add (PlayerState.WARP,WarpInit,WarpUpdate,WarpEnd);
		ps = PlayerState.IDLE;

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

		/*if (WrapAction (hitTag))
		{
			hitTag = string.Empty;
			if (isWarp [stageNum]) {
				ps = PlayerState.WARP;
			}
			return;
		}*/
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
		if (m_isMove) 
		{
			if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.D)) {
				SetAnimState ("isWalk", true);
				ps = PlayerState.WALK;
			}

			Ray ray = new Ray (this.transform.position, transform.forward);
			if (Physics.Raycast (ray, out hit, 0.3f)) {
				debugHitTag = hit.collider.tag;
				if (Input.GetKeyDown (KeyCode.Return)) {
					hitTag = hit.collider.tag;
				}
			}
		}
	}

	void IdelEnd()
	{
		
	}

	void WalkInit()
	{
		m_bText.SetText ("");
	}

	RaycastHit hit;
	void WalkUpdate()
	{


		prevPos = transform.position;

		Ray ray=new Ray(this.transform.position,transform.forward);
		if (Physics.Raycast (ray, out hit, 0.1f)) 
		{
			hitTag = hit.collider.tag;
		}
		//動ける状態なら
		if (m_isMove)
		{
			PlayerMove ();

			if (prevPos == transform.position) {
				ps = PlayerState.IDLE;
			}
		}

	}

	void WalkEnd()
	{
		m_bText.SetText (" ");
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

	void WarpInit()
	{
		m_isMove = false;
		WarpPosition = new Vector3 (playerPos.x, playerPos.y + 0.5f, playerPos.z);
		m_Rigid.useGravity = false;
		time = 4f;
		m_EffectManager.CreateParticle ("Shine", transform.position);


	}

	void WarpUpdate()
	{
		transform.position = Vector3.SmoothDamp (transform.position,WarpPosition ,ref vel,3f);

		if (time < 0f) {
			ps = PlayerState.IDLE;
			return;
		}
		time -= Time.deltaTime;
	}

	void WarpEnd()
	{
		if (isWarp [stageNum]) {
			this.transform.position = targetObj [stageNum].transform.position;
		}
		m_Rigid.useGravity = true;
		m_EffectManager.AllDeleteParticle ();
		m_isMove = true;
		m_bText.SetText (" ");
	}
	//ステートここまで

	bool isAction=false;
	/// <summary>
	/// ワープするアクション
	/// </summary>
	/// <returns><c>true</c>, if action was wraped, <c>false</c> otherwise.</returns>
	/// <param name="gameObjectTag">Game object tag.</param>
	public bool WrapAction(string gameObjectTag)
	{
		
		if (Input.GetKeyDown (KeyCode.Return)) 
		{
			
				switch (gameObjectTag) {
				case "Wrap_0":
					stageNum = 0;
			//this.transform.position = targetObj [0].transform.position;
			//ps=PlayerState.WARP;
					if (!isWarp [stageNum]) {
						playerPos = this.transform.position;
					}
					isWarp [stageNum] = true;
					return true;
				case "Wrap_1":
					stageNum = 1;
			//this.transform.position = targetObj [stageNum].transform.position;
					if (!isWarp [stageNum]) {
						playerPos = this.transform.position;
					}
					isWarp [stageNum] = true;
					return true;

				case "Wrap_2":
					stageNum = 2;
			//this.transform.position = targetObj [stageNum].transform.position;
					if (!isWarp [stageNum]) {
						playerPos = this.transform.position;
					}
					isWarp [stageNum] = true;
					return true;

				case "Wrap_3":
					stageNum = 3;
			//this.transform.position = targetObj [stageNum].transform.position;
					if (!isWarp [stageNum]) {
						playerPos = this.transform.position;
					}
					isWarp [stageNum] = true;
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


	void OnTriggerStay(Collider col)
	{
		switch (col.GetComponent<Collider> ().tag) {
		case "Wrap_0":
			{
				m_bText.SetText ("PressReturn");
				m_bText.TextAlphaUpdate ();

				break;
			}
		case "Wrap_1":
			{
				m_bText.SetText ("PressReturn");
				m_bText.TextAlphaUpdate ();

				break;
			}
		case "Wrap_2":
			{
				m_bText.SetText ("PressReturn");
				m_bText.TextAlphaUpdate ();
				break;
			}
		case "Wrap_3":
			{
				m_bText.SetText ("PressReturn");
				m_bText.TextAlphaUpdate ();
				break;
			}
		case "Target_0":
			{
				m_bText.SetText ("PressReturn");
				m_bText.TextAlphaUpdate ();
				break;
			}
		case "Target_1":
			{
				m_bText.SetText ("PressReturn");
				m_bText.TextAlphaUpdate ();
				break;
			}
		case "Target_2":
			{
				m_bText.SetText ("PressReturn");
				m_bText.TextAlphaUpdate ();
				break;
			}
		case "Target_3":
			{
				m_bText.SetText ("PressReturn");
				m_bText.TextAlphaUpdate ();
				break;
			}
		case "Untagged":
			{
				m_bText.SetText ("");
				//m_bText.TextAlphaUpdate ();
				break;
			}


		}
		if (WrapAction (col.GetComponent<Collider> ().tag)) {
			if (isWarp [stageNum]) {
				ps = PlayerState.WARP;
			}
		}
	}

	void OnColliderStay(Collider col)
	{
		switch (col.GetComponent<Collider>().tag)
		{
		//スイッチに触れたとき
		case "switch":
			{
				m_bText.SetText ("PressReturn");
				m_bText.TextAlphaUpdate ();
				break;
			}
		case "Item_1":
			{
				m_bText.SetText ("PressReturn");
				m_bText.TextAlphaUpdate ();
				break;
			}
		case "Item_2":
			{
				m_bText.SetText ("PressReturn");
				m_bText.TextAlphaUpdate ();
				break;
			}
		case "Item_3":
			{
				m_bText.SetText ("PressReturn");
				m_bText.TextAlphaUpdate ();
				break;
			}
		case "Untagged":
			{
				m_bText.SetText ("");
				//m_bText.TextAlphaUpdate ();
				break;
			}
		}
	}
}
