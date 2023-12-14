using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kingfar.BioFeedback.Mvvm.Pages
{
    public partial class AddSchemeDialogViewModel : DialogViewModel
    {
        private readonly IDialogService _dialogService;
        [ObservableProperty]
        private List<Patient> patients;
        [ObservableProperty]
        private Visibility _nextBtnVisibility=Visibility.Visible;
        [ObservableProperty]
        private Visibility _backBtnVisibility=Visibility.Collapsed;
        [ObservableProperty]
        private int _tabSelectIndex = 0;

        [ObservableProperty]
        private List<SchemeTypeOption> _schemeTypes;
        [ObservableProperty]
        private List<SchemeApplicationTypeOption> _schemeAppliactionTypes;
        public AddSchemeDialogViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }
        public override void OnDialogOpened(IDialogParameters parameters)
        {
            Title = "添加方案";
            SchemeTypes = new List<SchemeTypeOption> {
                new SchemeTypeOption("评估",SchemeType.Access),
                new SchemeTypeOption("个人生物反馈",SchemeType.Person),
                new SchemeTypeOption("团体生物反馈",SchemeType.Group),
            };
            SchemeAppliactionTypes = new List<SchemeApplicationTypeOption> {
                new SchemeApplicationTypeOption("放松训练",SchemeApplicationType.Relax),
                new SchemeApplicationTypeOption("脑波训练",SchemeApplicationType.Brainwave),
                new SchemeApplicationTypeOption("生理监测",SchemeApplicationType.Physiology),
                new SchemeApplicationTypeOption("情绪调节",SchemeApplicationType.Emotion),
                new SchemeApplicationTypeOption("脑认知增强",SchemeApplicationType.BrainCognition),
                new SchemeApplicationTypeOption("团队生物反馈",SchemeApplicationType.GroupBioFeedback),
                new SchemeApplicationTypeOption("VR场景训练",SchemeApplicationType.VR),
                new SchemeApplicationTypeOption("心理应激",SchemeApplicationType.Psychology),
                new SchemeApplicationTypeOption("HRV心理一致性",SchemeApplicationType.HRV),
            };
            Patients = new List<Patient>() {
                new Patient(1,"aaa"),
                new Patient(2,"bbb"),
                new Patient(3,"ccc"),
                new Patient(4,"ddd"),
                new Patient(5,"eee"),
                new Patient(6,"fff"),
                new Patient(7,"ggg"),
                new Patient(8,"hhh"),
                new Patient(9,"iii"),
            };
        }
        [RelayCommand]
        private void OnNextBtnClicked() {
            TabSelectIndex = 1;
            NextBtnVisibility= Visibility.Collapsed;
            BackBtnVisibility = Visibility.Visible;
        }

        [RelayCommand]
        private void OnBackBtnClicked()
        {
            TabSelectIndex = 0;
            NextBtnVisibility = Visibility.Visible;
            BackBtnVisibility = Visibility.Collapsed;
        }
    }
}
