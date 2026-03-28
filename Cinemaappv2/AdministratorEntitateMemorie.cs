using System.Linq;

namespace Rezervari
{
    public class AdministratorEntitateMemorie
    {
        public Locatie[] locatii;
        public int nrLocatii;

        public AdministratorEntitateMemorie()
        {
            locatii = new Locatie[10];
            nrLocatii = 0;
        }

        public void AdaugaLocatie(Locatie l)
        {
            locatii[nrLocatii] = l;
            nrLocatii++;
        }

        public Locatie CautaLocatieDupaNume(string numeCautat)
        {
            Locatie locatieGasita = locatii.Where(l => l != null && l.numeLocatie.Contains(numeCautat)).FirstOrDefault();

            return locatieGasita;
        }
    }
}