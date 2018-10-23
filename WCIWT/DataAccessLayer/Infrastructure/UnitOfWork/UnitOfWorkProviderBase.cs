using System;
using System.Threading;

namespace WCIWT.Infrastructure.UnitOfWork
{
    public abstract class UnitOfWorkProviderBase: IUnitOfWorkProvider
    {
        protected readonly AsyncLocal<IUnitOfWork> UnitOfWorkLocalInstance = new AsyncLocal<IUnitOfWork>();

        public abstract IUnitOfWork Create();

        public IUnitOfWork GetUnitOfWorkInstance()
        {
            return UnitOfWorkLocalInstance != null ? UnitOfWorkLocalInstance.Value : throw new InvalidOperationException("UnitOfWork was not created");
        }

        public void Dispose()
        {
            UnitOfWorkLocalInstance.Value?.Dispose();
            UnitOfWorkLocalInstance.Value = null;
        }
    }
}