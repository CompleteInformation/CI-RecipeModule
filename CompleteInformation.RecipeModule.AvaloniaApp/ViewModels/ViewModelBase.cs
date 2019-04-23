using ReactiveUI;
using ReactiveUI.Legacy;
using System;
using System.Collections.Generic;
using System.Text;

using CompleteInformation.RecipeModule.Core;

namespace CompleteInformation.RecipeModule.AvaloniaApp.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        protected static readonly RecipeApplication application;

        static ViewModelBase()
        {
            application = new RecipeApplication();
        }
    }
}
