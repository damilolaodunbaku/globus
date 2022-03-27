using Globus.App.Data.Contexts;
using Globus.App.Data.Repositories;
using Globus.App.Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Globus.App.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ILogger<UnitOfWork> _logger;
        private readonly GlobusContext _globusContext;

        public UnitOfWork(ILogger<UnitOfWork> logger,
            GlobusContext globusContext)
        {
            _logger = logger;
            _globusContext = globusContext;
            OTPS = new OTPRepository(globusContext);
        }

        public OTPRepository OTPS { get; set; }
        public void Complete()
        {
            try
            {
                _globusContext.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while saving changes to the database");
                throw;
            }
        }
    }
}
