using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCreate : MonoBehaviour {


	//Map生成に必要なPrefabを全て格納する
	[SerializeField,Header("Map生成に必要なPrefabを全て格納する")]
	List<GameObject> mapMaterial=new List<GameObject>();
	[SerializeField,Header("このゲーム攻略となるアイテム")]
	List<GameObject> keyItem =new List<GameObject>();
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
	[Header("ステージを制作する上での親となるオブジェクトを指定")]
	public GameObject _Parent;
	//FloorA 差5.8cm
	//
	float floorDir =5.8f;
	float WallDir_R =3.134f;
	float WallDir_L =2.59f;
	string stage2Matrix="115555555555555555511111166111:" +
						"116111111161111111616666161166:" +
						"161111666111661161116666161616:" +
						"161161111161111161616666161166:" +
						"111444444444444444446666166166:" +
						"666000000000000011111111161166:" +
						"000000000000000066666661111111";

	string stage3Matrix =
		"111111111111111111111111111111:" +
		"111111111111111111111111166111:" +
		"116111111161111111616666161166:" +
		"161111666111661161116666161616:" +
		"161161111161111161616666161166:" +
		"111444444444444444446666166166:" +
		"666010000100010011111111161166:" +
		"010010010001001066666661111111";

	string stage4Matrix =
		"111111111111111111111111111111111111111111111111:";

	public Transform startPosition;

	public GameObject parent;
	public int stageNum;
	// Use this for initialization
	void Start () {
		foreach (GameObject obj in Resources.LoadAll("Prefab/Item/")) {
			keyItem.Add (obj);
		}
		switch (stageNum) {
		case 0:
			{
				break;
			}
		case 1:
			{
				CreateFloor (stage2Matrix);
				break;
			}
		case 2:
			{
				CreateFloor (stage3Matrix);
				break;
			}
		case 3:
			{
				CreateFloor (stage4Matrix);
				break;
			}
		}
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
			//Debug.Log (x_map);
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
					ob = Instantiate (mapMaterial [obj], new Vector3 (startPosition.position.x+x * floorDir, 0f, startPosition.position.z+z*floorDir), Quaternion.identity);
					break;
				case (int)MaterialName.Floor_2:
					ob =Instantiate (mapMaterial [obj], new Vector3 (startPosition.position.x+x*floorDir, 0f, startPosition.position.z+z * floorDir), Quaternion.identity);
					break;
				case (int)MaterialName.Floor_3:
					ob=Instantiate (mapMaterial [obj], new Vector3 (startPosition.position.x+x * floorDir, 0f, startPosition.position.z+z * floorDir), Quaternion.identity);
					break;
				case (int)MaterialName.Wall_R:
					ob =Instantiate (mapMaterial [obj], new Vector3 (((startPosition.position.x+x *floorDir) -WallDir_R), -0.6f,startPosition.position.z+z * (floorDir)), Quaternion.identity);
					break;
				case (int)MaterialName.Wall_L:
					ob=Instantiate (mapMaterial [obj], new Vector3 (((startPosition.position.x+x * floorDir)+WallDir_L), -0.6f,startPosition.position.z +z * floorDir), Quaternion.identity);
					break;
				case (int)MaterialName.Wall_SQU:
					ob=Instantiate (mapMaterial [obj], new Vector3 (((startPosition.position.x+x * floorDir)), 0f, startPosition.position.z+z * floorDir), Quaternion.identity);
					break;
				default:
					break;
				}
				if (obj != 0) {
					ob.transform.SetParent (_Parent.transform);
					ob.layer = LayerMask.NameToLayer ("Map");
				}
			}
		}


		GameObject o = null;
		//キーアイテムを最後に設置
		switch (stageNum) {
		case 0:
			break;
		case 1:
		//	o = Instantiate (keyItem [stageNum], new Vector3 (22.36f, 17.35f, 177.7f), new Quaternion(-90f,0f,-180f,1f));
		//	o.transform.SetParent (_Parent.transform);
			break;
		case 2:
			break;
		case 3:
			break;
		}
	}
}
