using System;
using System.IO;
using NUnit.Framework;

namespace Streams.Tests
{
    [TestFixture]
    public class CombinedStreamTests
    {
        private static Stream MemStreamOfBytes(params byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        [Test]
        public void Constructor_NullUnderlyingStreams_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CombinedStream(null));
        }

        [Test]
        public void LengthShouldBeSumOfLengthsOfUnderlyingStreams()
        {
            var us1 = MemStreamOfBytes(0x1);
            var us2 = MemStreamOfBytes(0x1, 0x2);
            var us3 = MemStreamOfBytes(0x1, 0x2, 0x3);

            var cs = new CombinedStream(us1, us2, us3);
            Assert.That(cs.Length, Is.EqualTo(us1.Length + us2.Length + us3.Length), "Length");
        }

        [Test]
        public void ReadWholeStream()
        {
            var us1 = MemStreamOfBytes(0x1);
            var us2 = MemStreamOfBytes(0x2);
            var us3 = MemStreamOfBytes(0x3);

            var cs = new CombinedStream(us1, us2, us3);
            var buf = new byte[3];
            Assert.That(cs.Read(buf, 0, 3), Is.EqualTo(3), "Read");
            Assert.That(buf, Is.EqualTo(new[]{ 0x1, 0x2, 0x3 }), "readed buf");
        }

        [Test]
        public void CanSeekToEnd()
        {
            var us1 = MemStreamOfBytes(0x1);
            var us2 = MemStreamOfBytes(0x2);
            var us3 = MemStreamOfBytes(0x3);

            var cs = new CombinedStream(us1, us2, us3);
            Assert.That(cs.Seek(0, SeekOrigin.End), Is.EqualTo(3), "Seek");
            Assert.That(cs.Position, Is.EqualTo(3), "Position");
        }
    }
}
