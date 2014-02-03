using UnityEngine;
using System.Collections;

public abstract class MapInterpreter<T> : MapInterpreter where T:MapInterpreterBehaviour {
	public T behaviour;
	
	public void SetBehaviour(T behaviour){
		this.behaviour = behaviour;	
	}
		
	protected void CreateObject(VirtualMap map, MetricLocation l, VirtualCell.CellType type, VirtualMap.DirectionType orientation){
		if (orientation == VirtualMap.DirectionType.None && behaviour.randomOrientations)	orientation = (VirtualMap.DirectionType) DungeonGenerator.Random.Instance.Next(map.nDirections);
		physical_map.CreateObject(map,l,type,orientation);
	}


	
	public VirtualCell CheckAdvancedConversions(VirtualMap map, VirtualCell input_cell){
		VirtualCell output_cell = new VirtualCell(false,input_cell.location);
		output_cell.Type = input_cell.Type;
		output_cell.Orientation = input_cell.Orientation;
		if (output_cell.IsCorridorFloor()) CheckCorridorFloorConversion(map,output_cell);
		else if (output_cell.IsRoomFloor()) CheckRoomFloorConversion(map,output_cell);
		else if(output_cell.IsDoor()) {
			if(!behaviour.drawDoors) {
				output_cell.Type = VirtualCell.CellType.EmptyPassage;
			}
		}
		return output_cell;
	}
	
	

	
	protected void CheckRoomFloorConversion(VirtualMap map, VirtualCell cell){
		VirtualCell.CellType floor_type = cell.Type;
		CellLocation l = cell.location;
		
		if (behaviour.useAdvancedFloors){
			if (cell.IsRoomFloor()){
				CellLocation[] border_neighs;
				CellLocation[] floor_neighs;

				bool considerDoorsAsWalls = true;

				// Count how many border neighbours are non-walls 
				int countFloorNeighs = 0;
				bool[] validIndices = new bool[4];

				if (cell.IsTile()){
					// This was a tile, check neigh walls
					border_neighs = map.getAllNeighbours(l);
					for(int i=0; i<border_neighs.Length; i++){
						CellLocation other_l = border_neighs[i];
						if (!map.LocationIsOutsideBounds(other_l)  && other_l.isValid() 
						    && !(map.getCell(other_l).isWall())
						    && !(considerDoorsAsWalls && map.getCell(other_l).IsDoor())
						    ){
							countFloorNeighs++; 
							validIndices[i] = true;
						}
					}
				} else {
					// This was a border, check neigh floors instead
					floor_neighs = map.getAllNeighbours(l);
//					Debug.Log ("From " + l);
					for(int i=0; i<floor_neighs.Length; i++){
						CellLocation other_l = floor_neighs[i];
//						Debug.Log ("At " + other_l + " is " + map.getCell(other_l).Type);
						if (!map.LocationIsOutsideBounds(other_l)  && other_l.isValid() && 
						    (map.getCell(other_l).isFloor()  //|| map.getCell(other_l).IsNone()
						 	|| map.getCell(other_l).IsInsideRoomColumn ()	)// Treat inside room columns as floors here 
						    ){
							countFloorNeighs++; 
							validIndices[i] = true;
						}
					}
				}


				// Define the adbvanced floors
				//	Debug.Log (countFloorNeighs);
				if (countFloorNeighs == 2){
					// This is a room corner
					floor_type = VirtualCell.CellType.RoomFloorCorner;
					for(int i= 0; i<4; i++) {
						if (validIndices[i] && validIndices[(int)Mathf.Repeat(i+1,4)] ){
							cell.Orientation = map.directions[(int)Mathf.Repeat(i+3,4)];
							break;
						}
					}
				}
				else if (countFloorNeighs == 3) {
					floor_type = VirtualCell.CellType.RoomFloorBorder;
					for(int i= 0; i<4; i++) {
						if (validIndices[(int)Mathf.Repeat(i-1,4)] && validIndices[i] && validIndices[(int)Mathf.Repeat(i+1,4)]) {
							cell.Orientation = map.directions[(int)Mathf.Repeat(i+2,4)];
							break;
						}
					}
				}
				else if (countFloorNeighs == 4) {
					floor_type = VirtualCell.CellType.RoomFloorInside;
				}

			}
		}
		
		cell.Type = floor_type;
	}

	protected void CheckCorridorFloorConversion(VirtualMap map, VirtualCell cell){
		VirtualCell.CellType floor_type = cell.Type;
		CellLocation l = cell.location;

		if (behaviour.useAdvancedFloors){
			if (cell.IsCorridorFloor()){
				
				// Count how many border neighbours are non-walls
				int countFloorNeighs = 0;
				bool[] validIndices = new bool[4];

				if (cell.IsTile()){
					// This was a tile, check neigh walls
					CellLocation[] border_neighs = map.getAllNeighbours(l);
					for(int i=0; i<border_neighs.Length; i++){
						CellLocation other_l = border_neighs[i];
						if (!map.LocationIsOutsideBounds(other_l)  && other_l.isValid() && !(map.getCell(other_l).isWall())){
							countFloorNeighs++; 
							validIndices[i] = true;
						}
					}
				} else {
					// This was a border, check neigh floors instead
					CellLocation[] floor_neighs = map.getAllNeighbours(l);
					for(int i=0; i<floor_neighs.Length; i++){
						CellLocation other_l = floor_neighs[i];
						if (!map.LocationIsOutsideBounds(other_l)  && other_l.isValid() && map.getCell(other_l).isFloor()){
							countFloorNeighs++; 
							validIndices[i] = true;
						}
					}
				}

				// Define the adbvanced floors
				if (countFloorNeighs == 1){
					floor_type = VirtualCell.CellType.CorridorFloorU;
					for(int i= 0; i<4; i++) {
						if (validIndices[i]){
							cell.Orientation = map.directions[(int)Mathf.Repeat(i+3,4)];
							break;
						}
					}
				}
				else if (countFloorNeighs == 2){

					// Corridor I
					floor_type = VirtualCell.CellType.CorridorFloorI;
					for(int i= 0; i<4; i++) {
						if (validIndices[i]){
							cell.Orientation = map.directions[(int)Mathf.Repeat(i+1,4)];
							break;
						}
					}

					// Corridor L
					for(int i= 0; i<4; i++) {
						if (validIndices[i] && validIndices[(int)Mathf.Repeat(i+1,4)] ){
							// This and the next are valid: left turn (we consider all of them to be left turns(
							cell.Orientation = map.directions[(int)Mathf.Repeat(i+3,4)];
							floor_type = VirtualCell.CellType.CorridorFloorL;
							break;
						}
					}
				}
				else if (countFloorNeighs == 3) {
					floor_type = VirtualCell.CellType.CorridorFloorT;
					for(int i= 0; i<4; i++) {
						if (validIndices[(int)Mathf.Repeat(i-1,4)] && validIndices[i] && validIndices[(int)Mathf.Repeat(i+1,4)]) {
							// This, the one before and the next are valid: T cross (with this being the middle road)
							cell.Orientation = map.directions[(int)Mathf.Repeat(i+1,4)];
							break;
						}
					}
				}
				else if (countFloorNeighs == 4) {
					floor_type = VirtualCell.CellType.CorridorFloorX;
				}
			}
		}
		cell.Type = floor_type;
	}
	
	
	protected bool CheckColumnConversion(VirtualMap map, VirtualCell cell){
	//			VirtualMap.DirectionType wallDirection = VirtualMap.DirectionType.North;	
		VirtualCell.CellType column_type = VirtualCell.CellType.CorridorColumn;
		CellLocation l = cell.location;

		// Define inside-room columns
		bool createColumn = false;
		if (!map.IsRemovable(l,behaviour.drawWallCorners)){
			createColumn =  true;

			bool isInsideRoomColumn = true;
			bool isRoomColumn = false;

			isInsideRoomColumn = map.IsInRoom(l);	// We consider it to be an insideRoomColumn if it is in a room, at start
			foreach(VirtualMap.DirectionType dir in map.directions){
				CellLocation neigh_loc = map.getNeighbourInDirection(l,dir);
				if (!map.LocationIsOutsideBounds(neigh_loc)){
					VirtualCell neigh_cell = map.getCell(neigh_loc);
					
					if (neigh_cell.IsDoor()){
						column_type = VirtualCell.CellType.DoorColumn;
						isInsideRoomColumn = false;
	//							wallDirection = dir;
						break;
					} else if (!isRoomColumn && neigh_cell.IsCorridorWall()){
						column_type = VirtualCell.CellType.CorridorColumn;
						isInsideRoomColumn = false;
						// Do not break, as we need to check all the other walls for this
	//							wallDirection = dir;
	//							break;
					}  else if (neigh_cell.IsRoomWall()){
						column_type = VirtualCell.CellType.RoomColumn;
						isInsideRoomColumn = false;
						isRoomColumn = true;
	//							wallDirection = dir;
						// Do not break, as we need to check all the other walls to be sure
	//							break;
					}
				}
			}
			
			if (isInsideRoomColumn) {
				column_type = VirtualCell.CellType.InsideRoomColumn;
				createColumn = behaviour.createColumnsInRooms;
			}
					
		}
		
		// Override the type
		if (createColumn) {
			cell.Type = column_type;
		}
		
		return createColumn;
	}
}

public abstract class MapInterpreter {
	
	protected VirtualMap virtual_map;
	protected PhysicalMap physical_map;
	protected GeneratorBehaviour generator;
	
	public int Width;
	public int Height;
	
	virtual public void Initialise(VirtualMap virtual_map, PhysicalMap physical_map, GeneratorBehaviour generator){
		this.virtual_map = virtual_map;
		this.physical_map = physical_map;
		this.generator = generator;
	}
	abstract public void ReadMap(VirtualMap map);
	
	abstract public MetricLocation GetWorldLocation(CellLocation l);
	
	
	// This defaults to an orientation plane of XZ
	public Vector3 GetWorldPosition(CellLocation l){	
		MetricLocation world_location = GetWorldLocation(l);
		return new Vector3(world_location.x,0,world_location.y);
	}
	
}