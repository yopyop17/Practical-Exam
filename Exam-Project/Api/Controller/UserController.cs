using Exam_Project.Api.Commands;
using Exam_Project.Api.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Controller
{
    
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost, Route("newUser"), AllowAnonymous]
        //#endif
        public async Task<IActionResult> newUser([FromBody] NewUserCommand command)
        {
            return HandleResponseFull(await this._userService.NewUser(command));
        }

    }
}
