using Glimpse.AspNet.Tab;
using Optio.Web.Domain;
using Optio.Web.Models.Requests;
using Optio.Web.Models.Responses;
using Optio.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Optio.Web.Controllers.Api 
{
    [RoutePrefix("api/Articles")]
    public class ArticlesApiController : BaseApiController
    {
        IArticlesService _articlesService;

        public ArticlesApiController(IArticlesService articlesService)
        {
            _articlesService = articlesService;
        }


        [Route, HttpPost]
        public HttpResponseMessage Add(ArticlesAddRequest model)
        {
            
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {
                ItemResponse<int> resp = new ItemResponse<int>();
                resp.Item = _articlesService.Insert(model);

                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, CreateExceptionResponse(ex));
            }
        }


        [Route("{id:int}"), HttpPut]
        public HttpResponseMessage Put(ArticlesUpdateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {
                _articlesService.Put(model);
                SuccessResponse sr = new SuccessResponse();

                return Request.CreateResponse(HttpStatusCode.OK, sr);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, CreateExceptionResponse(ex));
            }
        }


        [Route, HttpGet]
        public HttpResponseMessage GetAll()
        {
            BaseResponse response = null;
            HttpStatusCode statusCode = HttpStatusCode.OK; 

            try
            {
                ItemsResponse<Article> resp = new ItemsResponse<Article>();
                resp.Items = _articlesService.GetAll();
                response = resp;
            }
            catch (Exception ex)
            {
                response = new ErrorResponse(ex);
                statusCode = HttpStatusCode.InternalServerError; 
            }
            return Request.CreateResponse(statusCode, response);
        }


        [Route("{id:int}"), HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {
                ItemResponse<Article> resp = new ItemResponse<Article>();
                resp.Item = _articlesService.GetById(id);
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, CreateExceptionResponse(ex));

            }

        }


        [Route("{id:int}"), HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {
                SuccessResponse sr = new SuccessResponse();
                _articlesService.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK, sr);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, CreateExceptionResponse(ex));

            }
        }


    }
  
        
}
