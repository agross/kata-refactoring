using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm
{  
  public class Pair
  {
    public Thing First { get; set; }
    public Thing Second { get; set; }
    public TimeSpan Duration { get; set; }
  }
  
  public enum SearchType
  {
    ShortestDuration,
    LongestDuration
  }

  public class Thing
  {
    public string Moniker { get; set; }
    public DateTime Date { get; set; }
  }

  public class Finder
  {
    private readonly List<Thing> _list;

    public Finder(List<Thing> list)
    {
      _list = list;
    }

    public Pair Find(SearchType searchType)
    {
      var durations = CalculateDurationsBetweenThings(_list);

      if(!durations.Any())
      {
        return new Pair();
      }

      return FindShortestOrLongestDuration(searchType, durations);
    }

    static Pair FindShortestOrLongestDuration(SearchType searchType, IEnumerable<Pair> durdations)
    {
      var answer = durdations.First();
      foreach (var result in durdations)
      {
        switch (searchType)
        {
          case SearchType.ShortestDuration:
            if (result.Duration < answer.Duration)
            {
              answer = result;
            }
            break;

          case SearchType.LongestDuration:
            if (result.Duration > answer.Duration)
            {
              answer = result;
            }
            break;
        }
      }
      return answer;
    }

    static IEnumerable<Pair> CalculateDurationsBetweenThings(IEnumerable<Thing> things)
    {
      return things.SelectMany((left, i) =>
      {
        return things
          .Skip(i + 1)
          .Select(right =>
          {
            var pair = EarliestFirst(left, right);
            pair.Duration = pair.Second.Date - pair.First.Date;
            return pair;
          });
      });
    }

    static Pair EarliestFirst(Thing left, Thing right)
    {
      if (left.Date < right.Date)
      {
        return new Pair {First = left, Second = right};
      }
      
      return new Pair {First = right, Second = left};
    }
  }
}
