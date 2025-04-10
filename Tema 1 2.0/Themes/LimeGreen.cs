
namespace Tema_1.Themes
{
    internal class LimeGreen : ThemeMode
    {
        #region Variables
        private static LimeGreen? _instance;
        #endregion

        #region Functions
        private LimeGreen()
        {
            MenuBackgroundColor = "#2F9C95";
            MenuItemBackgroundColor = "#A3F7B5";
            MenuItemForegroundColor = "#1E1E1E";
            BackgroundColor = "#E5F9E0";
            CanvasBackgroundColor = "#A3F7B5";
            CanvasBorderBrush = "#664147";
            TextForeground = "#1E1E1E";
            ButtonBackgroundColor = "#A3F7B5";
            ButtonForegroundColor = "#1E1E1E";
            ButtonBorderBrush = "#40C9A2";
        }

        public static LimeGreen Instance
        {
            get
            {
                return _instance ?? (_instance = new LimeGreen());
            }
        }
        #endregion
    }
}