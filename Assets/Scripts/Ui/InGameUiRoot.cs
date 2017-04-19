using UnityEngine;
using System.Collections.Generic;

namespace Ui
{
	public class InGameUiRoot : MonoBehaviour
	{
		public DialogController DialogController { 
			get; private set; 
		}

		public VirtualKeyboardController VirtualKeyboardController {
			get { return virtualKeyboardController; }
		}

		[SerializeField]
		List<InGamePlayerHealthBar> playerHealthbar;

		[SerializeField]
		List<InGamePlayerSpecialAttackBar> playerSpecialAttackBar;

		[SerializeField]
		VirtualKeyboardController virtualKeyboardController;

		[SerializeField]
		Wave wave;

		[SerializeField]
		Dialoger dialoger;

		void Awake()
		{
			DialogController = new DialogController(dialoger);
			ShowHudForPlayer(1, false);
		}

		public void ShowHudForPlayer(int index, bool show)
		{
			playerHealthbar[index].gameObject.SetActive(show);
			playerSpecialAttackBar[index].gameObject.SetActive(show);
		}

		public void OnPlayerHealthChanged(int index, float actual, float maxHealth)
		{
			playerHealthbar[index].Set(actual / maxHealth);		
		}
		public void OnPlayerSpecialAttackProgress(int index, float progress)
		{
			playerSpecialAttackBar[index].Set(progress);
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