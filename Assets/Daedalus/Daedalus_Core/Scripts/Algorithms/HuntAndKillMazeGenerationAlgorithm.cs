﻿
public class HuntAndKillMazeGenerationAlgorithm : MazeGenerationAlgorithm{
	override public void Start(VirtualMap map){
		// Pick a random cell and start from there
		CellLocation currentLocation = map.PickRandomCellAndMarkVisited();
		
		// Pick a previous direction
		VirtualMap.DirectionType previousDirection = VirtualMap.DirectionType.North;

		// Repeat until all cells have been visited
		while(!map.AllCellsVisited)
        {
			// Get a starting direction
            DirectionPicker directionPicker = new DirectionPicker(map, currentLocation, GeneratorValues.directionChangeModifier, previousDirection);
            
			VirtualMap.DirectionType direction = directionPicker.GetNextDirection(map, currentLocation);
			
			if(direction != VirtualMap.DirectionType.None)
			{
				// Create a corridor in the current cell and flag it as visited
            	currentLocation = map.CreateCorridor(currentLocation, direction);
            	map.FlagCellAsVisited(currentLocation);
				previousDirection = direction;
			}
			// or start from another visited cell
			// NOTE: This may be less performant!
            else
           	{
                currentLocation = map.GetRandomVisitedCell(currentLocation);
				
				// No visited cell available: proceed from a random unvisited cell
				if(currentLocation.x==-1)
				{
					currentLocation = map.PickRandomCellAndMarkVisited();
				}
         	}
       	}
	}
}