using System;
using System.IO;
using System.Threading.Tasks;

namespace REVUnit.Crlib.Extension
{
    public static class XStream
    {
        public static void Write(this Stream stream, byte[] buffer)
        {
            (stream ?? throw new ArgumentNullException(nameof(stream))).Write(buffer, 0,
                (buffer ?? throw new ArgumentNullException(nameof(buffer))).Length);
            if (X.AutoFlush) stream.Flush();
        }

        public static void Write(this Stream stream, string str)
        {
            byte[] buffer = X.Encoding.GetBytes(str);
            Write(stream, buffer);
        }

        public static async Task WriteAsync(this Stream stream, string str)
        {
            byte[] buffer = X.Encoding.GetBytes(str);
            await WriteAsync(stream, buffer);
        }

        public static void WriteLine(this Stream stream, string str)
        {
            Write(stream, str + X.NewLine);
        }

        public static async Task WriteLineAsync(this Stream stream, string str)
        {
            await WriteAsync(stream, str + X.NewLine);
        }

        public static async Task WriteAsync(this Stream stream, byte[] buffer)
        {
            await (stream ?? throw new ArgumentNullException(nameof(stream))).WriteAsync(buffer, 0,
                (buffer ?? throw new ArgumentNullException(nameof(buffer))).Length);
            if (X.AutoFlush) await stream.FlushAsync();
        }
    }
}