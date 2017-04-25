using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using PostCodeAPI.Data;

namespace PostCodeAPI.Tests.Data
{
    public class PostCodeRepositoryTests
    {
        [UnderTest] private PostCodeRepository _sut;
       
        [SetUp]
        public void SetUp()
        {
           _sut = new PostCodeRepository(new ConnectionStringProvider());
        }

        
        [Test]
        public async Task GivenValidPostCode_WhenGetSuburb_ThenWeSucceed()
        {
            var response = await _sut.GetSuburb(-9999); //Added only for testing purpose

            response.FirstOrDefault().Suburb.Should().Be("test data");
        }

        [Test]
        public async Task GivenInValidPostCode_WhenGetSuburb_ThenWeNotFound()
        {
            var response = await _sut.GetSuburb(-9998);

            response.Count.Should().Be(0);
        }

        [Test]
        public async Task GivenValidIdAndUpdateDetails_WhenUpdate_ThenWeGetTrue()
        {
            var response = await _sut.UpdateDetails(8610, 0, "mytestingupdate"); 

            response.Should().Be(true);
        }

    }
}