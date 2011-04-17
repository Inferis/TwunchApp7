using Inferis.TwunchApp.API;

namespace Inferis.TwunchApp.UI
{
    public class TwunchViewModel
    {
        private readonly Twunch source;

        public TwunchViewModel(Twunch source)
        {
            this.source = source;
        }

        public string Title { get { return source.Title; } }
        public string Date { get { return source.Date.ToLocalTime().ToString("dd/MM/yyyy HH:mm"); } }
        public int ParticipantCount { get { return source.Participants.Count;  } }
    }
}