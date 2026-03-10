using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp
{
    public class Loc
    {
        public char Rand { get; set; }
        public int Numar { get; set; }
        public decimal Pret { get; set; }
        public bool EsteSelectat { get; set; }

        public Loc(char rand, int numar, decimal pret)
        {
            Rand = rand;
            Numar = numar;
            Pret = pret;
            EsteSelectat = false;
        }

        public void SchimbaStarea()
        {
            EsteSelectat = !EsteSelectat;
        }
    }

    public class Sala
    {
        public string Nume { get; set; }
        public List<Loc> Locuri { get; set; }

        public Sala(string nume)
        {
            Nume = nume;
            Locuri = new List<Loc>();
            GenereazaLocuri();
        }

        private void GenereazaLocuri()
        {
            for (char r = 'A'; r <= 'I'; r++)
            {
                decimal pretRand = (r >= 'A' && r <= 'D') ? 25.0m : 35.0m;

                for (int n = 1; n <= 20; n++)
                {
                    Locuri.Add(new Loc(r, n, pretRand));
                }
            }
        }

        public void ResetareLocuri()
        {
            foreach (var loc in Locuri)
            {
                loc.EsteSelectat = false;
            }
        }

        public override string ToString()
        {
            return Nume;
        }
    }

    public class SistemRezervare
    {
        public List<Sala> Sali { get; set; }

        public SistemRezervare()
        {
            Sali = new List<Sala>();
        }

        public void AdaugaSala(string numeSala)
        {
            Sali.Add(new Sala(numeSala));
        }
    }
}
