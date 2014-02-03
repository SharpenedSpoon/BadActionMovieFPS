using System.Collections.Generic;
using UnityEngine;

public class RecursiveBacktrackerMazeGenerationAlgorithm : MazeGenerationAlgorithm{
	
	override public void Start(VirtualMap map){
//		Debug.Log("LOL");
		// Pick a random cell and start from there
		CellLocation currentLocation = map.PickRandomCellAndMarkVisited();
		
		// Pick a starting previous direction
		VirtualMap.DirectionType previousDirection = VirtualMap.DirectionType.North;
		
		List<CellLocation> previousLocations = new List<CellLocation>();
		
		// Repeat until all cells have been visited
		while(!map.AllCellsVisited)
        {
			// Get a starting direction
            DirectionPicker directionPicker = new DirectionPicker(map, currentLocation, GeneratorValues.directionChangeModifier, previousDirection);
			VirtualMap.DirectionType direction = directionPicker.GetNextDirection(map, currentLocation);
			
			if(direction != VirtualMap.DirectionType.None)
			{
				// Create a corridor in the current cell and flag it as visited
				previousLocations.Add(currentLocation);
				previousDirection = direction;
            	currentLocation = map.CreateCorridor(currentLocation, direction);
            	map.FlagCellAsVisited(currentLocation);
			}
            else
           	{
				// Backtrack
                currentLocation = previousLocations[previousLocations.Count-1];
				previousLocations.RemoveAt(previousLocations.Count-1);
				
         	}
       	}
	}
	
}