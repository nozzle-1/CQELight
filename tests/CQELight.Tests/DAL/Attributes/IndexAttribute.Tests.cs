using CQELight.DAL.Attributes;
using CQELight.TestFramework;
using FluentAssertions;
using Xunit;

namespace CQELight.Tests.DAL.Attributes
{
    public class IndexAttributeTests : BaseUnitTestClass
    {
        #region ctor & members

        #endregion

        #region ctor

        [Fact]
        public void IndexAttribute_Ctor_AsExpected()
        {
            var i = new IndexAttribute(true, "myIndex");
            i.IsUnique.Should().BeTrue();
            i.IndexName.Should().Be("myIndex");
        }

        #endregion

    }
}
