using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dlls;
using NHibernate;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            ISessionFactory sessionFactory = new DM().BuildFactory();
            using (ISession session = sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                     //SaveServices(session, transaction);
                    LoadServices(session, transaction);
                }
            }
        }

        private static void LoadServices(ISession session, ITransaction transaction)
        {
        
          var s =  session.Load<Container>(1); //<Container>()
          
        }

        private static void SaveServices(ISession session, ITransaction transaction)
        {
            Container c = new Container();
            ChildA post = new ChildA
            {
                Name = "test"
            };
            c.AddService(post);

            ChildB b = new ChildB();
            b.Value = "10";

            c.AddMiscellaneous(b);

            session.SaveOrUpdate(c);
            transaction.Commit();
        }
    }
}
