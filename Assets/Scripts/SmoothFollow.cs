using System;
using UnityEngine;
using Vis;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
	public Transform Target;
	public LevelFrame LevelFrame;
	public bool CheckLimits { get; set; }
	public bool Follow { get; set; }


	InGameCamera cam;

	void Awake()
	{
		cam = GetComponent<InGameCamera>();
		CheckLimits = true;
		Follow = true;
	}

	void Update()
	{
		if (Follow && Target)
		{
//			if (Target.position.x > (cam.GetPosition().x + cam.GetWidth() / 4.0f)) {
//				var posX = Mathf.Lerp (transform.position.x, Target.position.x, 0.1f);
//				transform.SetPositionX(posX);
//			} else if (Target.position.x < (cam.GetPosition().x - cam.GetWidth() / 4.0f)){
//				var posX = Mathf.Lerp (transform.position.x, Target.position.x, 0.1f);
//				transform.SetPositionX(posX);
//			}

			var posX = Mathf.Lerp (transform.position.x, Target.position.x, 0.1f);

			if (CheckLimits) {
				posX = Mathf.Min(LevelFrame.GetXMax() - cam.GetWidth()* 0.5f, posX);
				posX = Mathf.Max(LevelFrame.GetXMin() + cam.GetWidth()* 0.5f, posX);
			}

			transform.SetPositionX(posX);
		}
	}

	public IEnumerator AllignWithLimit()
	{
		var xMax = LevelFrame.GetXMax() - cam.GetWidth()* 0.5f;
		var xMin = LevelFrame.GetXMin() + cam.GetWidth()* 0.5f;

		while (transform.position.x > xMax) {
			var posX = Mathf.Lerp (transform.position.x, xMax, 0.1f);
			transform.SetPositionX(posX);
			if ( Mathf.Abs(transform.position.x - xMax) < 0.05f)
				break;
			
			yield return 0;
		}

		while (transform.position.x < xMin) {
			var posX = Mathf.Lerp (transform.position.x, xMin, 0.1f);
			transform.SetPositionX(posX);
			if ( Mathf.Abs(transform.position.x - xMin) < 0.05f)
				break;
			yield return 0;
		}
		yield  break;
	}
}


