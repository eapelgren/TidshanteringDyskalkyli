using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TidshanteringDyskalkyli
{
    public static class TimeParser
    {
        public static bool minuteSet { get; set; }

        public static string Hour { get; set; }

        public static string Minute { get; set; }

        public static Dictionary<string, string> TimeDictionary
        {
            get
            {
                return new Dictionary<string, string>()
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
                    ["tjugonio"] = "29"
                };
            }
        }

        public static bool? isHalv { get; set; }

        public static bool? beforeTime { get; set; }

        public static TimeReturnObject ParseTime(string time, bool isAM)
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

                foreach (var timeString in split)
                {
                    if (timeString == "halv")
                    {
                        isHalv = true;
                    }

                    else if (timeString == "i")
                    {
                        beforeTime = true;
                    }

                    else if (timeString == "över")
                    {
                        beforeTime = false;
                    }
                    else
                    {
                        var number = CheckIfNumber(timeString);
                        if (number != null && Minute == null)
                        {
                            Minute = number;
                        }
                        else if (number != null && Minute != null)
                        {
                            Hour = number;
                        }
                    }
                }

                int intDecimalMinute = int.Parse(Minute);
                int intDecimaalHour = int.Parse(Hour);

                if (isHalv == true)
                {
                    intDecimalMinute += 30;
                    intDecimaalHour--;
                }

                if (beforeTime == true)
                {
                    intDecimalMinute -= int.Parse(Minute) * 2;
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

                var stringReturnRepresentationOfHourInt = AdjustHourString(intDecimaalHour);
                var stringReturnRepresnationOfMinuteInt = intDecimalMinute.ToString();

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

        private static string AdjustHourString(int oldhour)
        {
            if (oldhour.ToString().Length == 1)
            {
                var i = oldhour.ToString();
                var i2 = i.Insert(0, "0");


                return i2;
            }
            else
            {
                
            return oldhour.ToString();
            }
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
        public string Hour { get; set; }
        public string Minute { get; set; }
        public string DispalyString { get; set; }
    }
}
