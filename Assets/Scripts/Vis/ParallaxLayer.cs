using UnityEngine;

namespace Vis
{
	public class ParallaxLayer : MonoBehaviour
	{
		[SerializeField]
		float speed;

		float camStartPosX;
		float startPosX;
		void Start()
		{
			camStartPosX = Camera.main.transform.position.x;
			startPosX = transform.position.x;
		}

		void Update()
		{
			var shift = Camera.main.transform.position.x - camStartPosX;
			var newPosX = (1 - speed) * shift;
			transform.SetPositionX(startPosX + newPosX);
		}

	}
}

