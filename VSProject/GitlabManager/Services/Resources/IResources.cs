using System.Windows;
using System.Windows.Media;

namespace GitlabManager.Services.Resources
{
    /// <summary>
    /// Interfaces for access to wpf resources (e.g. brushes)
    /// </summary>
    public interface IResources
    {
        public SolidColorBrush GetBrush(ComponentResourceKey resourceKey);
    }
}