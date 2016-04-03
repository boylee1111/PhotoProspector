using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using PhotoProspector.Data.Map;
using PhotoProspector.Domain.Helper;

namespace PhotoProspector.Data.Helper
{
    public class UnitOfWork : IUnitOfWork
    {
        public static readonly ISessionFactory _sessionFactory;
        private ITransaction _transaction;

        public ISession Session { get; private set; }

        static UnitOfWork()
        {
            _sessionFactory = Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString("Server=localhost;Database=photo_prospector;Uid=root;Pwd=admin;"))
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<PersonMap>();
                    m.FluentMappings.AddFromAssemblyOf<PhotoMap>();
                })
                //.ExposeConfiguration() // optional
                .BuildSessionFactory();
        }

        public UnitOfWork()
        {
            Session = _sessionFactory.OpenSession();
        }

        public void BeginTransaction()
        {
            _transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                Session.Close();
            }
        }

        public void Rollback()
        {
            if (_transaction.IsActive)
            {
                _transaction.Rollback();
            }
        }
    }
}
