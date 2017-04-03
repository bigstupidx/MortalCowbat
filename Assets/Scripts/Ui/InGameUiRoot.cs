using UnityEngine;

namespace Ui
{
	public class InGameUiRoot : MonoBehaviour
	{
		public DialogController DialogController { get; private set; }

		[SerializeField]
		InGamePlayerHealthBar playerHealthbar;

		[SerializeField]
		InGamePlayerSpecialAttackBar playerSpecialAttackBar;


		[SerializeField]
		Wave wave;

		[SerializeField]
		Dialoger dialoger;



		void Awake()
		{
			DialogController = new DialogController(dialoger);
		}

		public void OnPlayerHealthChanged(float actual, float maxHealth)
		{
			playerHealthbar.Set(actual / maxHealth);		
		}
		public void OnPlayerSpecialAttackProgress(float progress)
		{
			playerSpecialAttackBar.Set(progress);
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