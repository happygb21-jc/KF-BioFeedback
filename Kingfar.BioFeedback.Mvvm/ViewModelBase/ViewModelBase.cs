using System.Text.Json;
using System.Windows.Input;
using Prism.Ioc;

namespace Kingfar.BioFeedback.Mvvm
{
    public class ViewModelBase : WPFCommon.ViewModels.ViewModelBase
    {
        public override Type? GetRegisteredType(string name)
        {
            if (!AppViews.ViewTypeDic.TryGetValue(name, out var item)) return null;

            return item;
        }
    }
}