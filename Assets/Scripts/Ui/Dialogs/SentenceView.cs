using UnityEngine.UI;
using UnityEngine;

namespace Ui
{
	public class SentenceView : MonoBehaviour
	{
		[SerializeField]
		Image speaker;

		[SerializeField]
		Text text;
	
		[SerializeField]
		Text title;
	
		public void Init(Sprite speaker, string title, string text)
		{
			this.speaker.sprite = speaker;
			this.title.text = title;
			this.text.text = text;
		}
	}
}

