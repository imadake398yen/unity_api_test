using UnityEngine;
using System.Collections;
using LitJson;

public class Connection : MonoBehaviour {

	public UserData userData = new UserData();
	public string url = "";

	// Use this for initialization
	IEnumerator Start () {
		userData.name = "falcon";
		userData.age = 23;
		string json = JsonMapper.ToJson(userData);
		var form = new WWWForm ();
		form.AddField("jsondata", json);
		var www = new WWW (url, form);
		yield return www;
		print (www.text);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
