using System;
using UnityEngine;
using System.Collections;

namespace Lib
{
	public class MonoGod : MonoBehaviour
	{

		public void StartAction(Action action, float delay)
		{
			StartCoroutine(StartActionCoroutine(action, delay));
		}

		IEnumerator StartActionCoroutine(Action action, float delay) 
		{
			yield return new WaitForSeconds(delay);
			action();
		}
	}
}

