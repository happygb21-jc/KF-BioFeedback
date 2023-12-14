# Kingfar.BioFeedback.Prism.Common
此类库主要提供了一下自己魔改的内容，不是什么常规类路，为了方便给自己的ViewModel和View 提供支持
## Controls
存放一些扩展的控件
## Services
弹框服务，PageService的实现，以及ILoginSnackbarService（只是单纯增加了一个可以使用的状态卡提示，原因是WPF-UI中这玩意无法进行替换SnackbarPresenter，所以临时新建，把登录和主窗体的区分开）
## ViewModels
提供ViewModelBase