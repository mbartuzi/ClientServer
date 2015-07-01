using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace App2
{
    class Odczyt
    {
        // odczyt z pliku i dodawanie rekordów
        public Odczyt(Form1 form ,string fileName)
        {

            string line;
            string[] dane = new string[6];
            char[] spliters = { ';', '\n'};
            StreamReader file = new StreamReader("input.txt");
            int i = 0;
            while ((line = file.ReadLine()) != null)
            {
                if (i == 0) {
                    dane = line.Split(spliters);
                    for (int k = 0; k < dane.Length; k++)
                    {
                        form.addCols(dane[k], dane[k]);
                    }
                }
                else if (i != 0)
                {
                    dane = line.Split(spliters);
                    form.addRecordToGrid(dane);
                    //form.addRecordToGrid(dane);
                } i++;
            }

            file.Close();

        }
    }
}
