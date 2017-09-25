using Data.Core.Domain.Model.Entities;

namespace Sam.Core.Domain.Model.Interfaces
{
    public interface ISimpleEntity : IEntity
    {
        string Name { get; set; }
    }
}