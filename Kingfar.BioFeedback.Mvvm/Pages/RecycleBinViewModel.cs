using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.Mvvm.Pages
{
    public partial class RecycleBinViewModel : ViewModelBase
    {
        #region Service

        private readonly ISchemeSetService _schemeSetService;
        private readonly IAccountService _accountService;

        #endregion Service

        #region Page

        [ObservableProperty]
        private IList<SchemeSetDto> _schemeSetSource;

        [ObservableProperty]
        private int _schemeSetTotalCount;

        private int _schemeSetCountPerPage = 20;

        public int SchemeSetCountPerPage
        {
            get { return _schemeSetCountPerPage; }
            set { _schemeSetCountPerPage = value; this.OnPropertyChanged("CountPerPage"); SchemeSetCurrentPageChanged(true); }
        }

        private int _schemeSetCcurrent = 1;

        public int SchemeSetCurrent
        {
            get { return _schemeSetCcurrent; }
            set { _schemeSetCcurrent = value; this.OnPropertyChanged("Current"); SchemeSetCurrentPageChanged(); }
        }

        [ObservableProperty]
        private string _schemeSetSearchText = string.Empty;

        [ObservableProperty]
        private int _schemeTypeSelectedIndex = -1;

        #endregion Page

        [RelayCommand]
        private void SchemeSetSearch()
        {
            SchemeSetCurrentPageChanged(true);
        }

        [RelayCommand]
        private void SchemeSetClear()
        {
            SchemeSetSearchText = string.Empty;
            SchemeTypeSelectedIndex = -1;
            SchemeSetCurrentPageChanged(true);
        }

        public void SchemeSetCurrentPageChanged(bool isNeedTotalCount = false)
        {
            SchemeSetType? schemeType = SchemeTypeSelectedIndex == -1 ? null : SchemeTypeSelectedIndex == 0 ? SchemeSetType.Individual : SchemeSetType.Group;
            var result = _schemeSetService.GetListByPage((SchemeSetCurrent - 1) * SchemeSetCountPerPage, SchemeSetCountPerPage, SchemeSetSearchText, schemeType, null, isNeedTotalCount, true);
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (isNeedTotalCount)
                {
                    SchemeSetTotalCount = result.TotalCount;
                    if (SchemeSetCurrent != 1)
                        SchemeSetCurrent = 1;
                }
                SchemeSetSource = result.Data;
            });
        }

        public RecycleBinViewModel(ISchemeSetService schemeSetService, IAccountService accountService)
        {
            _schemeSetService = schemeSetService;
            _accountService = accountService;
            SchemeSetCurrentPageChanged(true);
            UserCurrentPageChanged(true);
            OrganizationCurrentPageChanged(true);
        }

        [RelayCommand]
        private async Task SchemeSetRestore(SchemeSetDto dto)
        {
            if (await _schemeSetService.RestoreAsync(dto.Id))
            {
                SnackbarService.Show("提示", $"{dto.Name} 数据已恢复", ControlAppearance.Info, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
                SchemeSetCurrentPageChanged(true);
            }
        }

        [RelayCommand]
        private async Task SchemeSetDelete(SchemeSetDto dto)
        {
            await DeleteData($"确定要彻底删除方案 {dto.Name} 吗？", async () =>
            {
                if (await _schemeSetService.DeleteByIdAsync(dto.Id))
                {
                    SnackbarService.Show("提示", $"方案 {dto.Name} 数据已彻底删除", ControlAppearance.Info, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
                    SchemeSetCurrentPageChanged(true);
                }
            });
        }

        private async Task DeleteData(string message, Func<Task> func)
        {
            if (await ShowConfirmDialogAsync(message) == ContentDialogResult.Primary)
            {
                await func();
            }
        }
    }

    public partial class RecycleBinViewModel : ViewModelBase
    {
        #region Subject

        [ObservableProperty]
        private IList<AppUserDto> _userSource;

        [ObservableProperty]
        private int _userTotalCount;

        private int _userCountPerPage = 20;

        public int UserCountPerPage
        {
            get { return _userCountPerPage; }
            set { _userCountPerPage = value; this.OnPropertyChanged("CountPerPage"); UserCurrentPageChanged(true); }
        }

        private int _userCcurrent = 1;

        public int UserCurrent
        {
            get { return _userCcurrent; }
            set { _userCcurrent = value; this.OnPropertyChanged("Current"); UserCurrentPageChanged(); }
        }

        [ObservableProperty]
        private string _userSearchText = string.Empty;

        [ObservableProperty]
        private int? _userSearchSexIndex = default!;

        #endregion Subject

        [RelayCommand]
        private void UserSearch()
        {
            UserCurrentPageChanged(true);
        }

        [RelayCommand]
        private void UserClear()
        {
            UserSearchText = string.Empty;
            UserSearchSexIndex = null;
            UserCurrentPageChanged(true);
        }

        public void UserCurrentPageChanged(bool isNeedTotalCount = false)
        {
            var result = _accountService.GetListByPage((UserCurrent - 1) * UserCountPerPage, UserCountPerPage, AppUserType.Subject, UserSearchText, UserSearchSexIndex == null ? null : UserSearchSexIndex == 0, isNeedTotalCount, true);
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (isNeedTotalCount)
                {
                    UserTotalCount = result.TotalCount;
                    if (UserCurrent != 1)
                        UserCurrent = 1;
                }
                UserSource = result.Data;
            });
        }

        [RelayCommand]
        private async Task UserRestore(AppUserDto dto)
        {
            var prefix = dto.UserType == AppUserType.Subject ? "患者" : "组织成员";
            if (await _accountService.RestoreAsync(dto.Id!.Value))
            {
                SnackbarService.Show("提示", $"{prefix} {dto.Name} 数据已恢复", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
                if (dto.UserType == AppUserType.Subject)
                    UserCurrentPageChanged(true);
                else
                    OrganizationCurrentPageChanged(true);
            }
        }

        [RelayCommand]
        private async Task UserDelete(AppUserDto dto)
        {
            var prefix = dto.UserType == AppUserType.Subject ? "患者" : "组织成员";
            await DeleteData($"确定要彻底删除{prefix} {dto.Name} 吗？", async () =>
            {
                if (await _accountService.DeleteByIdAsync(dto.Id!.Value, dto.UserType))
                {
                    SnackbarService.Show("提示", $"{prefix} {dto.Name} 数据已彻底删除", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
                    if (dto.UserType == AppUserType.Subject)
                        UserCurrentPageChanged(true);
                    else
                        OrganizationCurrentPageChanged(true);
                }
            });
        }
    }

    public partial class RecycleBinViewModel : ViewModelBase
    {
        #region Page

        [ObservableProperty]
        private IList<AppUserDto> _organizationSource;

        [ObservableProperty]
        private int _organizationTotalCount;

        private int _organizationCountPerPage = 20;

        public int OrganizationCountPerPage
        {
            get { return _organizationCountPerPage; }
            set { _organizationCountPerPage = value; this.OnPropertyChanged("CountPerPage"); OrganizationCurrentPageChanged(true); }
        }

        private int _organizationCcurrent = 1;

        public int OrganizationCurrent
        {
            get { return _organizationCcurrent; }
            set { _organizationCcurrent = value; this.OnPropertyChanged("Current"); OrganizationCurrentPageChanged(); }
        }

        [ObservableProperty]
        private string _organizationSearchText = string.Empty;

        [ObservableProperty]
        private int? _organizationSearchSexIndex = default!;

        [RelayCommand]
        private void OrganizationSearch()
        {
            OrganizationCurrentPageChanged(true);
        }

        [RelayCommand]
        private void OrganizationClear()
        {
            OrganizationSearchText = string.Empty;
            OrganizationSearchSexIndex = null;
            OrganizationCurrentPageChanged(true);
        }

        public void OrganizationCurrentPageChanged(bool isNeedTotalCount = false)
        {
            var result = _accountService.GetListByPage((OrganizationCurrent - 1) * OrganizationCountPerPage, OrganizationCountPerPage, AppUserType.Main, OrganizationSearchText, OrganizationSearchSexIndex == null ? null : OrganizationSearchSexIndex == 0, isNeedTotalCount, true);
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (isNeedTotalCount)
                {
                    OrganizationTotalCount = result.TotalCount;
                    if (OrganizationCurrent != 1)
                        OrganizationCurrent = 1;
                }
                OrganizationSource = result.Data;
            });
        }

        #endregion Page
    }
}