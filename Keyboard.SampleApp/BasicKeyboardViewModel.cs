﻿using MvvmDialogs;

namespace Keyboard.SampleApp
{
    class BasicKeyboardViewModel : ViewModelBase, IModalDialogViewModel
    {
        private bool? dialogResult;

        public bool? DialogResult
        {
            get
            {
                return dialogResult;
            }
            set
            {
                if (value == dialogResult)
                {
                    return;
                }

                dialogResult = value;
                OnPropertyChanged(() => DialogResult);
            }
        }
    }
}
