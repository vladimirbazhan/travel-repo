using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using WebApplication1.Models.EntityModels;
using WebApplication1.Models.Repositories;

namespace WebApplication1.Controllers
{
    public class CommentsController : ApiController
    {
        private const string ParentItemTypeTrip = "trip";
        private const string ParentItemTypeRoute = "route";
        private const string ParentItemTypeVisit = "visit";
        private const string ParentItemTypePhoto = "comment";

        [Route("api/comments/GetComment/{commentId}")]
        [HttpGet]
        public IHttpActionResult GetComment(int commentId)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Comment c = uow.Repo<CommentRepo>().Get(commentId);
              
                if (c == null)
                {
                    return NotFound();
                }

                CommentDTO cDto = new CommentDTO(c);
                
                return Ok(cDto);

            }
        }

        [Route("api/comments/UpdateComment/{commentId}")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage UpdateComment(int commentId, [FromBody] string newCommentText)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Comment c = uow.Repo<CommentRepo>().Get(commentId);

                if (c == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
               
                //Only owner of the comment can update it
                if (!String.Equals(User.Identity.GetUserId(), c.Author.Id))
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }

                c.Text = newCommentText;

                uow.Commit();

                var response = Request.CreateResponse(HttpStatusCode.OK);

                return response;
            }
        }


        [Route("api/comments/PostTripComment/{parentTripId}")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostTripComment(int parentTripId, [FromBody] string commentText)
        {
            return PostCommentGeneral(ParentItemTypeTrip, parentTripId, commentText);
        }

        [Route("api/comments/PostRouteComment/{parentRouteId}")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostRouteComment(int parentRouteId, [FromBody] string commentText)
        {
            return PostCommentGeneral(ParentItemTypeRoute, parentRouteId, commentText);
        }

        [Route("api/comments/PostVisitComment/{parentVisitId}")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostVisitComment(int parentVisitId, [FromBody] string commentText)
        {
            return PostCommentGeneral(ParentItemTypeVisit, parentVisitId, commentText);
        }



        [Route("api/comments/PostCommentComment/{parentCommentId}")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage PostCommentComment(int parentCommentId, [FromBody] string commentText)
        {
            string parentItemType;
            int parentItemId;

            using (IUnitOfWork uow = new UnitOfWork())
            {
                Comment parent = uow.Repo<CommentRepo>().Get(parentCommentId);
                if (parent.CommentsToTrips != null && parent.CommentsToTrips.Any())
                {
                    parentItemType = ParentItemTypeTrip;
                    parentItemId = parent.CommentsToTrips.First().Id;
                }
                else if (parent.CommentsToRoutes != null && parent.CommentsToRoutes.Any())
                {
                    parentItemType = ParentItemTypeRoute;
                    parentItemId = parent.CommentsToRoutes.First().Id;
                }
                else if (parent.CommentsToVisits != null && parent.CommentsToVisits.Any())
                {
                    parentItemType = ParentItemTypeVisit;
                    parentItemId = parent.CommentsToVisits.First().Id;
                }
                else if (parent.CommentsToPhotos != null)
                {
                    parentItemType = ParentItemTypePhoto;
                    parentItemId = parent.CommentsToPhotos.First().Id;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }

                return PostCommentImpl(uow, parentItemType, parentItemId, commentText);
            }
        }

        [Route("api/comments/DeleteComment/{commentId}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage DeleteComment(int commentId)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Comment c = uow.Repo<CommentRepo>().Get(commentId);

                if (c == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                //Only owner of the comment can delete it
                if (!String.Equals(User.Identity.GetUserId(), c.Author.Id))
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }

                uow.Repo<CommentRepo>().Delete(c);

                uow.Commit();

                return Request.CreateResponse(HttpStatusCode.OK);

            }
        }

        private HttpResponseMessage PostCommentGeneral(string tripItemType, int tripItemId, string commentText)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                return PostCommentImpl(uow, tripItemType, tripItemId, commentText);
            }
        }


        private HttpResponseMessage PostCommentImpl(IUnitOfWork uow, string tripItemType, int tripItemId, string commentText, Comment repplyTo = null)
        {
            Comment comment = new Comment();

            comment.Published = DateTime.Now;

            comment.ReplyTo = repplyTo;

            comment.Text = commentText;

            string usrId = User.Identity.GetUserId();
            var usr = uow.Repo<UserRepo>().Users.FirstOrDefault(x => x.Id == usrId);
            if (usr != null)
            {
                comment.Author = usr;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }


            switch (tripItemType)
            {
                case ParentItemTypeTrip:
                    uow.Repo<TripRepo>().AddComment(tripItemId, comment);
                    break;
                case ParentItemTypeVisit:
                    uow.Repo<VisitRepo>().AddComment(tripItemId,comment);
                    break;
                case ParentItemTypeRoute:
                    uow.Repo<RouteRepo>().AddComment(tripItemId, comment);
                    break;
                case ParentItemTypePhoto:
                    uow.Repo<PhotoRepo>().AddComment(tripItemId, comment);
                    break;
            }
                
            uow.Commit();

            var response = Request.CreateResponse(HttpStatusCode.Created, new CommentDTO(comment));

            return response;
        }
    }
}
