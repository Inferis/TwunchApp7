using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Inferis.TwunchApp.API;

namespace Inferis.TwunchApp.UI {
    public class AllTwunchesViewModel : Screen, ITwunchesViewModel {
        public AllTwunchesViewModel()
        {
            Name = "All";
            Twunches = new ObservableCollection<TwunchViewModel>();
        }

        public string Name { get; set; }
        public ObservableCollection<TwunchViewModel> Twunches { get; private set; }

        public void SetAllTwunches(IEnumerable<Twunch> twunches)
        {
            foreach (var twunch in twunches) {
                Twunches.Add(new TwunchViewModel(twunch));
            }
        }
    }
}
