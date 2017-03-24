using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ui
{
	public class Dialog : MonoBehaviour
	{
		[System.Serializable]
		public class Sentence
		{
			public string Text;
			public string Speaker;
			public int Position;
		}

		public string Name;
		public List<Sentence> Sentences;
	}
}

