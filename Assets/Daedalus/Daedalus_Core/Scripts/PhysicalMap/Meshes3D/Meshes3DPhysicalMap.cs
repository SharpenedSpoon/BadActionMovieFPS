using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Meshes3DPhysicalMap : PhysicalMap<Meshes3DPhysicalMapBehaviour>
{
	
	override public void CreateObject(VirtualMap map, MetricLocation l, VirtualCell.CellType cell_type, VirtualMap.DirectionType orientation){
		GameObject prefab = behaviour.GetPrefab(cell_type);
//		Debug.Log (cell_type);
		GameObject go = (GameObject)GameObject.Instantiate(prefab, new Vector3(l.x*behaviour.tileSize,0,l.y*behaviour.tileSize), Quaternion.identity);
		go.name = cell_type.ToString();

		switch(orientation){
			case VirtualMap.DirectionType.West: 	break;
			case VirtualMap.DirectionType.North: 	go.transform.localEulerAngles = new Vector3(0,90,0); break;
			case VirtualMap.DirectionType.East: 	go.transform.localEulerAngles = new Vector3(0,180,0); break;
			case VirtualMap.DirectionType.South: 	go.transform.localEulerAngles = new Vector3(0,270,0); break;
		}
		
		
		if (cell_type == VirtualCell.CellType.CorridorCeiling || cell_type == VirtualCell.CellType.RoomCeiling) {
			Vector3 tmpPos = go.transform.position;
			tmpPos.y += behaviour.wallHeight;
			go.transform.position = tmpPos;
			
			Vector3 tmpRot = go.transform.localEulerAngles;
			tmpRot.x = 180;
			go.transform.localEulerAngles = tmpRot;
		}
		
		AddToMapGameObject(cell_type,go, cell_type == VirtualCell.CellType.Door);
		
		// Checking ceiling
		if (VirtualCell.IsFloor(cell_type)){
			VirtualCell.CellType ceiling_type = VirtualCell.CellType.None;
			
			if (VirtualCell.IsRoomFloor(cell_type)){
				if (behaviour.addCeilingToRooms){
					ceiling_type = VirtualCell.CellType.RoomCeiling;
				}
			} else {
				if (behaviour.addCeilingToCorridors){
					ceiling_type = VirtualCell.CellType.CorridorCeiling;	
				}	
			}

			if (ceiling_type != VirtualCell.CellType.None){
				CreateObject(map,l,ceiling_type,orientation);
			}
		}
	
	}
	
	override public Vector3 GetStartPosition(){
		return GetWorldPosition(this.virtualMap.start)*behaviour.tileSize;
	}
	
	override public Vector3 GetEndPosition(){
		return GetWorldPosition(this.virtualMap.end)*behaviour.tileSize;
	}
	
}