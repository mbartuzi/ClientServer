using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Drawing;
using System.Windows.Forms;

namespace App2
{
    class Server
    {
        //pozyskiwanie listy dostepnych sieci
        public string localIPAddress()
        {
            ChooseNetwork okno = new ChooseNetwork();

            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                localIP = ip.ToString();

                String[] temp = localIP.Split('.');
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    okno.addItem(ip.ToString());
                }
                else localIP = null;
            }

            okno.ShowDialog();
            localIP = okno.selected();
            return localIP;
        }



    }
}
