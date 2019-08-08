using JetBrains.Annotations;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace JE.App.Pages
{
    public abstract class BaseViewModel : ReactiveObject, IDisposable
    {
        /// <summary>
        /// Since we use a ViewModel approach we need to manually call StateHasChanged on the component level
        /// </summary>
        public event EventHandler StateHasChanged;

        protected BaseViewModel()
        {
            Observable
                .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    h => PropertyChanged += h,
                    h => PropertyChanged -= h)
                .Subscribe(_ => UpdateState())
                .DisposeWith(CleanUp);
        }

        [NotNull]
        protected CompositeDisposable CleanUp { get; } = new CompositeDisposable();

        protected void UpdateState() => StateHasChanged?.Invoke(null, EventArgs.Empty);

        public void Dispose() => CleanUp.Dispose();
    }
}
