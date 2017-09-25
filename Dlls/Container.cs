using System.Collections.Generic;
using Data.Core.Domain.Model.Entities;

namespace Dlls
{
    public class Container : BaseEntity
    {
        public Container()
        {
            ALaCarteChildA = new HashSet<ChildA>();
            Miscellaneous = new HashSet<ChildB>();
        }

        public virtual ICollection<ChildA> ALaCarteChildA { get; set; }

        public virtual ICollection<ChildB> Miscellaneous { get; set; }

        public virtual void AddService(ChildA item)
        {
            if (!ALaCarteChildA.Contains(item))
            {
                ALaCarteChildA.Add(item);
            }
        }

        public virtual void RemoveService(ChildA item)
        {
            if (ALaCarteChildA.Contains(item))
            {
                ALaCarteChildA.Remove(item);
            }
        }

        public virtual void AddMiscellaneous(ChildB item)
        {
            if (!Miscellaneous.Contains(item))
            {
                Miscellaneous.Add(item);
            }
        }

        public virtual void RemoveMiscellaneous(ChildB item)
        {
            if (Miscellaneous.Contains(item))
            {
                Miscellaneous.Remove(item);
            }
        }
    }
}