using UnityEngine;
using System.Collections.Generic;

namespace Battle.Comp
{
	public class Visual : CharacterComponent
	{
		[SerializeField]
		List<Transform> pois;


		public Transform GetPoi(string name)
		{
			return pois.Find(x=>x.name.Equals(name));
		}
	}
}
