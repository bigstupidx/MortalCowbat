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

		public void SetPosition(Vector3 pos)
		{
			mainCamera.transform.position = pos;
		}

		public void SetPositionX(float pos)
		{
			mainCamera.transform.SetPositionX(pos);
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

