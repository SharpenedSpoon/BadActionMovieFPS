﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public enum MapPlaneOrientation{XZ, XY, YZ}

[Serializable]
abstract public class PhysicalMap<T> : PhysicalMap where T :PhysicalMapBehaviour{

	public T behaviour;

	override protected void FinalizeGeneration(){
		if (behaviour.enabledBatching) {
			StaticBatchingUtility.Combine (this.staticMapGo);	
			hadCombined = true;
		} else {
			hadCombined = false;
		}

		UpdateOrientation();
		
		// Additional placements, will take care of orientation
		if (behaviour.createPlayer) PlacePlayer();		
		if (generator.createStartAndEnd) PlaceEntranceAndExit();
	}
	
	
	protected void UpdateOrientation(){
		if (behaviour.AutomaticOrientation) return;
		
		switch(behaviour.mapPlaneOrientation){
		case MapPlaneOrientation.XY: this.rootMapGo.transform.localEulerAngles = new Vector3(-90,0,0); break;
		case MapPlaneOrientation.XZ: break;
		case MapPlaneOrientation.YZ: this.rootMapGo.transform.localEulerAngles = new Vector3(0,0,-90); break;
		}
	} 
	

	override protected void BuildRootGameObjects(){
		rootMapGo = new GameObject("Map");
		rootMapGo.transform.parent = this.behaviour.gameObject.transform;
		dynamicMapGo = new GameObject("DynamicMap");
		dynamicMapGo.transform.parent = rootMapGo.transform;
		this.dynamicMapTr = dynamicMapGo.transform;
		staticMapGo = new GameObject("StaticMap");
		staticMapGo.transform.parent = rootMapGo.transform;
		this.staticMapTr = staticMapGo.transform;	
	}
	
	private void PlacePlayer(){
		if (generator.createStartAndEnd && behaviour.playerPrefab != null) {
			GameObject playerGo = GameObject.Instantiate(behaviour.playerPrefab) as GameObject;
			playerGo.transform.position = this.GetStartPosition();// + Vector3.up*2;
			playerGo.transform.parent = this.dynamicMapTr;
			playerGo.name = "Player";
			
		}
	}
	
	protected void PlaceEntranceAndExit(){
		GameObject entrancePrefab = behaviour.entrancePrefab;
		GameObject entranceGo = Instantiate(entrancePrefab,Vector3.zero,Quaternion.identity) as GameObject;
		entranceGo.transform.position = this.GetStartPosition();
		entranceGo.transform.parent = this.staticMapTr;
		entranceGo.name = "Entrance";
		
		GameObject exitPrefab = behaviour.exitPrefab;
		GameObject exitGo = Instantiate(exitPrefab,Vector3.zero,Quaternion.identity) as GameObject;
		exitGo.transform.position = this.GetEndPosition();
		exitGo.transform.parent = this.staticMapTr;
		exitGo.name = "Exit";
	}
	
	
	// Get the position of a cell location in world coordinates, taking into account map orientation
	public Vector3 GetWorldPosition(CellLocation l){
		Vector3 pos = interpreter.GetWorldPosition(l);
		
		if (behaviour.AutomaticOrientation) return pos;
		
		switch(behaviour.mapPlaneOrientation){
		case MapPlaneOrientation.XY: pos = new Vector3(pos.x,pos.z,0);	break;
		case MapPlaneOrientation.XZ: break; // Default
		case MapPlaneOrientation.YZ: pos = new Vector3(0,pos.x,pos.z); 	break;
		}
		return pos;
	}
	
	
	
	
}
	
[Serializable]
abstract public class PhysicalMap : ScriptableObject{
	
	// Behaviour references, used during generation
	[SerializeField]
	protected GeneratorBehaviour generator;
	
	[SerializeField]
	protected MapInterpreter interpreter;
	
	// Dictionary used for debug
	[SerializeField]	
	public CellTypeGameObjectListDict gameObjectLists;
	
	[SerializeField]
	public GameObject rootMapGo;
	
	[SerializeField]
	public GameObject dynamicMapGo;
	
	[SerializeField]
	public GameObject staticMapGo;
	
	[SerializeField]
	public Transform staticMapTr;
	
	[SerializeField]
	public Transform dynamicMapTr;
	
	[SerializeField]
	protected VirtualMap virtualMap;

	protected bool hadCombined = false;

	 
	public void Initialise(VirtualMap map, GeneratorBehaviour g, MapInterpreter i){
		virtualMap = map;
		generator = g;
		this.interpreter = i;
		
		gameObjectLists = CellTypeGameObjectListDict.Create();
		foreach(VirtualCell.CellType ct in System.Enum.GetValues(typeof(VirtualCell.CellType)).Cast<VirtualCell.CellType>()){
			gameObjectLists.Set(ct,new GameObjectList());
		}
			
		interpreter.Initialise(virtualMap,this,generator);
		
	}

	virtual public void CleanUp(){
		// We need to destroy newly created assets
		DestroyImmediate (gameObjectLists);
		if (hadCombined && staticMapGo.GetComponentInChildren<MeshFilter>()) {
//			Debug.Log (staticMapGo.GetComponentInChildren<MeshFilter>().sharedMesh);
			DestroyImmediate(staticMapGo.GetComponentInChildren<MeshFilter>().sharedMesh);
		}
	}
		
	abstract protected void BuildRootGameObjects();
	
//	abstract protected void Initialise(VirtualMap map);
	
	public void Generate(){
		if (StartGeneration()){
			if (GetShallBuildRootGameObjects()) BuildRootGameObjects();
			
			interpreter.ReadMap(virtualMap);
			
			EndGeneration();
			FinalizeGeneration();
		}	
	}
	
	virtual protected void FinalizeGeneration(){
	}
	
	// May be overriden
	virtual protected bool StartGeneration(){
		return true; // Always generate
	}
	virtual protected void EndGeneration(){
	}
	
	
	abstract public Vector3 GetStartPosition();
	abstract public Vector3 GetEndPosition();
	
	virtual protected bool GetShallBuildRootGameObjects(){return true;}
	
	
	// TODO: Instead pass a VirtualCell to CreateObject so we can extract cell type and the orientation as well
	abstract public void CreateObject(VirtualMap map, MetricLocation l, VirtualCell.CellType cell_type, VirtualMap.DirectionType orientation);
		
	
	public void AddToMapGameObject(VirtualCell.CellType cell_type, GameObject go){
		this.AddToMapGameObject(cell_type,go,false);
	}
	
	public void AddToMapGameObject(VirtualCell.CellType cell_type, GameObject go, bool dynamic){
		if (dynamic) go.transform.parent = dynamicMapTr;
		else {
			go.transform.parent = staticMapTr;
			go.isStatic = true;
			foreach (Transform t in go.transform) {
				t.gameObject.isStatic = true;
			}
		}
		gameObjectLists.Get(cell_type).Add(go);
	}
	
	
	/****************
	 * Getters
	 ****************/
	
	
	/// <summary>
	/// Returns the GameObject at a certain world position, or null if none is found.
	///	Note that this will return the topmost gameobject at that position, so if there is a wall above a floor, the wall will be returned.
	/// </summary>
	/// <returns>
	/// The GameObject at the position.
	/// </returns>
	/// <param name='pos'>
	/// Vector3 position.
	/// </param>
	public GameObject GetObjectAtPosition(Vector3 pos){
		RaycastHit hit;
		if (Physics.Raycast(pos+Vector3.up*50, -Vector3.up, out hit, 100)){
			return hit.transform.gameObject;
		}
		return null;
	}
	
	/// <summary>
	/// Returns all the GameObjects of a chosen SelectionObjectType.
	/// </summary>
	/// <returns>
	/// A List<GameObject> containing all objects.
	/// </returns>
	/// <param name='type'>
	/// SelectionObjectType to use.
	/// </param>
	public List<GameObject> GetObjectsOfType(SelectionObjectType type){
		List<GameObject> list = new List<GameObject>();
		foreach(VirtualCell.CellType cell_type in SelectionManager.GetCellTypes(type)){
//			Debug.Log("Getting objects for cell type " + cell_type);
			List<GameObject> new_objs = GetObjectsOfType(cell_type);
			DebugUtils.Assert(new_objs != null, "Cannot find any list of type " + cell_type,this);
			if (new_objs != null) list.AddRange(new_objs);
		}
		return list;
	}
	
	protected List<GameObject> GetObjectsOfType(VirtualCell.CellType cell_type){
		return this.gameObjectLists.Get(cell_type);
	}
	
	public List<Vector3> GetPositionsOfType(SelectionObjectType type){
		List<GameObject> list = GetObjectsOfType(type);
		List<Vector3> positions = new List<Vector3>();
		foreach(GameObject go in list) positions.Add(go.transform.position);
		return positions;
	}
	
}
