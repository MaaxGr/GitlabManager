using System;
using Microsoft.Extensions.DependencyInjection;

namespace GitlabManager.Services.DI
{
    public class DynamicDependencyProviderImpl : IDynamicDependencyProvider
    {
        public IServiceProvider ServiceProvider { get; set; }

        public T GetInstance<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }
        
    }
}