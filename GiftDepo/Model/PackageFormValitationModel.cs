using System.ComponentModel;

namespace GiftDepo.Model
{
    public class PackageFormValitationModel : BaseViewModel
    {        
        private int _width;
        private int _height;
        private bool _hasNoErrors = false;
        private int _quantity = 1;

        
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    RaisePropertyChanged("Width");
                }
            }
        }
        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    RaisePropertyChanged("Height");
                }
            }
        }


        public bool HasNoErrors
        {
            get
            {
                return _hasNoErrors;
            }
            set
            {
                if (_hasNoErrors != value)
                {
                    _hasNoErrors = value;
                    RaisePropertyChanged("HasNoErrors");
                }
            }
        }

        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    RaisePropertyChanged("Quantity");
                }
            }
        }
    }
}
