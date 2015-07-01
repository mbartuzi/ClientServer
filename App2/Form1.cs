using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace App2
{
    
    public partial class Form1 : Form
    {

        Load config = new Load();
        TcpClient clnt = new TcpClient();
        public Form1()
        {
            InitializeComponent();
            //bool isConnected = false;
            
            
            string[] data = config.readConfig("config.ini");
            if (data.Length == 4) { //client 
                this.Show();
                Odczyt czytam = new Odczyt(this, data[3]);
                
            }
            else if (data.Length == 3) //server
            {
                btnConnect.Visible = false;
                btnSend.Visible = false;
                this.Show();
                int port = Convert.ToInt32(data[1]);
                Server srv = new Server();
                IPAddress ipA = IPAddress.Parse(srv.localIPAddress());
                TcpListener listener = new TcpListener(ipA, port);
                richTextBox1.Text += ("Nasłuchiwanie...\n");
                listener.Start();

                TcpClient server = listener.AcceptTcpClient();
                richTextBox1.Text += "Klient o adresie: "+ server.Client.RemoteEndPoint + " połączył się.\n";

                    NetworkStream nwStream = server.GetStream();
                    byte[] buffer = new byte[server.ReceiveBufferSize];


                    int bytesRead = nwStream.Read(buffer, 0, server.ReceiveBufferSize);

                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    richTextBox1.Text += "Odebrano dane.\n";

                    toTable(dataReceived); //convert received data to transpose Table
                    saveTable(data[2]);    // save to archive.dat

                 server.Close();
                 listener.Stop();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        public void addLog(string dataLog)
        {
            richTextBox1.Text += dataLog;
        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        
        public void addRecordToGrid(string [] dane) {
            this.grid.Rows.Add(dane);
        }

        public void addCols(string name, string header) {
            this.grid.Columns.Add(name, header);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {          
            
            string[] data = config.readConfig("config.ini");
            int port = Convert.ToInt32(data[2]);
            try
            {
                clnt.Connect(data[1], port);
                if (clnt.Connected == true)
                {
                    MessageBox.Show("Połączono");
                    richTextBox1.Text += "Nawiązano połączenie z serwerem: " + clnt.Client.RemoteEndPoint+"\n";
                    chgStateConn(false);
                }
            }
            catch {
                MessageBox.Show("Połaczenie nie powiodło się.\nUpewnij się, czy serwer jest uruchomiony.");
            }

        }

        private void btnSend_Click(object sender, EventArgs e)
        {

            string textToSend = dataToSend();
            try
            {
                NetworkStream nwStream = clnt.GetStream();
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                richTextBox1.Text += ("Wysłano dane.\n");
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);

            }
            catch {
                MessageBox.Show("Wysyłanie dnaych nie powiodło się.\nUpewnij się, czy jesteś połączony z serwerem.");
            }

        }
        public void toTable(string odebrane)
        {
            char[] spliters = { ';', '\n', '\r' };
            int rows = 0;
            int cols = 0;

            for (int i = 0; i < odebrane.Length; i++)
            {
                if (odebrane[i] == '\r') rows++;
                if (odebrane[i] == ';') cols++;
            }
            string[] cells = odebrane.Split(spliters);
            cols /= rows;
            int curr = cols;
            for (int i = 0; i < rows; i++)
            {
                addCols(i.ToString(), i.ToString());
                if (i == 0)
                {
                    for (int j = 0; j < cols - 1; j++)
                    {

                        grid.Rows.Add();
                        grid.Rows[j].HeaderCell.Value = cells[j];
                    }
                }

                for (int j = 0; j < cols - 1; j++)
                {
                    if (cells[curr] == "" && (curr % (cols)) == 4)
                    {
                        curr++;
                        j--;
                    }
                    else
                    {
                        grid.Rows[j].Cells[i].Value = cells[curr];
                        curr++;
                    }
                }

            }

            grid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;


        }

        public void saveTable(string fileName)
        {

            string wiersz = "";
            for (int i = 0; i < grid.Rows.Count - 1; i++)
            {
                wiersz += grid.Rows[i].HeaderCell.Value.ToString() + ";";
                for (int j = 0; j < grid.Columns.Count; j++)
                {
                    if (j != grid.Columns.Count - 1)
                        wiersz += grid.Rows[i].Cells[j].Value.ToString() + ";";
                    else
                        wiersz += grid.Rows[i].Cells[j].Value.ToString() + "\n";
                }

            }
            StreamWriter file = new StreamWriter(fileName);
            file.WriteLine(wiersz);

            file.Close();

        }



        public void chgStateConn(bool state) {
            btnConnect.Enabled = state;
        }

        public string dataToSend(){
        string temp = "";

            int ile = grid.ColumnCount;
            for (int i = 0 ; i < ile ; i++){
                string columnName = grid.Columns[i].Name;
                temp += columnName+";";
            }
            temp += "\n";
        
            for (int rows = 0; rows < grid.Rows.Count-1; rows++)
            {
                
                for (int col = 0; col < grid.Rows[rows].Cells.Count; col++)
                {
                    string value = grid.Rows[rows].Cells[col].Value.ToString();
                    temp += value+";";
                    if (col == grid.Rows[rows].Cells.Count) {
                        temp += "\n";
                    }
                    //MessageBox.Show(value);
                    
                }
                temp += "\r";
            }
            return temp;
        }
       


    }
}
