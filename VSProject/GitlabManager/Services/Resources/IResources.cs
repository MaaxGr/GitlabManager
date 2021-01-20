using System.Windows;
using System.Windows.Media;

namespace GitlabManager.Services.Resources
{
    public interface IResources
    {
        public SolidColorBrush GetBrush(ComponentResourceKey resourceKey);
    }
}