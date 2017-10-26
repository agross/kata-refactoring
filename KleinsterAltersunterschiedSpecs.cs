using System;
using Algorithm;
using Xunit;

public class KleinsterAltersunterschied
{
  [Fact]
  public void Selektiert_kleinsten_Unterschied()
  {
    var personen = new[]
    {
      new Kombination(new Person {Name = "Alex", Geburtsdatum = new DateTime(1900, 1, 1)},
        new Person {Name = "Peter", Geburtsdatum = new DateTime(1902, 1, 1)}),
      new Kombination(new Person {Name = "Alex", Geburtsdatum = new DateTime(1900, 1, 1)},
        new Person {Name = "Hanna", Geburtsdatum = new DateTime(1901, 1, 1)})
    };

    var selektor = new Finder.KleinsterAltersunterschied();

    var ergebis = selektor.ErmittleErgebnis(personen);

    Assert.Equal("Alex", ergebis.Person1.Name);
    Assert.Equal("Hanna", ergebis.Person2.Name);
  }
}