using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WCIWT.Infrastructure.UnitOfWork
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        private readonly IList<Action> afterCommitActions = new List<Action>();



        public async Task Commit()
        {
            await CommitCore();
            foreach (var action in afterCommitActions)
            {
                action();
            }

            afterCommitActions.Clear();
        }

        public void RegisterAction(Action action)
        {
            afterCommitActions.Add(action);
        }

        protected abstract Task CommitCore();

        public abstract void Dispose();

    }
}