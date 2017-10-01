using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteJson
{
    public class FormatHelper
    {
        
        /// <summary>
        /// format bye[], output like: 0x00000EB9883649ADFC85D3B92D1540A5CE99C5CB
        /// </summary>
        /// <param name="barr"></param>
        /// <returns></returns>
        public static string ByteArrayFormat(byte[] barr)
        {
            return BitConverter.ToString(barr).Replace("-", "");
        }
        /// <summary>
        /// format datetime output: yyyy-mm-dd
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DTFormat(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public static string DTOFormat(DateTimeOffset dto)
        {
            return dto.ToString();
        }

        /// <summary>
        /// format datetimeoffset,
        /// different specifier has different output, like below:
        /// 
        /// d: 10/31/2007    D: Wednesday, October 31, 2007  
        /// t: 9:00 PM  T: 9:00:00 PM    
        /// f: Wednesday, October 31, 2007 9:00 PM    
        /// F: Wednesday, October 31, 2007 9:00:00 PM
        /// g: 10/31/2007 9:00 PM    G: 10/31/2007 9:00:00 PM
        /// M: October 31    R: Thu, 01 Nov 2007 05:00:00 GMT
        /// s: 2007-10-31T21:00:00    u: 2007-11-01 05:00:00Z
        /// Y: October, 2007
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="specifier"></param>
        /// <returns></returns>
        public static string DTOFormat(DateTimeOffset dto, string specifier)
        {

            return dto.ToString(specifier);
        }

        /// <summary>
        /// format timespan output:365d 5h 23m 23s
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string TSFormat1(TimeSpan ts)
        {
            return ts.ToString("'d'd 'h'h 'm'm 's's");
        }

        /// <summary>
        /// format timespan output: 234 days, 2 hours, 8 minutes, 23 seconds
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string TSFormat2(TimeSpan ts)
        {
            List<string> list = new List<string>();
            int[] time = new[] { ts.Days, ts.Hours, ts.Minutes, ts.Seconds };
            string[] str = new[] { "days", "hours", "minutes", "second" };

            for (int i = 0; i < time.Length; i++)
            {
                if (time[i] > 0)
                {
                    list.Add(String.Format("{0} {1}", time[i], Pluralize(time[i], str[i])));
                }
            }

            return list.Count != 0 ? string.Join(", ", list.ToArray()) : "0 seconds";
        }

        private static string Pluralize(int n, string unit)
        {
            if (string.IsNullOrEmpty(unit)) return string.Empty;
            n = Math.Abs(n); // -1 should be singular, too
            return unit + (n == 1 ? string.Empty : "s");
        }
    }
}
