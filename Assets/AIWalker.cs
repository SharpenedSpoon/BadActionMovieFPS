using UnityEngine;
using System.Collections;
using System;

public class AIWalker : MonoBehaviour {

	public float speed = 5;

	// Use this for initialization
	void Start () {
		LerpMeOverTime();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected void LerpMeOverTime()
	{
		Vector3 startPosition = gameObject.transform.position;
		Vector3 endPosition = startPosition + 10*Vector3.forward;
		float timeInSeconds = 5.0f;
		
		Action<float> transformAction = delegate(float f){
			gameObject.transform.position = Vector3.Lerp(startPosition,endPosition,f);
		};
		
		StartCoroutine(SubfacetFunctions.TransformOverTime(transformAction,timeInSeconds));
	}
}
