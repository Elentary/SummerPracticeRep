using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerPractice
{
  abstract class Report
  {
    public abstract override string ToString();

    public abstract override bool Equals(object obj);

    public abstract override int GetHashCode();
  }
}