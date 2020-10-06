using NUnit.Framework;
using SadSharp.Helpers;

namespace Tests
{
    public class RollerTests
    {
        [Test]
        public void RollerCanParseDieString()
        {
            Roller.Create();
            
            for (var x = 0; x < 1000; x++)
            {
                var result = Roller.Next("1d6");
                Assert.True(result > 0 && result < 7);
            }
        }

        [Test]
        public void RollerAddsConstantTerm()
        {
            Roller.Create();

            var result = Roller.Next("1d1+20");
            Assert.AreEqual(21, result);
        }

        [Test]
        public void RollerRollsMultipleDice()
        {
            Roller.Create();

            var result = Roller.Next("6d1");
            Assert.AreEqual(6, result);
        }

        [Test]
        public void RollerCanParseMultipleDieStrings()
        {
            Roller.Create();

            var result = Roller.Next("1d1&2d1&3d1");
            Assert.AreEqual(6, result);

        }

        [Test]
        public void RollerCanTakeTestRollsAndReturnThem()
        {
            Roller.Create(null, 1, 2, 3, 4, 5, 6);

            Assert.AreEqual(1, Roller.Next("1d100+100")); //Roller ignores roll and returns the test
            Assert.AreEqual(2, Roller.Next("1d100+100"));
            Assert.AreEqual(3, Roller.Next("1d100+100"));
            Assert.AreEqual(4, Roller.Next("1d100+100"));
            Assert.AreEqual(5, Roller.Next("1d100+100"));
            Assert.AreEqual(6, Roller.Next("1d100+100"));

            Assert.AreEqual(10, Roller.Next("1d1+9")); //we are out of test rolls, so now we roll dice
        }

        [Test]
        public void RollerCanTakeSeed()
        {
            Roller.Create(12345);

            Assert.AreEqual(1, Roller.NextD6);
            Assert.AreEqual(1, Roller.NextD8);
            Assert.AreEqual(8, Roller.NextD10);
            Assert.AreEqual(7, Roller.NextD12);
            Assert.AreEqual(13, Roller.NextD16);
            Assert.AreEqual(17, Roller.NextD20);
            Assert.AreEqual(17, Roller.NextD100);
            Assert.AreEqual(7362, Roller.Next(1, 10001));
            Assert.AreEqual(22, Roller.Next("3d20"));

            Roller.Create(54321);

            Assert.AreEqual(3, Roller.NextD6);
            Assert.AreEqual(5, Roller.NextD8);
            Assert.AreEqual(5, Roller.NextD10);
            Assert.AreEqual(12, Roller.NextD12);
            Assert.AreEqual(6, Roller.NextD16);
            Assert.AreEqual(12, Roller.NextD20);
            Assert.AreEqual(80, Roller.NextD100);
            Assert.AreEqual(4407, Roller.Next(1, 10001));
            Assert.AreEqual(18, Roller.Next("3d20"));
        }
    }
}
