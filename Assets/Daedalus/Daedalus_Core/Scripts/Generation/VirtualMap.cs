using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class VirtualMap 
{
	private int width;	// The map Width
	private int height;	// The map Height
	public VirtualCell[,] cells;	// The cells in this map
	
	public List<VirtualRoom> rooms;
	
	public enum DirectionType { North, South, East, West, None };	// Directions
	public int nDirections{get; private set;}
	public DirectionType[] directions;

	public readonly List<CellLocation> visitedCells = new List<CellLocation>();	// Visited cells
	public readonly List<CellLocation> visitedAndBlockedCells = new List<CellLocation>(); // Visited and blocked (in all 4 directions) cells
	public readonly List<CellLocation> floorCells = new List<CellLocation>();	// Walkable cells
	public readonly List<CellLocation> borderCells = new List<CellLocation>(); 	// Walls, limits and passage cells
	public readonly List<CellLocation> noneCells = new List<CellLocation>();	// Parts between walls (where columns may be placed)
	
	public CellLocation start	=		new CellLocation(-1,-1);
	public CellLocation end		=		new CellLocation(-1,-1);
	
	public  List<CellLocation> roomCells = new List<CellLocation>();
	
//	private int connectedC=0; //numero di componeneti connnesse
//	private int indexConn=0;
//	private Stack<CellLocation> SC;
	
	// Constructor
	public VirtualMap (int width, int height)
	{
		this.width = 2*width +1;
		this.height = 2*height +1;
		cells = new VirtualCell[this.width, this.height];
		SetupDirections(new DirectionType[4]{DirectionType.North,DirectionType.East,DirectionType.South,DirectionType.West});
		Init();
		
	}
	public int Width
	{
		get {return width;}
	}
	public int Height
	{
		get {return height;}
	}
	public int ActualWidth
	{
		get {return (width-1)/2;}
	}
	public int ActualHeight
	{
		get {return (height-1)/2;}
	}
	
	private void SetupDirections(DirectionType[] _directions){
		this.directions = _directions;
		this.nDirections = this.directions.Length;
	}

	public DirectionType GetDirectionClockwise(DirectionType _dir, float delta){
		return this.directions[(int)Mathf.Repeat((int)_dir+delta,nDirections-1.0f)];
	}
	
	private void Init()
	{	
		// Initialise the virtual map with interleaved cells floors and walls, with None cells between walls
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				CellLocation location = new CellLocation(i, j);
				cells[i,j] = new VirtualCell(false,location);
				if(i%2==0)
				{
					if(j%2==0){
						cells[i,j].Type=VirtualCell.CellType.None;
						noneCells.Add(location);
					}
					else
					{
						cells[i,j].Type=VirtualCell.CellType.CorridorWall;
						borderCells.Add(location);
					}
				}
				else
				{
					if(j%2==0)
					{	
						cells[i,j].Type=VirtualCell.CellType.CorridorWall;
						borderCells.Add(location);
					}
					else
					{
						cells[i,j].Type=VirtualCell.CellType.CorridorFloor;
						floorCells.Add(location);
					}
				}
			}
		}
	}
	
	
	public CellLocation[] getAllNeighbours(CellLocation location){
		CellLocation[] neighs = new CellLocation[4];
		neighs[0] = getNeighbourInDirection(location,DirectionType.North);
		neighs[1] = getNeighbourInDirection(location,DirectionType.East);
		neighs[2] = getNeighbourInDirection(location,DirectionType.South);
		neighs[3] = getNeighbourInDirection(location,DirectionType.West);
		return neighs;
	}
	
	
	public CellLocation[] getAllSameNeighbours(CellLocation location){
		CellLocation[] neighs = new CellLocation[4];
		neighs[0] = getNextCellIndDirection(location,DirectionType.North);
		neighs[1] = getNextCellIndDirection(location,DirectionType.East);
		neighs[2] = getNextCellIndDirection(location,DirectionType.South);
		neighs[3] = getNextCellIndDirection(location,DirectionType.West);
		return neighs;
	}
	
	// Return the next virtual cell in the given direction (for floors, these are walls. for walls, these are floors)
	public CellLocation getNeighbourInDirection(CellLocation location, DirectionType direction)
	{
		 switch(direction)
	     {
	            case DirectionType.South:
	                return new CellLocation(location.x, location.y - 1);
	            case DirectionType.West:
	                return new CellLocation(location.x -1, location.y);
	            case DirectionType.North:
	                return new CellLocation(location.x, location.y + 1);
	            case DirectionType.East:
	                return new CellLocation(location.x +1, location.y);
	            default:
	                throw new InvalidOperationException();
	       }
	}
	

	
	// Return the next virtual cell of the same type in the given direction (i.e. floor for floor, wall for wall)
	public CellLocation getNextCellIndDirection(CellLocation location, DirectionType direction)
	{
		switch(direction)
	    {
	            case DirectionType.South:
	                return new CellLocation(location.x, location.y - 2);
	            case DirectionType.West:
	                return new CellLocation(location.x -2, location.y);
	            case DirectionType.North:
	                return new CellLocation(location.x, location.y + 2);
	            case DirectionType.East:
	                return new CellLocation(location.x +2, location.y);
	            default:
	                throw new InvalidOperationException();
	     }
	}
	
	// Check if a location is outside the map bounds
    public bool LocationIsOutsideBounds(CellLocation location)
    {
        return ((location.x < 0) || (location.x >= Width) || (location.y < 0) || (location.y >= Height));
    }
	
	
	public void createRooms()
	{
		RoomGenerator roomGenerator=new RoomGenerator();
		rooms=roomGenerator.createRooms(this);
	}
	// Mark this cell as visited
	public void FlagCellAsVisited(CellLocation location)
    {
        if (LocationIsOutsideBounds(location)) throw new ArgumentException("Location is outside of Map bounds", "location");
        if (this.cells[location.x, location.y].visited) throw new ArgumentException("Location is already visited", "location");

        this.cells[location.x, location.y].visited = true;
        visitedCells.Add(location);
    }
	//add a room cell to the map
	public void addRoomCell(CellLocation l)
	{
		roomCells.Add(l);
		this.cells[l.x,l.y].Type=VirtualCell.CellType.RoomFloor;
	}
	
	// Did we process all the cells in the map yet?
	public bool AllCellsVisited
    {
        get { return visitedCells.Count == ((Width -1)/2)*((Height-1)/2); }
    }
	
	// Pick a random cell in the map and mark it as Visited
	public CellLocation PickRandomCellAndMarkVisited ()
	{	
		VirtualCell currentCell;
		
		List<CellLocation> locations=new List<CellLocation>(floorCells);
		
		foreach (CellLocation l in visitedCells)
			locations.Remove(l);
		
		int index= DungeonGenerator.Random.Instance.Next(0,locations.Count-1);
		currentCell=cells[locations[index].x,locations[index].y];
		
		currentCell.visited = true;
		visitedCells.Add(currentCell.location);
		
		return currentCell.location;
	}
	// Does this cell have any neighbour in a certain direction?
	public bool HasAdjacentCellInDirection (CellLocation location, DirectionType direction)
	{
		if (LocationIsOutsideBounds(location)) return false;
		CellLocation l = getNextCellIndDirection(location, direction);
		return !LocationIsOutsideBounds(l);
	}
	// Is this cell's neighbour marked as Visited?
	public bool AdjacentCellInDirectionIsVisited(CellLocation location, DirectionType direction)
    {
        if (HasAdjacentCellInDirection(location, direction))
		{
	        switch(direction)
	        {
	            case DirectionType.South:
	                return this.cells[location.x, location.y - 2].visited;
	            case DirectionType.West:
	                return this.cells[location.x-2, location.y].visited;
	            case DirectionType.North:
	                return this.cells[location.x, location.y + 2].visited;
	            case DirectionType.East:
	                return this.cells[location.x + 2, location.y].visited;
	            default:
	                throw new InvalidOperationException();
	        }
		}
		
		return false;
    }
	// Get a random visited cell
	public CellLocation GetRandomVisitedCell(CellLocation location)
    {
		List<CellLocation> tempCells = new List<CellLocation>(visitedCells);
		
		//tempCells.Remove(location);
		
		// NOTE: when does visistedAndBlockedCells get populated???
		foreach (CellLocation l in visitedAndBlockedCells)
		{
			tempCells.Remove(l);
		}
	    foreach (CellLocation l in roomCells)
		{
			tempCells.Remove(l);
		}
		
        if (tempCells.Count == 0) return new CellLocation(-1,-1);
		
        int index = DungeonGenerator.Random.Instance.Next(0, tempCells.Count -1);

        return tempCells[index];   
    }
	
	// Create a corridor between one cell and another, digging the passage in-between
	public CellLocation CreateCorridor(CellLocation location, VirtualMap.DirectionType direction)
    {
		VirtualCell cell;
        CellLocation target_location = GetTargetLocation(location, direction);
		cell = this.getCell(target_location);
		cell.Orientation = direction;

		CellLocation connection_location = getNeighbourInDirection(location, direction);
		cell = this.getCell(connection_location);
		cell.Type = VirtualCell.CellType.EmptyPassage;
//		cell.Orientation = direction;		// Setting this would just change randomly the direction of some walls in tilemaps. This is removed for now!

		return target_location;
    }
	// Get target location from a valid cell/direction
	public CellLocation GetTargetLocation(CellLocation location, VirtualMap.DirectionType direction)
    {
        if (!HasAdjacentCellInDirection(location, direction)) 
			return new CellLocation(-1,-1);
		else
			return getNextCellIndDirection(location, direction);
        
    }
	
	// A dead end cell has one and only one direction free for walking (i.e. there is no wall there -> empty)
	public bool isDeadEnd(CellLocation l)
	{
		int emptyCount = 0;
		
		CellLocation[] locs = getAllNeighbours(l);
		foreach(CellLocation n_l in locs){
			if (this.cells[n_l.x,n_l.y].Type == VirtualCell.CellType.EmptyPassage) {
				emptyCount++;
//				Debug.Log("For loc " + l + " neigh " + n_l + " is empty!");	
			}
		}
		
		return emptyCount == 1;	
	}
	
	// A rock is an unreachable place surrounded by walls
	public bool isRock(CellLocation l)
	{
		int emptyCount = 0;
		
		CellLocation[] locs = getAllNeighbours(l);
		foreach(CellLocation n_l in locs){
			if (this.cells[n_l.x,n_l.y].Type == VirtualCell.CellType.EmptyPassage) {
				emptyCount++;
//				Debug.Log("For loc " + l + " neigh " + n_l + " is empty!");	
			}
		}
        
		if(emptyCount==0 && this.cells[l.x,l.y].connectedCells.Count>0){
//			Debug.Log ("Not a rock, just an isolated floor cell!");
			return false;
		}
		return emptyCount == 0;
	}
	
	public bool isFloor(CellLocation l){
		return this.getCell(l).isFloor();
	}
	
	public bool isRoomFloor(CellLocation l){
		return this.getCell(l).Type == VirtualCell.CellType.RoomFloor;	
	}
	
	public bool HasAdjacentFloor(CellLocation l){
		CellLocation[] locs = getAllSameNeighbours(l);
		foreach(CellLocation n_l in locs){
//			Debug.Log(n_l);
			if (!LocationIsOutsideBounds(n_l) && getCell(n_l).isFloor()) {
				return true;
			} 
		} 
		return false;
	}
	
	
	public VirtualCell getCell(CellLocation l){
		return this.cells[l.x,l.y];	
	}
	
	
	public IEnumerable<CellLocation> DeadEndCellLocations
    {
        get
		{
			//NOTE: This creates an enumerator, so that if a location becomes a dead end during the following removal it will be updated automatically (if following the order of the grid!)
			foreach(CellLocation l in floorCells)
                if (isDeadEnd(l)) {
					//Debug.Log("Location " + l + " is a dead end!");
					yield return new CellLocation(l.x, l.y);
				}
        }
    }
	
	
	public IEnumerable<CellLocation> WalkableLocations
    {
        get
		{
            foreach(CellLocation l in floorCells)	// Floor cells may also be rocks!
                if (cells[l.x,l.y].isFloor()) yield return new CellLocation(l.x, l.y);
        }
    }
	public IEnumerable<CellLocation> RoomWalkableLocations {
		get
		{
			foreach(CellLocation l in roomCells) yield return new CellLocation(l.x, l.y);
		}
	}
	
	public DirectionType CalculateDeadEndCorridorDirection(CellLocation location)
    {
	    if (!isDeadEnd(location)) throw new InvalidOperationException();
	
	    if (this.cells[location.x, location.y-1].Type == VirtualCell.CellType.EmptyPassage) return DirectionType.South;
	    if (this.cells[location.x, location.y+1].Type == VirtualCell.CellType.EmptyPassage) return DirectionType.North;
	    if (this.cells[location.x-1, location.y].Type == VirtualCell.CellType.EmptyPassage) return DirectionType.West;
	    if (this.cells[location.x+1, location.y].Type == VirtualCell.CellType.EmptyPassage) return DirectionType.East;
	
	    throw new InvalidOperationException();
   }
	public void CreateWall(CellLocation location, DirectionType direction)
    {
		CellLocation connection=getNeighbourInDirection(location, direction);
		
		if(!(this.cells[connection.x,connection.y].Type==VirtualCell.CellType.RoomWall))
		 this.cells[connection.x,connection.y].Type=VirtualCell.CellType.CorridorWall;
				
		// Remove the cell from his father
		CellLocation target = GetTargetLocation(location, direction);		
		this.cells[target.x,target.y].connectedCells.Remove(location);
		this.cells[location.x,location.y].connectedCells.Remove(target);

    }
	//is the cell in that direction a Rock?
	public bool AdjacentCellInDirectionIsRock(CellLocation location, DirectionType direction)
    {
        if (HasAdjacentCellInDirection(location, direction))
		{
			CellLocation l = getNextCellIndDirection(location, direction);
			return isRock(l);
		}
		return true;
    }

	// Check if two locations are the same
	public bool CompareLocations (CellLocation location1, CellLocation location2)
	{
		return (location1.x == location2.x && location1.y == location2.y);
	}
	
	
//	public void makeRock(CellLocation location)
//	{
//		if(HasAdjacentCellInDirection(location,DirectionType.North))
//		{
//			CreateWall(location,DirectionType.North);
//		}
//		
//		if(HasAdjacentCellInDirection(location,DirectionType.South))
//		{
//			CreateWall(location,DirectionType.South);
//		}
//		
//		if(HasAdjacentCellInDirection(location,DirectionType.East))
//		{
//			CreateWall(location,DirectionType.East);
//		}
//		
//		if(HasAdjacentCellInDirection(location,DirectionType.West))
//		{
//			CreateWall(location,DirectionType.West);
//		}
//		this.cells[location.x,location.y].Type=VirtualCell.CellType.Rock;
//		
//		if(this.cells[location.x,location.y].connectedCells.Count>0)
//			Debug.Log ("Failed assert rock");
//		
//	}
	
	// Can we put a door around this cell?
    public bool IsDoorable(CellLocation l)
	{
//			Debug.Log("Is " + l + " doorable?");
			// Cannot already have a door here
//			if ((!LocationIsOutsideBounds(new Location(l.x-1,l.y)) && this.cells[l.x-1,l.y].Type == VirtualCell.CellType.Door) || 
//			(!LocationIsOutsideBounds(new Location(l.x+1,l.y)) && this.cells[l.x+1,l.y].Type == VirtualCell.CellType.Door)  || 
//			(!LocationIsOutsideBounds(new Location(l.x,l.y-1)) &&this.cells[l.x,l.y-1].Type == VirtualCell.CellType.Door)  || 
//			(!LocationIsOutsideBounds(new Location(l.x,l.y+1)) &&this.cells[l.x,l.y+1].Type == VirtualCell.CellType.Door) ) {
//				Debug.Log("No connections to floors here!");
//				return false;
//			}
//			else
//			{
				// Not a rock
//				Debug.Log("Is dead end: " + isDeadEnd(l));
//				Debug.Log("Is there a rock? " + isRock(l));
			return !isRock(l);
//			}
            
	}

	public bool IsInRoom(CellLocation l){
		if (rooms == null || rooms.Count == 0) return false;
		foreach(VirtualRoom room in rooms){
			if (room.isInRoom(l)){
				return true;
			}
		}
		return false;
	}

	// Returns true if this cell can be removed from the map (i.e. not shown)
	public bool IsRemovable(CellLocation l, bool drawCorners)
	{
		VirtualCell cell = this.getCell(l);
		if(cell.isWall() || cell.IsNone() || cell.IsRock()){// || cell.IsColumn()){	// May be removed
			int validNeigh = 0;
			int wallCount = 0;
			int emptyCount = 0;
			
			CellLocation n;
			foreach(DirectionType dir in directions){
				n = getNeighbourInDirection(l,dir);
				if(!LocationIsOutsideBounds(n)){
					validNeigh++;
					VirtualCell neigh_cell = getCell(n);
//					if (l.x == 3 && l.y == 0) Debug.Log (neigh_cell.Type);
					if (neigh_cell.isWall()) wallCount++;
					else if (neigh_cell.IsNone() || neigh_cell.IsRock()) emptyCount++; 
				}
			}    
			// Show corners. Note that only None cells can be corners (surrounded by walls)
			if (drawCorners){
//				Debug.Log ("Cell " + l + " " +wallCount + " , " + emptyCount + " , " + validNeigh);
				if (cell.IsNone() && wallCount == validNeigh) {	
					// At least one neigh need not be removable as well for this to be a corner (and not an isolated None cell)
					foreach(DirectionType dir in directions){
						n = getNeighbourInDirection(l,dir);
						bool neighRemovable = true;
						if(!LocationIsOutsideBounds(n)) neighRemovable =  IsRemovable(n,drawCorners);
						if (!neighRemovable) return false;
					}
					return true;
				} 
			}
			return wallCount + emptyCount == validNeigh;
		}
		return false;
	}
	public MetricLocation getActualLocation(CellLocation l)
	{
		return new MetricLocation(l.x/2.0f,l.y/2.0f);
	}
//	public Location getActualLocation(CellLocation l)
//	{
//		return new Location(l.x/2 -1,l.y/2 -1);
//	}
	public void connectCells(CellLocation s, CellLocation e)
	{
//		Debug.Log("Connecting cell " + s + " and " + e);
		this.cells[s.x,s.y].connectedCells.Add(e);
		this.cells[e.x,e.y].connectedCells.Add(s);
	}
	public void disconnectCells(CellLocation s, CellLocation e)
	{
		this.cells[s.x,s.y].connectedCells.Remove(e);
		this.cells[e.x,e.y].connectedCells.Remove(s);
	}
	
	// Traverse connected cells to check whether there is connectivity between one cell and another
	// TODO: THIS WILL BE SLOW!!!! We are not using it!
//	public bool checkConnectivity()
//	{
//		indexConn=0;
//	    SC= new Stack<CellLocation>();
//		
//		foreach (CellLocation l in floorCells)
//		{
//			 if(cells[l.x,l.y].isFloor())
//			 {
//				if(cells[l.x,l.y].index==-1)
//					Tarjan(l);
//			 }
//		}
//		Debug.Log ("Componenti connesse: "+connectedC);
//		return connectedC==1;
//			
//	}
//	private void Tarjan(CellLocation l)
//	{
//		cells[l.x,l.y].index=indexConn;
//		cells[l.x,l.y].min_distance=indexConn;
//		indexConn++;
//		
//		SC.Push(new CellLocation(l.x,l.y));
//		foreach (CellLocation t in cells[l.x,l.y].connectedCells)
//		{
//			
//			if(cells[t.x,t.y].index==-1)
//			{
//				Tarjan(t);
//				cells[l.x,l.y].min_distance=Min(cells[l.x,l.y].min_distance,cells[t.x,t.y].min_distance);
//			}
//			else if(SC.Contains(t))
//				cells[l.x,l.y].min_distance=Min(cells[l.x,l.y].min_distance,cells[t.x,t.y].index);
//				
//		}
//		if(cells[l.x,l.y].isRoot())
//		{
//			connectedC++;
//			int count=0;
//			CellLocation h;
//			do
//			{
//				h=SC.Pop();
//				count++;
//			}while( h.x != l.x && h.y!=l.y);
//			
//		}
//	}
	private int Min(int a, int b)
	{
		return (a<=b)?a:b;
			
	}
	
	
	public void Print(){
		Debug.Log(width + "X" + height);
		string s ="";
		for (int i = 0; i < this.width; i++){
			for (int j = 0; j < this.height; j++){	
				if (cells[i,j].isFloor()){
					s+="o";
				} else if (cells[i,j].isWall()){
					s+="|";	
				} else {
					s+="x";
				}
			}
			s +="\n";
		}
		Debug.Log(s);
	}
}

