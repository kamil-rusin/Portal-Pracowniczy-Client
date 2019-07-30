using System;
using System.Collections.Generic;
using System.Globalization;
using Android.Content;
using Android.Views;
using Android.Widget;
using PPCAndroid.Domain;
using PPCAndroid.Shared.Domain;

namespace PPCAndroid.Shared.View
{
    public class EventsListAdapter : ArrayAdapter<EventTuple>
    {
        private readonly Context _context;
        private readonly int _resource;
        
        public EventsListAdapter(Context context, int textViewResourceId, IList<EventTuple> objects) : base(context, textViewResourceId, objects)
        {
            _context = context;
            _resource = textViewResourceId;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            var summary = $"{Math.Floor(GetItem(position).CountDifference().TotalMinutes)} min.";
            //var startWork = $"Od: {GetItem(position).StartWorkEvent.When:hh\\:mm\\:ss}";
            var startWork =
                $"Od: {GetItem(position).StartWorkEvent.When.ToLongTimeString().ToString(CultureInfo.CurrentCulture)}";
            var endWork = $"Do: {GetItem(position).EndWorkEvent.When.ToLongTimeString().ToString(CultureInfo.CurrentCulture)}";
            //var endWork = $"Do: {GetItem(position).EndWorkEvent.When:hh\\:mm\\:ss}";
            
            var eventTuple = new EventTuple(GetItem(position).StartWorkEvent,GetItem(position).EndWorkEvent);
            
            var inflater = LayoutInflater.From(_context);
            convertView = inflater.Inflate(_resource, parent, false);

            var summaryTv = convertView.FindViewById<TextView>(Resource.Id.summaryTextView);
            var entryTimeTv = convertView.FindViewById<TextView>(Resource.Id.startedWorkTextView);
            var exitTimeTv = convertView.FindViewById<TextView>(Resource.Id.leftWorkTextView);
            
            summaryTv.SetText(summary,TextView.BufferType.Normal);
            entryTimeTv.SetText(startWork,TextView.BufferType.Normal);
            exitTimeTv.SetText(endWork,TextView.BufferType.Normal);

            return convertView;
        }
    }
}