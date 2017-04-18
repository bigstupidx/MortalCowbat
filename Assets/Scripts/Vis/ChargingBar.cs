using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace Vis
{
	public class ChargingBar : MonoBehaviour
	{
		public SpriteRenderer Fill;
		public SpriteRenderer Bg;

		[SerializeField]
		Transform fillTr;

		[SerializeField]
		Color fromColor;

		[SerializeField]
		Color toColor;

		Vector3 originalScale;

		void Awake()
		{
			originalScale = transform.localScale;
		}


		public void SetValue(float value01)
		{
			fillTr.SetScaleX(value01);
			Fill.color =Color.Lerp(fromColor, toColor, value01);
			SetMaxed(value01.Equals(1.0f));
		}

		public void SetSortingOrder(int order)
		{
			Bg.sortingOrder = order;
			Fill.sortingOrder = order + 1;
		}

		void SetMaxed(bool maxed)
		{
			transform.SetScaleY(maxed? originalScale.y * 2.0f : originalScale.y);
		}

	}
}

