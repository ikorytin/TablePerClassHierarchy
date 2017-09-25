using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ConfOrm;
using ConfOrm.NH;
using ConfOrm.Patterns;
using ConfOrm.Shop.CoolNaming;
using ConfOrm.Shop.DearDbaNaming;
using ConfOrm.Shop.Inflectors;
using ConfOrm.Shop.Packs;
using ConfOrm.Shop.Patterns;
using Dlls;
using Data.Core.Domain.Extensions;
using Data.Core.Domain.Model.Entities;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Exceptions;
using NHibernate.Tool.hbm2ddl;
using Environment = NHibernate.Cfg.Environment;


namespace TestProject
{
    public class DM
    {
        internal virtual ISessionFactory BuildFactory()
        {
            Configuration cfg = GetConfiguration();
            return cfg.BuildSessionFactory();
        }

        public Configuration GetConfiguration()
        {
            HbmMapping generateMappings = GenerateMappigs();

            var cfg = new Configuration();
            cfg.SessionFactory().Proxy.Through<NHibernate.Bytecode.DefaultProxyFactoryFactory>().Integrate.Using<MsSql2008Dialect>()
                .AutoQuoteKeywords().Connected.By<SqlClientDriver>().ByAppConfing("DBConnectionString").CreateCommands
                .ConvertingExceptionsThrough<SQLStateConverter>();
            cfg.SetProperty("show_sql", "true");
            cfg.SetProperty("format_sql", "true");
            cfg.AddDeserializedMapping(generateMappings, string.Empty);
            cfg.Properties.Add(Environment.LinqToHqlGeneratorsRegistry, typeof(LinqToHqlGeneratorsRegistry).AssemblyQualifiedName);

            // We need it to create database schema
            new SchemaUpdate(cfg).Execute(true, true);
            return cfg;
        }

        public HbmMapping GenerateMappigs()
        {
            var orm = new ObjectRelationalMapper();

            orm.Patterns.PoidStrategies.Add(new NativePoidPattern());
            
            //// map .NET4 ISet<T> as a NHibernate's set
            orm.Patterns.Sets.Add(new UseSetWhenGenericCollectionPattern());
            var englishInflector = new EnglishInflector();
            IPatternsAppliersHolder patternsAppliers = (new SafePropertyAccessorPack())
            .Merge(new ClassPluralizedTableApplier(englishInflector))
            .Merge(new DiscriminatorValueAsEnumValuePack<ChildA, Types>(orm))
            .Merge(new DiscriminatorValueAsEnumValuePack<ChildB, Types>(orm))
            .Merge(new CoolPatternsAppliersHolder(orm));

            IEnumerable<Type> allPersistEntities = GetDomainEntities();

            IEnumerable<Type> roots = allPersistEntities.Where(t => t.IsAbstract && t.InheritedFromBaseEntity());

            IEnumerable<Type> hierarchyEntities = allPersistEntities.Where(t => typeof(IHierarchyEntity).IsAssignableFrom(t));

            IEnumerable<Type> separateEntities = allPersistEntities.Except(roots).Except(hierarchyEntities);
            orm.TablePerConcreteClass(separateEntities);

            var hierarchyRoots = hierarchyEntities.Where(t => t.IsAbstract && t.InheritedFromBaseEntity());
            orm.TablePerClassHierarchy(hierarchyRoots);

            orm.Cascade<ChildA, Container>(CascadeOn.Persist | CascadeOn.Merge);
            orm.Cascade<Container, ChildA>(CascadeOn.Persist);

            orm.Cascade<ChildB, Container>(CascadeOn.Persist | CascadeOn.Merge);
            orm.Cascade<Container, ChildB>(CascadeOn.Persist);

            var mapper = new Mapper(orm, patternsAppliers);

            HbmMapping mapping = mapper.CompileMappingFor(allPersistEntities);

            File.WriteAllText(@"c:\Test\mappings.xml", Serialize(mapping)); // сохраняем маппинги в файл.
            return mapping;
        }

        /// <summary>
        /// Gets all objects that are inherited from EntityBase.
        /// </summary>
        private IEnumerable<Type> GetDomainEntities()
        {
            Assembly domainAssembly = typeof(Container).Assembly;
            IEnumerable<Type> domainEntities =
                domainAssembly.GetTypes().Where(
                    t => (typeof(BaseEntity).IsAssignableFrom(t) && !t.IsGenericType && t != typeof(BaseEntity)));
            return domainEntities;
        }

        protected static string Serialize(HbmMapping hbmElement)
        {
            var setting = new XmlWriterSettings { Indent = true };
            var serializer = new XmlSerializer(typeof(HbmMapping));
            using (var memStream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(memStream, setting))
                {
                    serializer.Serialize(xmlWriter, hbmElement);
                    memStream.Flush();
                    byte[] streamContents = memStream.ToArray();

                    string result = Encoding.UTF8.GetString(streamContents);
                    return result;
                }
            }
        }
    }
}