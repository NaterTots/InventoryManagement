using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using NHibernate;
using NHibernate.Cfg;

namespace InventoryManagement.Data.Web
{
    public static class HibernateProvider
    {
        static ISessionFactory _factory;
        public static ISessionFactory Factory
        {
            get
            {
                return _factory;
            }
        }

        static HibernateProvider()
        {
            try
            {
                //log4net.Config.XmlConfigurator.Configure();
                Configuration configuration = new Configuration();
                //pull information from hibernate.cfg.xml file
                configuration.Configure();
                //pull information from imbedded *.hbm.xml files
                configuration.AddAssembly(System.Reflection.Assembly.GetExecutingAssembly());

                _factory = configuration.BuildSessionFactory();
            }
            catch (Exception e)
            {
                string s = e.Message;
                string a = e.StackTrace;
            }
        }
    }
}
