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
    public class ReviewController : ApiController
    {
        private readonly IReviewDataService _dataService;

        public ReviewController(IReviewDataService dataService)
        {
            _dataService = dataService;
        }

        //CreateReview
        public Review Post([FromBody]Review review)
        {
            var createdReview = _dataService.CreateReview(review);

            return createdReview;
        }
        //UpdateReview
        public Review Put([FromBody] Review review)
        {
            var updatedReview = _dataService.UpdateReview(review);
            return updatedReview;
        }

        //GetReview???
        public Review Get(int id)
        {
            var review = _dataService.GetReviewById(id);

            return review;
        }
    }
}
