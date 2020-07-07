using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace THNETII.Data.TextExtensions
{
    public class DataTableTextFormatter
    {

        public async Task WriteToAsync(TextWriter writer, DataTable dataTable,
            CancellationToken cancelToken = default)
        {

        }

        public void WriteTo(TextWriter writer, DataTable dataTable) =>
            WriteToAsync(writer, dataTable)
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    }
}
