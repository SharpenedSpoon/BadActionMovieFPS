using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SelectionManager : MonoBehaviour {
	
	public static Dictionary<SelectionObjectType,VirtualCell.CellType[]> selectionDict;

	public static void CreateSelectionDictionary(){
		selectionDict = new Dictionary<SelectionObjectType, VirtualCell.CellType[]>();
		
		VirtualCell.CellType[] values = System.Enum.GetValues(typeof(VirtualCell.CellType)).Cast<VirtualCell.CellType>().ToArray();
		selectionDict[SelectionObjectType.All] = new VirtualCell.CellType[values.Length];
		for (int i=0; i<values.Length; i++) selectionDict[SelectionObjectType.All][i] = values[i];
		
		
		selectionDict[SelectionObjectType.Walkable] = new VirtualCell.CellType[11]{
			VirtualCell.CellType.RoomFloor,
			VirtualCell.CellType.RoomFloorInside,
			VirtualCell.CellType.RoomFloorBorder,
			VirtualCell.CellType.RoomFloorCorner,
			VirtualCell.CellType.CorridorFloor,
			VirtualCell.CellType.CorridorFloorU,
			VirtualCell.CellType.CorridorFloorI,
			VirtualCell.CellType.CorridorFloorL,
			VirtualCell.CellType.CorridorFloorT,
			VirtualCell.CellType.CorridorFloorX,
			VirtualCell.CellType.Door};
		
		selectionDict[SelectionObjectType.Unwalkable] = new VirtualCell.CellType[7]{
			VirtualCell.CellType.Rock,
			VirtualCell.CellType.RoomWall,
			VirtualCell.CellType.CorridorWall,
			VirtualCell.CellType.CorridorColumn,
			VirtualCell.CellType.InsideRoomColumn,
			VirtualCell.CellType.RoomColumn,
			VirtualCell.CellType.DoorColumn
		};
		
		selectionDict[SelectionObjectType.Walls] = new VirtualCell.CellType[2]{VirtualCell.CellType.RoomWall,VirtualCell.CellType.CorridorWall};
		selectionDict[SelectionObjectType.Rocks] = new VirtualCell.CellType[1]{VirtualCell.CellType.Rock};
		selectionDict[SelectionObjectType.Columns] = new VirtualCell.CellType[4]{VirtualCell.CellType.CorridorColumn,VirtualCell.CellType.RoomColumn,VirtualCell.CellType.InsideRoomColumn,VirtualCell.CellType.DoorColumn};
		selectionDict[SelectionObjectType.Floors] = new VirtualCell.CellType[10]{
			VirtualCell.CellType.CorridorFloor,
			VirtualCell.CellType.RoomFloor,
			VirtualCell.CellType.CorridorFloorU,
			VirtualCell.CellType.CorridorFloorI,
			VirtualCell.CellType.CorridorFloorL,
			VirtualCell.CellType.CorridorFloorT,
			VirtualCell.CellType.CorridorFloorX,
			VirtualCell.CellType.RoomFloorInside,
			VirtualCell.CellType.RoomFloorBorder,
			VirtualCell.CellType.RoomFloorCorner
		};


		selectionDict[SelectionObjectType.Doors] = new VirtualCell.CellType[1]{VirtualCell.CellType.Door};
		selectionDict[SelectionObjectType.Rooms] = new VirtualCell.CellType[9]{
			VirtualCell.CellType.RoomWall,
			VirtualCell.CellType.RoomColumn,
			VirtualCell.CellType.RoomFloor,
			VirtualCell.CellType.RoomFloorInside,
			VirtualCell.CellType.RoomFloorBorder,
			VirtualCell.CellType.RoomFloorCorner,
			VirtualCell.CellType.RoomCeiling,
			VirtualCell.CellType.Door,
			VirtualCell.CellType.DoorColumn
		};
		selectionDict[SelectionObjectType.Corridors] = new VirtualCell.CellType[9]{
			VirtualCell.CellType.CorridorWall,
			VirtualCell.CellType.CorridorFloor,
			VirtualCell.CellType.CorridorFloorU,
			VirtualCell.CellType.CorridorFloorI,
			VirtualCell.CellType.CorridorFloorL,
			VirtualCell.CellType.CorridorFloorT,
			VirtualCell.CellType.CorridorFloorX,
			VirtualCell.CellType.CorridorColumn,
			VirtualCell.CellType.CorridorCeiling
		};
		selectionDict[SelectionObjectType.Ceilings] = new VirtualCell.CellType[2]{VirtualCell.CellType.RoomCeiling,VirtualCell.CellType.CorridorCeiling};
			
		
		selectionDict[SelectionObjectType.RoomFloors] = new VirtualCell.CellType[1]{
			VirtualCell.CellType.RoomFloor
		
		};
		selectionDict[SelectionObjectType.CorridorFloors] = new VirtualCell.CellType[6]{
			VirtualCell.CellType.CorridorFloor,
			VirtualCell.CellType.CorridorFloorU,
			VirtualCell.CellType.CorridorFloorI,
			VirtualCell.CellType.CorridorFloorL,
			VirtualCell.CellType.CorridorFloorT,
			VirtualCell.CellType.CorridorFloorX,
		};
	
		selectionDict[SelectionObjectType.RoomCeilings] = new VirtualCell.CellType[1]{VirtualCell.CellType.RoomCeiling};
		selectionDict[SelectionObjectType.CorridorCeilings] = new VirtualCell.CellType[1]{VirtualCell.CellType.CorridorCeiling};
			
		selectionDict[SelectionObjectType.RoomColumns] = new VirtualCell.CellType[1]{VirtualCell.CellType.RoomColumn};
		selectionDict[SelectionObjectType.CorridorColumns] = new VirtualCell.CellType[1]{VirtualCell.CellType.CorridorColumn};
		selectionDict[SelectionObjectType.InsideRoomColumns] = new VirtualCell.CellType[1]{VirtualCell.CellType.InsideRoomColumn};
		selectionDict[SelectionObjectType.DoorColumns] = new VirtualCell.CellType[1]{VirtualCell.CellType.DoorColumn};
	
		selectionDict[SelectionObjectType.RoomWalls] = new VirtualCell.CellType[1]{VirtualCell.CellType.RoomWall};
		selectionDict[SelectionObjectType.CorridorWalls] = new VirtualCell.CellType[1]{VirtualCell.CellType.CorridorWall};
	
	}
	
	public static VirtualCell.CellType[] GetCellTypes(SelectionObjectType selection_type){
		if (selectionDict == null) CreateSelectionDictionary();
		return selectionDict[selection_type];
	}
}
