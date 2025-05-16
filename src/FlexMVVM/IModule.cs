using Microsoft.Extensions.DependencyInjection;
using System;

namespace FlexMVVM
{
    public interface IModule
    {
        void Register(IServiceCollection services);
        void Initialize(IServiceProvider serviceProvider);
    }
}
