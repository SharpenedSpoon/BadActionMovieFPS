using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent (typeof (GeneratorBehaviour))]
public abstract class MapInterpreterBehaviour : MonoBehaviour {
	
	// Common parameters
	public bool drawRocks = false;
	public bool drawWallCorners = false;
	public bool drawDoors = false;
	public bool createColumnsInRooms = true;
	public bool randomOrientations = true;

	public bool useAdvancedFloors = true;
	
	abstract public MapInterpreter Generate();

}
