using HappyUtility.Iocs;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HappyUtility.Iocs
{

    public interface IDependency
    {
    }
    public interface ITransientDependency : IDependency
    {

    }

    public interface ISingletonDependency : IDependency
    {

    }
    public interface IScopedDependency : IDependency
    {

    }

    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AutoAddService(this IServiceCollection @this,
            IEnumerable<Assembly> assemblies, Func<Type, bool> predicate, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var services = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(predicate);

            services.ToList().ForEach(e =>
            {
                @this.Add(ServiceDescriptor.Describe(e, e, lifetime));
            });


            return @this;
        }
        /// <summary>
        /// 基于命名规范的注册 eg：Service->IService
        /// </summary>
        /// <param name="this"></param>
        /// <param name="assemblies"></param>
        /// <param name="predicate"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IServiceCollection AutoAddServiceToInterface(this IServiceCollection @this,
          IEnumerable<Assembly> assemblies, Func<Type, bool> predicate, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var services = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(predicate);

            services.ToList().ForEach(e =>
            {
                var interfaceType = e.GetInterface($"I{e.Name}");

                if (interfaceType == null)
                {
                    throw new Exception($"类型{e.Name}无默认接口！");
                }
                @this.Add(ServiceDescriptor.Describe(serviceType: interfaceType, e, lifetime));
            });

            return @this;
        }

        public static IServiceCollection AutoAddTransient(this IServiceCollection @this, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            return @this.AutoAddService(assemblies, predicate, ServiceLifetime.Transient);
        }
        public static IServiceCollection AutoAddSingleton(this IServiceCollection @this, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {

            return @this.AutoAddService(assemblies, predicate, ServiceLifetime.Singleton);

        }
        public static IServiceCollection AutoAddScoped(this IServiceCollection @this, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            return @this.AutoAddService(assemblies, predicate, ServiceLifetime.Scoped);
        }

        public static IServiceCollection AutoAddTransientToInterface(this IServiceCollection @this, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            return @this.AutoAddServiceToInterface(assemblies, predicate, ServiceLifetime.Transient);
        }

        public static IServiceCollection AutoAddSingletonToInterface(this IServiceCollection @this, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            return @this.AutoAddServiceToInterface(assemblies, predicate, ServiceLifetime.Singleton);
        }
        public static IServiceCollection AutoAddScopedToInterface(this IServiceCollection @this, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            return @this.AutoAddServiceToInterface(assemblies, predicate, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// 基于命名规范的注册 eg：Service->IService
        /// </summary>
        /// <param name="this"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection ResgisterAllSignedService(this IServiceCollection @this, IEnumerable<Assembly> assemblies)
        {
            var baseType = typeof(IDependency);

            var implementTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract && baseType.IsAssignableFrom(x));



            implementTypes.ToList().ForEach(implementType =>
            {
                if (typeof(IScopedDependency).IsAssignableFrom(implementType))
                {
                    var interfaceType = implementType.GetInterface($"I{implementType.Name}");
                    if (interfaceType != null)
                        @this.AddScoped(serviceType: interfaceType, implementType);
                }
                else if (typeof(ISingletonDependency).IsAssignableFrom(implementType))
                {
                    var interfaceType = implementType.GetInterface($"I{implementType.Name}");
                    if (interfaceType != null)
                        @this.AddSingleton(serviceType: interfaceType, implementType);
                }
                else
                {
                    var interfaceType = implementType.GetInterface($"I{implementType.Name}");
                    if (interfaceType != null)
                        @this.AddTransient(serviceType: interfaceType, implementType);
                }
            });


            return @this;
        }
    }


}