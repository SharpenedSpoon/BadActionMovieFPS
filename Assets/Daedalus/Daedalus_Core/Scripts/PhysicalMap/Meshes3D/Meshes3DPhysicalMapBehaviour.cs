using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Note: tileSize must be equal to wallSize+columnSize
public class Meshes3DPhysicalMapBehaviour : PhysicalMapBehaviour<GameObject,GameObjectChoice> {

	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public GameObjectChoice[] corridorCeilingVariations;

	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public GameObjectChoice[] roomCeilingVariations;
	
	
	public bool addCeilingToCorridors = false;
	public bool addCeilingToRooms = false;
	
	public float tileSize;
	public float columnSize;
	public float wallSize;
	public float wallHeight;
	
	
	override public void MeasureSizes(){
		this.tileSize = MeasureTileSize();
		this.columnSize = MeasureColumnSize();
		this.wallSize = MeasureWallSize();	
		this.wallHeight = MeasureWallHeight();	
	}
	
	public float MeasureTileSize(){	
		Bounds bounds;
		bounds = GetPrefab(VirtualCell.CellType.CorridorFloor).GetComponent<MeshFilter>().sharedMesh.bounds;
		return bounds.size.z;
	}
	
	public float MeasureColumnSize(){
		Bounds bounds;
		bounds = GetPrefab(VirtualCell.CellType.CorridorColumn).GetComponent<MeshFilter>().sharedMesh.bounds;
		return bounds.size.z;
	}

	public float MeasureWallSize(){
		Bounds bounds;
		bounds = GetPrefab(VirtualCell.CellType.CorridorWall).GetComponent<MeshFilter>().sharedMesh.bounds;
		return bounds.size.z;
	}
	
	public float MeasureWallHeight(){
		Bounds bounds;
		bounds = GetPrefab(VirtualCell.CellType.CorridorWall).GetComponent<MeshFilter>().sharedMesh.bounds;
		return bounds.size.y;
	}
	
	override public void CheckDefaults(){		
		
		// TODO: Instead of checking the type, we should use inheritance: there should be a PhysicalMap-MapInterpreter pair class that defines the default
		MapInterpreterBehaviour interpreterBehaviour = GetComponent<MapInterpreterBehaviour>();
		
		if (interpreterBehaviour is StandardMapInterpreterBehaviour){
			
			corridorWallVariations = CheckDefault("Prefabs/DefaultWall",corridorWallVariations);
			corridorFloorVariations = CheckDefault("Prefabs/DefaultFloor",corridorFloorVariations);
			corridorColumnVariations = CheckDefault("Prefabs/DefaultColumn",corridorColumnVariations);
			corridorCeilingVariations = CheckDefault("Prefabs/DefaultCeiling",corridorCeilingVariations);
			
			roomWallVariations = CheckDefault("Prefabs/DefaultWall",roomWallVariations);
			roomFloorVariations = CheckDefault("Prefabs/DefaultFloor",roomFloorVariations);
			roomColumnVariations = CheckDefault("Prefabs/DefaultColumn",roomColumnVariations);
			insideRoomColumnVariations = CheckDefault("Prefabs/DefaultColumn",insideRoomColumnVariations);
			roomCeilingVariations = CheckDefault("Prefabs/DefaultCeiling",roomCeilingVariations);
			
			corridorFloorUVariations = CheckDefault("Prefabs/DefaultFloor",corridorFloorUVariations);
			corridorFloorIVariations = CheckDefault("Prefabs/DefaultFloor",corridorFloorIVariations);
			corridorFloorLVariations = CheckDefault("Prefabs/DefaultFloor",corridorFloorLVariations);
			corridorFloorTVariations = CheckDefault("Prefabs/DefaultFloor",corridorFloorTVariations);
			corridorFloorXVariations = CheckDefault("Prefabs/DefaultFloor",corridorFloorXVariations);
			
			roomFloorInsideVariations = CheckDefault("Prefabs/DefaultFloor",roomFloorInsideVariations);
			roomFloorCornerVariations = CheckDefault("Prefabs/DefaultFloor",roomFloorCornerVariations);
			roomFloorBorderVariations = CheckDefault("Prefabs/DefaultFloor",roomFloorBorderVariations);

			doorVariations = CheckDefault("Prefabs/DefaultDoor",doorVariations);
			doorColumnVariations = CheckDefault("Prefabs/DefaultColumn",doorColumnVariations);
			rockVariations = CheckDefault("Prefabs/DefaultRock",rockVariations);
			
		} else if (interpreterBehaviour is TileMapInterpreterBehaviour){
			corridorWallVariations = CheckDefault("Prefabs/DefaultBlockWall",corridorWallVariations);
			corridorFloorVariations = CheckDefault("Prefabs/DefaultFloor",corridorFloorVariations);
			corridorColumnVariations = CheckDefault("Prefabs/DefaultBlockColumn",corridorColumnVariations);
			corridorCeilingVariations = CheckDefault("Prefabs/DefaultCeiling",corridorCeilingVariations);
			
			roomWallVariations = CheckDefault("Prefabs/DefaultBlockWall",roomWallVariations);
			roomFloorVariations = CheckDefault("Prefabs/DefaultFloor",roomFloorVariations);
			roomColumnVariations = CheckDefault("Prefabs/DefaultBlockColumn",roomColumnVariations);
			insideRoomColumnVariations = CheckDefault("Prefabs/DefaultBlockColumn",insideRoomColumnVariations);
			roomCeilingVariations = CheckDefault("Prefabs/DefaultCeiling",roomCeilingVariations);
			
			corridorFloorUVariations = CheckDefault("Prefabs/DefaultFloor",corridorFloorUVariations);
			corridorFloorIVariations = CheckDefault("Prefabs/DefaultFloor",corridorFloorIVariations);
			corridorFloorLVariations = CheckDefault("Prefabs/DefaultFloor",corridorFloorLVariations);
			corridorFloorTVariations = CheckDefault("Prefabs/DefaultFloor",corridorFloorTVariations);
			corridorFloorXVariations = CheckDefault("Prefabs/DefaultFloor",corridorFloorXVariations);
			
			roomFloorInsideVariations = CheckDefault("Prefabs/DefaultFloor",roomFloorInsideVariations);
			roomFloorCornerVariations = CheckDefault("Prefabs/DefaultFloor",roomFloorCornerVariations);
			roomFloorBorderVariations = CheckDefault("Prefabs/DefaultFloor",roomFloorBorderVariations);

			doorVariations = CheckDefault("Prefabs/DefaultBlockDoor",doorVariations);
			doorColumnVariations = CheckDefault("Prefabs/DefaultBlockColumn",doorColumnVariations);
			rockVariations = CheckDefault("Prefabs/DefaultBlockRock",rockVariations);			
		}
			
		if (!entrancePrefab) entrancePrefab = Resources.Load("Prefabs/DefaultEntrance",typeof(GameObject)) as GameObject;
		if (!exitPrefab) exitPrefab = Resources.Load("Prefabs/DefaultExit",typeof(GameObject)) as GameObject;
			
	}
	
	override protected GameObjectChoice[] GetVariations(VirtualCell.CellType cell_type){
		GameObjectChoice[] type_choices = null;
		switch(cell_type){
			case VirtualCell.CellType.CorridorCeiling: 	type_choices = corridorCeilingVariations;		break;
			case VirtualCell.CellType.RoomCeiling: 		type_choices = roomCeilingVariations;			break;
			default:									type_choices = base.GetVariations(cell_type);	break;
		}	
		return type_choices;
	}

	override public PhysicalMap Generate(VirtualMap map, GeneratorBehaviour generator, MapInterpreter interpreter){
		Meshes3DPhysicalMap physMap = ScriptableObject.CreateInstance<Meshes3DPhysicalMap>();
		physMap.Initialise(map,generator,interpreter);	
		physMap.behaviour = this;
		physMap.Generate();
		return physMap;
		
	}
}