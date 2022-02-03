using AutoMapper;
using Exam_Project.Api.Commands;
using Exam_Project.Api.Contracts;
using Exam_Project.Api.Data;
using Exam_Project.Api.DTO;
using Exam_Project.Api.Models;
using Exam_Project.Api.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Services
{
    public class OrderService : BaseService<IOrderService>, IOrderService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidator;
        private readonly AuthSettings _authSettings;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public OrderService(
            ILogger<IOrderService> logger,
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

        public async Task<Response<OrderDTO>> NewOrder(OrderCommand command)
        {
            var response = new Response<OrderDTO>() { Data = new OrderDTO() };
            try
            {
                var curUser = await this.ClientDbContext.Users.Where(w => w.Id == CurrentUser.User.Id).FirstOrDefaultAsync();

                if (curUser == null)
                {
                    response.AddValidationError(() => "Users not Exist", "Users not Exist");
                    return response;
                }

                this.Log(JsonConvert.SerializeObject(command), $"NEW_ORDER_TIME:{DateTime.Now}");

                var is_new = command.Id <= 0;

                var new_order = new Order();
                #region CREATE/UPDATE -ORDER


                new_order = this._mapper.Map<Order>(command);

                #region Add Products on ORDER

                #endregion

                #region DATA CLEAN-UP

                if (new_order.Customer.Id > 0)
                {
                    long userId = 0;
                    var loc_user = await ClientDbContext.Users.FirstOrDefaultAsync(f => f.Id == new_order.Customer.Id);
                    if (loc_user == null)
                    {
                        var systemuser = await ClientDbContext.Users.FirstOrDefaultAsync(f => f.Id == new_order.Customer.Id);
                        if (systemuser != null)
                        {
                            userId = systemuser.Id;
                        }
                        else
                            userId = new_order.Customer.Id;
                    }
                    else
                    {
                        userId = loc_user.Id;
                    }
                    new_order.CustomerId = userId;

                }
                else if (command.Customer.Id == -1)
                {
                    new_order.Customer = null;
                    new_order.Customer.Id = -1;
                }
                #endregion

                response.Success = await ClientDbContext.SaveChangesAsync() > 0;



                if (!response.Success)
                {
                    this.Log(JsonConvert.SerializeObject(command), $"FAILED_NEW_ORDER_TIME:{DateTime.Now}");
                    response.AddValidationError("", "FAILED! ORDER NOT CREATED!");
                    return response;
                }

                if (response.Success)
                {
                    #region LOG TO INVENTORY
                    //await this.InventoryTransaction(new_order);
                    #endregion
                }
                response.Data = _mapper.Map<OrderDTO>(new_order);

                this.Log(JsonConvert.SerializeObject(response), $"GENERATED_ORDER_RETURN:{DateTime.Now}");
                #endregion
            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }

    }
}
