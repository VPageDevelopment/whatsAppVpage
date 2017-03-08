using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using WhatsAppApi;

namespace WebWhatsApp
{
    public class ChatHub : Hub
    {
        static WhatsApp wa;

        public void Send (string to , string message )
        {
            if(wa.ConnectionStatus == ApiBase.CONNECTION_STATUS.LOGGEDIN)
            {
                wa.SendMessage(to, message);
                Clients.All.nofityMessage(string.Format("{0} : {1}" , to , message));
            }
        }

        public void Login(string phoneNumber , string password)
        {
            Thread thread = new Thread(t => {
                wa = new WhatsApp(phoneNumber, password, phoneNumber, true);
                wa.OnConnectSuccess += () => 
                {
                    Clients.All.notifyMessage("connected....");

                    // when login success 
                    // notify the user logged in 
                    // successfully ....

                    wa.OnLoginSuccess += (phone, data) =>
                    {
                        Clients.All.notifyMessage("Login success ... ");
                    };

                    // when login fails 
                    // notify the user login 
                    // process is failed ...

                    wa.OnLoginFailed += (data) =>
                    {
                        Clients.All.notifyMessage(string.Format("login failed : {0} " , data));
                    };

                    wa.Login();
                };

                wa.OnConnectFailed += (ex) =>
                {
                    Clients.All.notifyMessage(string.Format("Connection Failed : { 0 }" , ex));
                };
                wa.Connect();
            })
            { IsBackground = true};
            thread.Start();

        }
    }
}