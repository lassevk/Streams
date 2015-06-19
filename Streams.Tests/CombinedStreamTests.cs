using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable PossibleNullReferenceException

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

        [Test]
        [TestCase(0, 5)]
        [TestCase(1, 5)]
        [TestCase(2, 5)]
        [TestCase(3, 5)]
        [TestCase(4, 5)]
        [TestCase(5, 5)]
        public void CanSeekToAllPositionsAcrossMultipleStreams(int position, int length)
        {
            var streams = Enumerable.Range(0, length).Select(_ => MemStreamOfBytes(0x00)).ToArray();
            var combined = new CombinedStream(streams);

            combined.Seek(position, SeekOrigin.Begin);
            Assert.That(combined.Position, Is.EqualTo(position));
        }
    }
}
