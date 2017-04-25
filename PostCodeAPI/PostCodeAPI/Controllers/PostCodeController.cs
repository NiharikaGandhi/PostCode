using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PostCodeAPI.Data;
using PostCodeAPI.Message;

namespace PostCodeAPI.Controllers
{
    [RoutePrefix("postcode")]
    public class PostCodeController : ApiController
    {
        private readonly IPostCodeRepository _postCodeRepository;
        public PostCodeController(IPostCodeRepository postCodeRepository)
        {
            _postCodeRepository = postCodeRepository;
        }

       /// <summary>
       /// Get Suburb and unique idenfier based on postcode
       /// </summary>
       /// <param name="postcode"></param>
       /// <returns></returns>
       
       [HttpGet]
       [Route("{postcode}")]
       [ResponseType(typeof(PostCodeGetResponse))]
        public async Task<IHttpActionResult> GetSuburb(int postcode)
        {
            if (postcode <= 0)
            {
                return NotFound();
            }
            var response = await _postCodeRepository.GetSuburb(postcode);
            if (response != null && response.Count != 0)
            {
                return Ok(response);
            }
            return NotFound();
        }

        /// <summary>
        /// Update PostCode/Suburb based on unique Identifier
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> UpdatPostCodeSuburb(int id, PostCodeUpdateRequest request)
        {
            if (id <= 0 || (request.Postcode <= 0 && string.IsNullOrEmpty(request.Suburb)))
            {
                return BadRequest();
            }
            
            var response = await _postCodeRepository.UpdateDetails(id, request.Postcode, request.Suburb);
            if (response)
            {
                return Ok();
            }
           
            return NotFound();

        }
    }
}
