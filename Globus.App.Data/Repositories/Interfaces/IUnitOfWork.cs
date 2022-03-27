namespace Globus.App.Data.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        CustomerRepository Customers { get; }
        OTPRepository OTPS { get; }
        void Complete();
    }
}
