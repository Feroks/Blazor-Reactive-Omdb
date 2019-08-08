using Microsoft.AspNetCore.Components;
using System;
using System.Reactive.Linq;

namespace JE.App.Pages
{
    public abstract class BasePage<T> : ComponentBase, IDisposable
        where T : BaseViewModel
    {
        private IDisposable _vmUpdateSubscription;

        [Inject]
        public T ViewModel { get; set; }

        protected override void OnInit()
        {
            _vmUpdateSubscription = Observable
                .FromEventPattern(
                    h => ViewModel.StateHasChanged += h,
                    h => ViewModel.StateHasChanged -= h)
                .Subscribe(_ => Invoke(StateHasChanged));
        }

        public void Dispose()
        {
            _vmUpdateSubscription.Dispose();
            ViewModel.Dispose();
        }
    }
}
