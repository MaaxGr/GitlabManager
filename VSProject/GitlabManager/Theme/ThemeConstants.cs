using System.Windows;

namespace GitlabManager.Theme
{
    /// <summary>
    /// Constants for the application theme that are used globally in multiple xaml files.
    /// </summary>
    public static class ThemeConstants
    {

        #region FontSizes and Widths

        public static ComponentResourceKey DefaultFontSize =>
            new ComponentResourceKey(typeof(ThemeConstants), nameof(DefaultFontSize));
        
        public static ComponentResourceKey PageHeaderFontSize =>
            new ComponentResourceKey(typeof(ThemeConstants), nameof(PageHeaderFontSize));
        
        public static ComponentResourceKey SmallButtonWidth =>
            new ComponentResourceKey(typeof(ThemeConstants), nameof(SmallButtonWidth));
        
        #endregion

        #region Thicknesses

        public static ComponentResourceKey WidgetInnerThickness
            => new ComponentResourceKey(typeof(ThemeConstants), nameof(WidgetInnerThickness));

        #endregion
        
        #region App Colors

        public static ComponentResourceKey AccentColor 
            => new ComponentResourceKey(typeof (AdonisUI.Colors), nameof (AccentColor));
        
        public static ComponentResourceKey AccentLightColor
            => new ComponentResourceKey(typeof (AdonisUI.Brushes), nameof (AccentLightColor));
        
        public static ComponentResourceKey SuccessColor 
            => new ComponentResourceKey(typeof (AdonisUI.Colors), nameof (SuccessColor));

        public static ComponentResourceKey ErrorColor 
            => new ComponentResourceKey(typeof (AdonisUI.Colors), nameof (ErrorColor));
        
        public static ComponentResourceKey ErrorLightColor
            => new ComponentResourceKey(typeof (ThemeConstants), nameof (ErrorLightColor));

        public static ComponentResourceKey WidgetBackgroundColor
            => new ComponentResourceKey(typeof(ThemeConstants), nameof(WidgetBackgroundColor));
        
        #endregion


        #region App Brushes

        public static ComponentResourceKey AccentBrush 
            => new ComponentResourceKey(typeof (AdonisUI.Brushes), nameof (AccentBrush));
        
        public static ComponentResourceKey AccentLightBrush 
            => new ComponentResourceKey(typeof (AdonisUI.Brushes), nameof (AccentLightBrush));
        
        public static ComponentResourceKey SuccessBrush 
            => new ComponentResourceKey(typeof (AdonisUI.Brushes), nameof (SuccessBrush));

        public static ComponentResourceKey ErrorBrush 
            => new ComponentResourceKey(typeof (AdonisUI.Brushes), nameof (ErrorBrush));
     
        public static ComponentResourceKey ErrorLightBrush 
            => new ComponentResourceKey(typeof (ThemeConstants), nameof (ErrorLightBrush));

        public static ComponentResourceKey WidgetBackgroundBrush
            => new ComponentResourceKey(typeof(ThemeConstants), nameof(WidgetBackgroundBrush));

        
        #endregion

        
    }
}