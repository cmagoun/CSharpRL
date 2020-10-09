using NUnit.Framework;
using SadSharp.Helpers;

namespace SadSharpTests
{
    public class PadTests
    {
        [Test]
        public void PadAddsSpaces()
        {
            Assert.AreEqual(" 9", 9.Pad(2));
            Assert.AreEqual("  9", 9.Pad(3));
            Assert.AreEqual(" 99", 99.Pad(3));
            Assert.AreEqual("999", 999.Pad(3));
        }
    }
}
