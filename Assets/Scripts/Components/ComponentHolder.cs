using UnityEngine;
using System.Collections.Generic;


public class ComponentHolder : MonoBehaviour
{

	public List<CharacterComponent> components;

	public T Get<T>() where T: CharacterComponent
	{
		return components.Find(x => x is T) as T;
	}
}

