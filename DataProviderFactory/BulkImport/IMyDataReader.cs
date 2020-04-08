using System.Collections.Generic;
using System.Data;

namespace DataProviderFactory.BulkImport
{
    public interface IMyDataReader<T> : IDataReader
    {
        IList<T> Records { get; set; }
    }
}
