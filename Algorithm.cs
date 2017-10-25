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
  
  [Obsolete]
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
    readonly IEnumerable<Thing> _list;

    public Finder(IEnumerable<Thing> list)
    {
      _list = list;
    }

    [Obsolete("use strategy overload")]
    public Pair Find(SearchType searchType)
    {
      return Find(MapTypeToComparisonStrategy(searchType));
    }
    
    public Pair Find(ISearchStrategy strategy)
    {
      var durations = CalculateDurationsBetweenThings(_list);
      return FindShortestOrLongestDuration(strategy, durations);
    }

    static ISearchStrategy MapTypeToComparisonStrategy(SearchType searchType)
    {
      switch (searchType)
      {
        case SearchType.ShortestDuration:
          return new ShortestDurationStrategy();
        case SearchType.LongestDuration:
          return new LongestDurationStrategy();
        default:
          throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
      }
    }

    static Pair FindShortestOrLongestDuration(ISearchStrategy strategy, IEnumerable<Pair> durations)
    {
      return durations.Aggregate(durations.FirstOrDefault() ?? new Pair(), strategy.Select);
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
    public Pair Select(Pair current, Pair candidate)
    {
      return candidate.Duration > current.Duration ? candidate : current;
    }
  }

  class ShortestDurationStrategy : ISearchStrategy
  {
    public Pair Select(Pair current, Pair candidate)
    {
      return candidate.Duration < current.Duration ? candidate : current;
    }
  }

  public interface ISearchStrategy
  {
    Pair Select(Pair current, Pair candidate);
  }
}
