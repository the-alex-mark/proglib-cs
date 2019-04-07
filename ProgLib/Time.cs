using System;

namespace ProgLib
{
    public struct Time
    {
        public Time(Int32 Hour, Int32 Minute, Int32 Second)
        {
            this.Hour = Hour;
            this.Minute = Minute;
            this.Second = Second;
        }

        public Int32 Hour { get; set; }
        public Int32 Minute { get; set; }
        public Int32 Second { get; set; }

        public new String ToString()
        {
            return String.Format("{0}:{1}:{2}", Hour, Minute, Second);
        }
        public String ToString(String Format)
        {
            String Result = "";

            if (Format.IndexOf("hh") > -1 || Format.IndexOf("HH") > -1 || Format.IndexOf("mm") > -1 || Format.IndexOf("MM") > -1 || Format.IndexOf("ss") > -1 || Format.IndexOf("SS") > -1)
            {
                if (Format.IndexOf("hh") > -1 || Format.IndexOf("HH") > -1)
                    Result = (Format.IndexOf("hh") > -1) ? Format.Replace("hh", Hour.ToString()) : Format.Replace("HH", Hour.ToString());

                if (Format.IndexOf("mm") > -1 || Format.IndexOf("MM") > -1)
                    Result = (Format.IndexOf("mm") > -1) ? Format.Replace("mm", Minute.ToString()) : Format.Replace("MM", Minute.ToString());

                if (Format.IndexOf("ss") > -1 || Format.IndexOf("SS") > -1)
                    Result = (Format.IndexOf("ss") > -1) ? Format.Replace("ss", Second.ToString()) : Format.Replace("SS", Second.ToString());
            }
            else { Result = ToString(); }
            
            return Result;
        }
    }
}
