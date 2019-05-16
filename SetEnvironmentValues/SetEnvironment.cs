using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetEnvironmentValues
{
    public static class SetEnvironment
    {
        static Dictionary<string, string> timeZones = new Dictionary<string, string>()
        {
            { "GMT -1200", "Dateline Standard Time" },
            { "GMT -1100", "Samoa Standard Time" },
            { "GMT -1000", "Hawaiian Standard Time" },
            { "GMT -0900", "Alaskan Standard Time" },
            { "GMT -0800", "Pacific Standard Time" },
            { "GMT -0700", "Mountain Standard Time" },
            { "GMT -0600", "Central Standard Time" },
            { "GMT -0500", "Eastern Standard Time" },
            { "GMT -0400", "Atlantic Standard Time" },
            { "GMT -0350", "Newfoundland and Labrador Standard Time" },
            { "GMT -0300", "S.A. Eastern Standard Time" },
            { "GMT -0200", "Mid -Atlantic Standard Time" },
            { "GMT -0100", "Azores Standard Time" },
            { "GMT 0000", "GMT Standard Time" },
            {"GMT +0100", "Central Europe Standard Time"},
            { "GMT +0200", "Egypt Standard Time" },
            { "GMT +0300", "Russian Standard Time" },
            { "GMT +0400", "Arabian Standard Time" },
            { "GMT +0450", "Transitional Islamic State of Afghanistan Standard Time" },
            { "GMT +0500", "West Asia Standard Time" },
            { "GMT +0550", "India Standard Time" },
            { "GMT +0575", "Nepal Standard Time" },
            { "GMT +0600", "Central Asia Standard Time" },
            { "GMT +0700", "North Asia Standard Time" },
            { "GMT +0800", "China Standard Time" },
            { "GMT +0900", "Yakutsk Standard Time" },
            { "GMT +1000", "Tasmania Standard Time" },
            { "GMT +1100", "Central Pacific Standard Time" },
            { "GMT +1200", "Fiji Islands Standard Time" },
            { "GMT +1300", "Tonga Standard Time" },
        };
        
        //public SetEnvironment()
        //{
        //    timeZones.Add("GMT -1200", "Dateline Standard Time");
        //    timeZones.Add("GMT -1100", "Samoa Standard Time");

        //    timeZones.Add("GMT -1000", "Hawaiian Standard Time");
        //    timeZones.Add("GMT -0900", "Alaskan Standard Time");
        //    timeZones.Add("GMT -0800", "Pacific Standard Time");
        //    timeZones.Add("GMT -0700", "Mountain Standard Time");
        //    timeZones.Add("GMT -0600", "Central Standard Time");
        //    timeZones.Add("GMT -0500", "Eastern Standard Time");
        //    timeZones.Add("GMT -0400", "Atlantic Standard Time");
        //    timeZones.Add("GMT -0350", "Newfoundland and Labrador Standard Time");
        //    timeZones.Add("GMT -0300", "S.A. Eastern Standard Time");
        //    timeZones.Add("GMT -0200", "Mid -Atlantic Standard Time");
        //    timeZones.Add("GMT -0100", "Azores Standard Time");
        //    timeZones.Add("GMT 0000", "GMT Standard Time");

        //    timeZones.Add("GMT +0100", "Central Europe Standard Time");
        //    timeZones.Add("GMT +0200", "Egypt Standard Time");
        //    timeZones.Add("GMT +0300", "Russian Standard Time");
        //    timeZones.Add("GMT +3500", "Iran Standard Time");
        //    timeZones.Add("GMT +0400", "Arabian Standard Time");
        //    timeZones.Add("GMT +0450", "Transitional Islamic State of Afghanistan Standard Time");
        //    timeZones.Add("GMT +0500", "West Asia Standard Time");
        //    timeZones.Add("GMT +0550", "India Standard Time");
        //    timeZones.Add("GMT +0575", "Nepal Standard Time");
        //    timeZones.Add("GMT +0600", "Central Asia Standard Time");
        //    timeZones.Add("GMT +0700", "North Asia Standard Time");
        //    timeZones.Add("GMT +0800", "China Standard Time");
        //    timeZones.Add("GMT +0900", "Yakutsk Standard Time");
        //    timeZones.Add("GMT +1000", "Tasmania Standard Time");
        //    timeZones.Add("GMT +1100", "Central Pacific Standard Time");
        //    timeZones.Add("GMT +1200", "Fiji Islands Standard Time");
        //    timeZones.Add("GMT +1300", "Tonga Standard Time");
        //}

        public static void SetTimeZone(string s)
        {
            string timeZoneId = timeZones[s];
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "tzutil.exe",
                Arguments = "/s \"" + timeZoneId + "\"",
                UseShellExecute = false,
                CreateNoWindow = true
            });

            if (process != null)
            {
                process.WaitForExit();
                TimeZoneInfo.ClearCachedData();
            }
        }
        public static void SetTimeZoneForUtcOffset(string s)
        {
            string localUtc = s.Remove(s.LastIndexOf(':'));
            localUtc = localUtc.Replace(":", "");
            foreach(var v in timeZones)
            {
                if (v.Key.IndexOf(localUtc) != -1)
                    SetTimeZone(v.Key);
            }
        }

    }
}
