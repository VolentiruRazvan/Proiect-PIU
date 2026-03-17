namespace Rezervari
{
    public class Loc
    {
        public int rand;
        public int numar;
        public bool ocupat;
        public int pret;

        public Loc(int r, int n, int p)
        {
            rand = r;
            numar = n;
            pret = p;
            ocupat = false;
        }
    }
}