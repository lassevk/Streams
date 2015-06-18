using System;
using NUnit.Framework;

namespace Streams.Tests
{
    [TestFixture]
    public class CombinedStreamTests
    {
        [Test]
        public void Constructor_NullUnderlyingStreams_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CombinedStream(null));
        }
    }
}
