using System.Reactive;
using System.Reactive.Linq;
using Model;
using ReactiveUI;

namespace Shared.ViewModels
{
    public class ClickerViewModel : ReactiveObject
    {
        private readonly ClickerModel _clickerModel;

        public ClickerViewModel(ClickerModel clickerModel)
        {
            _clickerModel = clickerModel;
            IncrementClickerCommand = ReactiveCommand.Create(IncrementClicker);

            _clickedResult = IncrementClickerCommand
                .Select(CreateClickerText)
                .ToProperty(this, c => c.ClickedResult);
        }

        private int IncrementClicker()
        {
            _clickerModel.ClickCount++;
            return _clickerModel.ClickCount;
        }

        private readonly ObservableAsPropertyHelper<string> _clickedResult;
        public string ClickedResult => _clickedResult.Value;

        public ReactiveCommand<Unit, int> IncrementClickerCommand { get; }

        private string CreateClickerText(int clickCount)
        {
            return $"Clicked {clickCount} times!";
        }
    }
}