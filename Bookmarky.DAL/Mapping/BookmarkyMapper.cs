﻿using System.Collections.Generic;
using System.Linq;
using Bookmarky.DAL.EntityModels;
using Bookmarky.DTO;
using Bookmarky.Utility.Extensions;
using Omu.ValueInjecter;
using Bookmark_Dto = Bookmarky.DTO.Bookmark;
using Bookmark_Db = Bookmarky.DAL.EntityModels.Bookmark;
using Tag_Dto = Bookmarky.DTO.Tag;
using Tag_Db = Bookmarky.DAL.EntityModels.Tag;

namespace Bookmarky.DAL.Mapping
{
    public class BookmarkyMapper : IBookmarkyMapper
    {
        private readonly ConventionInjection _convention;

        public BookmarkyMapper(ConventionInjection convention)
        {
            _convention = convention;
        }

        public Bookmark_Dto MapToBookmarkDto(Bookmark_Db bookmarkDb)
        {
            var bookmarkDto = bookmarkDb.MapTo<Bookmark_Dto>(_convention);

            if (bookmarkDb.Tags != null)
            {
                bookmarkDto.Tags = bookmarkDb.Tags.Select(MapToTagDto);                
            }
            else
            {
                bookmarkDto.Tags = new List<Tag_Dto>();
            }

            if (bookmarkDb.Rating != null)
            {
                bookmarkDto.Review = bookmarkDb.Rating.MapTo<Review>(_convention);
            }
            else
            {
                bookmarkDto.Review = new Review();
            }

            return bookmarkDto;
        }

        public Bookmark_Db MapToBookmarkDb(Bookmark_Dto bookmarkDto)
        {
            var bookmarkDb = bookmarkDto.MapTo<Bookmark_Db>(_convention);

            if (bookmarkDto.Tags != null)
            {
                bookmarkDb.Tags = bookmarkDto.Tags.Select(MapToTagDb).ToList();
                
            }
            else
            {
                bookmarkDb.Tags = new List<Tag_Db>();
            }

            return bookmarkDb;
        }

        public Tag_Dto MapToTagDto(Tag_Db tagDb)
        {
            var tagDto = tagDb.MapTo<Tag_Dto>(_convention);

            return tagDto;
        }

        public Tag_Db MapToTagDb(Tag_Dto tagDto)
        {
            var tagDb = tagDto.MapTo<Tag_Db>(_convention);

            return tagDb;
        }

        public Review MapToReviewDto(Rating rating)
        {
            var review = rating.MapTo<Review>(_convention);

            return review;
        }

        public Rating MapToRatingDb(Review review)
        {
            var rating = review.MapTo<Rating>(_convention);

            return rating;
        }
    }
}