using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Review.Code.Configuration
{
    /// <summary>
    /// Represents the configuration for the JWT scheme.
    /// </summary>
    /// <see cref="https://weblog.west-wind.com/posts/2017/dec/12/easy-configuration-binding-in-aspnet-core-revisited"/>
    /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0"/>
    public class JWTSettings
    {
        public string ValidIssuer { get; set; }

        public string ValidAudience { get; set; }

        public int ExpiryInMinutes { get; set; }

        public int AuthenticationWindowInMinutes { get; set; }

        public string SecretKey { get; set; }
    }
}
