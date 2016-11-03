using UnityEngine;
using System;
using System.Collections;
using System.IO;
using TextEncryptor;
using LitJson;

public class Saver {

	public static bool Exists (string key) {
		string path = GetFilePath(key);
		return File.Exists(path);
	}

	public static void Delete (string key) {
		string path = GetFilePath(key);
		File.Delete(path);
	}

	public static void Save <T> (string key, T saveData) {
		string json = JsonMapper.ToJson(saveData);
		json += "[END]";
		string crypted = Crypt.Encrypt (json);
		string[] pathArray = key.Split('/');
		string path = Application.persistentDataPath;
		if (pathArray.Length > 1) {
			for (int i=0; i < pathArray.Length-1; i++) {
				path += "/" + pathArray[i];
			}
			DirectoryUtils.SafeCreateDirectory (path);	
		}
		string filePath = GetFilePath(key);
		FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
		BinaryWriter writer = new BinaryWriter (fileStream);
		writer.Write (crypted);
		writer.Close();
	}

	public static T Load <T> (string key) {
		string filePath = GetFilePath(key);
		FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
		BinaryReader reader = new BinaryReader(fileStream);

		if (reader == null) {
			fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			BinaryWriter writer = new BinaryWriter (fileStream);
			string st = Crypt.Encrypt("[END]");
			writer.Write (st);
			writer.Close();
			fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
		}

		string str = reader.ReadString();
		string decrypted = Crypt.Decrypt (str);
		decrypted = System.Text.RegularExpressions.Regex.Replace (decrypted, @"\[END\].*$", "");
		reader.Close();
		return JsonMapper.ToObject<T>(decrypted);
	}

	static string GetFilePath (string key) {
		return Application.persistentDataPath + "/" + key;
	}
	
}