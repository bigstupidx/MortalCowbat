using UnityEngine;

namespace Vis
{
	public class InGameCamera : MonoBehaviour
	{
		[SerializeField]
		Camera  mainCamera;
	
		public Vector3 GetPosition()
		{
			return mainCamera.transform.position;
		}

		public float GetWidth()
		{
			return GetHeight() * mainCamera.aspect;
		}

		public float GetHeight()
		{
			return mainCamera.orthographicSize * 2;
		}
	}
}

