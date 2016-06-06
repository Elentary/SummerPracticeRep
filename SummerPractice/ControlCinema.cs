using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace SummerPractice
{
  [ProtoContract]
  class ControlCinema
  {
    [ProtoMember(1)] public List<Cinema> Cinemas;
    [ProtoMember(2)] public List<Movie> Movies;

    public ControlCinema()
    {
      foreach (var field in this.GetType().GetProperties())
      {
        field.SetValue(this, null);
      }
    }

    public ControlCinema(List<Cinema> cinemas, List<Movie> movies)
    {
      Cinemas = cinemas;
      Movies = movies;
    }

    public Tuple<MovieReport, List<Cinema>> getLongestPeriodMovieInfo()
    {
      throw new NotImplementedException();
    }

    public PeriodReport[] getPeriodReport(Tuple<DateTime, DateTime> period)
    {
      throw new NotImplementedException();
    }

    public WeekReport getWeekReport(Tuple<DateTime, DateTime> period)
    {
      throw new NotImplementedException();
    }

    public MovieReport[] getMostProfitMovies()
    {
      throw new NotImplementedException();
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