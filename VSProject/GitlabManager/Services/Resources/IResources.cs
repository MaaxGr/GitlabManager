using System.Windows;
using System.Windows.Media;

namespace GitlabManager.Services.Resources
{
    /// <summary>
    /// Interfaces for access to wpf resources (e.g. brushes)
    /// </summary>
    public interface IResources
    {
        /// <summary>
        /// Get a Brush by specified resource key
        /// </summary>
        /// <param name="resourceKey">Resource key</param>
        /// <returns>corresponding brush or null</returns>
        public SolidColorBrush GetBrush(ComponentResourceKey resourceKey);
    }
}