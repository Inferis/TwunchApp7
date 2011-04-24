using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Globalization;
using Caliburn.Micro;
using Inferis.TwunchApp.API;

namespace Inferis.TwunchApp.UI {
    public class NearbyTwunchesViewModel : TwunchesViewModelBase {
        private GeoCoordinateWatcher watcher;
        private GeoCoordinate lastLocation = new GeoCoordinate(51.181858, 4.599001);
        private IEnumerable<Twunch> allTwunches;

        public NearbyTwunchesViewModel(INavigationService navigationService)
            : base("Nearby", navigationService)
        {   
            CurrentLocation = @"Locating...";

            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default) {
                MovementThreshold = 20
            };

            watcher.PositionChanged += (s, e) => {
                lastLocation = e.Position.Location;
                RefreshFilter();
            };
            watcher.Start();
        }

        public string CurrentLocation { get; set; }

        public override void SetAllTwunches(IEnumerable<Twunch> twunches)
        {
            allTwunches = twunches;
            RefreshFilter();
        }

        private void RefreshFilter()
        {
            CurrentLocation = string.Format(CultureInfo.InvariantCulture, "{0:0.000000}, {1:0.000000}", lastLocation.Latitude, lastLocation.Longitude);
            NotifyOfPropertyChange(() => CurrentLocation);

            Twunches.Clear();
            if (lastLocation.IsUnknown || allTwunches == null) return;
            foreach (var twunch in allTwunches) {
                if (!twunch.Coordinate.IsUnknown && twunch.Coordinate.GetDistanceTo(lastLocation) < 50000)
                    Twunches.Add(new TwunchItemViewModel(twunch));
            }
        }
    }
}
