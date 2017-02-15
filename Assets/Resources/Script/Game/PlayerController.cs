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
	[SerializeField]
	Transform MainCamera;


	Vector3 prevPos;


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

	void Awake()
	{
		state.Add (PlayerState.IDEL, IdelInit, IdelUpdate, IdelEnd);
		state.Add (PlayerState.WALK, WalkInit, WalkUpdate, WalkEnd);
		state.Add (PlayerState.JUMP, JumpInit, JumpUpdate, JumpEnd);
		ps = PlayerState.IDEL;


	}
		
	void Start () {
		//MainCameraの取得
		MainCamera = GameObject.FindWithTag ("MainCamera").transform;
		CameraUpdate ();
	}

	void Update () {
		CameraUpdate ();
		//stateのアップデートをよぶ
		state.Update ();

		//ステートを変更するところ
		ChangeState(ps);
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
		
		transform.Rotate (0f, Input.GetAxis("RightAndLeft"), 0f);

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

	}

	void IdelEnd()
	{
		
	}

	void WalkInit()
	{
		
	}

	void WalkUpdate()
	{
		prevPos = transform.position;

		PlayerMove ();
		PlayerRotate ();
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


}
