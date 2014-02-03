﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// From Unity 4.3 release notes:
// Editor: PropertyDrawer attributes on members that are arrays are now applied to each element in the array rather than the array as a whole.
// This was always the intention since there is no other way to apply attributes to array elements,
// but it didn't work correctly before. Apologies for the inconvenience to anyone who relied on the unintended behavior for custom drawing of arrays.


public abstract class PhysicalMapBehaviour<T,S> : PhysicalMapBehaviour where S : GenericChoice<T>, new() where T: class {

	// Variations

	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif
	public S[] corridorWallVariations;

	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] corridorFloorVariations;
	
	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] corridorColumnVariations;

	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] roomWallVariations;
	
	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] roomFloorVariations;
	
	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] roomColumnVariations;

	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] insideRoomColumnVariations;
	
	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] doorVariations;

	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] doorColumnVariations;

	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] rockVariations;

	/*
	 * Advanced floors
	 */
	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] corridorFloorUVariations;

	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] corridorFloorIVariations;

	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] corridorFloorLVariations;
	
	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] corridorFloorTVariations;
	
	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] corridorFloorXVariations;


	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] roomFloorInsideVariations;

	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] roomFloorBorderVariations;
	
	#if UNITY_4_2
	[List(showSize = false, showListLabel = true, showElementLabels = false)]
	#endif	
	public S[] roomFloorCornerVariations;
		
	public T GetPrefab(VirtualCell.CellType cell_type){
		S[] type_choices = null;
		type_choices = GetVariations(cell_type);
		if (type_choices == null) return default(T);
		
		// Get one with a weighted random
		int tot = 0;
		foreach(S choice in type_choices){
			tot+=choice.weight;
		}
		int rnd = DungeonGenerator.Random.Instance.Next(1,tot);
//		Debug.Log(cell_type + " " + tot + " " + rnd);
		
		int current = 0;
		foreach(S choice in type_choices){
			current += choice.weight;
			if (rnd <= current) {
//				Debug.Log("YTYPE " + cell_type + " CHOICE: " + choice.choice);
				return choice.choice;
			}
		}
		return default(T);	
	}
	
	
	virtual protected S[] GetVariations(VirtualCell.CellType cell_type){
		S[] type_choices = null;
		switch(cell_type){
			case VirtualCell.CellType.CorridorWall: 	type_choices = corridorWallVariations;		break;
			case VirtualCell.CellType.CorridorFloor: 	type_choices = corridorFloorVariations;		break;
			case VirtualCell.CellType.CorridorColumn:	type_choices = corridorColumnVariations;	break;
			case VirtualCell.CellType.RoomWall: 		type_choices = roomWallVariations;			break;
			case VirtualCell.CellType.RoomFloor: 		type_choices = roomFloorVariations;			break;
			case VirtualCell.CellType.RoomColumn:		type_choices = roomColumnVariations;		break;
			case VirtualCell.CellType.InsideRoomColumn:	type_choices = insideRoomColumnVariations;	break;
			case VirtualCell.CellType.Door:				type_choices = doorVariations;				break;
			case VirtualCell.CellType.DoorColumn:		type_choices = doorColumnVariations;		break;
			case VirtualCell.CellType.Rock:				type_choices = rockVariations;				break;
			case VirtualCell.CellType.CorridorFloorU:	type_choices = corridorFloorUVariations;	break;
			case VirtualCell.CellType.CorridorFloorI:	type_choices = corridorFloorIVariations;	break;
			case VirtualCell.CellType.CorridorFloorL:	type_choices = corridorFloorLVariations;	break;
			case VirtualCell.CellType.CorridorFloorT:	type_choices = corridorFloorTVariations;	break;
			case VirtualCell.CellType.CorridorFloorX:	type_choices = corridorFloorXVariations;	break;
			case VirtualCell.CellType.RoomFloorInside:	type_choices = roomFloorInsideVariations;	break;
			case VirtualCell.CellType.RoomFloorBorder:	type_choices = roomFloorBorderVariations;	break;
			case VirtualCell.CellType.RoomFloorCorner:	type_choices = roomFloorCornerVariations;	break;
			default: Debug.LogError("No perfab for cell type " + cell_type); break;
		}	
		return type_choices;
	}
	
		
	protected S[] CheckDefault(string defaultName, S[] variations){
		if (variations == null || variations.Length == 0) {
			variations = new S[1];
			variations[0] = new S();
			variations[0].choice = GetDefault(defaultName);
			variations[0].weight = 1;
		} else {
			foreach(S v in variations) {
				if (v.choice == null) v.choice =  GetDefault(defaultName);
				if (v.weight < 1) v.weight = 1;
			}
		}
//		Debug.Log(variations);
		return variations;
	}

	virtual protected T GetDefault(string defaultName){
		T default_object = Resources.Load(defaultName,typeof(T)) as T;
		DebugUtils.Assert(default_object != null, "No default of name " + defaultName + " could be found!",this);
		return default_object;
	}
	
	
	
}
	
[RequireComponent (typeof (GeneratorBehaviour))]
public abstract class PhysicalMapBehaviour : MonoBehaviour {
	
	// Common parameters
	public bool enabledBatching = false;
	public bool createPlayer = false;
	public GameObject playerPrefab;
	public MapPlaneOrientation mapPlaneOrientation;
	
	// Entrance and exit prefab, placed on the starting and ending floor tiles
	public GameObject entrancePrefab;
	public GameObject exitPrefab;
	
	
	
	// These should be set by derived classes
	virtual public bool AutomaticBatching{get{ return false;}}			// If true, this physical map performs batching already and not further batching is required
	virtual public bool AutomaticOrientation{get{ return false;}}		// If true, this physical map cannot be orientated
	virtual public bool ForcedOrientation{get{ return false;}}			// If true, this physical map has its orientation forced
	virtual public void MeasureSizes(){}
	abstract public void CheckDefaults();
	abstract public PhysicalMap Generate(VirtualMap map, GeneratorBehaviour generator, MapInterpreter interpreter);
	
	
}
