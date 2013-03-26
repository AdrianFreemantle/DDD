using System;

namespace Domain.Core.Ioc
{
    public class ServiceProviderAdapter : IServiceProvider
    {
        readonly Func<Type, object> resover;

        public ServiceProviderAdapter(Func<Type, object> resover)
        {
            this.resover = resover;
        }

        public object GetService(Type serviceType)
        {
            return resover.Invoke(serviceType);
        }
    }
}