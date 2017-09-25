using System.Collections.Generic;
using System.Text;
using Data.Core.Domain.Model.Entities;

namespace Data.Core.Domain.Extensions
{
    public static class CollectionExtensions
    {
        public static string ShowAll<T>(this ICollection<T> value) where T : NameEntity
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in value)
            {
                stringBuilder.Append(item.Name + ",");
            }

            string result = stringBuilder.ToString();
            result = result.Remove(result.LastIndexOf(','));

            return result;
        }
    }
}