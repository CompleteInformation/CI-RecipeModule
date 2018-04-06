using ReactiveUI;
using System;
using System.Collections.Generic;

namespace CompleteInformation.RecipeModule.AvaloniaApp.Helper
{
    class ReactiveHelper
    {
        private static ReactiveHelper instance;
        public static ReactiveHelper Instance
        {
            get
            {
                if (instance == null) {
                    instance = new ReactiveHelper();
                }
                return instance;
            }
        }

        private ReactiveHelper() { }

        public T[] ReactiveListToArray<T>(IReactiveList<T> list)
        {
            return this.ReactiveListToArray(list, x => true);
        }

        public T[] ReactiveListToArray<T>(IReactiveList<T> list, Predicate<T> predicate)
        {
            List<T> outList = new List<T>();
            foreach (T item in list) {
                if (predicate(item)) {
                    outList.Add(item);
                }
            }
            return outList.ToArray();
        }
    }
}
