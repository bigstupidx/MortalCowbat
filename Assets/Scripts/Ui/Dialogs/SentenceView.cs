using UnityEngine.UI;
using UnityEngine;

namespace Ui
{
	public class SentenceView : MonoBehaviour
	{
		[SerializeField]
		Image speaker;

		[SerializeField]
		Text sentence;
	
	
		public void Init(Sprite speaker, string text)
		{
			this.speaker.sprite = speaker;
			this.sentence.text = text;
		}
	}
}

