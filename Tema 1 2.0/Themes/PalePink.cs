
namespace Tema_1.Themes
{
    internal class PalePink : ThemeMode
    {
        #region Variables
        private static PalePink? _instance;
        #endregion

        #region Functions
        private PalePink()
        {
            MenuBackgroundColor = "#FB6376";
            MenuItemBackgroundColor = "#FFDCCC";
            MenuItemForegroundColor = "#1E1E1E";
            BackgroundColor = "#FFF9EC";
            CanvasBackgroundColor = "#FFDCCC";
            CanvasBorderBrush = "#5D2A42";
            TextForeground = "#1E1E1E";
            ButtonBackgroundColor = "#FFDCCC";
            ButtonForegroundColor = "#1E1E1E";
            ButtonBorderBrush = "#FCB1A6";
        }
        public static PalePink Instance
        {
            get
            {
                return _instance ?? (_instance = new PalePink());
            }
        }
        #endregion
    }
}
