using UnityEngine;
using System.Collections.Generic;

using System.ComponentModel;
using Vis;

namespace Battle.Comp
{
	public class Visual : CharacterComponent
	{
		[SerializeField]
		List<Transform> pois;


		public ChargingBar ChargingBar;

		public SpriteRenderer Ren;
		public Vector3 Position { get { return transform.position; }}

		[ SerializeField ][ReadOnlyAttribute(true)]
		int sortingOrder;

		public Transform GetPoi(string name)
		{
			return pois.Find(x=>x.name.Equals(name));
		}


		public override void UpdateMe()
		{
			const float maxY = 10;
			const float minY = -10;
			const int minSortingOrder = 10;
			const int maxSortingOrder = 100;

			float c = (Position.y - minY) / (maxY - minY);
			sortingOrder = minSortingOrder + (int)((maxSortingOrder - minSortingOrder) * (1 - c));
			Ren.sortingOrder = sortingOrder;
		}
	}
}
