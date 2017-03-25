using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace Vis
{
	public class ChargingBar : MonoBehaviour
	{
		[SerializeField]
		Transform fillTr;

		[SerializeField]
		Color fromColor;

		[SerializeField]
		Color toColor;

		[SerializeField]
		SpriteRenderer fill;

		Vector3 originalScale;

		void Awake()
		{
			originalScale = transform.localScale;
		}


		public void SetValue(float value01)
		{
			fillTr.SetScaleX(value01);
			fill.color =Color.Lerp(fromColor, toColor, value01);
			SetMaxed(value01.Equals(1.0f));
		}

		void SetMaxed(bool maxed)
		{
			transform.SetScaleY(maxed? originalScale.y * 2.0f : originalScale.y);
		}

	}
}

