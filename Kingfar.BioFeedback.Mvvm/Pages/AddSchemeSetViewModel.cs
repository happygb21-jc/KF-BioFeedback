using ImTools;
using Kingfar.BioFeedback.Mvvm.Dto;
using Kingfar.BioFeedback.Shared.Abstractions.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kingfar.BioFeedback.Mvvm.Pages
{
    public partial class AddSchemeSetViewModel : ViewModelBase
    {
        #region Service

        private readonly ISchemeService _schemeService;
        private readonly ISchemeSetService _schemeSetService;

        #endregion Service

        #region Property

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private object _dialogContent = default!;

        [ObservableProperty]
        private ObservableCollection<UserDto> patients = default!;

        [ObservableProperty]
        private Visibility _nextBtnVisibility = Visibility.Visible;

        [ObservableProperty]
        private Visibility _backBtnVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private int _tabSelectIndex = 0;

        [ObservableProperty]
        private List<TrainTypeOption> _trainTypes = default!;

        [ObservableProperty]
        private List<TrainApplicationTypeOption> _trainApplicationTypes = default!;

        [ObservableProperty]
        private ObservableCollection<SchemeDto> _schemeSets = default!;

        [ObservableProperty]
        private SelectionMode _patientListViewSelectionMode = SelectionMode.Single;

        [ObservableProperty]
        private IList<Services.SchemeDto> _source = default!;

        [ObservableProperty]
        private string _schemeSetName = string.Empty;

        #region RadioButton Property

        private bool _personChecked = true;

        public bool PersonChecked
        {
            get => _personChecked;
            set
            {
                _personChecked = value;
                RadioButtonChecked();
                OnPropertyChanged();
            }
        }

        #endregion RadioButton Property

        #endregion Property

        private int _current = 1;
        private int _pageSize = 30;
        private int _totalCount = 0;

        public AddSchemeSetViewModel(ISchemeService schemeService, ISchemeSetService schemeSetService)
        {
            _schemeService = schemeService;
            _schemeSetService = schemeSetService;
            TrainTypes = SearchOptionConst.SchemeTypeOptions.Where(e => e.Type != TrainType.Group).ToList();
            TrainApplicationTypes = SearchOptionConst.SchemeApplicationTypeOptions;
            CurrentPageChanged(true);
            SelectedItems = new ObservableCollection<SchemeDto>();
        }

        private void CurrentPageChanged(bool isNeedTotalCount = false)
        {
            var selectedTrainType = TrainTypes.Where(e => e.IsSelected).Select(e => e.Type).ToList();
            var selectedAppType = TrainApplicationTypes.Where(e => e.IsSelected).Select(e => e.Type).ToList();
            var result = _schemeService.GetListByPage((_current - 1) * _pageSize, _pageSize, SearchText, selectedTrainType, selectedAppType, isNeedTotalCount);
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (isNeedTotalCount)
                {
                    _totalCount = result.TotalCount;
                    if (_current != 1)
                        _current = 1;
                }
                Source = result.Data;
            });
        }

        private void RadioButtonChecked()
        {
            if (PersonChecked)
            {
                TrainTypes = SearchOptionConst.SchemeTypeOptions.Where(e => e.Type != TrainType.Group).ToList();
            }
            else
            {
                TrainTypes = SearchOptionConst.SchemeTypeOptions.Where(e => e.Type != TrainType.Individual).ToList();
            }
        }

        [RelayCommand]
        private void SchemeSearch()
        {
            CurrentPageChanged(true);
        }

        [RelayCommand]
        private void SchemeClear()
        {
            TrainTypes.ForEach(e => e.IsSelected = false);
            TrainApplicationTypes.ForEach(e => e.IsSelected = false);
            SearchText = string.Empty;
            CurrentPageChanged(true);
        }

        [RelayCommand]
        private void OnGoBack()
        {
            NavigationService.GoBack();
        }

        [RelayCommand]
        private async Task AddSchemeSet()
        {
            if (SelectedItems.Count == 0)
            {
                //SnackbarService.Show("错误", "未选择训练内容", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
                AppLogs.Debug("未选择训练内容");
                return;
            }
            var schemeSetType = PersonChecked ? Shared.SchemeSetType.Individual : Shared.SchemeSetType.Group;
            var dto = await _schemeSetService.AddOrUpdateTrainAsync(new AddOrUpdateSchemeSetDto(SchemeSetName, schemeSetType, SelectedItems.Select(i => i.Id).ToArray()));

            if (dto is not null)
            {
                NavigationService.Navigate(AppViews.SchemeSet);
            }
        }
    }

    public partial class AddSchemeSetViewModel : ViewModelBase
    {
        [ObservableProperty]
        public ObservableCollection<Services.SchemeDto> _selectedItems;

        // 删除项的方法
        [RelayCommand]
        private void DeleteSelectedItem(SchemeDto dto)
        {
            if (SelectedItems.Contains(dto))
            {
                SelectedItems.Remove(dto);
            }
        }
    }
}