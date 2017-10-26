using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Algorithm;

static class Personen
{
  public static IEnumerable<Kombination> ErzeugeKombinationen(IEnumerable<Person> personen)
  {
    return personen.SelectMany((person1, index) =>
      {
        return personen.Skip(index + 1)
          .Select(person2 => new Kombination(person1, person2));
      }
    );
  }
}

class PersonenKatharina : IEnumerable<Person>
{
  readonly IEnumerable<Person> _personen;

  public PersonenKatharina(IEnumerable<Person> personen)
  {
    _personen = personen;
  }

  public IEnumerator<Person> GetEnumerator()
  {
    return _personen.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return _personen.GetEnumerator();
  }

  public IEnumerable<Kombination> ErzeugeKombinationen()
  {
    return _personen.SelectMany((person1, index) =>
      {
        return _personen.Skip(index + 1)
          .Select(person2 => new Kombination(person1, person2));
      }
    );
  }
}
