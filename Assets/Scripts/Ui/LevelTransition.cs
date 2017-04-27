using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Ui
{
	public class LevelTransition : MonoBehaviour
	{
		[SerializeField]
		Image image;

		public IEnumerator Show()
		{
			image.enabled = true;
			yield return StartCoroutine(Utils.LerpWithEase(0,1, 0.5f, (t)=> {
				var c = image.color;
				c.a = t;
				image.color = c;
			},null));
		}

		public IEnumerator Hide()
		{
			yield return StartCoroutine(Utils.LerpWithEase(1,0, 0.5f, (t)=> {
				var c = image.color;
				c.a = t;
				image.color = c;
			},null));
			image.enabled = false;
		}
	}
}

