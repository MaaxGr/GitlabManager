using System.Windows;
using System.Windows.Media;

namespace GitlabManager.Services.Resources
{
    public class ResourcesImpl : IResources
    {

        public SolidColorBrush GetBrush(ComponentResourceKey resourceKey)
        {
            return Application.Current.TryFindResource(resourceKey) as SolidColorBrush;
        }
        
    }
}