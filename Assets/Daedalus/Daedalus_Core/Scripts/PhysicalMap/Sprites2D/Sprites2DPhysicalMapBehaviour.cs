using UnityEngine;
using System.Collections;
using System.Collections.Generic;
	
	
public class Sprites2DPhysicalMapBehaviour : PhysicalMapBehaviour<Sprite,SpriteChoice> {
	
	public float tileSize;
	public GameObject spritePrefab;
	
	override public PhysicalMap Generate(VirtualMap map, GeneratorBehaviour generator, MapInterpreter interpreter){
		Sprites2DPhysicalMap physMap = ScriptableObject.CreateInstance<Sprites2DPhysicalMap>();
		physMap.Initialise(map,generator,interpreter);
		physMap.behaviour = this;
		physMap.Generate();
		return physMap;
	} 
	
	
	Dictionary<string,Sprite> loadedSpritesDict;
	override public void CheckDefaults(){
		// We need to load the sprites in a special way, since we need to get also the sprite sheets
		loadedSpritesDict =  new Dictionary<string, Sprite>();
		Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites");
		for(int ii=0; ii< sprites.Length; ii++) loadedSpritesDict[sprites[ii].name] = sprites[ii];

		// Default tiles if not specified		
		corridorWallVariations = CheckDefault("DefaultSpriteSheet_0",corridorWallVariations);
		corridorFloorVariations = CheckDefault("DefaultSpriteSheet_2",corridorFloorVariations);
		corridorColumnVariations = CheckDefault("DefaultSpriteSheet_3",corridorColumnVariations);
		
		corridorFloorUVariations = CheckDefault("DefaultSpriteSheet_2",corridorFloorUVariations);
		corridorFloorIVariations = CheckDefault("DefaultSpriteSheet_2",corridorFloorIVariations);
		corridorFloorLVariations = CheckDefault("DefaultSpriteSheet_2",corridorFloorLVariations);
		corridorFloorTVariations = CheckDefault("DefaultSpriteSheet_2",corridorFloorTVariations);
		corridorFloorXVariations = CheckDefault("DefaultSpriteSheet_2",corridorFloorXVariations);
		

		roomWallVariations = CheckDefault("DefaultSpriteSheet_1",roomWallVariations);
		roomFloorVariations = CheckDefault("DefaultSpriteSheet_2",roomFloorVariations);
		roomColumnVariations = CheckDefault("DefaultSpriteSheet_3",roomColumnVariations);
		insideRoomColumnVariations = CheckDefault("DefaultSpriteSheet_3",insideRoomColumnVariations);
		
		roomFloorInsideVariations = CheckDefault("DefaultSpriteSheet_2",roomFloorInsideVariations);
		roomFloorCornerVariations = CheckDefault("DefaultSpriteSheet_2",roomFloorCornerVariations);
		roomFloorBorderVariations = CheckDefault("DefaultSpriteSheet_2",roomFloorBorderVariations);

		doorVariations = CheckDefault("DefaultSpriteSheet_2",doorVariations);
		doorColumnVariations = CheckDefault("DefaultSpriteSheet_3",doorColumnVariations);
		
		rockVariations = CheckDefault("DefaultSpriteSheet_4",rockVariations);
		
		if (!entrancePrefab) entrancePrefab = Resources.Load("DefaultEntrance",typeof(GameObject)) as GameObject;
		if (!exitPrefab) exitPrefab = Resources.Load("DefaultExit",typeof(GameObject)) as GameObject;

		mapPlaneOrientation = MapPlaneOrientation.XY;
	}

	override protected Sprite GetDefault(string defaultName){
		return loadedSpritesDict[defaultName];
	}

	override public bool ForcedOrientation{get{return true;}}

}