using Microsoft.Extensions.DependencyInjection;

namespace HostBuilderUtils.PreBuildServiceProvider
{
    internal static class IServiceCollectionExtensions
    {
        public static ServiceDescriptor? GetService<T>(this IServiceCollection services) => services?.FirstOrDefault(x => x.ServiceType.IsAssignableTo(typeof(T)));
    }
}