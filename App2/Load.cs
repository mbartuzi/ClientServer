using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Net;

namespace App2
{
    class Load
    {

        //sprawdzanie poprawności pliku konfiguracyjnego
        public int validate( string [] tab) {
            //server
            if (tab.Length == 6) //bez karetki
            {
                if ((tab[1] == "server") 
                    && (Convert.ToInt32(tab[3]) >= 1 && Convert.ToInt32(tab[3]) <= 65535) 
                    && (tab[5].Length > 5 && tab[5].Length < 32))
                {
                    return 2;
                }
                else return 0;
            }


                // client bez karetki or server z karetką
            else if (tab.Length == 8) { 
                if (tab[1] == "server")
                {
                    if ((Convert.ToInt32(tab[4]) >= 1 && Convert.ToInt32(tab[4]) <= 65535) && (tab[7].Length >= 4 && tab[7].Length <= 32))
                    {
                        return 1;
                    }else return 0;
                }
                else if (tab[1] == "client"){
                    IPAddress address;
                    if((System.Net.IPAddress.TryParse(tab[3], out address)==true) &&
                        (Convert.ToInt32(tab[5]) >= 1 && Convert.ToInt32(tab[5]) <= 65535) &&
                        (tab[7].Length >= 4 && tab[7].Length <= 32)){
                            return 20;
                    }else                    
                    return 0;
                }
                else return 0;
            }



            // client
            else if (tab.Length == 11)
            {  // z karetką
                if ((tab[1] == "client")
                    && (Convert.ToInt32(tab[4]) >= 1 && Convert.ToInt32(tab[4]) <= 65535)
                    && (tab[7].Length > 5 && tab[7].Length < 32))
                {
                    return 10;
                }
                else return 0;
            }
            else
            {
                return 0;
            }
        
        }
        //wybieranie pliku konfiguracyjnego
        private string chooseFile() {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Config files (.ini)|*.ini|All Files (*.*)|*.*";
            openFile.Multiselect = false;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string strfilename = openFile.InitialDirectory + openFile.FileName;
                return strfilename;
            }
            else
            {
                return "error";
            }
        }
        //odczyt pliku konfiguracyjnego
        public string[] readConfig(string path)
        {

            char[] spliters = { '=', '\r', '\n' };
            string text = System.IO.File.ReadAllText(path);
            string[] tab = text.Split(spliters);
//server
            if (validate(tab) == 2)
            {
                string[] afterValid = new string[3];
                afterValid[0] = tab[1];
                afterValid[1] = tab[3];
                afterValid[2] = tab[5];
                return afterValid;
            }
            else if (validate(tab) == 1) {
                string[] afterValid = new string[3];
                afterValid[0] = tab[1];
                afterValid[1] = tab[4];
                afterValid[2] = tab[7];
                return afterValid;
            }
//client
            else if (validate(tab) == 10)
            {
                string[] afterValid = new string[4];
                afterValid[0] = tab[1];
                afterValid[1] = tab[4];
                afterValid[2] = tab[7];
                afterValid[3] = tab[10];
                return afterValid;
            }
            else if (validate(tab) == 20)
            {
                string[] afterValid = new string[4];
                afterValid[0] = tab[1];
                afterValid[1] = tab[3];
                afterValid[2] = tab[5];
                afterValid[3] = tab[7];
                return afterValid;
            }
                /* ---------------------------------------------*/
            else
            {
                DialogResult result = MessageBox.Show("Błędny plik konfiguracyjny\nWciśnij OK, aby wybrać poprawny plik koniguracyjny");
                if (result == DialogResult.OK)
                {
                    readConfig(chooseFile());
                    return tab;
                }
                tab[0] = "error";
                return tab;
            }

        }


    }
}
