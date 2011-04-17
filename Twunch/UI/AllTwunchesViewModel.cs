using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Twunch.UI {
    public class AllTwunchesViewModel {
        public AllTwunchesViewModel()
        {
            Name = "All";
        }
        public string Name { get; set; }
    }
}
