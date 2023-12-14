using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Kingfar.BioFeedback.WPFCommon.Controls
{
    /// <summary>
    /// ConfirmDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ConfirmDialog : ContentDialog
    {
        public ConfirmDialog(ContentPresenter contentPresenter, string message, string primaryText = "确认", string secondaryText = "") : base(contentPresenter)
        {
            InitializeComponent();
            PrimaryButtonText = primaryText;
            SecondaryButtonText = secondaryText;
            Message.Text = message;
        }
    }
}