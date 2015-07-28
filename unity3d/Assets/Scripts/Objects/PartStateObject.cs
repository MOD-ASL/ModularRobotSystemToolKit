using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class PartStateObject {
	
	[XmlAttribute("name")]
	public string name;
	public Vector3 position;
	public Quaternion rotation;
	
	public PartStateObject () {
		name = "";
	}
}
