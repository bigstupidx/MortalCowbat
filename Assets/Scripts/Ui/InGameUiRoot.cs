using UnityEngine;

namespace Ui
{
	public class InGameUiRoot : MonoBehaviour
	{
		[SerializeField]
		InGamePlayerHealthBar playerHealthbar;


		public void OnPlayerHealthChanged(float actual, float maxHealth)
		{
			playerHealthbar.Set(actual / maxHealth);		
		}
	}
}