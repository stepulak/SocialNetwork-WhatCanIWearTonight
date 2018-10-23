namespace WCIWT.Infrastructure.UnitOfWork
{
    public interface IUnitOfWorkProvider
    {
        IUnitOfWork Create();

        IUnitOfWork GetUnitOfWorkInstance();
    }
}