namespace CinemaAppwpf
{
    public class AdministratorEntitateMemorie
    {
        public Locatie[] locatii;
        public int nrLocatii;

        public AdministratorEntitateMemorie()
        {
            locatii = new Locatie[100];
            nrLocatii = 0;
        }

        public void AdaugaLocatie(Locatie loc)
        {
            if (nrLocatii < 100)
            {
                locatii[nrLocatii] = loc;
                nrLocatii++;
            }
        }

        public Locatie? CautaLocatieDupaNume(string? nume)
        {
            for (int i = 0; i < nrLocatii; i++)
            {
                if (locatii[i].numeLocatie == nume)
                {
                    return locatii[i];
                }
            }
            return null; // Acum nu va mai da eroare ca returnam null
        }
    }
}