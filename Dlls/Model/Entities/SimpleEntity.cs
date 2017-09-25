using Data.Core.Domain.Model.Entities;

using Sam.Core.Domain.Model.Interfaces;

namespace Sam.Core.Domain.Model.Entities
{
    /// <summary>
    /// Represents simple entity. Entity with Id and Name properties.
    /// </summary>
    public abstract class SimpleEntity : BaseEntity, ISimpleEntity
    {
        #region Public Properties

        public virtual string Name { get; set; }

        #endregion
    }
}