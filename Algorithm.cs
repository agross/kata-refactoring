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

      var strategy = MapTypeToComparisonStrategy(searchType);
      return FindShortestOrLongestDuration(strategy, durations);
    }

    static ISearchStrategy MapTypeToComparisonStrategy(SearchType searchType)
    {
      switch (searchType)
      {
        case SearchType.ShortestDuration:
          return new ShortestDurationStrategy();
          break;
        case SearchType.LongestDuration:
          return new LongestDurationStrategy();
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
      }
    }

    static Pair FindShortestOrLongestDuration(ISearchStrategy strategy, IEnumerable<Pair> durations)
    {
      var answer = durations.First();
      foreach (var result in durations)
      {
        if (strategy.Select(answer, result))
          answer = result;
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

  class LongestDurationStrategy : ISearchStrategy
  {
    public bool Select(Pair current, Pair candidate)
    {
      return candidate.Duration > current.Duration;
    }
  }

  class ShortestDurationStrategy : ISearchStrategy
  {
    public bool Select(Pair current, Pair candidate)
    {
      return candidate.Duration < current.Duration;
    }
  }

  interface ISearchStrategy
  {
    bool Select(Pair current, Pair candidate);
  }
}
