using MvvmDialogs;
using System.Windows.Input;

namespace Keyboard.SampleApp
{
    class MainWindowViewModel : ViewModelBase
    {
        private readonly IDialogService dlgService;

        internal MainWindowViewModel()
        {
            dlgService = new DialogService();
        }

        public ICommand ExitCommand { get { return new RelayCommand(ExitApp); } }
        public ICommand BasicKeyboardCommand { get { return new RelayCommand(ShowBasicKeyboard); } }
        public ICommand StandardKeyboardCommand { get { return new RelayCommand(ShowStandrdKeyboard); } }

        private void ExitApp()
        {
            System.Windows.Application.Current.MainWindow.Close();
        }

        private void ShowBasicKeyboard()
        {
            BasicKeyboardViewModel vm = new BasicKeyboardViewModel();
            dlgService.ShowDialog<BasicKeyboardView>(this, vm);
        }

        private void ShowStandrdKeyboard()
        {
            StandardKeyboardViewModel vm = new StandardKeyboardViewModel();
            dlgService.ShowDialog<StandardKeyboardView>(this, vm);
        }
    }
}
