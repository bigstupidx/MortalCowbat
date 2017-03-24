using System.Collections.Generic;
using UnityEngine;

namespace Ui
{
	[System.Serializable]
	public class Dialog
	{
		[System.Serializable]
		public class Sentence
		{
			public Sprite Speaker;
			public string Title;
			public string Text;
			public string Position;
		}

		public string Name;
		public List<Sentence> Sentences;
	}
}

