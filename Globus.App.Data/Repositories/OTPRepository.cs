using Globus.App.Data.Contexts;
using Globus.App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globus.App.Data.Repositories
{
    public class OTPRepository : Repository<OTP>
    {
        public OTPRepository(GlobusContext dbContext)
            : base(dbContext)
        {
        }

        public GlobusContext GlobusContext { get { return dbContext as GlobusContext; } }

        public bool IsMobileNumberValidated(string emailAddress,string mobileNumber)
        {
            return GlobusContext.OTPs
                .Any(r => (r.RecipientEmailAddress.Equals(emailAddress)
                          || r.RecipientMobileNumber.Equals(mobileNumber))
                          && r.IsValidatedSuccessfully == true);
        }
    }
}
