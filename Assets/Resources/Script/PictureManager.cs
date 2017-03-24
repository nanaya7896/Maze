using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureManager : MonoBehaviour {

	[SerializeField]
	List<Sprite> sprites=new List<Sprite>();

	enum PictureName
	{
		IDEL =0,		//待機
		BLINK=1,		//瞬き
		SURPRISED=2,	//驚く
		ANGRY=3,		//怒る
		SAD=4,		//悲しい
		MORTIFYING=5	//悔しい
	}


	Image spr =null;
	Image m_Sprite
	{
		get
		{
			if (spr == null) {
				spr = GetComponent<Image> ();
			}
			return spr;
		}
	}

	public SentenceManager senMgr;

	// Use this for initialization
	void Start () {

	}
		
	// Update is called once per frame
	void Update () {

		if (senMgr.GetisSentenceEnd ()) {
			BlinkPicture ();
			return;
		}
		switch (senMgr.GetSentenceNumber ())
		{
		case 0:
			ChangePicture (PictureName.BLINK);
			break;
		case 1:
			ChangePicture (PictureName.MORTIFYING);
			break;
		case 2:
			ChangePicture (PictureName.SURPRISED);
			break;
		}
	}

	float time=3f;
	void BlinkPicture()
	{
		if (time < 0.5f) {
			ChangePicture (PictureName.BLINK);
		} else {
			ChangePicture (PictureName.IDEL);
		}
		time -= Time.deltaTime;
		if (time < 0f) {
			time = 3f;
		}
	}


	/// <summary>
	/// 指定した番号の絵に切り替える
	/// </summary>
	/// <param name="number">Number.</param>
	void ChangePicture(PictureName name)
	{

		int number = (int)name;
		if (m_Sprite.name == sprites [number].name) {
			return;
		}
		m_Sprite.sprite = sprites [number];
	}
}
