using System.Windows;
using System.Windows.Media;

namespace GitlabManager.Services.Resources
{
    /// <summary>
    /// Default implementation of <see cref="IResources"/>
    /// Tries to find the resources in the current application context
    /// </summary>
    public class ResourcesImpl : IResources
    {
        
        public SolidColorBrush GetBrush(ComponentResourceKey resourceKey)
        {
            return Application.Current.TryFindResource(resourceKey) as SolidColorBrush;
        }
        
    }
}