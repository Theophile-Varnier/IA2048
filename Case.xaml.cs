using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2048
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class Case : INotifyPropertyChanged
    {
        private int _val;

        private SolidColorBrush _backColor;

        public int val
        {
            get
            {
                return _val;
            }
            set
            {
                _val = value;
            }
        }

        public SolidColorBrush backColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
            }
        }
        private bool _hasMovedThisRound;
        public bool hasMovedThisRound
        {
            get
            {
                return _hasMovedThisRound;
            }
            set
            {
                _hasMovedThisRound = value;
            }
        }
        public Case()
        {
            _val = 0;
            InitializeComponent();
            DataContext = this;
        }

        public void Init(int val)
        {
            this._val = val;
            RaisePropertyChanged("val");
            this._backColor = new SolidColorBrush(Color.FromScRgb(1, (float)(val % 7)/7, (float)(val % 8)/8, (float)(val % 3)/3));
            RaisePropertyChanged("backColor");
        }
        public void Merge()
        {
            this._val = this._val * 2;
            RaisePropertyChanged("val");
            this._backColor = new SolidColorBrush(Color.FromScRgb(1, (float)(_val % 7) / 7, (float)(_val % 8) / 8, (float)(_val % 3) / 3));
            RaisePropertyChanged("backColor");
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
