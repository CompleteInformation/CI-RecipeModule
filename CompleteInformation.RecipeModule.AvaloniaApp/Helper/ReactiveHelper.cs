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

        public T[] ReactiveListToArray<T>(IReactiveList<T> list) =>
            this.ReactiveListToArray(list, _ => true);

        public T[] ReactiveListToArray<T>(IReactiveList<T> list, Predicate<T> predicate) =>
            this.ReactiveListToArray(list, predicate, x => x);

        public U[] ReactiveListToArray<T, U>(IReactiveList<T> list, Predicate<U> predicate, Func<T, U> pretransform) =>
            this.ReactiveListToArray(list, predicate, pretransform, x => x);

        public V[] ReactiveListToArray<T, U, V>(IReactiveList<T> list, Predicate<U> predicate, Func<T, U> pretransform, Func<U, V> posttransform)
        {
            List<V> outList = new List<V>();
            foreach (T item in list) {
                U transformed = pretransform(item);
                if (predicate(transformed)) {
                    outList.Add(posttransform(transformed));
                }
            }
            return outList.ToArray();
        }
    }
}
