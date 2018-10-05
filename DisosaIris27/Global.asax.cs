using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Threading;
using System.Net;

namespace DisosaIris27
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //keepAliveThread.Start();
        }


        static Thread keepAliveThread = new Thread(KeepAlive);

        protected void Application_End()
        {
            keepAliveThread.Abort();
        }

        static void KeepAlive()
        {
            while (true)
            {
                WebRequest req = WebRequest.Create("http://admin.disosagt.com/api/data/obtenerproducto/123");
                req.GetResponse();
                try
                {
                    Thread.Sleep(49000);
                }
                catch (ThreadAbortException)
                {
                    break;
                }
            }
        }
    }
}