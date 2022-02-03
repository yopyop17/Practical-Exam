using Exam_Project.Api.Constants;
using Exam_Project.Api.Data;
using Exam_Project.Api.Entities;
using Exam_Project.Api.Models;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Exam_Project.Api.Resources
{
    #region Grant JSON
    public class GrantDataAccessToken
    {
        public int Version { get; set; }
        public AccessToken AccessToken { get; set; }
    }
    public class AccessToken
    {
        public string ClientId { get; set; }
        public string Type { get; set; }
        public List<GrantClaims> Claims { get; set; }
    }

    public class GrantData
    {
        public string ClientId { get; set; }
        public GrantSubject Subject { get; set; }
    }

    public class GrantSubject
    {
        public List<GrantClaims> Claims { get; set; }
    }

    public class GrantClaims
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
    }
    #endregion
    public class SessionGrantStore : IPersistedGrantStore
    {
        private readonly ClientDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionGrantStore(ClientDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        private readonly Expression<Func<Sessions, PersistedGrant>> MapFunc = s =>
            new PersistedGrant
            {
                ClientId = s.ClientId,
                CreationTime = s.DateCreated,
                Data = s.GrantData,
                Expiration = s.Expiration,
                Key = s.Token,
                SubjectId = s.UserId.ToString(),
                Type = s.GrantType
            };

        PersistedGrant Map(Sessions session)
        {
            return MapFunc.Compile().Invoke(session);
        }

        private IQueryable<Sessions> Query()
        {
            return _dbContext.Sessions.Where(s => !s.DateDeleted.HasValue);
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            long userId;
            long.TryParse(subjectId, out userId);
            var grants = await Query().AsNoTracking().Where(s => s.UserId == userId && !s.LoggedOutDate.HasValue && !s.DateDeleted.HasValue).Select(MapFunc).ToListAsync();
            return grants;
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            var grant = await Query().AsNoTracking().Where(s => s.Token == key && !s.LoggedOutDate.HasValue).Select(MapFunc).FirstOrDefaultAsync();
            return grant;
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            long userId;
            long.TryParse(subjectId, out userId);
            var sessions = await Query().Where(s => s.UserId == userId && s.ClientId == clientId).ToListAsync();
            _dbContext.Sessions.RemoveRange(sessions);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            long userId;
            long.TryParse(subjectId, out userId);
            var sessions = await Query().Where(s => s.UserId == userId && s.ClientId == clientId && s.GrantType == type).ToListAsync();
            _dbContext.Sessions.RemoveRange(sessions);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(string key)
        {
            var session = await Query().FirstOrDefaultAsync(s => s.Token == key);
            if (session.ClientId == Keys.hybridClient)
                session.DateDeleted = DateTime.UtcNow;
            else
                session.LoggedOutDate = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }

        public async Task StoreAsync(PersistedGrant grant)
        {
            //var grandData = Newtonsoft.Json.Linq.JObject.Parse(grant.Data);
            bool isAdd = false;
            var email = string.Empty;
            var grandData = JsonConvert.DeserializeObject<GrantData>(grant.Data);
            if (grandData.Subject == null)
            {
                var grandDataAccess = JsonConvert.DeserializeObject<GrantDataAccessToken>(grant.Data);
                email = grandDataAccess.AccessToken.Claims.FirstOrDefault(s => s.Type == "username")?.Value;
            }
            else
                email = grandData.Subject.Claims.FirstOrDefault(s => s.Type == "name")?.Value;

            var session = await Query().FirstOrDefaultAsync(s => s.Token == grant.Key && !s.LoggedOutDate.HasValue);
            if (session == null)
            {
                long userId;
                long.TryParse(grant.SubjectId, out userId);


                var user = await _dbContext.Users.FirstOrDefaultAsync(e => e.Email == email);
                if (user != null)
                {
                    session = new Sessions
                    {
                        UserId = user.Id,
                        GrantType = grant.Type,
                        Token = grant.Key,
                        ClientId = grant.ClientId,
                        IsPushNotificationEnabled = true,
                        DateCreated = DateTime.UtcNow
                    };
                    _dbContext.Sessions.Add(session);
                    isAdd = true;

                }
            }
            session.GrantData = grant.Data;
            session.Expiration = grant.Expiration;
            session.RequestIdentifier = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            var request = _httpContextAccessor.HttpContext.Request;
            if (request.Method == "POST")
            {
                var form = _httpContextAccessor.HttpContext.Request.Form;
                Func<string, string> get = f => form[f].FirstOrDefault();

                var fUserAgent = get(LoginData.UserAgent);
                if (!string.IsNullOrEmpty(fUserAgent))
                    session.UserAgent = fUserAgent;

                var fDeviceToken = get(LoginData.DeviceToken);
                if (!string.IsNullOrEmpty(fDeviceToken))
                    session.DeviceToken = fDeviceToken;

                var fVersion = get(LoginData.Version);
                if (!string.IsNullOrEmpty(fVersion))
                    session.Version = fVersion;

                var fOSVersion = get(LoginData.OSVersion);
                if (!string.IsNullOrEmpty(fOSVersion))
                    session.OSVersion = fOSVersion;

                var fModel = get(LoginData.Model);
                if (!string.IsNullOrEmpty(fModel))
                    session.Model = fModel;

                var fBuild = get(LoginData.Build);
                if (!string.IsNullOrEmpty(fBuild))
                    session.Build = fBuild;

                var fManufacturer = get(LoginData.Manufacturer);
                if (!string.IsNullOrEmpty(fManufacturer))
                    session.Manufacturer = fManufacturer;
            }

            try
            {
                if (isAdd) { var chk = await _dbContext.SaveChangesAsync(); }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
