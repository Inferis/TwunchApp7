using System;
using System.Collections.Generic;
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

namespace Inferis.TwunchApp.UI {
    public class TwunchDetailViewModel : Screen {
        private Twunch item;

        public string Id { get; set; }
        public string Title { get { return item == null ? "" : item.Title; } }
        public string Date { get { return item == null ? "" : item.Date.ToLocalTime().ToString("dd/MM/yyyy HH:mm"); } }
        public List<string> Participants { get { return item == null ? null : item.Participants; } }
        
        protected Twunch Item
        {
            get { return item; }
            set
            {
                item = value;
                NotifyOfPropertyChange(() => Title);
                NotifyOfPropertyChange(() => Participants);
                NotifyOfPropertyChange(() => Date);
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            new Twunch.Fetcher(twunches => Item = twunches.FirstOrDefault(x => x.Id == Id), true);
        }
    }
}
