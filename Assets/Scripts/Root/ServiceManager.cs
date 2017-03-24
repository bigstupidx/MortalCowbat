using System.Collections.Generic;
using System;


namespace Lib
{
	public class ServiceManager
	{
		Dictionary<Type, IService> services;

		public ServiceManager()
		{
			services = new Dictionary<Type, IService>();
		}

		public void RegisterService(IService service)
		{
			services.Add(service.GetType(), service);
		}

		public T GetService<T>() where T : IService
		{
			return (T)services [typeof(T)];
		}
	}
}

