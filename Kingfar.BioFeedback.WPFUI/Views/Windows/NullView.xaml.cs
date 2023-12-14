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

namespace Kingfar.BioFeedback.WPFUI
{
    /// <summary>
    /// NullPage.xaml 的交互逻辑
    /// 由于CreateShell 为空时会关闭 应用，所以临时使用空界面占位后续解决
    /// </summary>
    public partial class NullView : FluentWindow
    {
        public NullView()
        {
            InitializeComponent();
        }
    }
}
