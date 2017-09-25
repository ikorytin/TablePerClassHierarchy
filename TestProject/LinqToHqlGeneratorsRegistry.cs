using NHibernate.Linq.Functions;

namespace TestProject
{
    public class LinqToHqlGeneratorsRegistry : DefaultLinqToHqlGeneratorsRegistry
    {
        public LinqToHqlGeneratorsRegistry()
        {
            this.Merge(new ConcatHqlGenerator());
        }
    }
}