using UnityEngine;
using System.Collections;
using System;

public class SubfacetFunctions : MonoBehaviour {

	// Example usage:
	/*
	protected void LerpMeOverTime()
    {
        Vector3 startPosition = gameObject.transform.position;
        Vector3 endPosition = new Vector3(-5,25,25);
        float timeInSeconds = 5.0f;
 
        Action<float> transformAction = delegate(float f){
            gameObject.transform.position = Vector3.Lerp(startPosition,end,f);
        };
 
        StartCoroutine(TransformOverTime(transformAction,timeInSeconds));
    }
    */

	

	/// <summary>
	/// Runs a transforming action over time. This action is usually a Lerp but does not have to be, it could be
	/// an arbitrary time based function.
	/// </summary>
	/// <returns>
	/// A coroutine that executes this transform over time.
	/// </returns>
	/// <param name='transform'>
	/// The transformation action. This is where the good stuff goes.
	/// </param>
	/// <param name='timeInSeconds'>
	/// Time in seconds.
	/// </param>
	/// <param name='keepRunning'>
	/// If this is ever evaluated to false the transforamtion will end.
	/// </param>
	public static IEnumerator TransformOverTime (Action<float> transform, float timeInSeconds, Func<bool> keepRunning = null)
	{
		Func<float,float > step = delegate(float f)
		{
			return Mathf.Clamp (f + (Time.smoothDeltaTime / timeInSeconds), 0.0f, 1.0f);
		};
		
		return TransformOverFunction (transform, step, keepRunning);
		
	}
	
	/// <summary>
	/// Transforms an action over a function. The return of the function is measured between 0.0 and 1.1. If the
	/// function returns 1.0 or greater the function exits. This is useful when you want to lerp but are not accumulating
	/// time in a linear fashion.
	/// </summary>
	/// <returns>
	/// A coroutine that runs the provided transform over the range of the function provided using the previous return value as input.
	/// </returns>
	/// <param name='transform'>
	/// The transforming action.
	/// </param>
	/// <param name='function'>
	/// The stepping function. This function is fed the result of last time.
	/// </param>
	/// <param name='keepRunning'>
	/// Keep running.
	/// </param>
	public static IEnumerator TransformOverFunction (Action<float> transform, Func<float, float> function, Func<bool> keepRunning = null)
	{
		Func<bool > @continue = keepRunning ?? delegate {
			return true;
		};
		
		for (float accumulator = 0.0f; accumulator <= 1.0f && @continue(); accumulator = function(accumulator))
		{
			transform (accumulator);
			
			yield return null;
			
			//we can't test as < 1.0 in the conditional statement or we would never get the last call to the
			//lerping instruction. So we have to break after. If we don't break the Clamp in the post instruction
			//keeps us locked in the loop.
			if (accumulator == 1.0f)
			{
				break;
			}
		}
	}
}
