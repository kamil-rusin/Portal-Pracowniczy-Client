using System;
using System.Reactive;
using System.Reactive.Disposables;
using Android.App;
using Android.OS;
using Android.Widget;
using PPCAndroid.Shared.Domain;
using PPCAndroid.Shared.View;
using ReactiveUI;
using Shared.ViewModels;

namespace PPCAndroid
{
    [Activity(Label = "SummaryActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class SummaryActivity :  BaseActivity<SummaryViewModel>
    {
        private ListView _summaryListView;
        private TextView _timeTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            OnCreateBase(savedInstanceState);
            
            IEventService eventService = new MemoryEventService();
            var adapter = new EventsListAdapter(this, Resource.Layout.adapter_view_layout, eventService.GetTuplesFromDay(DateTime.Now));
            _summaryListView.Adapter = adapter;
        }

        protected override void BindCommands(CompositeDisposable disposables)
        {
           
        }
        

        protected override void BindProperties(CompositeDisposable disposables)
        {
            this.OneWayBind(ViewModel, x => x.WorkTimeObservable, a => a._timeTextView.Text).DisposeWith(disposables);
        }

        protected override void RegisterViewModel()
        {
            ViewModel = new SummaryViewModel(AndroidObjectFactory.GetEventService());
        }

        protected override void RegisterInteractions()
        {
           /* this.WhenActivated(d => { d(ViewModel.SetAdapter.RegisterHandler(interaction =>
            {
                var adapter = new EventsListAdapter(this, Resource.Layout.adapter_view_layout, interaction.Input);
                _summaryListView.Adapter = adapter;
                interaction.SetOutput(Unit.Default);
            })); });*/
        }

        protected override void RegisterView()
        {
            SetContentView(Resource.Layout.activity_summary);
        }

        protected override void RegisterControls()
        {
            _summaryListView = FindViewById<ListView>(Resource.Id.eventsListView);
            _timeTextView = FindViewById<TextView>(Resource.Id.summedTimeTextView);
        }
    }
}