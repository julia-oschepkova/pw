using NUnit.Framework;

namespace Tests
{
    public class TestBase
    {
        [SetUp]
        public virtual void SetUp()
        {
        }

        [TearDown]
        public virtual void TearDown()
        {
        }
    }
}