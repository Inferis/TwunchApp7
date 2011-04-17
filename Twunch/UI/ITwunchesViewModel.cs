using System.Collections.Generic;
using Inferis.TwunchApp.API;

namespace Inferis.TwunchApp.UI
{
    public interface ITwunchesViewModel
    {
        void SetAllTwunches(IEnumerable<Twunch> twunches);
    }
}