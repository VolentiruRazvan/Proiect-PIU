using System;

namespace Rezervari
{
    public class Spectacol
    {
        public string numeSpectacol;
        public Loc[,] matriceLocuri;
        public int nrRanduri;
        public int nrColoane;

        public Spectacol(string nume, int r, int c, int pretBaza)
        {
            numeSpectacol = nume;
            nrRanduri = r;
            nrColoane = c;
            matriceLocuri = new Loc[r, c];

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    matriceLocuri[i, j] = new Loc(i, j, pretBaza + i * 5);
                }
            }
        }

        public void AfisareLocuri()
        {
            Console.WriteLine();
            for (int i = 0; i < nrRanduri; i++)
            {
                Console.Write("Linia " + (i + 1) + " (Pret " + matriceLocuri[i, 0].pret + "): ");
                for (int j = 0; j < nrColoane; j++)
                {
                    if (matriceLocuri[i, j].ocupat == true)
                    {
                        Console.Write("[X] ");
                    }
                    else
                    {
                        Console.Write("[" + (j + 1) + "] ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void GolesteLocuri()
        {
            for (int i = 0; i < nrRanduri; i++)
            {
                for (int j = 0; j < nrColoane; j++)
                {
                    matriceLocuri[i, j].ocupat = false;
                }
            }
        }
    }
}