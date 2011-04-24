using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Inferis.TwunchApp.API;

namespace Inferis.TwunchApp.UI {
    public class AllTwunchesViewModel : TwunchesViewModelBase {
        public AllTwunchesViewModel(INavigationService navigationService)
            : base("All", navigationService)
        {   
        }

        public override void SetAllTwunches(IEnumerable<Twunch> twunches)
        {
            foreach (var twunch in twunches) {
                Twunches.Add(new TwunchItemViewModel(twunch));
            }
        }
    }
}
