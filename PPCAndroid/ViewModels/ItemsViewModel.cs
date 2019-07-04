using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Legacy;

namespace PPCAndroid.ViewModels
{
    public class ItemsViewModel: ViewModelBase
    {
        private ReactiveList<Todo> _todos;

        public ReactiveList<Todo> Todos
        {
            get => _todos;
            set => this.RaiseAndSetIfChanged(ref _todos, value);
        }

        private ObservableAsPropertyHelper<bool> _canAdd;
        public bool CanAdd => _canAdd?.Value ?? false;

        private string _todoTitle;

        public string TodoTitle
        {
            get => _todoTitle;
            set => this.RaiseAndSetIfChanged(ref _todoTitle, value);
        }
        
        public ReactiveCommand AddCommand { get; private set; }

        public ItemsViewModel(IScreen hostScreen = null) : base(hostScreen)
        {
            this.WhenAnyValue(x => x.TodoTitle,
                title => 
                    !String.IsNullOrEmpty(title)).ToProperty(this, x => x.CanAdd, out _canAdd);

            AddCommand = ReactiveCommand.CreateFromTask( () =>
            {
                Todos.Add(new Todo() { Title = TodoTitle });
                TodoTitle = string.Empty;
                return Task.CompletedTask;

            }, this.WhenAnyValue(x => x.CanAdd, canAdd => canAdd && canAdd));
            
            //Dont forget to set ChangeTrackingEnabled to true.
            Todos = new ReactiveList<Todo>(){ChangeTrackingEnabled = true};
            
            Todos.Add(new Todo { IsDone = false, Title = "Go to Sleep" });
            Todos.Add(new Todo { IsDone = false, Title = "Go get some dinner" });
            Todos.Add(new Todo { IsDone = false, Title = "Watch GOT" });
            Todos.Add(new Todo { IsDone = false, Title = "Code code and code!!!!" });
            
            //Lets detect when ever a todo Item is marked as done 
            //IF it is, it is sent to the bottom of the list
            //Else nothing happens
            Todos.ItemChanged.Where(x => x.PropertyName == "IsDone" && x.Sender.IsDone)
                .Select(x => x.Sender)
                .Subscribe(x =>
                {
                    if (x.IsDone)
                    {
                        Todos.Remove(x);
                        Todos.Add(x);
                    }
                });
        }
    }
}