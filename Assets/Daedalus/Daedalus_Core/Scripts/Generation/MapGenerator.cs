using System.Collections.Generic;
using System;
using UnityEngine;

public enum MazeGenerationAlgorithmChoice {HuntAndKill, RecursiveBacktracker}

public class MapGenerator 
{
	
	bool verbose = false;
	public VirtualMap Generate (int width, int height, bool useSeed, int seed, bool willCreateRooms)
	{
		if (useSeed)	DungeonGenerator.Random.Instance.Initialize(seed);
		else 			DungeonGenerator.Random.Instance.Initialize();
		
		// Create a new map  
		VirtualMap map = new VirtualMap(width, height);
		
		MazeGenerationAlgorithm algorithm;
		switch(GeneratorValues.algorithmChoice){
			case MazeGenerationAlgorithmChoice.HuntAndKill: algorithm = new HuntAndKillMazeGenerationAlgorithm(); break;
			case MazeGenerationAlgorithmChoice.RecursiveBacktracker: algorithm = new RecursiveBacktrackerMazeGenerationAlgorithm(); break;
			default: algorithm = new HuntAndKillMazeGenerationAlgorithm(); break;
		}
		algorithm.Start(map);
		
		Sparsify(map);
		if (verbose) Debug.Log("Sparsified!");
		
		OpenDeadEnds(map);
		if (verbose) Debug.Log("Opened dead ends!");
				
		CreateRocks(map);
		if (verbose) Debug.Log("Added rocks!");
		
		if (willCreateRooms) {
			map.createRooms();
			if (verbose) Debug.Log("Added rooms!");
			
			CreateDoors(map);
			if (verbose) Debug.Log("Added doors!");
		}
		
		if (GeneratorValues.createStartAndEnd) CreateStartAndEnd(map);
		
		return map;
	}
	
	// Sparsify the map by removing dead-end cells.
	public void Sparsify(VirtualMap map)
	{
		// Compute the number of cells to remove as a percentage of the total number of cells in the map
        int noOfDeadEndCellsToRemove = (int) Math.Floor((decimal) GeneratorValues.sparsenessModifier / 100 * (map.ActualWidth*map.ActualHeight));
 		if (verbose) Debug.Log("Sparsify: removing  " + GeneratorValues.sparsenessModifier + "% i.e. " + noOfDeadEndCellsToRemove + " out of " + map.ActualWidth*map.ActualHeight + " cells");
 
		int noOfRemovedCells = 0;
		IEnumerable<CellLocation> deads;
		while(noOfRemovedCells < noOfDeadEndCellsToRemove)
		{
			// We sweep and remove all current dead ends
			deads = map.DeadEndCellLocations;	
			int currentlyRemovedCells=0;
			foreach(CellLocation location in deads)
			{
//				Debug.Log("Dead at " + location);
//				Debug.Log(map.CalculateDeadEndCorridorDirection(location));
				map.CreateWall(location, map.CalculateDeadEndCorridorDirection(location));
				currentlyRemovedCells++;
				if(++noOfRemovedCells == noOfDeadEndCellsToRemove) break;
			}
			if(currentlyRemovedCells==0) {
//				Debug.Log("We have no more dead ends!");
				break;	// No more dead endss
			} 
//			Debug.Log("We removed a total of " + noOfRemovedCells + " cells"); 
		}
	}
	
	// Open dead ends by linking them to rooms
	public void OpenDeadEnds(VirtualMap map)
    {
//		Debug.Log("DEAD END MOD: " + GeneratorValues.openDeadEndModifier);
		if (GeneratorValues.openDeadEndModifier == 0) return;
		
		IEnumerable<CellLocation> deads = map.DeadEndCellLocations;
        foreach (CellLocation deadEnd in deads)
        {
			if (DungeonGenerator.Random.Instance.Next(1,99) < GeneratorValues.openDeadEndModifier){
                CellLocation currentLocation = deadEnd;
//				int count=0;
                do
                {
                    // Initialize the direction picker not to select the dead-end corridor direction
                    DirectionPicker directionPicker = new DirectionPicker(map,currentLocation,map.CalculateDeadEndCorridorDirection(currentLocation));
//                    Debug.Log("We have a dead and " + directionPicker);
					VirtualMap.DirectionType direction = directionPicker.GetNextDirection(map,currentLocation);
//					Debug.Log("We choose dir " + direction);
					if (direction == VirtualMap.DirectionType.None)                          
						throw new InvalidOperationException("Could not remove the dead end!");
//						Debug.Log("Cannot go that way!");
                    else
	                    // Create a corridor in the selected direction
	                    currentLocation = map.CreateCorridor(currentLocation, direction);
//					count++;
                } while (map.isDeadEnd(currentLocation) && currentLocation != deadEnd); // Stop when you intersect an existing corridor, or when you end back to the starting cell (that means we could not remove the dead end, happens with really small maps
//				Debug.Log("Dead end removed");
            }
        }
    } 
	
	public void CreateDoors(VirtualMap map)
	{
		RoomGenerator roomG = new RoomGenerator();
		foreach(VirtualRoom r in map.rooms) roomG.CreateDoors(map,r);
	}
	
	public void CreateRocks(VirtualMap map)
	{
		foreach (CellLocation c in map.visitedCells)
		{
			if(map.isRock(c)) map.cells[(int)c.x,(int)c.y].Type=VirtualCell.CellType.Rock;
		}
	}
	
	public void CreateStartAndEnd(VirtualMap map)
	{
		// Choose a walkable cell as starting point
		int index;
		List<CellLocation> walk;
		if (GeneratorValues.forceStartAndEndInRooms) walk = new List<CellLocation>(map.RoomWalkableLocations);	//  && map.rooms.Count > 0
		else walk = new List<CellLocation>(map.WalkableLocations);
			
		index = DungeonGenerator.Random.Instance.Next(0,walk.Count-1);
		if(index !=-1 && walk.Count != 0)
		{
			map.start = new CellLocation(walk[index].x,walk[index].y);

			// Choose the most distant cell from the starting point as the ending point
			int max=0;
			foreach(CellLocation l in walk)
			{
				int dis=Math.Abs(map.start.x-l.x)+Math.Abs(map.start.y-l.y);
				if(dis>max)
				{
					max=dis;
					map.end = new CellLocation(l.x,l.y);
				}
			}
		}
	}	
}
