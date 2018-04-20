using ReactiveUI;
using System;

namespace CompleteInformation.RecipeModule.AvaloniaApp.Helper
{
    public class SimpleObservable<T> : ReactiveObject
    {
        protected T value;
        public T Value
        {
            get => this.value;
            set
            {
                this.RaiseAndSetIfChanged(ref this.value, value);
            }
        }

        public SimpleObservable(T value)
        {
            this.Value = value;
        }
    }
}
