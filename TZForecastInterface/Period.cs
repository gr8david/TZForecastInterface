using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TZForecastInterface
{
    class Period
    {
        public int PeriodValue;
        public int PeriodSeq;
        public int Year;
        public int Month;
        public DateTime FirstDate;
        public DateTime LastDate;

        public Period(int p_Period)
        {
            PeriodValue = p_Period;
            PeriodSeq = (p_Period / 100) * 12 + (p_Period % 100);
            Year = p_Period / 100;
            Month = p_Period % 100;
            FirstDate = new DateTime(Year, Month, 1);
            LastDate = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
        }

        public int PeriodDiff(Period p2)
        {
            return Math.Abs(((Year * 12) + Month) - ((p2.Year * 12) + p2.Month));
        }

     

        public void PeriodAdd(int periods)
        {
        
            PeriodSeq = PeriodSeq + periods;
            if ((PeriodSeq % 12) == 0)
            {
                Year = (PeriodSeq / 12) - 1;
                Month = 12;
            }
            else
            {
                Year = (PeriodSeq / 12);
                Month = (PeriodSeq % 12);
            }
            PeriodValue = (Year * 100) + Month;
            FirstDate = new DateTime(Year, Month, 1);
            LastDate = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
        }
    }
}
