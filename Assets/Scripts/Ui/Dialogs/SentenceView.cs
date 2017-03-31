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

		public void MakeFullscreen()
		{
			(transform as RectTransform).anchorMax = Vector3.one;
			(transform as RectTransform).anchorMin = Vector3.zero;
			(transform as RectTransform).offsetMin = Vector3.zero;
			(transform as RectTransform).offsetMax = Vector3.zero;
		}
	}
}

