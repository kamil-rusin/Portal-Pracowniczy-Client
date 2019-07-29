using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using PPCAndroid.Domain;
using PPCAndroid.JobServices;
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            OnCreateBase(savedInstanceState);
            
            //Create objects, TODO: get tuples
            var tuple1 = new EventTuple(new StartWorkEvent(DateTime.Now),new EndWorkEvent(DateTime.Now));
            var tuple2 = new EventTuple(new StartWorkEvent(DateTime.Now),new EndWorkEvent(DateTime.Now));
            var tuple3 = new EventTuple(new StartWorkEvent(DateTime.Now),new EndWorkEvent(DateTime.Now));

            var tupleList = new List<EventTuple> {tuple1, tuple2, tuple3};

            var adapter = new EventsListAdapter(this, Resource.Layout.adapter_view_layout, tupleList);
            _summaryListView.Adapter = adapter;
        }

        protected override void BindCommands(CompositeDisposable disposables)
        {
           
        }
        

        protected override void BindProperties(CompositeDisposable disposables)
        {
            
        }

        protected override void RegisterViewModel()
        {
            ViewModel = new SummaryViewModel();
        }

        protected override void RegisterInteractions()
        {

        }

        protected override void RegisterView()
        {
            SetContentView(Resource.Layout.activity_summary);
        }

        protected override void RegisterControls()
        {
            _summaryListView = FindViewById<ListView>(Resource.Id.eventsListView);
        }
    }
}