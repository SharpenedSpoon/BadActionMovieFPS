﻿using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class SerializeManager<T> {

	public string SerializeObject(T pObject) {
		string xmlizedString = null;
		MemoryStream memoryStream = new MemoryStream();
		XmlSerializer xs = new XmlSerializer(typeof(T));
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xs.Serialize(xmlTextWriter, pObject);
		memoryStream = (MemoryStream) xmlTextWriter.BaseStream;
		xmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
		return xmlizedString;
	}

	public object DeserializeObject(string pXmlizedString) {
		XmlSerializer xs = new XmlSerializer(typeof(T));
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
		return xs.Deserialize(memoryStream);
	}

	private string UTF8ByteArrayToString(byte[] characters) {
		UTF8Encoding encoding = new UTF8Encoding();
		string constructedString = encoding.GetString(characters);
		return constructedString;
	}

	private byte[] StringToUTF8ByteArray(string pXmlString) {
		UTF8Encoding encoding = new UTF8Encoding();
		byte[] byteArray = encoding.GetBytes(pXmlString);
		return byteArray;
	}

}
