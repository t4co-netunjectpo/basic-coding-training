using System.Collections.Generic;
using SolidSample.Good.Domain;

namespace SolidSample.Good.DataAccess;

public class InMemorySalesRepository : ISalesRepository
{
    public IReadOnlyList<SalesRecord> GetAll()
    {
        return new List<SalesRecord>
        {
            new SalesRecord("Apple", 100m),
            new SalesRecord("Banana", 200m),
            new SalesRecord("Cherry", 300m)
        };
    }
}
