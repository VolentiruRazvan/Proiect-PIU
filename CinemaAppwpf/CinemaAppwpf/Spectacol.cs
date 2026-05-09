namespace CinemaAppwpf
{
    public class Spectacol
    {
        public string numeSpectacol;
        public int nrRanduri;
        public int nrColoane;
        public Loc[,] matriceLocuri;

        public Spectacol(string nume, int randuri, int coloane, float pretBaza)
        {
            numeSpectacol = nume;
            nrRanduri = randuri;
            nrColoane = coloane;
            matriceLocuri = new Loc[10, 10]; // Alocare statica pentru simplitate

            for (int i = 0; i < nrRanduri; i++)
            {
                for (int j = 0; j < nrColoane; j++)
                {
                    matriceLocuri[i, j] = new Loc(i + 1, j + 1, pretBaza);
                }
            }
        }
    }
}