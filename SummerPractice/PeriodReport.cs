using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerPractice
{
    public class PeriodReport: CinemaReport
    {
      public readonly Tuple<DateTime, DateTime> Period;
      public readonly SortedDictionary<Movie, int> Attendance;
      public readonly double TotalProfit, AverageProfit;

      public PeriodReport(Cinema cinema, Tuple <DateTime, DateTime> period) : base(cinema)
      {
        Period = period;
        foreach (var movie in cinema.Movies)
        {
          for (DateTime date = period.Item1; date <= period.Item2;)
          {
            if (Attendance.ContainsKey(movie))
              Attendance[movie] += cinema.Attendance[new Tuple<Movie, DateTime>(movie, date)];
            else
              Attendance.Add(movie, cinema.Attendance[new Tuple<Movie, DateTime>(movie, date)]);
            date = date.AddDays(1);
          }
          TotalProfit += Attendance[movie] * movie.Cost;
        }

        AverageProfit = TotalProfit / (period.Item2 - period.Item1).Days;
      }

      public override string ToString()
      {
        return $"{base.ToString()}, Period: {Period}, Attendance: {Attendance}," +
               $" TotalProfit: {TotalProfit}, AverageProfit: {AverageProfit}";
      }

      protected bool Equals(PeriodReport other)
      {
        return base.Equals(other) && Equals(Period, other.Period) && Equals(Attendance, other.Attendance)
               && TotalProfit.Equals(other.TotalProfit) && AverageProfit.Equals(other.AverageProfit);
      }

      public override bool Equals(object obj)
      {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((PeriodReport) obj);
      }

      public override int GetHashCode()
      {
        unchecked
        {
          int hashCode = base.GetHashCode();
          hashCode = (hashCode * 397) ^ (Period != null ? Period.GetHashCode() : 0);
          hashCode = (hashCode * 397) ^ (Attendance != null ? Attendance.GetHashCode() : 0);
          hashCode = (hashCode * 397) ^ TotalProfit.GetHashCode();
          hashCode = (hashCode * 397) ^ AverageProfit.GetHashCode();
          return hashCode;
        }
      }
    }
}
