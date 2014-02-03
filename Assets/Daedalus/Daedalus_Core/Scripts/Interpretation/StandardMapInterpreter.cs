using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class StandardMapInterpreter : MapInterpreter<StandardMapInterpreterBehaviour> {
	
	private List<CellLocation> borders;
	
	override public void Initialise(VirtualMap virtual_map, PhysicalMap physical_map, GeneratorBehaviour generator){
		base.Initialise(virtual_map,physical_map,generator);
		Width = (virtual_map.Width - 1) / 2;
		Height = (virtual_map.Height - 1) / 2;
	}
	
	override public void ReadMap (VirtualMap map)
	{
		borders = new List<CellLocation> (map.borderCells);
		
		// Place floors with walls around
		foreach (CellLocation l in map.floorCells) {
			if (!map.IsRemovable (l,false)) {

				CreateFloor(map, l);

				CreateWall (map, l, VirtualMap.DirectionType.North);
				CreateWall (map, l, VirtualMap.DirectionType.South);
				CreateWall (map, l, VirtualMap.DirectionType.East);
				CreateWall (map, l, VirtualMap.DirectionType.West);
			} else {
				if (behaviour.drawRocks) CreateRock(map, l);	
			}
		}
		
		// TODO: THIS SHOULD BE PART OF THE ABSTRACT INTERPRETER
		// Place columns in the intersections
		foreach(CellLocation l in map.noneCells){
			
			VirtualCell cell = new VirtualCell(false,l);
			bool createColumn = CheckColumnConversion(map,cell);
			
			if (createColumn){
				MetricLocation actual_l = GetWorldLocation(l);
				CreateObject(map,actual_l,cell.Type,cell.Orientation);
			}
		}	
		
	}
	
	
	override public MetricLocation GetWorldLocation(CellLocation l){
		return this.virtual_map.getActualLocation(l);	
	}
	
	
	
	private void CreateRock(VirtualMap map, CellLocation l){
		MetricLocation actual_l = map.getActualLocation(l);
		CreateObject(map,actual_l,VirtualCell.CellType.Rock,map.getCell(l).Orientation);
	}
	
	private void CreateFloor (VirtualMap map, CellLocation l)
	{
		VirtualCell cell = CheckAdvancedConversions(map,map.getCell(l));
		MetricLocation actual_l = map.getActualLocation(l);
		CreateObject(map,actual_l,cell.Type,cell.Orientation);
	}

	private void CreateWall (VirtualMap map, CellLocation l, VirtualMap.DirectionType direction)
	{
		CellLocation target = map.getNeighbourInDirection (l, direction);
		if (borders.Contains (target)) {	
			VirtualCell cell = CheckAdvancedConversions(map,map.getCell(target));

			if (cell.Type != VirtualCell.CellType.EmptyPassage) {
//				VirtualCell.CellType cellType = map.cells [target.x, target.y].Type;
				MetricLocation c = GetWallLocation (map.getActualLocation (l), direction);
//				CreateObject(map,c,cellType,direction);
				CreateObject(map,c,cell.Type,direction);


				//TODO: apply texture: set inner and outer side, could be necessary rotate 180
			}
			borders.Remove (target);
		}
		
	}

	private MetricLocation GetWallLocation (MetricLocation tile, VirtualMap.DirectionType direction)
	{
		float x = 0;
		float y = 0;
		
		switch (direction) {
		case VirtualMap.DirectionType.North:
			{
				x = tile.x;
				y = tile.y + 0.5f;
				break;
			}
		case VirtualMap.DirectionType.South:
			{
				x = tile.x;
				y = tile.y - 0.5f;
				break;
			}
		case VirtualMap.DirectionType.East:
			{
				x = tile.x + 0.5f;
				y = tile.y;
				break;
			}
		case VirtualMap.DirectionType.West:
			{
				x = tile.x - 0.5f;
				y = tile.y;
				break;
			}
		}
		return new MetricLocation (x, y);
	}

}
