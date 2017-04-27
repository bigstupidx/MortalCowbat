using UnityEngine;
using System.Collections.Generic;

namespace Vis
{
	public class Environment : MonoBehaviour
	{
		Transform visualRoot;

		List<LevelFrame> levelFrames;

		void Awake()
		{
			levelFrames = new List<LevelFrame>();
			visualRoot = transform.Find("Root");
			for (int i = 0; i < transform.childCount; ++i) {
				var tr = transform.GetChild(i);

				var levelFrame = tr.GetComponent<LevelFrame>();
				if (levelFrame != null) {
					levelFrames.Add(levelFrame);	
				}
			}
		}

		public LevelFrame Frame(int index)
		{
			return levelFrames[index];
		}
	}
}

