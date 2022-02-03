using AutoMapper;
using Exam_Project.Api.Commands;
using Exam_Project.Api.Constants;
using Exam_Project.Api.Contracts;
using Exam_Project.Api.Data;
using Exam_Project.Api.DTO;
using Exam_Project.Api.Helpers;
using Exam_Project.Api.Models;
using Exam_Project.Api.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Services
{
    public class UserService : BaseService<IUserService>, IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidator;
        private readonly AuthSettings _authSettings;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(
               ILogger<IUserService> logger,
               IHttpContextAccessor httpContextAccessor,
               ClientDbContext clientDbContext,
               UserManager<ApplicationUser> userManager,
               IOptions<AuthSettings> authSettings,
               IMapper mapper,
               SignInManager<ApplicationUser> signInManager,
               IPasswordValidator<ApplicationUser> passwordValidator)
            :
            base(logger, httpContextAccessor, clientDbContext)
        {
            this._mapper = mapper;
            this._userManager = userManager;
            this._authSettings = authSettings.Value;
            this._passwordValidator = passwordValidator;
            this._signInManager = signInManager;
        }

        public async Task<Response<UserDTO>> NewUser(NewUserCommand command)
        {
            var response = new Response<UserDTO>();
            try
            {

                var aspUser = await this._userManager.FindByEmailAsync(command.Email);
                if (aspUser == null)
                {
                    aspUser = new ApplicationUser()
                    {
                        UserName = command.Email,
                        Email = command.Email,
                        RegisteredDate = DateTime.UtcNow,
                    };

                    var result = await this._userManager.CreateAsync(aspUser);
                    if (result.Succeeded)
                    {
                        var resultPass = await _userManager.AddPasswordAsync(aspUser, command.Password);

                        var new_user = new User()
                        {
                            Id = aspUser.Id,
                            FirstName = command.FirstName,
                            LastName = command.LastName,
                            Email = command.Email,
                        };

                        this.ClientDbContext.Users.Add(new_user);
                        response.Data = this._mapper.Map<UserDTO>(new_user);

                    }
                    response.Success = await this.ClientDbContext.SaveChangesAsync() > 0;
                    if (response.Success)
                    {
                        response.Data.Message = "User successfully registered";
                    }
                }
                else
                {
                    response.AddValidationError("Email Alrready Taken", "Email Already Taken");
                }
            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }

        public async Task<Response<string>> AuthBearer(LoginMediaCommand command)
        {
            var response = new Response<string>();
            try
            {
                var bearer = await HttpRequestHelper.Auth<dynamic>($"{this._authSettings.Authority}connect/token",
                        command, this._authSettings.ClientSecret);

                response.Data = JsonConvert.SerializeObject(bearer);


                if (response.Data.Contains("error_key"))
                {
                    response.ErrorMessage = "Invalid credentials";
                }
                else
                {
                    response.Success = true;
                    response.Data = "access_token:" + command.MediaToken + LoginData.GoogleAccessToken;
                }

            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }

    }
}
