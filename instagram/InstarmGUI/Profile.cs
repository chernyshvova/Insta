using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InstarmGUI
{
    public class Profile : INotifyPropertyChanged
    {
        public string name { get; set; }
        public string password { get; set; }
        public string tag { get; set; }
        public string proxyHost { get; set; }
        public string proxyPort { get; set; }
        public string proxyUsername { get; set; }
        public string proxyPassword { get; set; }

        private bool _isFiltered;
        public bool IsFiltered
        {
            get { return _isFiltered; }
            set { _isFiltered = value; RaisePropertyChanged("IsFiltered"); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; RaisePropertyChanged("IsSelected"); }
        }
        

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        internal void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}