using System;
using UnityEngine;

namespace Lib
{
	public class Root : MonoBehaviour
	{
		[SerializeField]
		string localizationFile;

		public static Root Instance { get { return instance; }}
		public ServiceManager Services { get; private set; }

		static Root instance;


		void Awake()
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			Init();
		}

		void Init()
		{
			Services = new ServiceManager();
			Services.RegisterService(new Localizations());


			Services.GetService<Localizations>().Load(localizationFile);
		}

	}
}

