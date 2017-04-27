using UnityEngine;


namespace Vis
{
	public class GameVisual : MonoBehaviour
	{
		public Transform EnvContainer;
		public Environment Env { get; private set;}

		public void LoadEnvironment(string name)
		{
			var env = CreateEnvironment(name);
			PlaceEnvironment(env);
			Env = env.GetComponent<Environment>();
		}

		GameObject CreateEnvironment(string name)
		{
			var prefab = Resources.Load<GameObject>("Prefabs/Env/Env_" +  name);
			return Instantiate(prefab);
		}

		void PlaceEnvironment(GameObject env)
		{
			for (int i = 0; i < EnvContainer.childCount; ++i) {
				Destroy(EnvContainer.GetChild(i).gameObject);
			}

			env.transform.SetParent(EnvContainer);
			env.transform.localPosition = Vector3.zero;
		}
	}
}

