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
    }
}