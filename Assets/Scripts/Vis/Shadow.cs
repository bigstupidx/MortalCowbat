using System;
using UnityEngine;


namespace Vis
{
	public class Shadow : MonoBehaviour
	{

		float vertDistToPivot;
		Transform pivot;

		void Awake()
		{
			pivot = transform.parent.parent.FindChild("Pivot");
			vertDistToPivot = transform.position.y - pivot.position.y;
		}

		void LateUpdate()
		{
			var posY = pivot.position.y + vertDistToPivot;
			transform.SetPositionY(posY);
		}
	}
}

