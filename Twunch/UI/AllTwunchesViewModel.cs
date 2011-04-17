using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Inferis.TwunchApp.API;

namespace Inferis.TwunchApp.UI {
    public class AllTwunchesViewModel : Conductor<Screen>, ITwunchesViewModel {
        private readonly INavigationService navigationService;

        public AllTwunchesViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            Name = "All";
            Twunches = new ObservableCollection<TwunchViewModel>();
        }

        public string Name { get; set; }
        public ObservableCollection<TwunchViewModel> Twunches { get; private set; }
        public TwunchViewModel SelectedTwunch
        {
            get { return null; }
            set { 
                if (value != null)
                    navigationService.Navigate(new Uri("/UI/TwunchDetail.xaml?id=" + value.Id));
            }
        }

        public void SetAllTwunches(IEnumerable<Twunch> twunches)
        {
            foreach (var twunch in twunches) {
                Twunches.Add(new TwunchViewModel(twunch));
            }
        }
    }
}
