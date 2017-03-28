using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour {

	public Text text;
	float alpha=0.01f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (text.color.a > 1.0f) {
			text.color = new Color(1f,1f,1f,1f);
			alpha *= -1.0f;
		} else if (text.color.a < 0.0f) {
			text.color = new Color(1f,1f,1f,0f);
			alpha *= -1.0f;
		}
		text.color += new Color(1f,1f,1f,alpha);	
	}
}
