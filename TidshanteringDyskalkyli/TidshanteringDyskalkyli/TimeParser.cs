using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TidshanteringDyskalkyli
{
    public class TimeParser
    {
        public static bool minuteSet { get; set; }

        public static string Hour { get; set; }

        public static string Minute { get; set; }

        public static Dictionary<string, string> TimeDictionary
        {
            get
            {
                return new Dictionary<string, string>
                {
                    ["ett"] = "01",
                    ["två"] = "02",
                    ["tre"] = "03",
                    ["fyra"] = "04",
                    ["fem"] = "05",
                    ["sex"] = "06",
                    ["sju"] = "07",
                    ["åtta"] = "08",
                    ["nio"] = "09",
                    ["tio"] = "10",
                    ["elva"] = "11",
                    ["tolv"] = "12",
                    ["tretton"] = "13",
                    ["fjorton"] = "14",
                    ["femton"] = "15",
                    ["kvart"] = "15",
                    ["sexton"] = "16",
                    ["sjuton"] = "17",
                    ["arton"] = "18",
                    ["nitton"] = "19",
                    ["tjugo"] = "20",
                    ["tjugofem"] = "25",
                    ["tjogosex"] = "26",
                    ["tjugosju"] = "27",
                    ["tjugoåtta"] = "28",
                    ["tjugonio"] = "29",
                    ["Halv"] = "30"
                };
            }
        }

        public static bool? isHalv { get; set; }

        public static bool? beforeTime { get; set; }

        public TimeReturnObject ParseTime(string time, bool isAM)
        {
            try
            {
                isHalv = null;
                beforeTime = false;
                minuteSet = false;
                Hour = null;
                Minute = null;

                var timeLower = time.ToLower();

                var split = timeLower.Split(" ".ToCharArray());

                //CHECK AND REMOVE FOR TIME INVARIANTS I & OVER ETC
                foreach (var timeString in split)
                {
                    if (timeString == "halv")
                    {
                        isHalv = true;
                        var split2 = split.Where(s => !s.Equals(timeString)).ToArray();
                        split = split2;
                    }

                    else if (timeString == "i")
                    {
                        beforeTime = true;
                        var split2 = split.Where(s => !s.Equals(timeString)).ToArray();
                        split = split2;
                    }

                    else if (timeString == "över")
                    {
                        beforeTime = false;
                        var split2 = split.Where(s => !s.Equals(timeString)).ToArray();
                        split = split2;
                    }
                }
                foreach (var timestring in split)
                {
                    var number = CheckIfNumber(timestring);
                    if (number == null)
                    {
                        var stringToRemove = timestring;
                        var numIndex = Array.IndexOf(split, stringToRemove);
                        split = split.Where((val, idx) => idx != numIndex).ToArray();
                    }
                    if (split.Length == 1 && number != null)
                    {
                        Hour = number;
                        Minute = 0.ToString();
                    }

                    else if (number != null && Minute == null)
                    {
                        Minute = number;
                    }
                    else if (number != null && Minute != null)
                    {
                        Hour = number;
                    }
                }

                var intDecimalMinute = int.Parse(Minute);
                var intDecimaalHour = int.Parse(Hour);

                if (isHalv == true)
                {
                    intDecimalMinute += 30;
                    intDecimaalHour--;
                }

                if (beforeTime == true)
                {
                    intDecimalMinute -= int.Parse(Minute)*2;
                }

                if (intDecimalMinute < 0)
                {
                    intDecimaalHour--;
                    intDecimalMinute += 60;
                }

                if (!isAM)
                {
                    intDecimaalHour += 12;
                }

                var stringReturnRepresentationOfHourInt = AdjustTimeInsertZeroToString(intDecimaalHour);
                var stringReturnRepresnationOfMinuteInt = AdjustTimeInsertZeroToString(intDecimalMinute);

                var timeReturnObject = new TimeReturnObject
                {
                    Hour = stringReturnRepresentationOfHourInt,
                    Minute = stringReturnRepresnationOfMinuteInt,
                    DispalyString = stringReturnRepresentationOfHourInt + ":" + stringReturnRepresnationOfMinuteInt
                };

                return timeReturnObject;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Could not parse Text");
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        private static string AdjustTimeInsertZeroToString(int oldTime)
        {
            if (oldTime.ToString().Length == 1)
            {
                var i = oldTime.ToString();
                var i2 = i.Insert(0, "0");

                return i2;
            }
            return oldTime.ToString();
        }

        private static string AdjustMinuteString(int oldminute)
        {
            if (oldminute.ToString().Length == 1)
            {
                var i = oldminute.ToString();
                var i2 = i.Insert(0, "0");


                return i2;
            }
            return oldminute.ToString();
        }

        private static string CheckIfNumber(string time)
        {
            string decimalMinute;
            TimeDictionary.TryGetValue(time, out decimalMinute);
            return decimalMinute;
        }
    }

    public class TimeReturnObject
    {
        public string Hour;
        public string Minute;
        public string DispalyString;

    }


}
