using Exam_Project.Api.Data;
using Exam_Project.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Services
{
    public class BaseService<T>
    {
        protected ClaimsPrincipalUser CurrentUser { get; private set; }

        public ILogger Logger;
        public ClientDbContext ClientDbContext;
        public IHttpContextAccessor HttpContextAccessor;


        public BaseService(ILogger<T> logger,
            IHttpContextAccessor httpContextAccessor,
            ClientDbContext clientDbContext)
        {
            Logger = logger;
            ClientDbContext = clientDbContext;
            HttpContextAccessor = httpContextAccessor;
            if (httpContextAccessor?.HttpContext?.User?.Identity != null)
            {
                if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    CurrentUser = new ClaimsPrincipalUser(httpContextAccessor.HttpContext.User);
                }

                if (long.TryParse(httpContextAccessor.HttpContext.Request.Headers["sub"], out long newId))
                {
                    CurrentUser = new ClaimsPrincipalUser(new ClaimsUser() { Id = newId });
                }
            }
        }


        public long CurrentUserStore
        {
            get
            {
                return CurrentUser?.User?.StoreId ?? 0;
            }
        }
        protected void HandleException(Exception ex)
        {
            LogError(ex);
        }


        //public List<long> GetStoresOnSelectedLocation(long barangayId)
        //{
        //    return ClientDbContext.Store.SelectMany(sm => sm.Territories
        //        .Where(w => w.Store.IsOnlineSelling && w.Store.IsActive && w.Store.Status == Core.Constants.StoreStatus.Validated)
        //        .Where(w => !w.Store.DateDeleted.HasValue)
        //        .Where(w => w.BarangayId == barangayId))
        //        .Select(s => s.StoreId).ToList();
        //}

        //public List<StoreTerritoriesDTO> GetDeliveryFee(long barangayId, long storeId)
        //{
        //    var terdel = ClientDbContext.Store.Where(w => storeId == w.Id)
        //        .SelectMany(s => s.Territories
        //            .Where(w => w.BarangayId == barangayId))
        //        .Select(s => new StoreTerritoriesDTO()
        //        {
        //            StoreId = s.StoreId,
        //            BarangayId = s.BarangayId,
        //            DeliverylLeadTime_Hours = s.DeliverylLeadTime_Hours,
        //            Id = s.Id,
        //            Truck_DeliveryFee = s.Truck_DeliveryFee,
        //            Motor_DeliveryFee = s.Motor_DeliveryFee,
        //        }).ToList();
        //    return terdel;
        //}


        //public List<StoreTerritoriesDTO> GetDeliveryFee(long barangayId, List<long> storeIds)
        //{
        //    var terdel = ClientDbContext.Store.Where(w => storeIds.Contains(w.Id))
        //        .SelectMany(s => s.Territories
        //            .Where(w => w.BarangayId == barangayId))
        //        .Select(s => new StoreTerritoriesDTO()
        //        {
        //            StoreId = s.StoreId,
        //            BarangayId = s.BarangayId,
        //            DeliverylLeadTime_Hours = s.DeliverylLeadTime_Hours,
        //            Id = s.Id,
        //            Truck_DeliveryFee = s.Truck_DeliveryFee,
        //            Motor_DeliveryFee = s.Motor_DeliveryFee,
        //        }).ToList();
        //    return terdel;
        //}

        protected void HandleException(Response response, Exception ex)
        {
            LogError(ex);
            if (response != null)
            {
                response.ErrorMessage = ex.Message;
            }
        }

        protected void LogError(Exception ex)
        {
            Logger.LogError(string.Format("Error! {0} {1}", ex.InnerException, ex.StackTrace));
        }

        protected void Log(string message, string location)
        {
            Logger.LogInformation(string.Format("Warning! {0} {1}", location, message));
        }
    }
}
