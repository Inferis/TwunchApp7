using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Inferis.TwunchApp.API;
using LinqToVisualTree;
using Microsoft.Phone.Controls;

namespace Inferis.TwunchApp.UI {
    public class MainPageViewModel : Conductor<Screen>.Collection.OneActive {
        public MainPageViewModel(NearbyTwunchesViewModel nearbyTwunchesViewModel, AllTwunchesViewModel allTwunchesViewModel, AboutViewModel aboutViewModel)
        {
            Pivots = new ObservableCollection<object> { nearbyTwunchesViewModel, allTwunchesViewModel, aboutViewModel };
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

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

        public object SelectedPivot
        {
            get { return null; }
            set
            {
                var page = (PhoneApplicationPage)GetView();
                if (page.ApplicationBar != null)
                    page.ApplicationBar.IsVisible = !(value is AboutViewModel);
            }
        }

        public void RefreshTwunches()
        {
            new SequentialResult(LoadTwunches().GetEnumerator()).Execute(new ActionExecutionContext());
        }
    }
}
