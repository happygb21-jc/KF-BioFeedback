using System;
using System.Text.Json;
using System.Threading.Tasks;
using Kingfar.BioFeedback.Mvvm.Controls;
using Prism.Ioc;

namespace Kingfar.BioFeedback.Mvvm
{
    public class ViewModelBase : ObservableObject, INavigationAware
    {
        protected readonly IContentDialogService ContentDialogService = ContainerLocator.Container.Resolve<IContentDialogService>();
        protected readonly INavigationService NavigationService = ContainerLocator.Container.Resolve<INavigationService>();
        protected readonly ISnackbarService SnackbarService = ContainerLocator.Container.Resolve<ISnackbarService>();

        public virtual void OnNavigatedTo()
        {
        }

        public virtual void OnNavigatedFrom()
        {
        }

        public virtual Type GetRegisteredType(string name)
        {
            return ContainerLocator.Container.Resolve<object>(name).GetType();
        }

        protected Type? GetViewType(string key)
        {
            if (!AppViews.ViewTypeDic.TryGetValue(key, out var item)) return null;

            return item;
        }

        internal Task<ContentDialogResult> ShowDialogAsync(string title, object dialogContent, Func<Task<bool>> checkDataFunc)
        {
            return new ValidationContentDialog(ContentDialogService.GetContentPresenter(), checkDataFunc)
            {
                Title = title,
                Content = dialogContent,
                PrimaryButtonText = "保存",
                CloseButtonText = "取消",
            }.ShowAsync();
        }

        internal Task<ContentDialogResult> ShowDialogAsync(string title, object dialogContent, Func<bool> checkDataFunc)
        {
            return new ValidationContentDialog(ContentDialogService.GetContentPresenter(), checkDataFunc)
            {
                Title = title,
                Content = dialogContent,
                PrimaryButtonText = "保存",
                CloseButtonText = "取消",
            }.ShowAsync();
        }

        internal Task<ContentDialogResult> ShowConfirmDialogAsync(string message, string primaryText = "确认", string secondaryText = "")
        {
            return new ConfirmDialog(ContentDialogService.GetContentPresenter(), message, primaryText, secondaryText).ShowAsync();
        }

        internal T DeepClone<T>(T obj)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            };

            string json = JsonSerializer.Serialize(obj, options);
            return JsonSerializer.Deserialize<T>(json, options)!;
        }
    }
}