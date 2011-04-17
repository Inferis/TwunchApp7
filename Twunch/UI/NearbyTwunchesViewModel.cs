using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Globalization;
using Caliburn.Micro;
using Inferis.TwunchApp.API;

namespace Inferis.TwunchApp.UI {
    public class NearbyTwunchesViewModel : Screen, ITwunchesViewModel {
        private GeoCoordinateWatcher watcher;
        private GeoCoordinate lastLocation = new GeoCoordinate(51.181858, 4.599001);
        private IEnumerable<Twunch> allTwunches;

        public NearbyTwunchesViewModel()
        {
            Name = "Nearby";
            Twunches = new ObservableCollection<TwunchViewModel>();
            CurrentLocation = @"Locating...";

            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default) {
                MovementThreshold = 20
            };

            watcher.PositionChanged += this.watcher_PositionChanged;
            watcher.Start();
        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            lastLocation = e.Position.Location;
            RefreshFilter();
        }

        public string Name { get; set; }
        public string CurrentLocation { get; set; }
        public ObservableCollection<TwunchViewModel> Twunches { get; private set; }

        public void SetAllTwunches(IEnumerable<Twunch> twunches)
        {
            allTwunches = twunches;
            RefreshFilter();
        }

        private void RefreshFilter()
        {
            CurrentLocation = string.Format(CultureInfo.InvariantCulture, "{0:0.000000}, {1:0.000000}", lastLocation.Latitude, lastLocation.Longitude);
            NotifyOfPropertyChange(() => CurrentLocation);

            Twunches.Clear();
            if (lastLocation.IsUnknown) return;
            foreach (var twunch in allTwunches) {
                var ll = new GeoCoordinate(twunch.Latitude, twunch.Longitude);
                if (ll.GetDistanceTo(lastLocation) < 50000)
                    Twunches.Add(new TwunchViewModel(twunch));
            }
        }
    }
}
