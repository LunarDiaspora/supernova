using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace LuminalNAudioEngine
{
    public class RewindingStream : WaveStream
    {
        WaveStream source;

        public RewindingStream(WaveStream src)
        {
            source = src;
        }

        public override WaveFormat WaveFormat
        {
            get { return source.WaveFormat; }
        }

        public override long Length
        {
            get { return source.Length; }
        }

        public override long Position
        {
            get { return source.Position; }
            set { source.Position = value; }
        }

        public delegate void OnEnd();
        public event OnEnd PlaybackEnd;

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = source.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    if (source.Position == 0)
                    {
                        // something wrong with the source stream
                        break;
                    }
                    source.Position = 0;
                    totalBytesRead += bytesRead;
                    if (PlaybackEnd != null) PlaybackEnd();
                    return totalBytesRead;
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }
}
