using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookmarky.DTO;

namespace Bookmarky.DAL.Service
{
    public interface IReviewDataService
    {
        Review CreateReview(Review review);
        Review UpdateReview(Review review);
        Review GetReviewById(int id);
    }
}
