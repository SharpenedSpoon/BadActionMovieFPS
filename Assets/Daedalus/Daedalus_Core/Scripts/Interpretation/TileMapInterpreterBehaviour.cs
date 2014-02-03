using UnityEngine;
using System.Collections;

public class TileMapInterpreterBehaviour : MapInterpreterBehaviour {
	
	public bool fillWithFloors = true;
//	public bool fillWithCeilings = true;	// This is a consequence of fillWithFloors anyway

	override public MapInterpreter Generate(){
		TileMapInterpreter interpreter = new TileMapInterpreter();
		interpreter.behaviour = this;
		return interpreter;
	}
}
