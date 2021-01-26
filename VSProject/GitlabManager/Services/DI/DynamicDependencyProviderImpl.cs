using System;
using Microsoft.Extensions.DependencyInjection;

namespace GitlabManager.Services.DI
{

    /// <summary>
    /// Default implementation of <see cref="IDynamicDependencyProvider"/>
    /// ServiceProvider instances is set from <see cref="AppDependencyInjection"/>
    /// </summary>
    public class DynamicDependencyProviderImpl : IDynamicDependencyProvider
    {
        public IServiceProvider ServiceProvider { get; set; }

        public T GetInstance<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }
        
    }
}