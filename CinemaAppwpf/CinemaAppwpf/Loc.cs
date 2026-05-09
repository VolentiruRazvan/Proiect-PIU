namespace CinemaAppwpf
{
    public class Loc
    {
        public int rand;
        public int numar;
        public bool ocupat;
        public float pret;

        public Loc() { }

        public Loc(int r, int n, float p)
        {
            rand = r;
            numar = n;
            pret = p;
            ocupat = false;
        }
    }
}