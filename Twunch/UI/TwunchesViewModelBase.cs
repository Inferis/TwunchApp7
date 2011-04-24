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
        public TwunchItemViewModel SelectedTwunch
        {
            get { return null; }
            set
            {
                if (value != null)
                    navigationService.Navigate(new Uri("/UI/TwunchDetail.xaml?id=" + value.Id));
            }
        }


        public abstract void SetAllTwunches(IEnumerable<Twunch> twunches);
    }
}