using Kingfar.BioFeedback.Entites;
using Kingfar.BioFeedback.Services;
using System;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.Mvvm.Pages;

public partial class SchemeSetDetailViewModel : PaginationViewModelBase<AppUserDto>
{
    private readonly ISchemeSetService _schemeSetService;

    [ObservableProperty]
    private string _schemeSetName;

    [ObservableProperty]
    private SchemeSetType _schemeSetType;

    [ObservableProperty]
    private string _trainName;

    [ObservableProperty]
    private string _startBtnName;

    [ObservableProperty]
    private ControlAppearance _startBtnType;

    [ObservableProperty]
    private List<TrainDetialDto>? _trains;

    [ObservableProperty]
    private List<ExperimentDto>? _experiments;

    [ObservableProperty]
    private List<AppUserDto>? _subject;

    private int _trainSelectedIndex { get; set; } = 0;
    private SchemeSetDetailDto _schemeSetDetailDto;

    public int TrainSelectedIndex
    {
        get { return _trainSelectedIndex; }
        set { _trainSelectedIndex = value; this.OnPropertyChanged("TrainSelectedIndex"); ChangePageData(); }
    }

    public SchemeSetDetailViewModel(Guid schemeId, ISchemeSetService schemeSetService)
    {
        _schemeSetService = schemeSetService;
        init(schemeId);
    }

    private async void init(Guid schemeId)
    {
        _schemeSetDetailDto = await _schemeSetService.GetByIdAsync(schemeId);
        SchemeSetName = _schemeSetDetailDto.Name;
        SchemeSetType = _schemeSetDetailDto.Type;
        Trains = _schemeSetDetailDto.Trains;
        TrainName = _schemeSetDetailDto.Trains[TrainSelectedIndex].Name;
        Experiments = _schemeSetDetailDto.Trains[TrainSelectedIndex].Experiments;
        ChangeBtn();
    }

    private void ChangeBtn()
    {
        StartBtnName = !_schemeSetDetailDto.IsPublished ? "开始训练" : "结束训练";
        StartBtnType = !_schemeSetDetailDto.IsPublished ? ControlAppearance.Success : ControlAppearance.Danger;
    }

    private void ChangePageData()
    {
        TrainName = _schemeSetDetailDto.Trains[TrainSelectedIndex].Name;
        Experiments = _schemeSetDetailDto.Trains[TrainSelectedIndex].Experiments;
    }

    [RelayCommand]
    private async Task StartOrStopSchemeSet()
    {
        _schemeSetDetailDto.IsPublished = !_schemeSetDetailDto.IsPublished;
        await _schemeSetService.ChangePublishStateAsync(_schemeSetDetailDto.Id, _schemeSetDetailDto.IsPublished);
        ChangeBtn();
    }

    [RelayCommand]
    private void OnGoBack()
    {
        NavigationService.GoBack();
    }


    public override void CurrentPageChanged(bool isNeedTotalCount = false)
    {
        throw new NotImplementedException();
    }

    public override void ClearItem()
    {
        throw new NotImplementedException();
    }
}