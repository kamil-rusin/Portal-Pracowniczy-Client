using System.Reactive.Linq;
using Java.Util;
using ReactiveUI;

namespace PPCAndroid.ViewModels
{
    public class Todo : ReactiveObject
    {
        public string Title { get; set; }
        private bool _isDone;
        public bool IsDone
        {
            get => _isDone;
            set => this.RaiseAndSetIfChanged(ref _isDone, value);
        }
        public bool IsEnabled => !IsDone;
    }
}