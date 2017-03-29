using UnityEngine;

namespace Vis
{
	public class InGameCamera : MonoBehaviour
	{
		public SmoothFollow Follower { get { return follow; } }

		[SerializeField]
		Camera  mainCamera;

		[SerializeField]
		SmoothFollow follow;

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

