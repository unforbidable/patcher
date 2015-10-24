using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.IO
{
    /// <summary>
    /// A faster version of Stream that locally caches Length and Position.
    /// </summary>
    public class ReadOnlyFileStream : FileStream
    {
        private long position;
        private long length;

        public ReadOnlyFileStream(string path, FileMode fileMode, FileAccess fileAccess) 
            : base(path, fileMode, fileAccess)
        {
            position = base.Position;
            length = base.Length;
        }

        public override long Length
        {
            get { return length; }
        }

        public override long Position
        {
            get { return position; }
            set
            {
                base.Position = value;
                position = value;
            }
        }

        public override long Seek(long offset, SeekOrigin seekOrigin)
        {
            switch (seekOrigin)
            {
                case SeekOrigin.Begin:
                    position = offset;
                    break;
                case SeekOrigin.Current:
                    position += offset;
                    break;
                case SeekOrigin.End:
                    position = Length + offset;
                    break;
            }
            return base.Seek(offset, seekOrigin);
        }

        public override int Read(byte[] array, int offset, int count)
        {
            position += count;
            return base.Read(array, offset, count);
        }

        public override int ReadByte()
        {
            position += 1;
            return base.ReadByte();
        }
    }
}
