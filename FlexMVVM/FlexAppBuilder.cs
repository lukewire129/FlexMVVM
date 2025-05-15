using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;

namespace FlexMVVM
{
    public sealed class FlexAppBuilder
    {
        private readonly ServiceCollection _services = new ServiceCollection ();
        private IServiceProvider _serviceProvider;
        private List<IModule> _modules;
        public FlexAppBuilder()
        {
            _modules = new List<IModule> ();
            // Lazy-load the ConfigurationManager, so it isn't created if it is never used.
            // Don't capture the 'this' variable in AddSingleton, so MauiAppBuilder can be GC'd.
            var configuration = new Lazy<ConfigurationManager> (() => new ConfigurationManager ());
            _services.AddSingleton<IConfiguration> (sp => configuration.Value);
        }

        public IServiceCollection Services => _services;

        public IServiceProvider Build()
        {
            foreach (var module in _modules)
            {
                module.Register (_services);
            }
            foreach (var nameContainer in NameContainer.RegisterType)
            {
                _services.TryAddSingleton (nameContainer.Value);
            }
            _serviceProvider = _services.BuildServiceProvider ();
            foreach (var module in _modules)
            {
                module.Initialize (_serviceProvider);
            }
            return _serviceProvider;
        }

        public void AddModules(List<IModule> modules)
        {
            if (modules.Count == 0)
                return;

            _modules.AddRange (modules);
        }

        public void AddModule<T>()
        {
            var module = (IModule)Activator.CreateInstance (typeof (T));

            _modules.Add (module);
        }
    }
}
