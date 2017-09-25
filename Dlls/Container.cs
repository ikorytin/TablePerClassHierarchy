using System.Collections.Generic;
using Data.Core.Domain.Model.Entities;

namespace Dlls
{
    public class Container : BaseEntity
    {
        public Container()
        {
            CollectionOne = new HashSet<ChildA>();
            CollectionTwo = new HashSet<ChildB>();
        }

        public virtual ICollection<ChildA> CollectionOne { get; set; }

        public virtual ICollection<ChildB> CollectionTwo { get; set; }
    

    public virtual void AddService(ChildA item)
        {
            if (!CollectionOne.Contains(item))
            {
                CollectionOne.Add(item);
            }
        }

        public virtual void RemoveService(ChildA item)
        {
            if (CollectionOne.Contains(item))
            {
                CollectionOne.Remove(item);
            }
        }

        public virtual void AddMiscellaneous(ChildB item)
        {
            if (!CollectionTwo.Contains(item))
            {
                CollectionTwo.Add(item);
            }
        }

        public virtual void RemoveMiscellaneous(ChildB item)
        {
            if (CollectionTwo.Contains(item))
            {
                CollectionTwo.Remove(item);
            }
        }
    }
}