using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace REVUnit.Crlib.Extension
{
    public static class XStream
    {
        public static bool AutoFlush { get; set; } = true;

        public static void Write(this Stream stream, byte[] buffer)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            stream.Write(buffer, 0, buffer.Length);
            if (AutoFlush) stream.Flush();
        }

        public static void Write(this Stream stream, string str)
        {
            byte[] buffer = Encoding.Default.GetBytes(str);
            Write(stream, buffer);
        }

        public static async Task WriteAsync(this Stream stream, string str)
        {
            byte[] buffer = Encoding.Default.GetBytes(str);
            await WriteAsync(stream, buffer).ConfigureAwait(false);
        }

        public static void WriteLine(this Stream stream, string str)
        {
            Write(stream, str + Environment.NewLine);
        }

        public static async Task WriteLineAsync(this Stream stream, string str)
        {
            await WriteAsync(stream, str + Environment.NewLine).ConfigureAwait(false);
        }

        public static async Task WriteAsync(this Stream stream, byte[] buffer)
        {
            await (stream ?? throw new ArgumentNullException(nameof(stream))).WriteAsync(buffer, 0,
                (buffer ?? throw new ArgumentNullException(nameof(buffer))).Length);
            if (AutoFlush) await stream.FlushAsync().ConfigureAwait(false);
        }
    }
}