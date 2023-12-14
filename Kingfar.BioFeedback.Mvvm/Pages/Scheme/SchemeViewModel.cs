using ImTools;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.Mvvm.Pages
{
    public partial class SchemeViewModel : PaginationViewModelBase<Services.SchemeDto>
    {
        #region Service

        private readonly ISchemeService _schemeService;

        #endregion Service

        #region Property

        [ObservableProperty]
        private List<TrainTypeOption> _trainTypes = default!;

        [ObservableProperty]
        private List<TrainApplicationTypeOption> _trainApplicationTypes = default!;

        [ObservableProperty]
        private List<TrainTypeOption>? _selectedTrainAppliactionTypes = default!;

        [ObservableProperty]
        private List<TrainTypeOption>? _selectedTrainTypes = default!;

        [ObservableProperty]
        private string _searchText = default!;

        #endregion Property

        public SchemeViewModel(ISchemeService schemeService)
        {
            initSeachTypeOptions();
            _schemeService = schemeService;
            CurrentPageChanged(true);
        }

        private void initSeachTypeOptions()
        {
            TrainTypes = SearchOptionConst.SchemeTypeOptions;
            TrainApplicationTypes = SearchOptionConst.SchemeApplicationTypeOptions;
            AddTrainTypes = DeepClone(SearchOptionConst.SchemeTypeOptions);
            AddTrainApplicationTypes = DeepClone(SearchOptionConst.SchemeApplicationTypeOptions);
        }

        [RelayCommand]
        private async Task TestAddTrain()
        {
            AddOrUpdateSchemeDto addOrUpdateTrainDto = new AddOrUpdateSchemeDto()
            {
                Name = "测试" + DateTime.Now,
                Type = TrainType.Individual,
                ApplicationTypes = new List<TrainApplicationType> {
                    TrainApplicationType.HRV,TrainApplicationType.Relax
                },
            };

            await _schemeService.AddOrUpdateTrainAsync(addOrUpdateTrainDto);
            CurrentPageChanged(true);
        }

        [RelayCommand]
        private async Task OnAddSchemeSet()
        {
            var uiMessageBox = new Wpf.Ui.Controls.MessageBox
            {
                Title = "打开Expert",
                Content =
                "编辑训练内容，调用Expert实现",
            };

            var result = await uiMessageBox.ShowDialogAsync();
        }

        public override void CurrentPageChanged(bool isNeedTotalCount = false)
        {
            var selectedTrainType = TrainTypes.Where(e => e.IsSelected).Select(e => e.Type).ToList();
            var selectedAppType = TrainApplicationTypes.Where(e => e.IsSelected).Select(e => e.Type).ToList();
            var result = _schemeService.GetListByPage((Current - 1) * CountPerPage, CountPerPage, SearchText, selectedTrainType, selectedAppType, isNeedTotalCount);
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (isNeedTotalCount)
                {
                    TotalCount = result.TotalCount;
                    if (Current != 1)
                        Current = 1;
                }
                Source = result.Data;
            });
        }

        [RelayCommand]
        private async Task Delete(Services.SchemeDto dto)
        {
            await DeleteData($"确定要删除{dto.Name}么？", () => _schemeService.TombstonedByIdAsync(dto.Id));
        }

        public override void ClearItem()
        {
            TrainTypes.ForEach(e => e.IsSelected = false);
            TrainApplicationTypes.ForEach(e => e.IsSelected = false);
            SearchText = string.Empty;
        }
    }

    public partial class SchemeViewModel : PaginationViewModelBase<SchemeDto>
    {
        [ObservableProperty]
        private string _schemeName = default!;

        [ObservableProperty]
        private object _addSchemeDialog = default!;

        [ObservableProperty]
        private List<TrainTypeOption> _addTrainTypes = default!;

        [ObservableProperty]
        private List<TrainApplicationTypeOption> _addTrainApplicationTypes = default!;

        private Guid? _checkEntityDataId = null;

        [RelayCommand]
        private async Task AddScheme(object? id)
        {
            AddOrUpdateSchemeDto addOrUpdateTrainDto = await InitDialogProperty(id);

            if (addOrUpdateTrainDto != null)
            {
                var result = await ShowDialogAsync(id is null ? "添加训练" : "编辑训练", AddSchemeDialog, CheckAddData);
                if (result == ContentDialogResult.Primary)
                {
                    addOrUpdateTrainDto.SetValue(SchemeName, AddTrainTypes.First(e => e.IsSelected).Type, AddTrainApplicationTypes.Where(e => e.IsSelected).Select(e => e.Type).ToList());
                    await _schemeService.AddOrUpdateTrainAsync(addOrUpdateTrainDto);
                    CurrentPageChanged(true);
                }
            }
        }

        private async Task<AddOrUpdateSchemeDto> InitDialogProperty(object? id)
        {
            if (id is not null && Guid.TryParse(id.ToString(), out Guid trainId))
            {
                _checkEntityDataId = trainId;
                var addOrUpdateTrainDto = await _schemeService.GetByIdAsync<AddOrUpdateSchemeDto>(trainId);
                SchemeName = addOrUpdateTrainDto.Name;
                AddTrainTypes.ForEach(e => e.IsSelected = e.Type == addOrUpdateTrainDto.Type ? true : false);
                AddTrainApplicationTypes.ForEach(e => e.IsSelected = addOrUpdateTrainDto.ApplicationTypes.Contains(e.Type));
                return addOrUpdateTrainDto;
            }
            else
            {
                SchemeName = string.Empty;
                AddTrainTypes.ForEach(e => e.IsSelected = false);
                AddTrainApplicationTypes.ForEach(e => e.IsSelected = false);
                return new AddOrUpdateSchemeDto();
            }
        }

        private async Task<bool> CheckAddData()
        {
            if (string.IsNullOrWhiteSpace(SchemeName))
            {
                SnackbarService.Show("错误", "训练名称不能为空", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
                return false;
            }
            if (!string.IsNullOrWhiteSpace(SchemeName) && await _schemeService.ExistsByNameAsync(SchemeName, _checkEntityDataId))
            {
                SnackbarService.Show("错误", "训练名称已存在", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
                return false;
            }
            if (AddTrainTypes.Count(e => e.IsSelected) <= 0)
            {
                SnackbarService.Show("错误", "未选择训练类型", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
                return false;
            }
            if (AddTrainApplicationTypes.Count(e => e.IsSelected) <= 0)
            {
                SnackbarService.Show("错误", "未选择应用类型", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
                return false;
            }

            return true;
        }
    }
}