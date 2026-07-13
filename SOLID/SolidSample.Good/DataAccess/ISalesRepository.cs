using System.Collections.Generic;
using SolidSample.Good.Domain;

namespace SolidSample.Good.DataAccess;

public interface ISalesRepository
{
    IReadOnlyList<SalesRecord> GetAll();
}
