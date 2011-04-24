using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;
using Inferis.TwunchApp.API;
using LinqToVisualTree;
using Microsoft.Phone.Controls.Maps;

namespace Inferis.TwunchApp.UI
{
    public class TwunchDetailViewModel : Screen
    {
        private Twunch item;

        public string Id { get; set; }

        public string PageTitle
        {
            get { return item == null ? "" : item.Title; }
        }

        public string Date
        {
            get { return item == null ? "" : item.Date.ToLocalTime().ToString("dd/MM/yyyy HH:mm"); }
        }

        public string Address
        {
            get { return item == null ? "" : item.Address; }
        }

        public GeoCoordinate Location
        {
            get { return item == null ? GeoCoordinate.Unknown : item.Coordinate; }
        }

        public List<string> Participants
        {
            get { return item == null ? new List<string>() : item.Participants; }
        }

        protected Twunch Item
        {
            get { return item; }
            set
            {
                item = value;
                NotifyOfPropertyChange(() => PageTitle);
                NotifyOfPropertyChange(() => Participants);
                NotifyOfPropertyChange(() => Date);
                NotifyOfPropertyChange(() => Location);
                NotifyOfPropertyChange(() => Address);
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            new Twunch.Fetcher(twunches => Item = twunches.FirstOrDefault(x => x.Id == Id), true).Execute(new ActionExecutionContext());
        }

        protected override void OnActivate()
        {
            var map = ((FrameworkElement)GetView()).Descendants().OfType<Map>().SingleOrDefault(x => x.Name == "Location");
            var layer = new MapLayer();

            map.Children.Add(layer);
            layer.AddChild(new Pushpin() { Background = new SolidColorBrush(Colors.Red), Content = Address }, Location);
            base.OnActivate();
        }
    }
} ;
