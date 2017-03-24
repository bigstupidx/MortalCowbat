using System;
using UnityEngine;
using System.Collections;

namespace Ui
{
	public class Dialoger : MonoBehaviour
	{
		public DialogDb dialogDb;
		public GameObject sentenceViewPrefab;

		public void ShowDialog(string name)
		{
			var dialog = dialogDb.Dialogs.Find(x=>x.Name.Equals(name));		
			if (dialog != null) {
				StartCoroutine(ShowDialog(dialog));
			}
		}

		SentenceView CreateSentenceView()
		{
			var viewGo = Instantiate(sentenceViewPrefab);
			return viewGo.GetComponent<SentenceView>();
		}


		IEnumerator ShowDialog(Dialog dialog)
		{

			for (int i = 0; i < dialog.Sentences.Count; ++i) {
				yield return 0;
			}
		}
	}
}

