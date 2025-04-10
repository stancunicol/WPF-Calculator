
namespace Tema_1.Themes
{
    internal class LavenderViolet : ThemeMode
    {
        #region Variables
        private static LavenderViolet? _instance;
        #endregion

        #region Functions
        private LavenderViolet()
        {
            MenuBackgroundColor = "#5E548E";
            MenuItemBackgroundColor = "#BE95C4";
            MenuItemForegroundColor = "#1E1E1E";
            BackgroundColor = "#E0B1CB";
            CanvasBackgroundColor = "#BE95C4";
            CanvasBorderBrush = "#231942";
            TextForeground = "#1E1E1E";
            ButtonBackgroundColor = "#BE95C4";
            ButtonForegroundColor = "#1E1E1E";
            ButtonBorderBrush = "#9F86C0";
        }

        public static LavenderViolet Instance
        {
            get
            {
                return _instance ?? (_instance = new LavenderViolet());
            }
        }
        #endregion
    }
}