using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerPractice
{
  public class CinemaReport : Report
  {
    public readonly String Name, Adress, CEO;
    private readonly CinemaType Type;
    public readonly int Capacity;
    public readonly double AverageCost;

    public CinemaReport(Cinema cinema)
    {
      Name = cinema.Name;
      Adress = cinema.Adress;
      CEO = cinema.CEO;
      CinemaType.TryParse(cinema.getType(), out Type);
      Capacity = cinema.Capacity;

      AverageCost = 0;
      foreach (var movie in cinema.Movies)
      {
        AverageCost += movie.Cost;
      }
      AverageCost /= cinema.Movies.Count();
    }

    public String getType()
    {
      return Type.ToString();
    }

    public override string ToString()
    {
      return $"Name: {Name}, Adress: {Adress}, Ceo: {CEO}, Type: {Type}," +
             $"Capacity: {Capacity}, AverageCost: {AverageCost}";
    }

    protected bool Equals(CinemaReport other)
    {
      return string.Equals(Name, other.Name) && string.Equals(Adress, other.Adress)
             && string.Equals(CEO, other.CEO) && Type == other.Type && Capacity == other.Capacity
             && AverageCost.Equals(other.AverageCost);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((CinemaReport) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = (Name != null ? Name.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (Adress != null ? Adress.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (CEO != null ? CEO.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (int) Type;
        hashCode = (hashCode * 397) ^ Capacity;
        hashCode = (hashCode * 397) ^ AverageCost.GetHashCode();
        return hashCode;
      }
    }
  }
}