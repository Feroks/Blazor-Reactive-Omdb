using JetBrains.Annotations;
using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace JE.App.Pages
{
    public class BaseViewModel : ReactiveObject, IDisposable
    {
        /// <summary>
        /// Since we use a ViewModel approach we need to manually call StateHasChanged on the component level
        /// </summary>
        public event EventHandler StateHasChanged;

        [NotNull]
        protected CompositeDisposable CleanUp { get; } = new CompositeDisposable();

        protected void UpdateState() => StateHasChanged?.Invoke(null, EventArgs.Empty);

        public void Dispose() => CleanUp.Dispose();
    }
}
