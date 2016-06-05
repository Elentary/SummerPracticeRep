using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SummerPractice
{
  class Cinema
  {
    public String Name, Adress, CEO;
    private CinemaType Type;
    public int Capacity;
    public List<Movie> Movies;
    public SortedDictionary<Movie, Tuple<DateTime, DateTime>> Dates;
    public SortedDictionary<Tuple<Movie, DateTime>, int> Attendance;

    public Cinema()
    {
      foreach (var field in this.GetType().GetProperties())
      {
        field.SetValue(this, null);
      }
    }

    public Cinema(String name, String type, int capacity, String adress, String ceo, List<Movie> movies
      ,SortedDictionary<Movie, Tuple<DateTime, DateTime>> dates, SortedDictionary<Tuple<Movie, DateTime>, int> attendance)
    {
      Name = name;
      if (!CinemaType.TryParse(type, out Type))
      {
        Exception e = new WarningException("Невозможно преобразовать к типу перечисления");
        Program.log.Warn(e);
        throw e;
      }
      Capacity = capacity;
      Adress = adress;
      CEO = ceo;
      Movies = movies;
      Dates = dates;
      Attendance = attendance;
    }

    public String getType()
    {
      return Type.ToString();
    }

    public CinemaReport getInfo()
    {
      throw new NotImplementedException();
    }

    public DailyReport getDayReport()
    {
      throw new NotImplementedException();
    }

    public void update()
    {
      throw new NotImplementedException();
    }

    public Tuple<DateTime, DateTime> getDates(Movie movie)
    {
      throw new NotImplementedException();
    }

    public override string ToString()
    {
      return $"Name: {Name}, Adress: {Adress}, Ceo: {CEO}, Type: {Type}, Capacity: {Capacity}, Movies: {Movies}, Dates: {Dates}, Attendance: {Attendance}";
    }

    protected bool Equals(Cinema other)
    {
      return string.Equals(Name, other.Name) && string.Equals(Adress, other.Adress) && string.Equals(CEO, other.CEO)
             && Type == other.Type && Capacity == other.Capacity && Equals(Movies, other.Movies)
             && Equals(Dates, other.Dates) && Equals(Attendance, other.Attendance);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((Cinema) obj);
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
        hashCode = (hashCode * 397) ^ (Movies != null ? Movies.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (Dates != null ? Dates.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (Attendance != null ? Attendance.GetHashCode() : 0);
        return hashCode;
      }
    }
  }
}