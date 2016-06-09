using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ProtoBuf;

namespace SummerPractice
{
  [ProtoContract]
  public class ControlCinema
  {
    [ProtoMember(1)] public List<Cinema> Cinemas;
    [ProtoMember(2)] public List<Movie> Movies;

    public ControlCinema()
    {
      Cinemas = new List<Cinema>();
      Movies = new List<Movie>();
    }

    public ControlCinema(List<Cinema> cinemas, List<Movie> movies)
    {
      Cinemas = cinemas;
      Movies = movies;
    }

    public Tuple<List<Tuple<MovieReport, List<Cinema>>>, int> getLongestPeriodMovieInfo()
    {
      int best = 0;
      foreach (var cinema in Cinemas)
      {
        foreach (var obj in cinema.Dates)
        {
          if ((obj.Value.Item2 - obj.Value.Item1).Days > best)
          {
            best = (obj.Value.Item2 - obj.Value.Item1).Days;
          }
        }
      }
      SortedDictionary<Movie, List<Cinema>> temp = new SortedDictionary<Movie, List<Cinema>>();
      foreach (var cinema in Cinemas)
      {
        foreach (var obj in cinema.Dates)
        {
          if ((obj.Value.Item2 - obj.Value.Item1).Days == best)
          {
            best = (obj.Value.Item2 - obj.Value.Item1).Days;
            if (!temp.ContainsKey(obj.Key))
              temp.Add(obj.Key, new List<Cinema>());
            temp[obj.Key].Add(cinema);
          }
        }
      }
      List<Tuple<MovieReport, List<Cinema>>> result = new List<Tuple<MovieReport, List<Cinema>>>();
      foreach (var obj in temp)
      {
        result.Add(new Tuple<MovieReport, List<Cinema>>(new MovieReport(obj.Key), obj.Value));
      }
      return new Tuple<List<Tuple<MovieReport, List<Cinema>>>, int>(result, best);
    }

    public PeriodReport[] getPeriodReport(Tuple<DateTime, DateTime> period)
    {
      PeriodReport[] reports = new PeriodReport[Cinemas.Count];
      int i = 0;
      foreach (var cinema in Cinemas)
      {
        reports[i++] = new PeriodReport(cinema, period);
      }
      return reports;
    }

    public WeekReport getWeekReport(Tuple<DateTime, DateTime> period)
    {
      return new WeekReport(period, Movies.ToArray());
    }

    public Tuple<MovieReport[], double> getMostProfitMovies()
    {
      double best = 0;
      foreach (var movie in Movies)
      {
        int attendance = 0;
        foreach (var cinema in Cinemas)
        {
          if (cinema.Dates.ContainsKey(movie))
            for (DateTime day = cinema.Dates[movie].Item1; day != cinema.Dates[movie].Item2;)
            {
              if (cinema.Attendance.ContainsKey(new Tuple<Movie, DateTime>(movie, day)))
                attendance += cinema.Attendance[new Tuple<Movie, DateTime>(movie, day)];
              day = day.AddDays(1);
            }
        }
        if (attendance * movie.Cost > best)
          best = attendance * movie.Cost;
      }
      List<MovieReport> result = new List<MovieReport>();
      foreach (var movie in Movies)
      {
        int attendance = 0;
        foreach (var cinema in Cinemas)
        {
          if (cinema.Dates.ContainsKey(movie))
            for (DateTime day = cinema.Dates[movie].Item1; day != cinema.Dates[movie].Item2;)
            {
              if (cinema.Attendance.ContainsKey(new Tuple<Movie, DateTime>(movie, day)))
                attendance += cinema.Attendance[new Tuple<Movie, DateTime>(movie, day)];
              day = day.AddDays(1);
            }
        }
        if (attendance * movie.Cost == best && !result.Contains(new MovieReport(movie)))
          result.Add(new MovieReport(movie));
      }
      return new Tuple<MovieReport[], double>(result.ToArray(), best);
    }

    public override string ToString()
    {
      return $"Cinemas: {Cinemas}, Movies: {Movies}";
    }

    protected bool Equals(ControlCinema other)
    {
      return Equals(Cinemas, other.Cinemas) && Equals(Movies, other.Movies);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((ControlCinema) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return ((Cinemas != null ? Cinemas.GetHashCode() : 0) * 397) ^ (Movies != null ? Movies.GetHashCode() : 0);
      }
    }
  }
}