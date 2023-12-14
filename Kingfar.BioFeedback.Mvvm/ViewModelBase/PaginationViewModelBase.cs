using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.Mvvm
{
    public abstract partial class PaginationViewModelBase<TEntity> : ViewModelBase
    {
        [ObservableProperty]
        private IList<TEntity> _source;

        [ObservableProperty]
        private int _totalCount;

        protected bool IsNeedTotalCount = false;

        private int _countPerPage = 20;

        public int CountPerPage
        {
            get { return _countPerPage; }
            set { _countPerPage = value; this.OnPropertyChanged("CountPerPage"); CurrentPageChanged(true); }
        }

        private int _current = 1;

        public int Current
        {
            get { return _current; }
            set { _current = value; this.OnPropertyChanged("Current"); CurrentPageChanged(); }
        }

        [RelayCommand]
        private void Search()
        {
            CurrentPageChanged(true);
        }

        [RelayCommand]
        private void Clear()
        {
            ClearItem();
            CurrentPageChanged(true);
        }

        public abstract void CurrentPageChanged(bool isNeedTotalCount = false);

        public abstract void ClearItem();

        public async Task DeleteData(string message, Func<Task<bool>> func)
        {
            if (await ShowConfirmDialogAsync(message) == ContentDialogResult.Primary)
            {
                var result = await func();
                if (result)
                {
                    SnackbarService.Show("提示", "删除成功", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
                    //if (Source.Count > 1)
                    //{
                    //    CurrentPageChanged();
                    //}
                    //else
                    //{
                    //    if (Current > 1)
                    //    {
                    //        Current = Current - 1;
                    //    }
                    //    else
                    //    {
                    //        CurrentPageChanged(true);
                    //    }
                    //}
                    CurrentPageChanged(true);
                }
            }
        }
    }
}