using Microsoft.Extensions.DependencyInjection;

namespace FlexMVVM.WPF
{
    public static class Extentions
    {
        public static IServiceCollection RegisterWindow<T>(this IServiceCollection services, T window, string key = null) where T : class
        {
            string _key = key == null ? typeof (T).FullName : key;

            NameContainer.RegisterType[_key] = typeof (T);
            return services;
        }

        public static IServiceCollection RegisterWindow<T>(this IServiceCollection services, string key = null)where T : class
        {
            string _key = key ==null? typeof(T).FullName : key;

            NameContainer.RegisterType[_key] = typeof(T);
            return services;
        }

        public static IServiceCollection RegisterLayout<T>(this IServiceCollection services, string key = null) where T : class
        {
            string _key = key == null ? typeof (T).FullName : key;

            NameContainer.RegisterType[_key] = typeof (T);
            return services;
        }

        public static IServiceCollection RegisterComponent<T>(this IServiceCollection services, string key = null) where T : class
        {
            string _key = key == null ? typeof (T).FullName : key;

            NameContainer.RegisterType[_key] = typeof (T);
            return services;
        }

        public static FlexAppBuilder Module<T>(this FlexAppBuilder builder) where T : IModule
        {
            builder.AddModule<T> ();
            return builder;
        }
    }
}
