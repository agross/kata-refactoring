using System;
using System.Collections.Generic;
using System.Linq;
using Algorithm;
using Xunit;

namespace Refactoring_1
{
  public class GoldenMaster
  {
    [Theory]
    [InlineData(FT.One, 286, "M", "A")]
    [InlineData(FT.Two, 19221, "W", "B")]
    public void Approve_saves_answer_to_database(FT ft, int d, string p1Moniker, string p2Moniker)
    {
      var things = new[]
      {
        new Thing {Moniker = "A", Date = new DateTime(1980, 5, 23)},
        new Thing {Moniker = "B", Date = new DateTime(1991, 4, 27)},
        new Thing {Moniker = "C", Date = new DateTime(1954, 4, 19)},
        new Thing {Moniker = "W", Date = new DateTime(1938, 9, 11)},
        new Thing {Moniker = "M", Date = new DateTime(1979, 8, 11)}
      };

      var finder = new Finder(things.ToList());

      F savedAnswer = null;

      finder.FindForTesting(ft,
        answer =>
        {
          savedAnswer = answer;
        });

      Assert.Equal(TimeSpan.FromDays(d), savedAnswer.D);
      Assert.Equal(p1Moniker, savedAnswer.P1.Moniker);
      Assert.Equal(p2Moniker, savedAnswer.P2.Moniker);
    }

    [Fact]
    public void Approve_does_not_save_to_database_with_empty_thing_list()
    {
      var things = new List<Thing>();

      var finder = new Finder(things.ToList());

      var wasSavedToDatabase = false;

      finder.FindForTesting(FT.One,
        _ =>
        {
          wasSavedToDatabase = true;
        });

      Assert.False(wasSavedToDatabase);
    }
  }
}
