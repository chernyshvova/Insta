using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace InstarmGUI
{
    class Commands : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged("Commands");}
        }


        public Command cmd { get; set; }


        public static List<Commands> GetCommandsList()
        {
            List<Commands> list = new List<Commands>();
            list.Add(new Commands() { Name = "Опубликовать пост", cmd = Command.Command1 });     //Заменить на константы хранящиеся через контракт
            list.Add(new Commands() { Name = "Лайк", cmd = Command.Command2 });    
            list.Add(new Commands() { Name = "Лайк хэштега", cmd = Command.Command3 });
            list.Add(new Commands() { Name = "Комментировать", cmd = Command.Command4 });
            list.Add(new Commands() { Name = "Сменить аватар", cmd = Command.Command5 });
            list.Add(new Commands() { Name = "Подписаться", cmd = Command.Command6 });
            list.Add(new Commands() { Name = "Отписаться", cmd = Command.Command7 });
            list.Add(new Commands() { Name = "Сообщение директ", cmd = Command.Command8 });
            return list;
        }
        public static string GetCmd(Command cmd)
        {
            switch (cmd)
            {
                case Command.Command1:
                    return "-post --path";            //Заменить на константы хранящиеся через контракт
                case Command.Command2:
                    return "-like --single";
                case Command.Command3:
                    return "-like --mass";
                case Command.Command4:
                    return "-comment";
                case Command.Command5:
                    return "-avatar --path";
                case Command.Command6:
                    return "-follow";
                case Command.Command7:
                    return "-unfollow";
                case Command.Command8:
                    return "-direct --send";
                default:
                    return string.Empty;
            }
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
        protected virtual void NotifyPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string propertyName in propertyNames) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                PropertyChanged(this, new PropertyChangedEventArgs("HasError"));
            }
        }


        #endregion
    }
    public enum Command { Command1, Command2, Command3, Command4, Command5, Command6, Command7, Command8, Command9 , Default}

}
