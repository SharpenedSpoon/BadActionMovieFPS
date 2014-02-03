using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GeneratorBehaviour : MonoBehaviour
{
	public static int MAX_SIZE = 16;
	
	public int seed;
	public bool useSeed;
	
	public int MapWidth;
	public int MapHeight;
	
	public MazeGenerationAlgorithmChoice algorithmChoice;
	
	// These should be percents
	public int directionChangeModifier;		
	public int sparsenessModifier;
	public int openDeadEndModifier;
	
	public bool createRooms = true;
	public int minRooms;
	public int maxRooms;
	public int minRoomWidth;
	public int maxRoomWidth;
	public int minRoomHeight;
	public int maxRoomHeight;
	
	public int doorsDensityModifier;
	
	public bool createStartAndEnd = true;
	public bool forceStartAndEndInRooms = true;

	DateTime preDate;
	DateTime postDate;
	
	[SerializeField]
	GameObject rootMapGo = null;
	
	[SerializeField]
	private VirtualMap virtualMap;
	
	[SerializeField]
	public PhysicalMap physicalMap;
	
	[SerializeField]
	public PhysicalMapBehaviour physicalMapBehaviour;
	
	[SerializeField]
	public MapInterpreterBehaviour interpreterBehaviour;
	
	public bool printTimings = true;
	public void Generate ()
	{
		// Make sure we have a PhysicalMapBehaviour component
		physicalMapBehaviour = gameObject.GetComponent<PhysicalMapBehaviour>();
		if (physicalMapBehaviour == null) {
			Debug.LogError("You need to attach a PhysicalMapBehaviour to this gameObject to enable generation!",this);
			return;
		}
		
		// Make sure we have a MapInterpreterBehaviour component
		interpreterBehaviour = gameObject.GetComponent<MapInterpreterBehaviour>();
		if (interpreterBehaviour == null) {
			Debug.LogError("You need to attach a MapInterpreterBehaviour to this gameObject to enable generation!",this);
			return;
		}
		
		// Remove the existing map
		if (rootMapGo != null){
			physicalMap.CleanUp();
			DestroyImmediate (physicalMap);

			if (Application.isPlaying) Destroy(rootMapGo);
			else DestroyImmediate(rootMapGo);	
			virtualMap = null;
			physicalMap = null;

		}
		
		if (printTimings) preDate = System.DateTime.Now;
		
		ForceCommonSenseOptions();
		physicalMapBehaviour.MeasureSizes();
		SetGeneratorValues();
		
		MapGenerator mapGenerator = new MapGenerator ();
		virtualMap = mapGenerator.Generate (MapWidth, MapHeight, useSeed, seed, createRooms);
		
		MapInterpreter interpreter = interpreterBehaviour.Generate();
		
		physicalMap = physicalMapBehaviour.Generate(virtualMap,this,interpreter);

		this.rootMapGo = physicalMap.rootMapGo;
			
		if (printTimings){
			postDate = System.DateTime.Now;
			TimeSpan timeDifference = postDate.Subtract (preDate);
			Debug.Log ("Generated in " + timeDifference.TotalMilliseconds.ToString () + " ms");
		}
		
		SendMessage("DungeonGenerated",SendMessageOptions.DontRequireReceiver);
		
		
	}
	
	
	private void ForceCommonSenseOptions(){
		if (!createRooms || !createStartAndEnd) forceStartAndEndInRooms = false;
//		if (!createStartAndEnd) createPlayer = false;
		
		if (maxRooms < minRooms) maxRooms = minRooms;
		if (maxRoomHeight < minRoomHeight) maxRoomHeight = minRoomHeight;
		if (maxRoomWidth < minRoomWidth) maxRoomWidth = minRoomWidth;		
		
		this.physicalMapBehaviour.CheckDefaults();
	}
	
	private void SetGeneratorValues()
	{
		GeneratorValues.seed = seed;
		GeneratorValues.algorithmChoice = algorithmChoice;
		GeneratorValues.directionChangeModifier = directionChangeModifier;
		GeneratorValues.sparsenessModifier = sparsenessModifier;
		GeneratorValues.openDeadEndModifier = openDeadEndModifier;
		
		GeneratorValues.minRooms = minRooms;
		GeneratorValues.maxRooms = maxRooms;
		GeneratorValues.minRoomWidth = minRoomWidth;
		GeneratorValues.maxRoomWidth = maxRoomWidth;
		GeneratorValues.minRoomHeight = minRoomHeight;
		GeneratorValues.maxRoomHeight = maxRoomHeight;
		GeneratorValues.doorsDensityModifier = doorsDensityModifier;
		
		GeneratorValues.createStartAndEnd = createStartAndEnd;
		GeneratorValues.forceStartAndEndInRooms = forceStartAndEndInRooms;

	}
	
	public PhysicalMap getPhysicalMap(){
		return this.physicalMap;	
	}
	public VirtualMap getVirtualMap(){
		return this.virtualMap;	
	}
}
	
