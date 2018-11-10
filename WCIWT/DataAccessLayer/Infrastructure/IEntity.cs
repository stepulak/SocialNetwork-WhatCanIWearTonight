using System;

namespace WCIWT.Infrastructure
{
    public interface IEntity
    {
        Guid Id { get; set; }
        
        string TableName { get; }
    }
}
