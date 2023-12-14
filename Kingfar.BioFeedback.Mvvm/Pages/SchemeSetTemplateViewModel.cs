using System.Threading.Tasks;
using System;
using SqlSugar;
using System.Xml.Linq;

namespace Kingfar.BioFeedback.Mvvm.Pages;

public partial class SchemeSetTemplateViewModel : PaginationViewModelBase<SchemeSetTemplateDto>
{
    #region Service

    private readonly ISchemeSetTemplateService _schemeSetTemplateService;

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
    private object _addSchemeSetDialog;

    [ObservableProperty]
    private string _schemeSetName = string.Empty;

    #endregion Property

    public SchemeSetTemplateViewModel(ISchemeSetTemplateService schemeSetTemplateService)
    {
        _schemeSetTemplateService = schemeSetTemplateService;
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
    private async Task Delete(SchemeSetTemplateDto dto)
    {
        await DeleteData($"确定要删除 {dto.Name} 模板吗？", () => _schemeSetTemplateService.DeleteByIdAsync(dto.Id));
    }

    [RelayCommand]
    private async Task CopyToSchemeSet(SchemeSetTemplateDto dto)
    {
        //增加数据方案名称
        var result = await ShowDialogAsync("创建方案", AddSchemeSetDialog, CheckDialogData);
        if (result == ContentDialogResult.Primary)
        {
            if (await _schemeSetTemplateService.CreateToSchemeSetAsync(dto.Id, SchemeSetName))
            {
                SnackbarService.Show("提示", "模板创建成功", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
            }
        }
    }

    private bool CheckDialogData()
    {
        if (string.IsNullOrWhiteSpace(SchemeSetName))
        {
            SnackbarService.Show("错误", "方案名称不能为空", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
            return false;
        }
        return true;
    }

    public override void CurrentPageChanged(bool isNeedTotalCount = false)
    {
        SchemeSetType? schemeType = SchemeTypeSelectedIndex == -1 ? null : SchemeTypeSelectedIndex == 0 ? SchemeSetType.Individual : SchemeSetType.Group;
        var result = _schemeSetTemplateService.GetListByPage((Current - 1) * CountPerPage, CountPerPage, SearchText, schemeType, isNeedTotalCount);
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