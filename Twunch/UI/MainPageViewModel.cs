using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Inferis.TwunchApp.API;

namespace Inferis.TwunchApp.UI {
    public class MainPageViewModel : Screen
    {
        public MainPageViewModel()
        {
            Pivots = new ObservableCollection<object>();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Pivots.Add(new NearbyTwunchesViewModel());
            Pivots.Add(new AllTwunchesViewModel());
            Pivots.Add(new AboutViewModel());

            new SequentialResult(LoadTwunches().GetEnumerator()).Execute(new ActionExecutionContext());
        }

        private IEnumerable<IResult> LoadTwunches()
        {
            IEnumerable<Twunch> twunches = null;
            yield return new Twunch.Fetcher(x => { twunches = x; });
            foreach (var pivot in Pivots.OfType<ITwunchesViewModel>()) {
                Execute.OnUIThread(() => pivot.SetAllTwunches(twunches));
            }
        }

        public ObservableCollection<object> Pivots { get; set; }
    }
}
