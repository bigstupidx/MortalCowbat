using UnityEngine;

namespace Ui
{
	public class InGameUiRoot : MonoBehaviour
	{
		[SerializeField]
		InGamePlayerHealthBar playerHealthbar;

		[SerializeField]
		Wave wave;

		public void OnPlayerHealthChanged(float actual, float maxHealth)
		{
			playerHealthbar.Set(actual / maxHealth);		
		}

		public void OnWave(int actual, int from)
		{
			wave.SetWave(actual, from);
		}

		public void OnLeft(int left)
		{
			wave.SetLeft(left);
		}
	}
}