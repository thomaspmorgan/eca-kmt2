﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace ECA.WebApi.Security
{
    /// <summary>
    /// The debug web api user is a simple user for debugging through Swagger.  This user is equivalent
    /// to the ECA Test user.
    /// </summary>
    public class DebugWebApiUser : WebApiUser
    {
        /// <summary>
        /// The eca test user id.
        /// </summary>
        public static readonly Guid DEBUG_USER_ID = Guid.Parse("3ffd6d01-cae3-4157-86fa-1e4e5618086a");

        /// <summary>
        /// Creates the debug web api user.
        /// </summary>
        public DebugWebApiUser()
            : base(new List<Claim>
            {
                new Claim(WebApiUser.EMAIL_KEY, "ECATest1@statedept.us"),
                new Claim(WebApiUser.EXPIRATION_DATE_KEY, ((int)((DateTime.UtcNow - EPOCH).TotalSeconds) + (60 * 10)).ToString()), //expires in 10 mins
                new Claim(WebApiUser.GIVEN_NAME_KEY, "Debug"),
                new Claim(WebApiUser.ISSUED_AT_TIME_KEY, ((int)((DateTime.UtcNow - EPOCH).TotalSeconds) - 60).ToString()), //issued one minute ago
                new Claim(WebApiUser.FULL_NAME_KEY, "Debug"),
                new Claim(WebApiUser.SURNAME_KEY, "User"),
                new Claim(WebApiUser.USER_ID_KEY, DEBUG_USER_ID.ToString()),
                new Claim(WebApiUser.VALID_NOT_BEFORE_DATE_KEY,  ((int)((DateTime.UtcNow - EPOCH).TotalSeconds) - 60).ToString()), //valid from one minute ago
            })
        {

        }

    }
}