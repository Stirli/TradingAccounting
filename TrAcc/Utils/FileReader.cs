using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace TrAcc.Utils
{
    public class FileReader:IDisposable
    {
        private readonly int _codepage;
        public Stream InputStream { get; set; }
        public FileReader(Stream inputStream, int codepage = 65001)
        {
            _codepage = codepage;
            InputStream = inputStream;
        }

        public IEnumerable<string> ReadLines()
        {
            int rb;
            List<byte> line = new List<byte>();
            while ((rb = InputStream.ReadByte()) > -1)
            {
                byte b = (byte)rb;
                if (b==0x0A || b==0x0D)
                {
                    if (line.Any())
                    {
                        yield return System.Text.Encoding.GetEncoding(_codepage).GetString(line.ToArray());
                        line.Clear();
                    }
                }
                else
                {
                    line.Add(b);
                }
            }
        }

        public void Dispose()
        {
            InputStream.Dispose();
        }
    }
}