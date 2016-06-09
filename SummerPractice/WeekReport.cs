using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerPractice
{
  public class WeekReport : Report
  {
    public readonly Tuple<DateTime, DateTime> Period = new Tuple<DateTime, DateTime>(DateTime.Now, DateTime.Now);
    public readonly List<MovieReport> MovieReports = new List<MovieReport>();

    public WeekReport(Tuple<DateTime, DateTime> period, Movie[] movies)
    {
      Period = period;
      foreach (var movie in movies)
      {
        foreach (var cinema in movie.Cinemas)
        {
          Tuple<DateTime, DateTime> moviePeriod = cinema.getDates(movie);
          if (moviePeriod == null)
            continue;
          if (MovieReports.Contains(new MovieReport(movie)) || moviePeriod.Item1 > period.Item2 ||
              moviePeriod.Item2 < period.Item1)
            continue;
          MovieReports.Add(new MovieReport(movie));
        }
      }
    }

    public override string ToString()
    {
      return $"Period: {Period}, MovieReports: {MovieReports}";
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((WeekReport) obj);
    }

    protected bool Equals(WeekReport other)
    {
      return Equals(Period, other.Period) && Equals(MovieReports, other.MovieReports);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = (Period != null ? Period.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (MovieReports != null ? MovieReports.GetHashCode() : 0);
        return hashCode;
      }
    }
  }
}