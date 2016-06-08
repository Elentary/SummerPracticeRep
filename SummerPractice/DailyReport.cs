using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerPractice
{
    public class DailyReport: CinemaReport
    {
      public readonly DateTime Date;
      public readonly SortedDictionary<Movie, int> Attendance;

      public DailyReport(Cinema cinema, DateTime date) : base(cinema)
      {
        Date = date;
        foreach (var movie in cinema.Movies)
        {
          Attendance.Add(movie, cinema.Attendance[new Tuple<Movie, DateTime>(movie, date)]);
        }
      }

      public override string ToString()
      {
        return $"{base.ToString()}, Date: {Date}, Attendance: {Attendance}";
      }

      protected bool Equals(DailyReport other)
      {
        return base.Equals(other) && Date.Equals(other.Date) && Equals(Attendance, other.Attendance);
      }

      public override bool Equals(object obj)
      {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((DailyReport) obj);
      }

      public override int GetHashCode()
      {
        unchecked
        {
          int hashCode = base.GetHashCode();
          hashCode = (hashCode * 397) ^ Date.GetHashCode();
          hashCode = (hashCode * 397) ^ (Attendance != null ? Attendance.GetHashCode() : 0);
          return hashCode;
        }
      }
    }
}
