using InstarmGUI.Database;
using InstarmGUI.Helpers;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace InstarmGUI.ViewModel
{
    class ProfileViewModel : ViewModelBase
    {
        public RelayCommand DoStuffCommand { get; set; }
        public RelayCommand SelectAllCommand { get; set; }
        public RelayCommand ClearAllCommand { get; set; }
        public RelayCommand PathCommand { get; set; }
        public RelayCommand PathExeCommand { get; set; }
        public RelayCommand PathDbCommand { get; set; }
        public List<Profile> ProfilesList { get; set; }
        public List<Commands> CommandsList { get; set; }
        private DbHelperSQLite dbHelper = new DbHelperSQLite();
        private Commands _currentSelection;
        private Command currentState;
        private bool _isActive;
        private string _data;
        private string _message;
        private string _path;

        private string _pathExe;
        private string _pathDb;

        #region ComboBox Variables
        private bool _panelPost;
        private bool _panelLike;
        private bool _panelLikeMass;
        private bool _panelComment;
        private bool _panelAvatar;
        private bool _panelFollow;
        private bool _panelUnfollow;
        private bool _panelDirect;
        #endregion


        #region Constructor

        public ProfileViewModel()
        {
            PathExe = Properties.Settings.Default.PathToExe;
            PathDb = Properties.Settings.Default.PathToDb;

            _isActive = false;
            ProfilesList = dbHelper.GetProfiles(PathDb);
             CommandsList = Commands.GetCommandsList();
            Items = CollectionViewSource.GetDefaultView(ProfilesList);
            Items.Filter = FilterProfile;
            DoStuffCommand = new RelayCommand(ExecuteCommand, DoStuffCanExecute);
            SelectAllCommand = new RelayCommand(SelectAll);
            ClearAllCommand = new RelayCommand(ClearAll, ClearCanExecute);
            PathCommand = new RelayCommand(PathButton);
            PathExeCommand = new RelayCommand(PathExeBtn);
            PathDbCommand = new RelayCommand(PathDbBtn);
        }
        #endregion

        #region Filter


        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }

        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterText", typeof(string), typeof(ProfileViewModel), new PropertyMetadata("", FilterText_Changed));

        private static void FilterText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProfileViewModel current = d as ProfileViewModel;
            if (current != null)
            {
                current.Items.Filter = null;
                current.Items.Filter = current.FilterProfile;
            }
        }


        public ICollectionView Items
        {
            get { return (ICollectionView)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value);          }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ICollectionView), typeof(ProfileViewModel), new PropertyMetadata(null));



        private bool FilterProfile(object obj)
        {
            bool result = true;
            Profile current = obj as Profile;
            if (!string.IsNullOrWhiteSpace(FilterText) && current != null && !current.name.Contains(FilterText) && !current.tag.Contains(FilterText))  //Добавить фильтр по прокси
            {
                //  current.IsSelected = false;
                current.IsFiltered = true;
                result = false;
            }
            else
            {
                current.IsFiltered = false;
            }
            return result;
        }
        bool ClearCanExecute(object param)
        {
            foreach (var obj in ProfilesList)
                if (obj.IsSelected) return true;
            return false;
        }
        bool DoStuffCanExecute(object param)
        {
            foreach (var obj in ProfilesList)
                if (obj.IsSelected && CheckFieldsPredicate()) return true;
            return false;
        }

        #endregion


        #region ComboBox Select

        public bool ShowButton
        {
            get { return _isActive; }
            set { _isActive = value; RaisePropertyChanged("ShowButton"); }
        }

        public bool ShowPanelPost
        {
            get { return _panelPost; }
            set {  _panelPost = value; RaisePropertyChanged("ShowPanelPost"); }
        }
        public bool ShowPanelLike
        {
            get { return _panelLike; }
            set { _panelLike = value; RaisePropertyChanged("ShowPanelLike"); }
        }
        public bool ShowPanelLikeMass
        {
            get { return _panelLikeMass; }
            set { _panelLikeMass = value; RaisePropertyChanged("ShowPanelLikeMass"); }
        }
        public bool ShowPanelComment
        {
            get { return _panelComment; }
            set { _panelComment = value; RaisePropertyChanged("ShowPanelComment"); }
        }
        public bool ShowPanelAvatar
        {
            get { return _panelAvatar; }
            set { _panelAvatar = value; RaisePropertyChanged("ShowPanelAvatar"); }
        }
        public bool ShowPanelFollow
        {
            get { return _panelFollow; }
            set { _panelFollow = value; RaisePropertyChanged("ShowPanelFollow"); }
        }
        public bool ShowPanelUnFollow
        {
            get { return _panelUnfollow; }
            set { _panelUnfollow = value; RaisePropertyChanged("ShowPanelUnFollow"); }
        }
        public bool ShowPanelDirect
        {
            get { return _panelDirect; }
            set { _panelDirect = value; RaisePropertyChanged("ShowPanelDirect"); }
        }
        #endregion

        #region Buttons

        void ExecuteCommand(object param)
        {
            Command bindedCmd = currentState;
            bool strValid = true;
            if (string.IsNullOrEmpty(PathExe))
            {
                MessageBox.Show("Error.Path to instarm.exe is empty!");
                strValid = false;
            }
            if (string.IsNullOrEmpty(PathExe))
            {
                MessageBox.Show("Error.Path to instagram.db is empty!");
                strValid = false;
            }
            if (strValid)
            {
                string workPath = PathDb.Replace(@"\instagram.db", "");
                foreach (var obj in ProfilesList)
                {
                    if (obj.IsSelected)
                    {
                        Process p = new Process();
                        p.StartInfo.FileName = PathExe;
                        p.StartInfo.Arguments = GenerateCmd(bindedCmd, obj.name);
                        p.StartInfo.WorkingDirectory = workPath;
                        p.Start();
                    }
                }
            }
        }

        private string GenerateCmd(Command cmd, string username)
        {
            string cmdToExecute = Commands.GetCmd(cmd) + " " + username;
            if (!string.IsNullOrEmpty(Data))
                cmdToExecute += " " + Data;
            if (!string.IsNullOrEmpty(Path))
                cmdToExecute += " " + Path;
            if (!string.IsNullOrEmpty(Message))
                cmdToExecute += " " + Message;
            return cmdToExecute;
        }
        private bool CheckFieldsPredicate()
        {
            switch (currentState)
            {
                case Command.Command1:
                    if (!string.IsNullOrEmpty(Message)&& !string.IsNullOrEmpty(Path))
                    {
                        return true;
                    }
                    return false;
                case Command.Command2:
                    if (!string.IsNullOrEmpty(Data))
                    {
                        return true;
                    }
                    return false;
                case Command.Command3:
                    if (!string.IsNullOrEmpty(Data))
                    {
                        return true;
                    }
                    return false;
                case Command.Command4:
                    if (!string.IsNullOrEmpty(Message) && !string.IsNullOrEmpty(Data))
                    {
                        return true;
                    }
                    return false;
                case Command.Command5:
                    if (!string.IsNullOrEmpty(Path))
                    {
                        return true;
                    }
                    return false;
                case Command.Command6:
                    if (!string.IsNullOrEmpty(Data))
                    {
                        return true;
                    }
                    return false;
                case Command.Command7:
                    if (!string.IsNullOrEmpty(Data))
                    {
                        return true;
                    }
                    return false;
                case Command.Command8:
                    if (!string.IsNullOrEmpty(Message) && !string.IsNullOrEmpty(Data))
                    {
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        void SelectAll(object param)
        {
            foreach (var obj in ProfilesList)
            {
                if (!obj.IsFiltered)
                {
                    obj.IsSelected = true;
                }
            }
        }
        void ClearAll(object param)
        {
            foreach (var obj in ProfilesList)
                obj.IsSelected = false;
        }

        public void PathButton(object param)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images | *.jpg; *.jpeg; *.png;|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                Path = openFileDialog.FileName;
            }
        }
        #endregion

        #region SidePanel

        public Commands CurrentSelection
        {
            get { return _currentSelection; }
            set
            {
                _currentSelection = value;
                OnPropertyChanged(nameof(CurrentSelection));
                SelectChoise(_currentSelection);
            }
        }
        
        private void CobmoSwitch()
        {
            ShowPanelPost = ShowPanelLike = ShowPanelLikeMass = ShowPanelComment = ShowPanelAvatar = ShowPanelFollow = ShowPanelUnFollow = ShowPanelDirect = false;
            Data = Message = Path = string.Empty;
    }

        private void SelectChoise(Commands command)
        {
            CobmoSwitch();
            switch (command.cmd)
            {
                case Command.Command1:
                    currentState = Command.Command1;
                    ShowPanelPost = true;
                    break;
                case Command.Command2:
                    currentState = Command.Command2;
                    ShowPanelLike = true;
                    break;
                case Command.Command3:
                    currentState = Command.Command3;
                    ShowPanelLikeMass = true;
                    break;
                case Command.Command4:
                    currentState = Command.Command4;
                    ShowPanelComment = true;
                    break;
                case Command.Command5:
                    currentState = Command.Command5;
                    ShowPanelAvatar = true;
                    break;
                case Command.Command6:
                    currentState = Command.Command6;
                    ShowPanelFollow = true;
                    break;
                case Command.Command7:
                    currentState = Command.Command7;
                    ShowPanelUnFollow = true;
                    break;
                case Command.Command8:
                    currentState = Command.Command8;
                    ShowPanelDirect = true;
                    break;
                default:
                    currentState = Command.Default;
                    break;
            }
        }

        #region Panel Data Controls
        public string Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged(nameof(Path));
            }
        }



        #endregion



        #endregion


        #region Settings

        public string PathExe
        {
            get { return _pathExe; }
            set
            {
                _pathExe = value;
                OnPropertyChanged(nameof(PathExe));
            }
        }
        public string PathDb
        {
            get { return _pathDb; }
            set
            {
                _pathDb = value;
                OnPropertyChanged(nameof(PathDb));
                ProfilesList = dbHelper.GetProfiles(PathDb);
                Items = CollectionViewSource.GetDefaultView(ProfilesList);
                OnPropertyChanged(nameof(Items));
            }
        }
       
        
        public void PathExeBtn(object param)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Executables | *.exe;|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                PathExe = openFileDialog.FileName;
                Properties.Settings.Default.PathToExe = PathExe; 
                Properties.Settings.Default.Save();
            }
        }
        public void PathDbBtn(object param)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Database | *.db;|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                PathDb = openFileDialog.FileName;
                Properties.Settings.Default.PathToDb = PathDb;
                Properties.Settings.Default.Save();
            }
        }
        
        
        #endregion
    }
}
