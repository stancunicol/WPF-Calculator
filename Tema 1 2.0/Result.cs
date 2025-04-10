
namespace Tema_1
{
    public class Result
    {
        #region Variables
        private string _subtotal = "0";
        #endregion

        #region Functions
        public string Subtotal
        {
            get
            {
                return _subtotal;
            }
            set
            {
                _subtotal = value;
            }
        }
        #endregion
    }
}