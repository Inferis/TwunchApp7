using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Inferis.TwunchApp.API;

namespace Inferis.TwunchApp.UI
{
    public abstract class TwunchesViewModelBase : Screen, ITwunchesViewModel
    {
        private readonly INavigationService navigationService;

        protected TwunchesViewModelBase(string name, INavigationService navigationService)
        {
            this.navigationService = navigationService;
            Name = name;
            Twunches = new ObservableCollection<TwunchItemViewModel>();
        }

        public string Name { get; set; }
        public ObservableCollection<TwunchItemViewModel> Twunches { get; private set; }

        public void NavigateTwunches(TwunchItemViewModel item)
        {
            if (item != null) {
                var to = new Uri("/UI/TwunchDetailView.xaml?id=" + item.Id, UriKind.RelativeOrAbsolute);
                navigationService.Navigate(to);
            }
        }


        public abstract void SetAllTwunches(IEnumerable<Twunch> twunches);
    }
}