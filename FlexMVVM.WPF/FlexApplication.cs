using FlexMVVM.Navigation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;

namespace FlexMVVM.WPF
{
    public abstract class FlexApplication : Application
    {
        protected FlexFluent flex;
        protected IServiceProvider ServiceProvider => _serviceProvider;

        private IServiceProvider _serviceProvider;
        private DependencyObject Shell;
        private DependencyObject MainLayout;
        private FlexAppBuilder builder;

        protected FlexApplication()
        {
            this.flex = new FlexFluent ();
            this.builder = FlexApp.CreateBuilder ();
            this.builder.Services.AddSingleton<INavigator, Navigator> ();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup (e);
            
            this.Render ();
            this.ModuleContext (this.builder);
            this.RegisterService (this.builder.Services);

            Type winType = NameContainer.RegisterType["FlexFrameworkWindow"];

            Type contentType = NameContainer.RootLayout;
            builder.AddModules (flex.GetModules());

            _serviceProvider = builder.Build ();
            NameContainer.ServiceProvider = _serviceProvider;
            Shell = (DependencyObject)_serviceProvider.GetService (winType);
            MainLayout = (DependencyObject)_serviceProvider.GetService (contentType);

            OnInitialized ();
        }

        protected abstract void Render();
        protected virtual void ModuleContext(FlexAppBuilder builder){}
        protected virtual void RegisterService(IServiceCollection services){}

        protected virtual void OnInitialized()
        {
            if (Shell is Window window)
            {
                window.Content = MainLayout;
                window.Show ();
            }
        }
    }

    public class FlexFluent
    {
        private List<IModule> _modules;
        public FlexFluent()
        {
            NameContainer.RegisterType["FlexFrameworkWindow"] = typeof (FlexWindow);
            this._modules = new List<IModule> ();
        }

        public FlexFluent Window<T>(Func<T> window = null)
        {
            if (window == null)
                return this;
            NameContainer.RegisterType["FlexFrameworkWindow"] = typeof (T);
            return this;
        }

        public FlexFluent MainLayout<T>()
        {
            NameContainer.RootLayout = typeof (T);
            return this;
        }

        public FlexFluent MainLayout<T>(Func<T> content)
        {
            NameContainer.RootLayout = typeof (T);
            return this;
        }

        public FlexFluent AddModule<T>() where T : IModule
        {
            var module = (IModule)Activator.CreateInstance (typeof (T));
            this._modules.Add (module);
            return this;
        }

        public FlexFluent AddModules(params IModule[] modules)
        {
            this._modules.AddRange (modules);
            return this;
        }

        public List<IModule> GetModules() => this._modules;
    }
}
