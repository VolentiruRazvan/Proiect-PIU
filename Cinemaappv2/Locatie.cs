namespace Rezervari
{
    public class Locatie
    {
        public string numeLocatie;
        public Spectacol[] spectacole;
        public int nrSpectacole;

        public Locatie(string nume)
        {
            numeLocatie = nume;
            spectacole = new Spectacol[10];
            nrSpectacole = 0;
        }

        public void AdaugaSpectacol(Spectacol s)
        {
            spectacole[nrSpectacole] = s;
            nrSpectacole++;
        }
    }
}