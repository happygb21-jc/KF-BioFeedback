﻿using Prism.Common;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Windows;

namespace Kingfar.BioFeedback.Prism.Common
{
    public class DialogHostService : DialogService, IHostDialogService
    {
        private readonly IContainerExtension _containerExtension;

        public DialogHostService(IContainerExtension containerExtension) : base(containerExtension)
        {
            _containerExtension = containerExtension;
        }

        public IDialogResult ShowWindow(string name)
        {
            IDialogResult dialogResult = new DialogResult(ButtonResult.None);

            var content = _containerExtension.Resolve<object>(name);

            if (!(content is Window dialogContent))
                throw new NullReferenceException("A dialog's content must be a Window");

            if (dialogContent is Window view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
                ViewModelLocator.SetAutoWireViewModel(view, true);

            if (!(dialogContent.DataContext is IDialogAware viewModel))
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogAware interface");

            if (dialogContent is IDialogWindow dialogWindow)
            {
                ConfigureDialogWindowEvents(dialogWindow, result => { dialogResult = result; });
            }

            MvvmHelpers.ViewAndViewModelAction<IDialogAware>(viewModel, d => d.OnDialogOpened(null));
            dialogContent.ShowDialog();
            return dialogResult;
        }
    }
}