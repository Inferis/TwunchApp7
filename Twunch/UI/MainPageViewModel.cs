using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Inferis.TwunchApp.API;

namespace Inferis.TwunchApp.UI {
    public class MainPageViewModel : Conductor<Screen>.Collection.OneActive
    {
        public MainPageViewModel(NearbyTwunchesViewModel nearbyTwunchesViewModel, AllTwunchesViewModel allTwunchesViewModel, AboutViewModel aboutViewModel)
        {
            Pivots = new ObservableCollection<object> {nearbyTwunchesViewModel, allTwunchesViewModel, aboutViewModel};
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            new SequentialResult(LoadTwunches().GetEnumerator()).Execute(new ActionExecutionContext());
        }

        private IEnumerable<IResult> LoadTwunches()
        {
            IEnumerable<Twunch> twunches = null;
            yield return new Twunch.Fetcher(x => { twunches = x; }, false);
            foreach (var pivot in Pivots.OfType<ITwunchesViewModel>()) {
                Execute.OnUIThread(() => pivot.SetAllTwunches(twunches));
            }
        }

        public ObservableCollection<object> Pivots { get; set; }
    }
}
