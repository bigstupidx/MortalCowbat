using UnityEngine;

namespace Vis
{
	public class ParallaxLayer : MonoBehaviour
	{
		[SerializeField]
		float speed;

		float camStartPosX;

		void Awake()
		{
			camStartPosX = Camera.main.transform.position.x;
		}

		void Update()
		{
			var shift = Camera.main.transform.position.x - camStartPosX;

			var newPosX = (1 - speed) * shift;
			transform.SetPositionX(newPosX);
		}

	}
}

