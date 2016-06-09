using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProtoBuf;

namespace SummerPractice
{
  [ProtoContract]
  public class Cinema : IComparable<Cinema>
  {
    [ProtoMember(1)] public String Name;
    [ProtoMember(2)] public String Adress;
    [ProtoMember(3)] public String CEO;
    [ProtoMember(4)] private CinemaType Type;
    [ProtoMember(5)] public int Capacity;
    [ProtoMember(6, AsReference = true)] public List<Movie> Movies;
    [ProtoMember(7, AsReference = false)] public SortedDictionary<Movie, Tuple<DateTime, DateTime>> Dates;
    [ProtoMember(8, AsReference = false)] public SortedDictionary<Tuple<Movie, DateTime>, int> Attendance;

    public Cinema()
    {
      foreach (var field in this.GetType().GetProperties())
      {
        field.SetValue(this, null);
      }
      Dates = new SortedDictionary<Movie, Tuple<DateTime, DateTime>>();
      Attendance = new SortedDictionary<Tuple<Movie, DateTime>, int>(new MovieDateComparer());
      Movies = new List<Movie>();
    }

    public Cinema(String name, String type, int capacity, String adress, String ceo, List<Movie> movies
      , SortedDictionary<Movie, Tuple<DateTime, DateTime>> dates,
      SortedDictionary<Tuple<Movie, DateTime>, int> attendance)
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
      return new CinemaReport(this);
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
      if (Dates.ContainsKey(movie))
        return new Tuple<DateTime, DateTime>(Dates[movie].Item1, Dates[movie].Item2);
      else
        return null;
    }

    public int CompareTo(Cinema other)
    {
      if (other.Name != Name)
        return Name.CompareTo(other.Name);
      return Adress.CompareTo(other.Adress);
    }

    public override string ToString()
    {
      return
        $"Name: {Name}, Adress: {Adress}, Ceo: {CEO}, Type: {Type}, Capacity: {Capacity}, Movies: {Movies}, Dates: {Dates}, Attendance: {Attendance}";
    }

    protected bool Equals(Cinema other)
    {
      return string.Equals(Name, other.Name) && string.Equals(Adress, other.Adress) && string.Equals(CEO, other.CEO)
             && Type == other.Type && Capacity == other.Capacity;
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