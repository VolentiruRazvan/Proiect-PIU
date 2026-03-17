using System;

namespace Rezervari
{
    class Program
    {
        static Locatie[] vectorLocatii = new Locatie[5];
        static int nrLocatii = 0;

        static void Main(string[] args)
        {
            InitializareDate();
            Jurnal.AdaugaInregistrare("A inceput programul");

            int optiune = -1;
            Locatie locCurent = null;
            Spectacol specCurent = null;

            while (optiune != 0)
            {
                Console.Clear();
                Console.WriteLine("1. Alege Locatie");
                Console.WriteLine("2. Alege Spectacol");
                Console.WriteLine("3. Cauta spectacol");
                Console.WriteLine("4. Vezi locuri");
                Console.WriteLine("5. Rezerva loc");
                Console.WriteLine("6. Goleste locuri");
                Console.WriteLine("7. Jurnal actiuni");
                Console.WriteLine("0. Iesire");

                if (locCurent != null) Console.WriteLine("Locatie aleasa: " + locCurent.numeLocatie);
                if (specCurent != null) Console.WriteLine("Spectacol ales: " + specCurent.numeSpectacol);

                Console.Write("Alege: ");
                optiune = int.Parse(Console.ReadLine());

                if (optiune == 1)
                {
                    for (int i = 0; i < nrLocatii; i++)
                    {
                        Console.WriteLine((i + 1) + " - " + vectorLocatii[i].numeLocatie);
                    }
                    Console.Write("Alege numarul: ");
                    int l = int.Parse(Console.ReadLine());
                    locCurent = vectorLocatii[l - 1];
                    specCurent = null;
                    Jurnal.AdaugaInregistrare("A ales locatia " + locCurent.numeLocatie);
                }
                else if (optiune == 2)
                {
                    if (locCurent != null)
                    {
                        for (int i = 0; i < locCurent.nrSpectacole; i++)
                        {
                            Console.WriteLine((i + 1) + " - " + locCurent.spectacole[i].numeSpectacol);
                        }
                        Console.Write("Alege numarul: ");
                        int s = int.Parse(Console.ReadLine());
                        specCurent = locCurent.spectacole[s - 1];
                        Jurnal.AdaugaInregistrare("A ales spectacolul " + specCurent.numeSpectacol);
                    }
                }
                else if (optiune == 3)
                {
                    Console.Write("Ce cauti? ");
                    string textCautat = Console.ReadLine();
                    int gasit = 0;

                    for (int i = 0; i < nrLocatii; i++)
                    {
                        for (int j = 0; j < vectorLocatii[i].nrSpectacole; j++)
                        {
                            if (vectorLocatii[i].spectacole[j].numeSpectacol.Contains(textCautat))
                            {
                                Console.WriteLine("Gasit: " + vectorLocatii[i].spectacole[j].numeSpectacol + " la " + vectorLocatii[i].numeLocatie);
                                gasit = 1;
                            }
                        }
                    }
                    if (gasit == 0) Console.WriteLine("Nu exista.");
                    Jurnal.AdaugaInregistrare("A cautat: " + textCautat);
                    Console.ReadLine();
                }
                else if (optiune == 4)
                {
                    if (specCurent != null)
                    {
                        specCurent.AfisareLocuri();
                        Console.ReadLine();
                    }
                }
                else if (optiune == 5)
                {
                    if (specCurent != null)
                    {
                        specCurent.AfisareLocuri();
                        Console.Write("Rand: ");
                        int r = int.Parse(Console.ReadLine());
                        Console.Write("Loc: ");
                        int c = int.Parse(Console.ReadLine());

                        if (specCurent.matriceLocuri[r - 1, c - 1].ocupat == true)
                        {
                            Console.WriteLine("Loc ocupat!");
                        }
                        else
                        {
                            specCurent.matriceLocuri[r - 1, c - 1].ocupat = true;
                            Console.WriteLine("Rezervat. Costa: " + specCurent.matriceLocuri[r - 1, c - 1].pret);
                            Jurnal.AdaugaInregistrare("A rezervat la " + specCurent.numeSpectacol + " rand " + r + " loc " + c);
                        }
                        Console.ReadLine();
                    }
                }
                else if (optiune == 6)
                {
                    if (specCurent != null)
                    {
                        specCurent.GolesteLocuri();
                        Console.WriteLine("Locuri resetate.");
                        Jurnal.AdaugaInregistrare("A golit locurile la " + specCurent.numeSpectacol);
                        Console.ReadLine();
                    }
                }
                else if (optiune == 7)
                {
                    Jurnal.ArataInregistrari();
                    Console.ReadLine();
                }
            }
        }

        static void InitializareDate()
        {
            Locatie l1 = new Locatie("Cinema");
            l1.AdaugaSpectacol(new Spectacol("Film 1", 5, 5, 20));
            l1.AdaugaSpectacol(new Spectacol("Film 2", 4, 8, 25));

            Locatie l2 = new Locatie("Teatru");
            l2.AdaugaSpectacol(new Spectacol("Piesa 1", 3, 10, 50));

            vectorLocatii[0] = l1;
            vectorLocatii[1] = l2;
            nrLocatii = 2;
        }
    }
}