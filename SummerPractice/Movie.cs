using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace SummerPractice
{
  [ProtoContract]
  public class Movie : IComparable<Movie>
  {
    [ProtoMember(1)] public String Title;
    [ProtoMember(2)] public String Genre;
    [ProtoMember(3)] public String Director;
    [ProtoMember(4)] public String ManStar;
    [ProtoMember(5)] public String WomanStar;
    [ProtoMember(6)] public String Company;
    [ProtoMember(7)] public int Year;
    [ProtoMember(8)] public double Cost;
    [ProtoMember(9, AsReference = true)] public List<Cinema> Cinemas;

    public Movie()
    {
      foreach (var field in this.GetType().GetProperties())
      {
        field.SetValue(this, null);
      }
      Cinemas = new List<Cinema>();
    }

    public Movie(String[] strVals, int year, double cost, List<Cinema> cinemas)
    {
      if (strVals.Length != 6)
        throw new Exception("Invalid dimension of parameters array");
      Title = strVals[0];
      Genre = strVals[1];
      Director = strVals[2];
      ManStar = strVals[3];
      WomanStar = strVals[4];
      Company = strVals[5];

      Cost = cost;
      Year = year;
      Cinemas = cinemas;
    }

    public MovieReport getInfo()
    {
      return new MovieReport(this);
    }

    public void update()
    {
      throw new NotImplementedException();
    }

    public Dictionary<Cinema, double> getMovieProfits()
    {
      Dictionary<Cinema, double> result = new Dictionary<Cinema, double>();
      foreach (var cinema in Cinemas)
      {
        PeriodReport report = new PeriodReport(cinema,
          new Tuple<DateTime, DateTime>(DateTime.MinValue, DateTime.MaxValue));
        result.Add(cinema, Cost * report.Attendance[this]);
      }
      return result;
    }

    protected bool Equals(Movie other)
    {
      return string.Equals(Title, other.Title) && string.Equals(Genre, other.Genre)
             && string.Equals(Director, other.Director) && string.Equals(ManStar, other.ManStar)
             && string.Equals(WomanStar, other.WomanStar) && string.Equals(Company, other.Company)
             && Year == other.Year && Cost.Equals(other.Cost);
    }

    public int CompareTo(Movie other)
    {
      if (Title.CompareTo(other.Title) != 0)
        return Title.CompareTo(other.Title);
      else if (Director.CompareTo(other.Director) != 0)
        return Director.CompareTo(other.Director);
      else if (Genre.CompareTo(other.Genre) != 0)
        return Genre.CompareTo(other.Genre);
      else
        return 0;
    }

    public override string ToString()
    {
      return
        $"Title: {Title}, Genre: {Genre}, Director: {Director}, ManStar: {ManStar}, WomanStar: {WomanStar}, Company: {Company}, Year: {Year}, Cost: {Cost}, Cinemas: {Cinemas}";
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((Movie) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = (Title != null ? Title.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (Genre != null ? Genre.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (Director != null ? Director.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (ManStar != null ? ManStar.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (WomanStar != null ? WomanStar.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (Company != null ? Company.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ Year;
        hashCode = (hashCode * 397) ^ Cost.GetHashCode();
        hashCode = (hashCode * 397) ^ (Cinemas != null ? Cinemas.GetHashCode() : 0);
        return hashCode;
      }
    }
  }

  public class MovieDateComparer : Comparer<Tuple<Movie, DateTime>>
  {
    public override int Compare(Tuple<Movie, DateTime> x, Tuple<Movie, DateTime> y)
    {
      if (x.Item1.CompareTo(y.Item1) != 0)
        return x.Item1.CompareTo(y.Item1);
      else if (x.Item2 == y.Item2)
        return 0;
      else if (x.Item2 < y.Item2)
        return -1;
      else
      {
        return 1;
      }
    }
  }
}