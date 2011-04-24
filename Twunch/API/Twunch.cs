using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml;
using Caliburn.Micro;

namespace Inferis.TwunchApp.API {
    public class Twunch {
        public Twunch()
        {
            Participants = new List<string>();
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public DateTimeOffset Date { get; set; }
        public Uri MapUri { get; set; }
        public Uri Link { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<string> Participants { get; private set; }
        public bool Closed { get; set; }
        public GeoCoordinate Coordinate
        {
            get
            {
                try {
                    return new GeoCoordinate(Latitude, Longitude);
                }
                catch (Exception) {
                    return GeoCoordinate.Unknown;
                }
            }
        }


        public class Fetcher : IResult {
            private readonly Action<IEnumerable<Twunch>> callback;
            private readonly bool allowCache;
            private static List<Twunch> cache = null;

            public Fetcher(Action<IEnumerable<Twunch>> callback, bool allowCache)
            {
                this.callback = callback;
                this.allowCache = allowCache;
            }

            public void Execute(ActionExecutionContext context)
            {
                if (cache != null && allowCache) {
                    ReturnResult(cache);
                    return;
                }

                var request = WebRequest.CreateHttp("http://www.twunch.be/events.xml");
                request.BeginGetResponse(ResponseReceived, request);
            }

            private void ResponseReceived(IAsyncResult ar)
            {
                var result = new List<Twunch>();
                var request = (HttpWebRequest)ar.AsyncState;
                var response = request.EndGetResponse(ar);
                using (var reader = XmlReader.Create(response.GetResponseStream(), new XmlReaderSettings() { IgnoreWhitespace = true })) {
                    reader.ReadToFollowing("twunches");
                    while (reader.ReadToFollowing("twunch")) {
                        var twunch = ReadTwunch(reader);
                        result.Add(twunch);
                    }
                }

                cache = result.Where(t => t.Date > DateTimeOffset.Now).OrderBy(t => t.Date).ToList();
                ReturnResult(cache);
            }

            private void ReturnResult(List<Twunch> result)
            {
                callback(result);
                Completed(this, new ResultCompletionEventArgs());
            }

            private Twunch ReadTwunch(XmlReader reader)
            {
                var result = new Twunch();
                reader.Read();
                while (!reader.EOF) {
                    switch (reader.Name) {
                        case "id":
                            result.Id = reader.ReadElementContentAsString();
                            break;
                        case "title":
                            result.Title = reader.ReadElementContentAsString();
                            break;
                        case "address":
                            result.Address = reader.ReadElementContentAsString();
                            break;
                        case "note":
                            result.Note = reader.ReadElementContentAsString();
                            break;
                        case "map":
                            result.MapUri = new Uri(reader.ReadElementContentAsString());
                            break;
                        case "link":
                            result.Link = new Uri(reader.ReadElementContentAsString());
                            break;
                        case "date": {
                                var d = DateTimeOffset.Now;
                                DateTimeOffset.TryParseExact(reader.ReadElementContentAsString(), @"yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out d);
                                result.Date = d;
                                break;
                            }
                        case "longitude": {
                                double l = 0;
                            string s = reader.ReadElementContentAsString();
                            double.TryParse(s, NumberStyles.Float, new CultureInfo("en-US"), out l);
                                result.Longitude = l;
                                break;
                            }
                        case "latitude": {
                                double l = 0;
                                string s = reader.ReadElementContentAsString();
                                double.TryParse(s, NumberStyles.Float, new CultureInfo("en-US"), out l);
                                result.Latitude = l;
                                break;
                            }
                        case "closed":
                            result.Closed = reader.ReadElementContentAsBoolean();
                            break;
                        case "participants":
                            reader.ReadToFollowing("participant");
                            do {
                                result.Participants.Add(reader.ReadElementContentAsString());
                            } while (reader.ReadToNextSibling("participant"));
                            break;
                        default:
                            return result;
                    }
                }

                return result;
            }


            public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };
        }
    }
}
