using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
	public class InGamePlayerHealthBar : MonoBehaviour
	{
		[SerializeField]
		Image fill;

		float fullScale;

		public void Set(float value01)
		{
			fill.fillAmount = value01;
		}
	}
}

