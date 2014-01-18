using System;
using System.IO;

public class FileIO {

	private static string basepath = "_MyData/";

	public static void SaveToFile(string filename, string data) {
		string filepath = basepath + filename;
		StreamWriter sr = File.CreateText(filepath);
		/*if (File.Exists(filepath)) {
			//sr = File.AppendText(filepath);
			return;
		} else {
			var sr = File.CreateText(filepath);
		}*/
		
		sr.Write(data);
		sr.Close();
	}

	public static string ReadFromFile(string filename) {
		string filepath = basepath + filename;
		if (File.Exists(filepath)) {
			var sr = File.OpenText(filepath);
			return sr.ReadToEnd();
		} else {
			return null;
		}
	}
}
