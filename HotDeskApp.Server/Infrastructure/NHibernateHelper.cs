using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using HotDeskApp.Server.Models.Desk.Entities;
using HotDeskApp.Server.Models.Tokens.BlacklistToken.Entities;
using HotDeskApp.Server.Models.Tokens.RefreshToken.Entities;
using HotDeskApp.Server.Models.UserEntity.Entities;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using ISession = NHibernate.ISession;

namespace HotDeskApp.Server.Infrastructure;

public class NHibernateHelper
{
    private static ISessionFactory _sessionFactory;

    public static ISessionFactory SessionFactory
    {
        get
        {
            if (_sessionFactory == null)
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                _sessionFactory = Fluently.Configure()
                    .Database(
                        MsSqlConfiguration.MsSql2012.ConnectionString(connectionString)
                    )
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<UserEntity>())
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<Desk>())
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<RefreshToken>())
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<BlacklistToken>())
                    /*
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<Models.Mailing.MailEntity.MailEntity>())
                    */
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                    .BuildSessionFactory();
            }

            return _sessionFactory;
        }
    }

    public static ISession OpenSession()
    {
        return SessionFactory.OpenSession();
    }
}