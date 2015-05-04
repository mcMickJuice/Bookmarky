using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Bookmarky.DAL.Service;
using Bookmarky.DTO;

namespace Bookmarky.WebAPI.Controllers
{
    [EnableCors(origins: "http://localhost:9000", headers: "*", methods: "*")]
    public class TagController : ApiController
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        public IEnumerable<Tag> Get()
        {
            var tags = _tagService.GetAllTags();

            return tags;
        }

        [AcceptVerbs("POST")]
        public int CreateTag(Tag tag)
        {
            var createdId = _tagService.CreateTag(tag);

            return createdId;
        }
    }
}
