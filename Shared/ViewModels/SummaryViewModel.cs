using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using PPCAndroid.Domain;
using PPCAndroid.Shared.Domain;
using ReactiveUI;

namespace Shared.ViewModels
{
    public class SummaryViewModel : ViewModelBase
    {
        private readonly IEventService _eventService;
        
        #region Interactions
        public Interaction<List<EventTuple>, Unit> SetAdapter { get; }
        #endregion
        public string WorkTimeObservable => _workTimeObservable.Value;
        private readonly ObservableAsPropertyHelper<string> _workTimeObservable;
        private string _workTime;
        private string WorkTime
        {
            get => _workTime;
            set => this.RaiseAndSetIfChanged(ref _workTime, value);
        }
        
        public SummaryViewModel(IEventService eventService)
        {
            _eventService = eventService;
            
            _workTimeObservable = this
                .WhenAnyValue(x => x.WorkTime)
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this,n => n.WorkTimeObservable);

            WorkTime = "Dzisiaj pracowano: " + EventsCounter.CountWorkTime(_eventService.GetTuplesFromDay(DateTime.Now)).ToString(@"hh\:mm\:ss");
            //SetAdapter = new Interaction<List<EventTuple>, Unit>();
            //SetAdapter.Handle(_eventService.GetTuplesFromDay(DateTime.Now)).ObserveOn(RxApp.MainThreadScheduler);
        }
    }
}