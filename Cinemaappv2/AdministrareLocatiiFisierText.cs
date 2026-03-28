using System.IO;
using System.Linq;

namespace Rezervari
{
    public class AdministrareLocatiiFisierText
    {
        public string numeFisier = "Locatii.txt";

        public void AdaugaLocatie(Locatie l)
        {
            using (StreamWriter sw = File.AppendText(numeFisier))
            {
                sw.WriteLine(l.numeLocatie);
            }
        }

        public Locatie[] CitesteLocatii()
        {
            if (!File.Exists(numeFisier))
            {
                return new Locatie[0];
            }

            string[] linii = File.ReadAllLines(numeFisier);
            Locatie[] locatii = new Locatie[linii.Length];

            for (int i = 0; i < linii.Length; i++)
            {
                locatii[i] = new Locatie(linii[i]);
            }

            return locatii;
        }

        public Locatie CautaLocatieFisier(string nume)
        {
            Locatie[] toate = CitesteLocatii();
            return toate.Where(l => l.numeLocatie.Contains(nume)).FirstOrDefault();
        }

        public void ModificaLocatie(string numeVechi, string numeNou)
        {
            if (!File.Exists(numeFisier)) return;

            string[] linii = File.ReadAllLines(numeFisier);
            for (int i = 0; i < linii.Length; i++)
            {
                if (linii[i] == numeVechi)
                {
                    linii[i] = numeNou;
                }
            }

            File.WriteAllLines(numeFisier, linii);
        }
    }
}