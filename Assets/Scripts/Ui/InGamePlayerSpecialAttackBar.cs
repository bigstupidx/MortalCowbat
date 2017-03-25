using UnityEngine.UI;
using UnityEngine;

namespace Ui
{
	public class InGamePlayerSpecialAttackBar : MonoBehaviour
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

