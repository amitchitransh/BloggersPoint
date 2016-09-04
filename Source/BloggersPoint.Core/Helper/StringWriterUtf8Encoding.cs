using System.IO;
using System.Text;

namespace BloggersPoint.Core.Helper
{
    public sealed class StringWriterUtf8Encoding : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
