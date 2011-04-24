using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using Caliburn.Micro;
using Inferis.TwunchApp.API;
using LinqToVisualTree;
using MetroInMotionUtils;
using Microsoft.Phone.Controls;

namespace Inferis.TwunchApp.UI {
    public abstract class TwunchesViewModelBase : Screen, ITwunchesViewModel {
        private readonly INavigationService navigationService;
        private ItemFlyInAndOutAnimations _flyOutAnimation = new ItemFlyInAndOutAnimations();

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
            // flyin the title again
            var titleView = ((FrameworkElement)GetView())
                .Descendants().OfType<TwunchItemView>()
                .Where(x => x.DataContext == item)
                .SelectMany(x => x.Descendants().OfType<FrameworkElement>())
                .SingleOrDefault(el => el.Name == "Title");
            FrameworkElement exitAnimationElement = null;

            if (titleView != null) {
                // animate the element, navigating to the new page when the animation finishes
                _flyOutAnimation.ItemFlyOut(titleView, () => NavigateToTwunchWithId(item.Id));
            }
            else {
                NavigateToTwunchWithId(item.Id);
                return;
            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            // flyin the title again
            ((PhoneApplicationPage) view).Loaded += (s, e) => ((PhoneApplicationPage)s).Dispatcher.BeginInvoke(() => _flyOutAnimation.ItemFlyIn());
        }

        private void NavigateToTwunchWithId(string id)
        {
            var to = new Uri("/UI/TwunchDetailView.xaml?id=" + id, UriKind.RelativeOrAbsolute);
            navigationService.Navigate(to);
        }

        public abstract void SetAllTwunches(IEnumerable<Twunch> twunches);
    }
}