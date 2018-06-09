using System.ComponentModel;

namespace GiftDepo.ModelView
{
    public class AddPackageValitationModel : INotifyPropertyChanged
    {
        private int _width;
        private int _height;
        private bool _hasNoErrors = false;

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
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }        
    }
}
