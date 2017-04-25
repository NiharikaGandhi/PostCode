using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using PostCodeAPI.Controllers;
using PostCodeAPI.Data;
using PostCodeAPI.Message;

namespace PostCodeAPI.Tests.Controllers
{
    public class PostCodeControllerTests
    {
        [UnderTest]
        public PostCodeController _sut;
        [Fake]
        public IPostCodeRepository _PostCodeRepository;

        [SetUp]
        public void SetUp()
        {
            Fake.InitializeFixture(this);
            _sut.Request = new HttpRequestMessage();
            _sut.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
        }

        [Test]
        public async Task GivenValidPostCode_WhenSuburb_ThenResponseOK()
        {
            A.CallTo(() => _PostCodeRepository.GetSuburb(123))
                .Returns(new List<PostCodeGetResponse>()
                {
                    new PostCodeGetResponse()
                    {
                        Suburb = "abc",
                        Id = 1,
                        PostCode = 123
                    }
                });

            var response = await _sut.GetSuburb(123);

            var result = response as OkNegotiatedContentResult<List<PostCodeGetResponse>>;

            result.Content.FirstOrDefault().Suburb.Should().Be("abc");
            result.Content.FirstOrDefault().Id.Should().Be(1);

        }

        [Test]
        public async Task GivenInValidPostCode_WhenSuburb_ThenResponseOK()
        {
            A.CallTo(() => _PostCodeRepository.GetSuburb(123))
                .Returns(new List<PostCodeGetResponse>());

            var response = await _sut.GetSuburb(123);

            var result = response as OkNegotiatedContentResult<List<PostCodeGetResponse>>;

            result.Should().BeNull();

        }

        [Test]
        public async Task GivenValidId_WhenUpdate_ThenResponsTrue()
        {
            A.CallTo(() => _PostCodeRepository.UpdateDetails(1, 123, "abc"))
                .Returns(true);

            var response = await _sut.UpdatPostCodeSuburb(1, new PostCodeUpdateRequest()
            {
                Postcode = 123,
                Suburb = "abc"
            });

            response.ExecuteAsync(new CancellationToken()).Result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GivenInValidId_WhenUpdate_ThenResponseFalse()
        {
            A.CallTo(() => _PostCodeRepository.UpdateDetails(1, 123, "abc"))
                .Returns(false);

            var response = await _sut.UpdatPostCodeSuburb(1, new PostCodeUpdateRequest()
            {
                Postcode = 123,
                Suburb = "abc"
            });

            response.ExecuteAsync(new CancellationToken()).Result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
