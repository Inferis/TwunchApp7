using System;
using Caliburn.Micro;
using Inferis.TwunchApp.API;

namespace Inferis.TwunchApp.UI
{
    public class TwunchItemViewModel : Screen
    {
        private readonly Twunch source;

        public TwunchItemViewModel(Twunch source)
        {
            this.source = source;
        }

        public string Id { get { return source.Id; } }
        public string Title { get { return source.Title; } }
        public string Date { get { return source.Date.ToLocalTime().ToString("dd/MM/yyyy HH:mm"); } }
        public int ParticipantCount { get { return source.Participants.Count;  } }
    }
}