using Microsoft.Extensions.DependencyInjection;

namespace HostBuilderUtils.PreBuildServiceProvider
{
    /// <summary>
    /// Wraps the configured factory for creating service providers with an extending service collection
    /// </summary>
    public class ServiceProviderFactory : IServiceProviderFactory<ServiceProviderFactoryBuilder>, IServiceProviderFactory<IServiceCollection>
    {
        private readonly ServiceProviderFactoryBuilder _builder;
        private readonly IServiceProviderFactory<IServiceCollection> _factory;
        public ServiceProviderFactory(IServiceProviderFactory<IServiceCollection> factory)
        {
            _factory = factory;
            _builder = new ServiceProviderFactoryBuilder(_factory, null);
        }
        #region IServiceProviderFactory<ServiceProviderFactory.Builder>
        public ServiceProviderFactoryBuilder CreateBuilder(IServiceCollection services) => _builder.AddContainer(services);
        public IServiceProvider CreateServiceProvider(ServiceProviderFactoryBuilder containerBuilder) => containerBuilder.CreateServiceProvider();
        #endregion

        #region IServiceProviderFactory<IServiceCollection>
        public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder) => CreateBuilder(containerBuilder).CreateServiceProvider();
        IServiceCollection IServiceProviderFactory<IServiceCollection>.CreateBuilder(IServiceCollection services) => services;
        #endregion
    }
    public class ServiceProviderFactoryBuilder
    {
        internal IServiceCollection? _services;
        private readonly IServiceProviderFactory<IServiceCollection> _factory;
        public ServiceProviderFactoryBuilder(IServiceProviderFactory<IServiceCollection> factory, IServiceCollection? services)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _services = services;
        }
        public ServiceProviderFactoryBuilder AddContainer(IServiceCollection services)
        {
            if (_services == null)
                _services = services;
            else
                ReplaceServices(services);
            return this;
        }
        private void ReplaceServices(IServiceCollection services)
        {
            if (_services != null)
                foreach (var item in services.ToList())
                {
                    if (_services.FirstOrDefault(x => x.ServiceType == item.ServiceType) is ServiceDescriptor service)
                        _services.Remove(service);
                    _services.Add(item);
                }
        }
        public IServiceProvider CreateServiceProvider() => _factory.CreateServiceProvider(_services ?? throw new ArgumentNullException(nameof(_services)));
    }
}