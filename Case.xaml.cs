using System;
using System.Collections.Generic;
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
    public partial class Case : UserControl
    {
        public int val { get; set; }
        public bool hasMovedThisRound { get; set; }
        public Case()
        {
            InitializeComponent();
        }

        public void Init(int val)
        {
            this.val = val;
            this.Value.Content = val;
            this.background.Fill = new SolidColorBrush(Color.FromScRgb(1, (float)(val % 7)/7, (float)(val % 8)/8, (float)(val % 3)/3));
        }
        public void Merge()
        {
            this.val = this.val*2;
            this.Init(this.val);
        }
    }
}
