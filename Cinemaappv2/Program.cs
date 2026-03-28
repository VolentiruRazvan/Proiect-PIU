using System;

namespace Rezervari
{
    class Program
    {
        static AdministratorEntitateMemorie admin = new AdministratorEntitateMemorie();
        static AdministrareLocatiiFisierText adminLocatiiFisier = new AdministrareLocatiiFisierText();
        static AdministrareSpectacoleFisierText adminSpectacoleFisier = new AdministrareSpectacoleFisierText();

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
                Console.WriteLine("=== MENIU ===");
                Console.WriteLine("1. Alege Locatie");
                Console.WriteLine("2. Alege Spectacol");
                Console.WriteLine("3. Cauta locatie (cu LINQ)");
                Console.WriteLine("4. Vezi locuri");
                Console.WriteLine("5. Rezerva loc");
                Console.WriteLine("6. Goleste locuri");
                Console.WriteLine("7. Jurnal actiuni");
                Console.WriteLine("--- TEMA FISIERE ---");
                Console.WriteLine("8. Salveaza o locatie noua in fisier");
                Console.WriteLine("9. Modifica nume locatie in fisier");
                Console.WriteLine("10. Salveaza un spectacol nou in fisier");
                Console.WriteLine("11. Cauta spectacol in fisier");
                Console.WriteLine("0. Iesire");

                if (locCurent != null) Console.WriteLine("\nLocatie aleasa: " + locCurent.numeLocatie);
                if (specCurent != null) Console.WriteLine("Spectacol ales: " + specCurent.numeSpectacol);

                Console.Write("\nAlege: ");
                optiune = int.Parse(Console.ReadLine());

                if (optiune == 1)
                {
                    for (int i = 0; i < admin.nrLocatii; i++)
                    {
                        Console.WriteLine((i + 1) + " - " + admin.locatii[i].numeLocatie);
                    }
                    Console.Write("Alege numarul: ");
                    int l = int.Parse(Console.ReadLine());
                    locCurent = admin.locatii[l - 1];
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
                    Console.Write("Ce locatie cauti? ");
                    string textCautat = Console.ReadLine();

                    Locatie gasit = admin.CautaLocatieDupaNume(textCautat);

                    if (gasit != null)
                    {
                        Console.WriteLine("Gasit: " + gasit.numeLocatie);
                        locCurent = gasit;
                        specCurent = null;
                    }
                    else
                    {
                        Console.WriteLine("Nu exista.");
                    }
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
                        Console.Write("Rand (Litera A-H): ");
                        string litera = Console.ReadLine().ToUpper();
                        int r = litera[0] - 'A' + 1;

                        Console.Write("Loc (1-10): ");
                        int c = int.Parse(Console.ReadLine());

                        if (r < 1 || r > specCurent.nrRanduri || c < 1 || c > specCurent.nrColoane)
                        {
                            Console.WriteLine("Rand sau loc invalid!");
                        }
                        else if (specCurent.matriceLocuri[r - 1, c - 1].ocupat == true)
                        {
                            Console.WriteLine("Loc ocupat!");
                        }
                        else
                        {
                            specCurent.matriceLocuri[r - 1, c - 1].ocupat = true;
                            Console.WriteLine("Rezervat. Costa: " + specCurent.matriceLocuri[r - 1, c - 1].pret);
                            Jurnal.AdaugaInregistrare("A rezervat la " + specCurent.numeSpectacol + " rand " + litera[0] + " loc " + c);
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
                        Console.ReadLine();
                    }
                }
                else if (optiune == 7)
                {
                    Jurnal.ArataInregistrari();
                    Console.ReadLine();
                }
                else if (optiune == 8)
                {
                    Console.Write("Nume locatie noua: ");
                    string nume = Console.ReadLine();
                    Locatie lNou = new Locatie(nume);
                    adminLocatiiFisier.AdaugaLocatie(lNou);
                    Console.WriteLine("Salvat in Locatii.txt");
                    Console.ReadLine();
                }
                else if (optiune == 9)
                {
                    Console.Write("Numele locatiei de modificat: ");
                    string numeVechi = Console.ReadLine();
                    Console.Write("Numele nou: ");
                    string numeNou = Console.ReadLine();
                    adminLocatiiFisier.ModificaLocatie(numeVechi, numeNou);
                    Console.WriteLine("Modificat in fisier.");
                    Console.ReadLine();
                }
                else if (optiune == 10)
                {
                    Console.Write("Nume spectacol nou: ");
                    string nume = Console.ReadLine();
                    Spectacol sNou = new Spectacol(nume, 8, 10, 20);
                    adminSpectacoleFisier.AdaugaSpectacol(sNou);
                    Console.WriteLine("Salvat in Spectacole.txt cu valori standard.");
                    Console.ReadLine();
                }
                else if (optiune == 11)
                {
                    Console.Write("Ce spectacol cauti in fisier? ");
                    string nume = Console.ReadLine();
                    Spectacol gasit = adminSpectacoleFisier.CautaSpectacolFisier(nume);
                    if (gasit != null)
                    {
                        Console.WriteLine("Gasit: " + gasit.numeSpectacol + " cu " + gasit.nrRanduri + " randuri.");
                    }
                    else
                    {
                        Console.WriteLine("Nu a fost gasit in fisier.");
                    }
                    Console.ReadLine();
                }
            }
        }

        static void InitializareDate()
        {
            Locatie c1 = new Locatie("Cinema City Suceava");
            c1.AdaugaSpectacol(new Spectacol("Dune: Partea II", 8, 10, 25));
            c1.AdaugaSpectacol(new Spectacol("Oppenheimer", 8, 10, 25));
            c1.AdaugaSpectacol(new Spectacol("Deadpool & Wolverine", 8, 10, 30));

            Locatie c2 = new Locatie("Cinema City Botosani");
            c2.AdaugaSpectacol(new Spectacol("Joker: Folie a Deux", 8, 10, 20));
            c2.AdaugaSpectacol(new Spectacol("Inside Out 2", 8, 10, 20));

            Locatie c3 = new Locatie("Cinema City Iasi");
            c3.AdaugaSpectacol(new Spectacol("Interstellar", 8, 10, 30));
            c3.AdaugaSpectacol(new Spectacol("The Batman", 8, 10, 25));
            c3.AdaugaSpectacol(new Spectacol("Inception", 8, 10, 25));
            c3.AdaugaSpectacol(new Spectacol("Avatar: Calea Apei", 8, 10, 30));

            Locatie c4 = new Locatie("Cinema Dorohoi");
            c4.AdaugaSpectacol(new Spectacol("Furiosa: O saga Mad Max", 8, 10, 15));
            c4.AdaugaSpectacol(new Spectacol("Gladiatorul II", 8, 10, 15));

            Locatie t1 = new Locatie("Sala de teatru Dorohoi");
            t1.AdaugaSpectacol(new Spectacol("O scrisoare pierduta", 8, 10, 40));
            t1.AdaugaSpectacol(new Spectacol("Take, Ianke si Cadir", 8, 10, 35));

            Locatie t2 = new Locatie("Sala de teatru Suceava");
            t2.AdaugaSpectacol(new Spectacol("Dineu cu prosti", 8, 10, 50));
            t2.AdaugaSpectacol(new Spectacol("Gaitele", 8, 10, 45));

            Locatie t3 = new Locatie("Sala de teatru Iasi");
            t3.AdaugaSpectacol(new Spectacol("Faust", 8, 10, 70));
            t3.AdaugaSpectacol(new Spectacol("Chirita in provintie", 8, 10, 60));
            t3.AdaugaSpectacol(new Spectacol("Visul unei nopti de vara", 8, 10, 60));

            admin.AdaugaLocatie(c1);
            admin.AdaugaLocatie(c2);
            admin.AdaugaLocatie(c3);
            admin.AdaugaLocatie(c4);
            admin.AdaugaLocatie(t1);
            admin.AdaugaLocatie(t2);
            admin.AdaugaLocatie(t3);
        }
    }
}