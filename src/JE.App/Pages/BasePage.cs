﻿using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace JE.App.Pages
{
    public abstract class BasePage<T> : ComponentBase, IDisposable
        where T : BaseViewModel
    {
        [Inject]
        public T ViewModel { get; set; }

        [NotNull]
        protected  CompositeDisposable CleanUp { get; } = new CompositeDisposable();

        protected override void OnInitialized()
        {
            Observable
                .FromEventPattern(
                    h => ViewModel.StateHasChanged += h,
                    h => ViewModel.StateHasChanged -= h)
                .SelectMany(async _ =>
                {
                    await InvokeAsync(StateHasChanged);
                    return Unit.Default;
                })
                .Subscribe()
                .DisposeWith(CleanUp);
        }

        public void Dispose()
        {
            CleanUp.Dispose();
            ViewModel.Dispose();
        }
    }
}
