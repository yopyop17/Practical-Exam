using Exam_Project.Api.Commands;
using Exam_Project.Api.Constants;
using Exam_Project.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Controller
{
    //#if !DEBUG
    [Authorize]
    //#endif

    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected string GetUserAgent(HttpRequest Request)
        {
            string userAgent = string.Empty;
            try
            {
                StringValues result;
                if (!Request.Headers.TryGetValue("UserAgent", out result))
                    userAgent = "";
                userAgent = result.FirstOrDefault();

                if (string.IsNullOrWhiteSpace(userAgent))
                    userAgent = Request.Headers["User-Agent"].ToString();
            }
            catch
            {
                userAgent = string.Empty;
            }
            return userAgent;
        }

        #region HANDLE RESPONSE
        protected IActionResult HandleResponse<T>(Response<Result<T>> response) where T : class
        {
            if (response.HasError)
                return BadRequest(response);

            if (response.Status == 403)
                return Forbid();

            response.Success = true;
            return Ok(response.Data);
        }


        protected IActionResult HandleResponse<T>(Response<Result<T>> response, BaseCommand command) where T : class
        {
            if (response.HasError)
                return BadRequest(response);

            if (response.Status == 403)
                return Forbid();

            response.Success = true;
            if (command.isNG_JSON_CALLBACK)
            {
                return Content(string.Format(Constant.ng_json_callback, JsonConvert.SerializeObject(response.Data.Data)));
            }
            else
                return Ok(response.Data);
        }

        protected IActionResult HandleResponse<T>(Response<T> response) where T : class
        {
            if (response == null || response.HasError)
                return BadRequest(response);

            if (response.Status == 403)
                return Forbid();

            response.Success = true;
            return Ok(response.Data);
        }
        protected IActionResult HandleResponseFull<T>(Response<T> response) where T : class
        {
            if (response.HasError)
                return BadRequest(response);

            if (response.Status == 403)
                return Forbid();

            response.Success = true;
            return Ok(response);
        }
        protected IActionResult HandleResponse(Response<bool> response)
        {
            if (response.HasError)
                return BadRequest(response);

            if (response.Status == 403)
                return Forbid();

            response.Success = true;
            return Ok(response.Data);
        }
        protected IActionResult HandleResponse(Response<int> response)
        {
            if (response.HasError)
                return BadRequest(response);

            if (response.Status == 403)
                return Forbid();

            response.Success = true;
            return Ok(response.Data);
        }
        protected IActionResult HandleResponse(Response response)
        {
            if (response.HasError)
                return Error(response);

            if (response.Status == 403)
                return Forbid();

            response.Success = true;
            return Ok(response);
        }
        protected IActionResult Error(Response response)
        {
            return Error(response, System.Net.HttpStatusCode.BadRequest);
        }
        protected IActionResult Error(Response response, System.Net.HttpStatusCode statusCode)
        {
            var error = new ErrorDTO();
            if (!response.HasError)
                error.Message = response.ErrorMessage;
            else
                error.Message = response.ErrorMessage;

            error.Key = response.ErrorKey?.ToLower();
            error.Status = (int)statusCode;

            return BadRequest(error);
        }
        #endregion
    }
}
