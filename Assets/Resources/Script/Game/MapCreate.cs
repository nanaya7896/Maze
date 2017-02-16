using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCreate : MonoBehaviour {

	//Map生成に必要なPrefabを全て格納する
	[SerializeField]
	List<GameObject> mapMaterial=new List<GameObject>();

	//Map生成用のマテリアルの名前をつける(番号でふりわける)
	enum MaterialName
	{
		None =0,
		Floor_1=1,
		Floor_2=2,
		Floor_3=3,
		Wall_R =4,
		Wall_L=5,
		Wall_SQU =6
	};
	public GameObject _Parent;
	//FloorA 差5.8cm
	float floorDir =5.8f;
	float WallDir_R =3.134f;
	float WallDir_L =2.59f;
	string mapMatrix=	"55555555555555555555:" +
						"11611111116111111161:" +
						"16111116111116111111:" +
						"16116111116111116111:" +
						"11144444444444444444";
	float r =2.8f;


	// Use this for initialization
	void Start () {
		CreateFloor (mapMatrix);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void CreateFloor(string map_Matrix)
	{
		//：を基準にmap_matrix_arrayを配列として分割しています
		string[] map_matrix_array = map_Matrix.Split (':');



		//map_matrix_arrayの配列の数の最大数としてループ
		for (int x = 0; x < map_matrix_array.Length; x++)
		{
		//xを元に配列の要素を取り出す
			string x_map =map_matrix_array[x];
			Debug.Log (x_map);
			//１配列に格納されている文字の数でx軸をループ
			for (int z = 0; z < x_map.Length; z++) 
			{
				int obj =int.Parse(x_map.Substring(z,1));
				GameObject ob=null;
				switch (obj) 
				{
				case (int)MaterialName.None:
					Debug.Log ("空洞です");
					break;
				case (int)MaterialName.Floor_1:
					ob = Instantiate (mapMaterial [obj], new Vector3 (x * floorDir, 0f, z*floorDir), Quaternion.identity);
					break;
				case (int)MaterialName.Floor_2:
					ob =Instantiate (mapMaterial [obj], new Vector3 (x*floorDir, 0f, z * floorDir), Quaternion.identity);
					break;
				case (int)MaterialName.Floor_3:
					ob=Instantiate (mapMaterial [obj], new Vector3 (x * floorDir, 0f, z * floorDir), Quaternion.identity);
					break;
				case (int)MaterialName.Wall_R:
					ob =Instantiate (mapMaterial [obj], new Vector3 (((x *floorDir) -WallDir_R), -0.6f, z * (floorDir)), Quaternion.identity);
					break;
				case (int)MaterialName.Wall_L:
					ob=Instantiate (mapMaterial [obj], new Vector3 (((x * floorDir)+WallDir_L), -0.6f, z * floorDir), Quaternion.identity);
					break;
				case (int)MaterialName.Wall_SQU:
					ob=Instantiate (mapMaterial [obj], new Vector3 (((x * floorDir)), 0f, z * floorDir), Quaternion.identity);
					break;
				default:
					break;
				}
				ob.transform.SetParent (_Parent.transform);
			}

		}
	}
}
