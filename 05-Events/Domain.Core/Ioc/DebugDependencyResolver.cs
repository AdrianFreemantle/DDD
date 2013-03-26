using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Core.Ioc
{
    public class DebugDependencyResolver : IServiceProvider
    {
        private readonly List<object> services = new List<object>();

        public void RegisterService(object service)
        {
            services.Add(service);
        }

        public void AddService(object serviceInstance)
        {
            var instance = GetService(serviceInstance.GetType());

            if (instance != null)
            {
                throw new InvalidOperationException(String.Format("Already contain a service of type {0}",
                    serviceInstance.GetType().FullName));
            }

            services.Add(serviceInstance);
        }

        public object GetService(Type serviceType)
        {
            return services.FirstOrDefault(serviceType.IsInstanceOfType);
        }
    }
}
