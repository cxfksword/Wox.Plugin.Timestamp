using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Wox.Plugin.Timestamp
{
    public class Main : IPlugin
    {
        private static readonly Regex regTimestamp = new Regex(@"^1\d{9}$", RegexOptions.IgnoreCase);

        public List<Result> Query(Query query)
        {
            var list = new List<Result>();

            var q = query.Search.Trim();

            // convert timestamp to frendly date
            if (q.Length == 10)
            {
                var match = regTimestamp.Match(q);
                if (!match.Success)
                {
                    return list;
                }

                var timestamp = int.Parse(q);
                var date = UnixTimeStampToDateTime(timestamp).ToString();
                list.Add(new Result()
                {
                    IcoPath = "Images\\app.png",
                    Title = date,
                    SubTitle = "Copy date to clipboard",
                    Action = (c) =>
                    {
                        Clipboard.SetText(date);
                        return true;
                    }
                });

                return list;
            }

            // convert datetime to timestamp
            if (q.Length > 0)
            {
                DateTime date;
                if (DateTime.TryParse(q, out date))
                {
                    var timestamp = DateTimeToUnixTimeStamp(date).ToString();
                    list.Add(new Result()
                    {
                        IcoPath = "Images\\app.png",
                        Title = timestamp,
                        SubTitle = "Copy unix timestamp to clipboard",
                        Action = (c) =>
                        {
                            Clipboard.SetText(timestamp);
                            return true;
                        }
                    });

                    return list;
                }
            }

            // show current timestamp
            if (q == "")
            {
                var timestamp = DateTimeToUnixTimeStamp(DateTime.Now).ToString();
                list.Add(new Result()
                {
                    IcoPath = "Images\\app.png",
                    Title = timestamp,
                    SubTitle = "Copy unix timestamp to clipboard",
                    Action = (c) =>
                    {
                        Clipboard.SetText(timestamp);
                        return true;
                    }
                });
            }

            return list;
        }

        public void Init(PluginInitContext context)
        {
        }

        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds((double)unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public int DateTimeToUnixTimeStamp(DateTime date)
        {
            var timeSpan = (date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0));
            return (int)timeSpan.TotalSeconds;
        }


    }
}
