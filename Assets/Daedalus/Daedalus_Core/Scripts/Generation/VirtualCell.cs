using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;

public class VirtualCell
{
	public bool visited;
	public CellLocation location;
	
	//variables for the graph representation
	public int min_distance;
	public int index;
	public List<CellLocation> connectedCells = new List<CellLocation> ();
	
	public enum CellType
	{
		// Passage cells
		EmptyPassage,	// Empty is a passage!
		Border,
		CorridorWall,
		RoomWall,
		Door,

		// Floor cells
		None,
		Rock,	// Non-passable
		CorridorFloor,
		RoomFloor,

		// Advanced floors
		CorridorFloorU,
		CorridorFloorI,
		CorridorFloorL,
		CorridorFloorT,
		CorridorFloorX,
		RoomFloorInside,
		RoomFloorBorder,
		RoomFloorCorner,

		// Ceiling
		CorridorCeiling,
		RoomCeiling,
		
		// Column cells
		CorridorColumn,
		RoomColumn,
		InsideRoomColumn,
		DoorColumn,
		
	};
    
	private CellType type;
	private VirtualMap.DirectionType orientation = VirtualMap.DirectionType.None;
	
	public VirtualCell (bool visited, CellLocation location)
	{
		this.visited = visited;
		this.location = location;
		this.index = -1;
	}
	public VirtualCell (CellLocation location):this(false,location){	
	}
	
	public CellType Type {
		get { return type; }
		set { type = value; }
	}
	
	public VirtualMap.DirectionType Orientation {
		get { return orientation;}
		set { orientation = value;}
	}
	
	public bool isRoot (){
		return index == min_distance;
	}

	// Returns true if this cell represent a tile
	public bool IsTile(){
		return this.location.x % 2 == this.location.y % 2;
	}
	// True if this cell represents a border
	public bool IsBorder(){
		return !IsTile();
	}


	public bool isFloor(){
		return IsFloor (type);
	}
	public bool isWall(){
		return type == CellType.CorridorWall || type==CellType.RoomWall;	
	}
	public bool IsColumn(){
		return Type == CellType.CorridorColumn || type == CellType.DoorColumn || type == CellType.InsideRoomColumn || type == CellType.RoomColumn;
	}
	public bool IsInsideRoomColumn(){
		return type == CellType.InsideRoomColumn;
	}

	public bool IsCorridorWall(){
		return type == CellType.CorridorWall;	
	}
	public bool IsCorridorFloor(){
		return IsCorridorFloor(type);
	}
	public bool IsRock(){
		return type == CellType.Rock;	
	}
	public bool IsEmpty(){
		return type == CellType.EmptyPassage;	
	}
	public bool IsDoor(){
		return type == CellType.Door;	
	}
	public bool IsRoomFloor(){
		return IsRoomFloor(type);
	}
	public bool IsRoomWall(){
		return type == CellType.RoomWall;	
	}
	public bool IsNone(){
		return type == CellType.None;	
	}


	public static bool IsFloor(CellType type){
		return IsCorridorFloor(type) || IsRoomFloor(type);
	}
	public static bool IsCorridorFloor(CellType type){
		return type == CellType.CorridorFloor || type == CellType.CorridorFloorU ||  type == CellType.CorridorFloorI ||type == CellType.CorridorFloorL ||type == CellType.CorridorFloorT ||type == CellType.CorridorFloorX;	
	}
	public static bool IsRoomFloor(CellType type){
		return type == CellType.RoomFloor || type == CellType.RoomFloorBorder || type == CellType.RoomFloorCorner || type == CellType.RoomFloorInside;	
	}
}




public struct CellLocation
{
	public int x;
	public int y;
	
	public CellLocation (int x, int y)
	{
		this.x = x;
		this.y = y;
	}
	
	override public string ToString(){
		return "("+this.x+","+this.y+")";	
	}
	
	public static bool operator ==(CellLocation a, CellLocation b)
	{
		return a.x == b.x && a.y == b.y;	
	}
	
	public static bool operator !=(CellLocation a, CellLocation b){
		return !(a == b);	
	}
	
	public override bool Equals(System.Object obj)
    {
        // If parameter is null, return false.
        if (obj == null)
        {
            return false;
        }

        // If parameter cannot be cast to CellLocation, return false.
		CellLocation p;
		if (obj is CellLocation){
        	p = (CellLocation)obj;
		} else {
            return false;
        }

        // Return true if the fields match.
        return this == p;
    }

    public bool Equals(CellLocation p)
    {
        // If parameter is null, return false.
        if ((object)p == null)
        {
            return false;
        }

        // Return true if the fields match.
        return this == p;
    }

    public override int GetHashCode()
    {
        return x ^ y;
    }
	
	public bool isValid(){
		return this.x != -1 && this.y != -1;	
	}
}

public struct MetricLocation
{
	public float x;
	public float y;
	
	public MetricLocation (float x, float y)
	{
		this.x = x;
		this.y = y;
	}
}
