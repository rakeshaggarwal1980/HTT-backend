﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace HTTAPI.Helpers
{
    /// <summary>
    /// Application helper methods
    /// </summary>
    public static class AppHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static IWebHostEnvironment HostingEnvironment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// Service provider for geeting injected services
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Current date time
        /// </summary>
        public static DateTime CurrentDate => DateTime.Now;

        /// <summary>
        /// Get logged user claims
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static UserClaim GetUserClaimDetails(ClaimsIdentity identity)
        {
            UserClaim userClaim = null;
            if (identity != null)
            {
                if (identity.Claims.Any())
                {
                    IEnumerable<Claim> claims = identity.Claims;

                    userClaim = new UserClaim
                    {
                        Name = identity.FindFirst("Name").Value
                    };
                    var emailClaim = identity.FindFirst(ClaimTypes.Email) ?? identity.FindFirst(ClaimTypes.Upn);
                    userClaim.Email = emailClaim.Value;
                }
            }
            return userClaim;
        }


        /// <summary>
        /// Get the email of request user
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string GetActiveUserEmailId(this ClaimsIdentity identity)
        {
            var emailClaim = identity.FindFirst(ClaimTypes.Email) ?? identity.FindFirst(ClaimTypes.Upn);
            return emailClaim != null ? emailClaim.Value : string.Empty;
        }

        /// <summary>
        /// Get the Name of request user
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string GetActiveUserName(this ClaimsIdentity identity)
        {
            if (identity != null)
            {
                if (identity.Claims.Any())
                {
                    var name = identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name);
                    return name != null ? name.Value : string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the column value.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static object GetColumnValue(String columnName, object entity)
        {
            var pinfo = entity.GetType().GetProperty(columnName);
            if (pinfo == null) { return null; }
            return pinfo.GetValue(entity, null);
        }

        /// <summary>
        /// Sets the column value.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="value">The value.</param>
        public static void SetColumnValue(String columnName, object entity, object value)
        {
            var pinfo = entity.GetType().GetProperty(columnName);
            if (pinfo != null) { pinfo.SetValue(entity, value, null); }
        }

        /// <summary>
        /// Maps the audit columns.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="identity"></param>
        public static void MapAuditColumns(this object model, ClaimsIdentity identity)
        {
            if (identity != null)
            {
                var authorizedInfo = GetUserClaimDetails(identity);
                if (model != null && authorizedInfo != null)
                {
                    var emailId = authorizedInfo.Email;
                    var createdBy = Convert.ToString(GetColumnValue(Constants.CreatedBy, model));
                    if (string.IsNullOrEmpty(createdBy))
                    {
                        // SetColumnValue(Constants.IsActiveColumn, model, true);
                        SetColumnValue(Constants.CreatedDate, model, CurrentDate);
                        SetColumnValue(Constants.CreatedBy, model, emailId);
                    }
                    SetColumnValue(Constants.ModifiedDate, model, CurrentDate);
                    SetColumnValue(Constants.ModifiedBy, model, emailId);
                }
            }
        }

    }
}