using UnityEngine;
using System.Collections;

public class SaveSample : MonoBehaviour {

	const string fileName = "UserInfoData";
	public UserInfoData userInfo;

	public void Save () {
		Saver.Save<UserInfoData>(fileName, userInfo);
	}
	
	public void Update () {
		userInfo = Saver.Load<UserInfoData>(fileName);
	}
}
