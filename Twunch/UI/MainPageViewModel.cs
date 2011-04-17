using System.Collections.ObjectModel;
using Caliburn.Micro;

namespace Twunch.UI {
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
        }

        public ObservableCollection<object> Pivots { get; set; }
    }
}
