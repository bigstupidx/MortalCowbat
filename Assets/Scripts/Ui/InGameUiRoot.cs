using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

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
		LevelTransition levelTransition;

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

		public IEnumerator ShowLevelTransition()
		{
			yield return StartCoroutine(levelTransition.Show());
		}

		public IEnumerator HideLevelTransition()
		{
			yield return StartCoroutine(levelTransition.Hide());
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