using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebWhatsApp
{
    public class Status
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}