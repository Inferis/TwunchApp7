using System.Linq;
using Caliburn.Micro;
using LinqToVisualTree;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Inferis.TwunchApp.UI {
    public class AboutViewModel : Screen {
        public AboutViewModel()
        {
            Name = "About";
        }
        public string Name { get; set; }
    }
}
