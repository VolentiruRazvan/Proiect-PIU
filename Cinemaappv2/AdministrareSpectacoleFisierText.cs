using System.IO;
using System.Linq;

namespace Rezervari
{
    public class AdministrareSpectacoleFisierText
    {
        public string numeFisier = "Spectacole.txt";

        public void AdaugaSpectacol(Spectacol s)
        {
            using (StreamWriter sw = File.AppendText(numeFisier))
            {
                sw.WriteLine(s.numeSpectacol + "," + s.nrRanduri + "," + s.nrColoane + "," + s.matriceLocuri[0, 0].pret);
            }
        }

        public Spectacol[] CitesteSpectacole()
        {
            if (!File.Exists(numeFisier))
            {
                return new Spectacol[0];
            }

            string[] linii = File.ReadAllLines(numeFisier);
            Spectacol[] spectacole = new Spectacol[linii.Length];

            for (int i = 0; i < linii.Length; i++)
            {
                string[] date = linii[i].Split(',');
                spectacole[i] = new Spectacol(date[0], int.Parse(date[1]), int.Parse(date[2]), int.Parse(date[3]));
            }

            return spectacole;
        }

        public Spectacol CautaSpectacolFisier(string nume)
        {
            Spectacol[] toate = CitesteSpectacole();
            return toate.Where(s => s.numeSpectacol.Contains(nume)).FirstOrDefault();
        }

        public void ModificaSpectacol(string numeVechi, string numeNou)
        {
            if (!File.Exists(numeFisier)) return;

            string[] linii = File.ReadAllLines(numeFisier);
            for (int i = 0; i < linii.Length; i++)
            {
                string[] date = linii[i].Split(',');
                if (date[0] == numeVechi)
                {
                    linii[i] = numeNou + "," + date[1] + "," + date[2] + "," + date[3];
                }
            }

            File.WriteAllLines(numeFisier, linii);
        }
    }
}