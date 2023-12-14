using System.Threading.Tasks;
using System;
using SqlSugar;

namespace Kingfar.BioFeedback.Mvvm.Pages;

public partial class SchemeSetViewModel : PaginationViewModelBase<Services.SchemeSetDto>
{
    #region Service

    private readonly ISchemeSetService _schemeSetService;

    #endregion Service

    #region Property

    [ObservableProperty]
    private List<TrainTypeOption> _trainTypes;

    [ObservableProperty]
    private List<TrainApplicationTypeOption> _trainAppliactionTypes;

    [ObservableProperty]
    private string _dialogResultText = string.Empty;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private int _schemeTypeSelectedIndex = -1;

    [ObservableProperty]
    private int _publishStateSelectedIndex = -1;

    #endregion Property

    public SchemeSetViewModel(ISchemeSetService schemeSetService)
    {
        _schemeSetService = schemeSetService;
        initSeachTypeOptions();
        CurrentPageChanged(true);
    }

    private void initSeachTypeOptions()
    {
        TrainTypes = SearchOptionConst.SchemeTypeOptions;
        TrainAppliactionTypes = SearchOptionConst.SchemeApplicationTypeOptions;
    }

    public override void ClearItem()
    {
        SearchText = string.Empty;
        SchemeTypeSelectedIndex = -1;
        PublishStateSelectedIndex = -1;
    }

    [RelayCommand]
    private void GoAddPage()
    {
        var nav = NavigationService.GetNavigationControl();
        if (nav != null)
        {
            nav.NavigateWithHierarchy(GetRegisteredType(AppViews.AddSchemeSet));
        }
    }

    [RelayCommand]
    private void TrainManagement()
    {
        var nav = NavigationService.GetNavigationControl();
        if (nav != null)
        {
            nav.NavigateWithHierarchy(GetRegisteredType(AppViews.Scheme));
        }
    }

    [RelayCommand]
    private async Task Delete(SchemeSetDto dto)
    {
        await DeleteData($"确定要删除 {dto.Name} 吗？", () => _schemeSetService.TombstonedByIdAsync(dto.Id));
    }

    [RelayCommand]
    private async Task CopyToTemp(SchemeSetDto dto)
    {
        var checkResult = await _schemeSetService.CheckAssociatedAsync(dto.Id);
        Guid? targetId = null;
        if (checkResult is not null)
        {
            var dialogResult = await ShowConfirmDialogAsync($"方案集 {dto.Name} 的模板已存在，是否覆盖？");
            if (dialogResult == ContentDialogResult.Primary)
            {
                targetId = checkResult.Value;
            }
            else
            {
                return;
            }
        }
        var result = await _schemeSetService.CopyToTemplateAsync(dto.Id, targetId);
        if (result)
        {
            SnackbarService.Show("提示", "模板创建成功", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
        }
    }

    [RelayCommand]
    private void SelectDetail(SchemeSetDto dto)
    {
        var nav = NavigationService.GetNavigationControl();
        if (nav != null)
        {
            nav.NavigateWithHierarchy(GetViewType(AppViews.SchemeSetDetail)!, new SchemeSetDetailViewModel(dto.Id, _schemeSetService));
        }
    }

    public override void CurrentPageChanged(bool isNeedTotalCount = false)
    {
        SchemeSetType? schemeType = SchemeTypeSelectedIndex == -1 ? null : SchemeTypeSelectedIndex == 0 ? SchemeSetType.Individual : SchemeSetType.Group;
        bool? isPublished = PublishStateSelectedIndex == -1 ? null : PublishStateSelectedIndex == 0;
        var result = _schemeSetService.GetListByPage((Current - 1) * CountPerPage, CountPerPage, SearchText, schemeType, isPublished, isNeedTotalCount);
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
}