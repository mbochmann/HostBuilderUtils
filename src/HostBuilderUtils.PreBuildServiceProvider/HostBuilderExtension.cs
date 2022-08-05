using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Collections;

namespace HostBuilderUtils.PreBuildServiceProvider
{
    public static class HostBuilderExtension
    {
        static IServiceProvider? _serviceProvider;
        public static IHostBuilder AddPreBuildServiceProvider(this IHostBuilder hostBuilder)
        {
            if (_serviceProvider == null)
                try
                {
                    IHostBuilder hostBuilderClone = hostBuilder.DeepCopy();
                    hostBuilderClone.ConfigureServices(ConfigureService);
                    hostBuilderClone.Build(); //Build fails with DefaultServiceProviderFactory
                }
                catch { }
            return hostBuilder;
        }
        private static void ConfigureService(HostBuilderContext _, IServiceCollection services)
        {
            if (_serviceProvider == null)
            {
                ServiceProviderFactory factory;
                if (services.GetService<IServiceProviderFactory<IServiceCollection>>() is ServiceDescriptor serviceDescriptor &&
                    GetFactory(serviceDescriptor) is IServiceProviderFactory<IServiceCollection> foundFactory)
                {
                    factory = new ServiceProviderFactory(foundFactory);
                    services.Replace(ServiceDescriptor.Singleton((IServiceProviderFactory<IServiceCollection>)factory));
                }
                else
                {
                    factory = new ServiceProviderFactory(new DefaultServiceProviderFactory());
                    services.AddSingleton<IServiceProviderFactory<IServiceCollection>>(factory);
                }
                _serviceProvider = (factory.CreateBuilder(services).CreateServiceProvider())!;
            }
        }
        private static IServiceProviderFactory<IServiceCollection>? GetFactory(ServiceDescriptor serviceDescriptor)
        {
            if (serviceDescriptor.ImplementationInstance is IServiceProviderFactory<IServiceCollection> factoryInstance)
                return factoryInstance;
            if (serviceDescriptor.ImplementationFactory is Func<IServiceProvider, object> implFactoryInstance)
                return implFactoryInstance(null!) as IServiceProviderFactory<IServiceCollection>; //Not tested yet
            return null;
        }
        public static IHostBuilder ConfigureServices(this IHostBuilder hostBuilder, Action<HostBuilderContext, IServiceCollection, IServiceProvider> services)
        {
            if (hostBuilder is null) throw new ArgumentNullException(nameof(hostBuilder));
            if (services != null)
                hostBuilder.ConfigureServices((ctx, sp) => services(ctx, sp, _serviceProvider!));
            return hostBuilder;
        }

        #region Hostbuilder Cloning
        /// <summary>
        /// Creates a shallow copy of <paramref name="hostBuilder"/>.
        /// </summary>
        /// <param name="hostBuilder"><see cref="IHostBuilder"/> which will be shallow copied</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private static IHostBuilder ShallowCopy(this IHostBuilder hostBuilder)
        {
            Type type = hostBuilder?.GetType() ?? throw new ArgumentNullException(nameof(hostBuilder));
            if (Activator.CreateInstance(type) is IHostBuilder host)
            {
                var bindingflags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public;
                var fields = type.GetFields(bindingflags);
                foreach (var field in fields)
                    field.SetValue(host, field.GetValue(hostBuilder));
                var properties = type.GetProperties(bindingflags);
                foreach (var property in properties)
                    if (property.CanWrite)
                        property.SetValue(host, property.GetValue(hostBuilder));
                return host;
            }
            throw new InvalidOperationException(type.FullName + " needs a parameterless constructor");
        }
        /// <summary>
        /// Creates a shallow copy of <paramref name="hostBuilder"/>.
        /// </summary>
        /// <param name="hostBuilder"><see cref="IHostBuilder"/> which will be shallow copied</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private static IHostBuilder DeepCopy(this IHostBuilder hostBuilder)
        {
            Type type = hostBuilder?.GetType() ?? throw new ArgumentNullException(nameof(hostBuilder));
            if (Activator.CreateInstance(type) is IHostBuilder host)
                return hostBuilder.DeepCopyTo(host);
            throw new InvalidOperationException(type.FullName + " needs a parameterless constructor");
        }
        /// <summary>
        /// Creates a shallow copy of <paramref name="hostBuilder"/>.
        /// </summary>
        /// <param name="hostBuilder"><see cref="IHostBuilder"/> which will be shallow copied</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private static IHostBuilder DeepCopyTo(this IHostBuilder from, IHostBuilder to)
        {
            Type type = from?.GetType() ?? throw new ArgumentNullException(nameof(from));
            if (to is IHostBuilder host)
            {
                var bindingflags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public;
                var fields = type.GetFields(bindingflags);
                foreach (var field in fields)
                {
                    if (IsIList(field.FieldType) && CreateList(field.FieldType) is IList newlist && field.GetValue(from) is IList list)
                    {
                        foreach (var item in list)
                            newlist.Add(item);
                        field.SetValue(host, newlist);
                    }
                    else
                        field.SetValue(host, field.GetValue(from));
                }
                var properties = type.GetProperties(bindingflags);
                foreach (var property in properties)
                    if (property.CanWrite)
                        property.SetValue(host, property.GetValue(from));
                return host;
            }
            throw new InvalidOperationException(type.FullName + " needs a parameterless constructor");
        }
        #endregion

        private static bool IsIList(Type type) => type.IsAssignableTo(typeof(IList));
        private static IList? CreateList(Type type) => Activator.CreateInstance(type) as IList;
    }
}