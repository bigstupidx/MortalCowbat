using System.Collections.Generic;
using UnityEngine;

namespace Ui
{
	[System.Serializable]
	public class Dialog
	{
		[System.Serializable]
		public class Parameter
		{
			public DialogParameterKey Key;
			public string Value;
		}

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
		public DialogCondition Condition;
		public List<Parameter> Parameters;
	}
}

