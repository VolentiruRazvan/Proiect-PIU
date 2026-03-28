using System;

namespace Rezervari
{
    public static class Jurnal
    {
        public static string[] inregistrari = new string[100];
        public static int indexJurnal = 0;

        public static void AdaugaInregistrare(string actiune)
        {
            if (indexJurnal < 100)
            {
                inregistrari[indexJurnal] = actiune;
                indexJurnal++;
            }
        }

        public static void ArataInregistrari()
        {
            Console.WriteLine("--- JURNAL ---");
            for (int i = 0; i < indexJurnal; i++)
            {
                Console.WriteLine(inregistrari[i]);
            }
        }
    }
}