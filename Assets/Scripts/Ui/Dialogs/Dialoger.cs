using UnityEngine;
using System.Collections;
using System;

namespace Ui
{
	public class Dialoger : MonoBehaviour
	{
		public DialogDb dialogDb;
		public GameObject leftSentencePrefab;
		public GameObject rightSentencePrefab;

		public void ShowDialog(string name)
		{
			var dialog = dialogDb.Dialogs.Find(x=>x.Name.Equals(name));		
			if (dialog != null) {
				StartCoroutine(ShowDialog(dialog));
			}
		}

		SentenceView CreateSentenceView(Dialog.Sentence sentence)
		{
			GameObject viewGo = null;

			if (sentence.Position.Equals("left")) {
				viewGo = Instantiate(leftSentencePrefab);
			} else if (sentence.Position.Equals("right")) {
				viewGo = Instantiate(rightSentencePrefab);
			}
			var view = viewGo.GetComponent<SentenceView>();
			view.Init(sentence.Speaker, sentence.Title, sentence.Text);
			return view;
		}


		IEnumerator ShowDialog(Dialog dialog)
		{

			for (int i = 0; i < dialog.Sentences.Count; ++i) {
				var sentence = dialog.Sentences[i];
				var view = CreateSentenceView(sentence);

				var canvasTr = FindCanvasTransform();
				view.transform.SetParent(canvasTr);
				view.transform.SetAsLastSibling();
				view.transform.localPosition = Vector3.zero;

				while (true) {
					if (Input.anyKeyDown || Input.GetMouseButtonDown(0)) {
						yield return 0;
						break;						
					}
					yield return 0;
				}
					
				Destroy(view.gameObject);
			}
		}

		Transform FindCanvasTransform()
		{
			return GameObject.FindObjectOfType<Canvas>().transform;
		}

	}
}

