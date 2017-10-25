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
      var durdations = CalculateDurationsBetweenThings(_list);

      if(durdations.Count < 1)
      {
        return new Pair();
      }

      var answer = FindShortestOrLongestDuration(searchType, durdations);

      return answer;
    }

    static Pair FindShortestOrLongestDuration(SearchType searchType, List<Pair> durdations)
    {
      Pair answer = durdations[0];
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

    static IEnumerable<Pair> CalculateDurationsBetweenThings(List<Thing> things)
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
      var pair = new Pair();
      if (left.Date < right.Date)
      {
        pair.First = left;
        pair.Second = right;
      }
      else
      {
        pair.First = right;
        pair.Second = left;
      }
      return pair;
    }
  }
}
