using System.Windows;

namespace GitlabManager.Theme
{
    public static class ThemeConstants
    {
        
        /**
         * App Colors
         */
        public static ComponentResourceKey SuccessColor 
            => new ComponentResourceKey(typeof (AdonisUI.Colors), nameof (SuccessColor));

        public static ComponentResourceKey ErrorColor 
            => new ComponentResourceKey(typeof (AdonisUI.Colors), nameof (ErrorColor));
        
        public static ComponentResourceKey ErrorLightColor
            => new ComponentResourceKey(typeof (ThemeConstants), nameof (ErrorLightColor));
        
        /**
         * App Brushes
         */
        public static ComponentResourceKey SuccessBrush 
            => new ComponentResourceKey(typeof (AdonisUI.Brushes), nameof (SuccessBrush));

        public static ComponentResourceKey ErrorBrush 
            => new ComponentResourceKey(typeof (AdonisUI.Brushes), nameof (ErrorBrush));
     
        public static ComponentResourceKey ErrorLightBrush 
            => new ComponentResourceKey(typeof (ThemeConstants), nameof (ErrorLightBrush));
        
    }
}