using JetBrains.Annotations;
using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace JE.App.Pages
{
    public class BaseViewModel : ReactiveObject, IDisposable
    {
        [NotNull]
        protected CompositeDisposable CleanUp { get; } = new CompositeDisposable();

        public void Dispose()
        {
            CleanUp.Dispose();
        }
    }
}
