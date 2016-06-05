using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerPractice
{
  class Movie
  {
    public String Title, Genre, Director, ManStar, WomanStar, Company;
    public int Year;
    public double Cost;
    public Cinema[] Cinemas;

    protected bool Equals(Movie other)
    {
      return string.Equals(Title, other.Title) && string.Equals(Genre, other.Genre)
             && string.Equals(Director, other.Director) && string.Equals(ManStar, other.ManStar)
             && string.Equals(WomanStar, other.WomanStar) && string.Equals(Company, other.Company)
             && Year == other.Year && Cost.Equals(other.Cost) && Equals(Cinemas, other.Cinemas);
    }

    public override string ToString()
    {
      //check how cinemas is formatting
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

    public Movie()
    {
      foreach (var field in typeof(Movie).GetProperties())
      {
        field.SetValue(this, null);
      }
    }

    public Movie(String[] strVals, int year, double cost, Cinema[] cinemas)
    {
      if (strVals.Length != 5)
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
  }
}
