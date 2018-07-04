using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using CompleteInformation.RecipeModule.Core;

namespace CompleteInformation.RecipeModule.AvaloniaApp.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        private readonly RecipeApplication application;

        protected ViewModelBase()
        {
            if (application == null) {
                application = new RecipeApplication();
            }
        }
    }
}
