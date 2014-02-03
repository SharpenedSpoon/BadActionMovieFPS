using UnityEngine;
using System.Collections;

public class TileMapInterpreter : MapInterpreter<TileMapInterpreterBehaviour> {
	
	override public void Initialise(VirtualMap virtual_map, PhysicalMap physical_map, GeneratorBehaviour generator){
		base.Initialise(virtual_map,physical_map,generator);
		Width = virtual_map.Width;
		Height = virtual_map.Height;	
	}
	
	override public void ReadMap (VirtualMap map)
	{
		VirtualCell[,] output_cells = new VirtualCell[Width,Height];

		VirtualCell input_cell;
		VirtualCell.CellType cell_type;
		for (int i = 0; i < Width; i++)
		{
			for (int j = 0; j < Height; j++)
			{
				cell_type = VirtualCell.CellType.None;
				input_cell = map.cells[i,j];
				if(!map.IsRemovable(new CellLocation(i,j),behaviour.drawWallCorners))
				{
					if(input_cell.IsEmpty()){			
						// Empty passages should be filled with floors
						if (CheckRoom(map,new CellLocation(i,j)))
							cell_type = VirtualCell.CellType.RoomFloor;
						else
							cell_type = VirtualCell.CellType.CorridorFloor;

					} else if (input_cell.IsNone()){
						
						// TODO: THIS SHOULD BE PART OF THE ABSTRACT INTERPRETER
						// Place columns in the intersections
						CellLocation l = input_cell.location;
						VirtualCell tmp_column_cell = new VirtualCell(false,l);
						bool createColumn = CheckColumnConversion(map,tmp_column_cell);
						
						if (createColumn){
							cell_type = tmp_column_cell.Type;

						} else{
						
							// 'None' cells should be filled with walls
	 						if(CheckRoomBorder(map,new CellLocation(i,j)))
								cell_type = VirtualCell.CellType.RoomWall;
							else if (CheckRoom(map,new CellLocation(i,j)))
								cell_type = VirtualCell.CellType.RoomFloor;
							else
								cell_type = VirtualCell.CellType.CorridorWall;
						}
					} else {
						cell_type = input_cell.Type;	
					}
					
				} else {
					if (behaviour.drawRocks) cell_type = VirtualCell.CellType.Rock;
				}

				output_cells[i,j] = new VirtualCell(input_cell.location);
				output_cells[i,j].Type = cell_type;
				output_cells[i,j].Orientation = input_cell.Orientation;
//				Debug.Log (cell_type);
			}
		}

		// We can now override the initial map
		for (int i = 0; i < Width; i++)
		{
			for (int j = 0; j < Height; j++)
			{
				map.cells[i,j].Type = output_cells[i,j].Type;
				map.cells[i,j].Orientation = output_cells[i,j].Orientation;
			}
		}

		// After having defined (and overwritten) the initial map, we do a second pass
		for (int i = 0; i < Width; i++)
		{
			for (int j = 0; j < Height; j++)
			{
				input_cell = output_cells[i,j];
				cell_type = input_cell.Type;
				MetricLocation actual_location = this.GetWorldLocation(input_cell.location);

				if (cell_type != VirtualCell.CellType.None) {

					input_cell = CheckAdvancedConversions(map,input_cell);

					if (input_cell.Type != VirtualCell.CellType.EmptyPassage) CreateObject(map,actual_location,input_cell.Type,input_cell.Orientation);


					// Fill with floors
					VirtualCell.CellType floor_cell_type;
					if (behaviour.fillWithFloors ){
						if (ShouldBeFilled(cell_type)){
							if (CheckRoom(map,new CellLocation(i,j))){
								if (input_cell.IsDoor() || input_cell.IsEmpty()) {
									input_cell.Type = VirtualCell.CellType.RoomFloor;
									CheckRoomFloorConversion(map,input_cell);
								} else {
									input_cell.Type = VirtualCell.CellType.RoomFloor;
								}
								floor_cell_type = input_cell.Type;
							} else {
								if (input_cell.IsDoor() || input_cell.IsEmpty()){
									input_cell.Type = VirtualCell.CellType.CorridorFloor;
									CheckCorridorFloorConversion(map,input_cell);
								} else {
									input_cell.Type = VirtualCell.CellType.CorridorFloor;
								}
								floor_cell_type = input_cell.Type;
							}
							CreateObject(map,actual_location,floor_cell_type,input_cell.Orientation);
						}
					} else {
						// Special case: when not filling with floors AND we do not draw doors, we still need to place a floor underneath the empty passage representing the doors!
						if(input_cell.IsEmpty()){		// Decomment this if you want floors underneath doors ALWAYS: // || input_cell.IsDoor()){
							input_cell.Type = VirtualCell.CellType.CorridorFloor;
							CheckCorridorFloorConversion(map,input_cell);
							floor_cell_type = input_cell.Type;
							CreateObject(map,actual_location,floor_cell_type,input_cell.Orientation);
						}
					}

				} 
			}
		}
	}

	private bool ShouldBeFilled(VirtualCell.CellType type){
		return !(type == VirtualCell.CellType.CorridorFloor || type==VirtualCell.CellType.RoomFloor);
	}
	
	// True if this location belongs to a room's borders
	private bool CheckRoomBorder(VirtualMap map, CellLocation l)
	{
		if (map.rooms == null) return false;
		foreach (VirtualRoom r in map.rooms)
		{
			if(r.isInBorder(l))
				return true;
		}
		return false;
	}
	
	// True if this location is inside a room
	private bool CheckRoom(VirtualMap map, CellLocation l)
	{
		if (map.rooms == null) return false;
		foreach (VirtualRoom r in map.rooms)
		{
			if(r.isInRoom(l))
				return true;
		}
		return false;
	}
	
	override public MetricLocation GetWorldLocation(CellLocation l){
		MetricLocation actual_location = this.virtual_map.getActualLocation(l);
		actual_location.x *= 2;	// Double, since a tilemap is two times as big
		actual_location.y *= 2;
		return actual_location;
	}
	
}
