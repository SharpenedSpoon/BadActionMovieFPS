using UnityEngine;
using System.Collections;

public class Sprites3DPhysicalMapBehaviour : PhysicalMapBehaviour<Texture2D,Texture2DChoice> {
	
	public int tileSize;
	public GameObject tilePrefab;
	
	override public void MeasureSizes(){
		this.tileSize = MeasureTileSize();
	}
	
	public int MeasureTileSize(){	
		Bounds bounds;
		bounds = tilePrefab.GetComponent<MeshFilter>().sharedMesh.bounds;
		return (int)bounds.size.z;
	}
	
	override public void CheckDefaults(){
		
		if (tilePrefab == null) {
			tilePrefab = Resources.Load("Prefabs/DefaultTile",typeof(GameObject)) as GameObject;
		}

		// Default assets if not specified		
		corridorWallVariations = CheckDefault("Textures/DefaultWallTexture",corridorWallVariations);
		corridorFloorVariations = CheckDefault("Textures/DefaultFloorTexture",corridorFloorVariations);
		corridorColumnVariations = CheckDefault("Textures/DefaultColumnTexture",corridorColumnVariations);
		
		corridorFloorUVariations = CheckDefault("Textures/DefaultFloorTexture",corridorFloorUVariations);
		corridorFloorIVariations = CheckDefault("Textures/DefaultFloorTexture",corridorFloorIVariations);
		corridorFloorLVariations = CheckDefault("Textures/DefaultFloorTexture",corridorFloorLVariations);
		corridorFloorTVariations = CheckDefault("Textures/DefaultFloorTexture",corridorFloorTVariations);
		corridorFloorXVariations = CheckDefault("Textures/DefaultFloorTexture",corridorFloorXVariations);


		roomWallVariations = CheckDefault("Textures/DefaultWallTexture",roomWallVariations);
		roomFloorVariations = CheckDefault("Textures/DefaultFloorTexture",roomFloorVariations);
		roomColumnVariations = CheckDefault("Textures/DefaultColumnTexture",roomColumnVariations);
		insideRoomColumnVariations = CheckDefault("Textures/DefaultColumnTexture",insideRoomColumnVariations);
		
		roomFloorInsideVariations = CheckDefault("Textures/DefaultFloorTexture",roomFloorInsideVariations);
		roomFloorCornerVariations = CheckDefault("Textures/DefaultFloorTexture",roomFloorCornerVariations);
		roomFloorBorderVariations = CheckDefault("Textures/DefaultFloorTexture",roomFloorBorderVariations);


		doorVariations = CheckDefault("Textures/DefaultDoorTexture",doorVariations);
		doorColumnVariations = CheckDefault("Textures/DefaultColumnTexture",doorColumnVariations);
		
		rockVariations = CheckDefault("Textures/DefaultRockTexture",rockVariations);
		
		if (!entrancePrefab) entrancePrefab = Resources.Load("Prefabs/DefaultEntrance",typeof(GameObject)) as GameObject;
		if (!exitPrefab) exitPrefab = Resources.Load("Prefabs/DefaultExit",typeof(GameObject)) as GameObject;
			
	}
	
	override public PhysicalMap Generate(VirtualMap map, GeneratorBehaviour generator, MapInterpreter interpreter){
		Sprites3DPhysicalMap physMap = ScriptableObject.CreateInstance<Sprites3DPhysicalMap>();
		physMap.Initialise(map,generator,interpreter);
		physMap.behaviour = this;
		physMap.Generate();
		return physMap;
	}
}