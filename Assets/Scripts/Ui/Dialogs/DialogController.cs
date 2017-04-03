using System.Collections;
using System;

namespace Ui
{
	public class DialogController
	{
		Dialoger dialoger;
		public Action startAction;
		public Action finishAction;

		public DialogController(Dialoger dialoger)
		{
			this.dialoger = dialoger;
		}

		public IEnumerator OpenDialog(string name)
		{
			yield return dialoger.StartCoroutine(dialoger.ShowDialog(name, startAction, finishAction));
		}


		public void OnLevelStarted(int level)
		{
			var dialog = FindDialog(
				DialogCondition.LevelStarted, 
				new Dialog.Parameter {
					Key = DialogParameterKey.Level,
					Value = level.ToString()
				}
			);

			if (dialog != null) {
				ShowDialog(dialog.Name);
			}
		}

		public void OnWaveStarted(int level, int wave)
		{
			var dialog = FindDialog(
				DialogCondition.WaveStarted, 
				new Dialog.Parameter {
					Key = DialogParameterKey.Level,
					Value = level.ToString()
				},
				new Dialog.Parameter {
					Key = DialogParameterKey.Wave,
					Value = wave.ToString()
				} 
			);

			if (dialog != null) {
				ShowDialog(dialog.Name);
			}
		}

		public void OnPlayerDied(int level, int wave)
		{
			var dialog = FindDialog(
				DialogCondition.PlayerDied, 
				new Dialog.Parameter {
					Key = DialogParameterKey.Level,
					Value = level.ToString()
				},
				new Dialog.Parameter {
					Key = DialogParameterKey.Wave,
					Value = wave.ToString()
				} 
			);

			if (dialog != null) {
				ShowDialog(dialog.Name);
			}
		}

		public void OnWaveFinished(int level, int wave)
		{
			var dialog = FindDialog(
				DialogCondition.WaveFinished, 
				new Dialog.Parameter {
					Key = DialogParameterKey.Level,
					Value = level.ToString()
				},
				new Dialog.Parameter {
					Key = DialogParameterKey.Wave,
					Value = wave.ToString()
				} 
			);

			if (dialog != null) {
				ShowDialog(dialog.Name);
			}
		}

		public void OnAllWavesFinished(int level)
		{
			var dialog = FindDialog(
				DialogCondition.AllWavesFinished, 
				new Dialog.Parameter {
					Key = DialogParameterKey.Level,
					Value = level.ToString()
				}
			);

			if (dialog != null) {
				ShowDialog(dialog.Name);
			}
		}

		void ShowDialog(string name)
		{
			dialoger.StartCoroutine(OpenDialog(name));
		}

		Dialog FindDialog(DialogCondition condition, params Dialog.Parameter[] parameters)
		{
			var dialog = dialoger.dialogDb.Dialogs.Find(x=>x.Condition == condition);
			if (dialog != null) {
				for (int i = 0; i < parameters.Length; ++i) {
					var dialogParameter = dialog.Parameters.Find(x=>x.Key == parameters[i].Key);									
					if (dialogParameter != null && !dialogParameter.Value.Equals(parameters[i].Value)) {
						return null	;
					}
				}
			}
			return dialog;
		}
	}
}

