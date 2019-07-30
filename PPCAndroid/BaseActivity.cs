using System.Reactive.Disposables;
using Android.OS;
using ReactiveUI;

namespace PPCAndroid
{
    public abstract class BaseActivity<TViewModel> : ReactiveActivity<TViewModel> where TViewModel : class
    {
        protected void OnCreateBase(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RegisterView();
            RegisterControls();
            RegisterInteractions();
            RegisterViewModel();

            this.WhenActivated(disposables =>
            {
                BindProperties(disposables);
                BindCommands(disposables);
            });
        }

        protected abstract void RegisterView();
        protected abstract void RegisterControls();
        protected abstract void BindProperties(CompositeDisposable disposables);
        protected abstract void BindCommands(CompositeDisposable disposables);    
        protected abstract void RegisterViewModel();
        protected abstract void RegisterInteractions();
    }
}