using System.Threading.Tasks;
using WCIWT.Infrastructure.UnitOfWork;

namespace BusinessLayerTesting.FacadesTests.Common
{
    internal class StubUow : UnitOfWorkBase
    {
        protected override Task CommitCore()
        {
            return Task.Delay(15);
        }

        public override void Dispose()
        {
        }
    }
}