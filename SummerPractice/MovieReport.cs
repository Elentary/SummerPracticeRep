using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerPractice
{
  class MovieReport
    {
      public readonly String Title, Genre, Director, ManStar, WomanStar, Company;
      public readonly int Year;

      public MovieReport(Movie movie)
      {
        Title = movie.Title;
        Genre = movie.Genre;
        Director = movie.Director;
        ManStar = movie.ManStar;
        WomanStar = movie.WomanStar;
        Company = movie.Company;
        Year = movie.Year;
      }

      public override string ToString()
      {
        return $"Title: {Title}, Genre: {Genre}, Director: {Director}, ManStar: {ManStar}," +
               $" WomanStar: {WomanStar}, Company: {Company}, Year: {Year}";
      }

      public override bool Equals(object obj)
      {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MovieReport) obj);
      }

      protected bool Equals(MovieReport other)
      {
        return string.Equals(Title, other.Title) && string.Equals(Genre, other.Genre)
               && string.Equals(Director, other.Director) && string.Equals(ManStar, other.ManStar)
               && string.Equals(WomanStar, other.WomanStar) && string.Equals(Company, other.Company) && Year == other.Year;
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
          return hashCode;
        }
      }
    }
}
