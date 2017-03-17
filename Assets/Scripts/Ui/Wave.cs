using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
	public class Wave : MonoBehaviour
	{
		[SerializeField]
		Text wave;

		[SerializeField]
		Text left;

		public void SetWave(int actual, int from)
		{
			wave.text = string.Format("Wave {0}/{1}", actual, from);
		}

		public void SetLeft(int value)
		{
			left.text = value.ToString();
		}
	}
}

