using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DebugMode : MonoBehaviour {


	static bool isDebug = false;
	public bool debug=false;

	GameManager gManager =null;
	GameManager m_gManager
	{
		get
		{
			if (gManager == null) {
				gManager = GameObject.FindWithTag ("GameManager").GetComponent<GameManager>();
			}
			return gManager;
		}
	}

	PlayerController playerController=null;
	PlayerController m_PlayerCon
	{
		get
		{
			if (playerController == null) {
				playerController = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
			}
			return playerController;
		}
	}

	GameObject camera=null;
	GameObject m_Camera
	{
		get
		{
			if (camera == null) {
				camera = GameObject.FindWithTag ("MainCamera");
			}
			return camera;
		}
	}

	Light spotLight=null;
	Light m_SpotLight
	{
		get
		{
			if (spotLight == null) {
				spotLight = GameObject.FindWithTag ("Light").GetComponent<Light>();
			}

			return spotLight;
		}
	}


	[Header("デバッグで表記される文字の詳細設定")]
	public GUIStyle DetailStyle;

	// Use this for initialization
	void Start () 
	{
		isDebug = debug;
	}

	void OnGUI()
	{
		
		if (isDebug) 
		{
			GUI.Box (new Rect(0,0,Screen.width /3f,Screen.height),"");
			GUI.Label (new Rect (10,0 , 200, 100), "GameMode : DebugMode",DetailStyle);
			GUI.Label (new Rect (10,50 , 200, 100), "現在のシーン : "+ SceneManage.Instance.GetCurrentSceneName(),DetailStyle);
			GUI.Label (new Rect (10,100 , 200, 100), "Playerの現在のステート : "+m_PlayerCon.ps,DetailStyle);
			GUI.Label (new Rect (10,150 , 200, 100), "Playerの現在の位置 : "+m_PlayerCon.transform.position,DetailStyle);
			GUI.Label (new Rect (10,200 , 200, 100), "Playerの現在の向き : "+m_PlayerCon.transform.eulerAngles,DetailStyle);
			GUI.Label (new Rect (10,250 , 200, 100), "Playerがヒットしたオブジェクト名 : "+m_PlayerCon.hitTag,DetailStyle);
			GUI.Label (new Rect (10,300 , 200, 100), "Lightの明るさ : "+m_SpotLight.intensity,DetailStyle);
			GUI.Label (new Rect (10,350 , 200, 100), "Lightの範囲 : "+m_SpotLight.range,DetailStyle);
			GUI.Label (new Rect (10,400 , 200, 100), "Lightの色 : "+m_SpotLight.color,DetailStyle);
			/*GUI.Label (new Rect (10,500 , 200, 100), "",DetailStyle);
			GUI.Label (new Rect (10,550 , 200, 100), "",DetailStyle);
			GUI.Label (new Rect (10,600 , 200, 100), "",DetailStyle);
			GUI.Label (new Rect (10,650 , 200, 100), "",DetailStyle);
			GUI.Label (new Rect (10,700 , 200, 100), "",DetailStyle);
			GUI.Label (new Rect (10,750 , 200, 100), "",DetailStyle);*/
		}
	}


}
