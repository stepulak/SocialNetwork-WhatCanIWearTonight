using System;
using System.Threading.Tasks;

namespace WCIWT.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        Task Commit();

        void RegisterAction(Action action);
    }
}