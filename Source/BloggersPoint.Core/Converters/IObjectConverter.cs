using BloggersPoint.Core.Models;

namespace BloggersPoint.Core.Converters
{
    public interface IObjectConverter
    {
            ConversionResult Convert<T>(T dataObject);
    }
}
