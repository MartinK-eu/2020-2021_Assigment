using System;
using System.IO;
using System.Text;

namespace D0JAQL_feleves
{
    class Program
    {
        static string file = "";

        static void Main(string[] args)
        {
            Console.Write("Melyik fájlból olvassam be a növényeket? (nev.txt): ");
            file = Console.ReadLine();
            Noveny[] novenyek = new Noveny[0];
            if (File.Exists(file))
            {
                novenyek = Inditas(file); //név, fényigény, vízigény, legutóbbi_öntözés(x, y, z), szúrós //első sor = növények száma
                Console.WriteLine("File beolvasva, továbblépéshez üss entert!");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Nincs ilyen fájl, kilépéskor létre fog jönni! Továbblépéshez üss entert!");
                Console.ReadLine();
            }
            FoMenu(ref novenyek);
        }

        static void FoMenu(ref Noveny[] novenyek)
        {
            Console.Clear();
            Console.WriteLine("Növénygondozás(nyomd meg a számot, amit szeretnél csinálni):\n1 - Növények adatainak listázása\n2 - Növény felvétele\n3 - Növény törlése\n4 - Adatmódosítás\n5 - Öntözendő növények kilistázása\n6 - Kilépés és változtatások mentése");
            string posibbleCommands = "123456"; 
            char command = Console.ReadKey().KeyChar;
            if (!posibbleCommands.Contains(command))
            {
                Console.WriteLine("Ez a menüpont (" + command + ") nem létezik, válassz másikat! (Üss entert a továbblépéshez)");
                Console.ReadLine();
            }
            else
            {
                Console.Clear();
                switch (command)
                {
                    case '1':
                        Menu1(novenyek);
                        break;
                    case '2':
                        Menu2(ref novenyek);
                        break;
                    case '3':
                        Menu3(ref novenyek);
                        break;
                    case '4':
                        Menu4(ref novenyek, -1);
                        break;
                    case '5':
                        Menu5(ref novenyek);
                        break;
                    case '6':
                        Kilepes(novenyek, file);
                        break;
                    default:
                        break;
                }
            }
            FoMenu(ref novenyek);
        }

        #region General

        static DateTime DatumBeolvas()
        {
            string[] localSplit;
            DateTime date;
            bool dateError;
            do
            {
                dateError = false;
                Console.Write("Ekkor volt utoljára locsolva:\nÉv: ");
                string local = Console.ReadLine() + "/";
                Console.Write("Hónap: ");
                local += Console.ReadLine() + "/";
                Console.Write("Nap: ");
                local += Console.ReadLine();
                localSplit = local.Split('/');
                try
                {
                    date = new DateTime(int.Parse(localSplit[0]), int.Parse(localSplit[1]), int.Parse(localSplit[2]));
                }
                catch (Exception)
                {
                    dateError = true;
                    Console.WriteLine("Hibás adatok, Próbálja újra. Üss entert továbblépéshez!");
                    Console.ReadLine();
                }
            } while (dateError);

            return new DateTime(int.Parse(localSplit[0]), int.Parse(localSplit[1]), int.Parse(localSplit[2]));
        }

        static int KeresNev(Noveny[] novenyek)
        {
            bool found = false;
            int index;
            do
            {
                Console.Write("Mi a neve?: ");
                string keresendo = Console.ReadLine();
                index = 0;
                while (index < novenyek.Length && !found)
                {
                    if (novenyek[index].Nev.ToLower() == keresendo.ToLower())
                        found = true;
                    else
                        index++;
                }
                if (!found)
                {
                    Console.WriteLine("Nincs ilyen növény, jól add meg a nevét! Üss entert továbblépéshez!");
                    Console.ReadLine();
                }
            } while (!found);
            
            return index;
        }
        #endregion

        static void Menu1(Noveny[] novenyek) //Listázás
        {
            for (int i = 0; i < novenyek.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine((i + 1) + " - " + novenyek[i].Nev);
                Console.ResetColor();
                Console.WriteLine(novenyek[i].Informacio() + "\n-------------");
            }
            Console.WriteLine("Üss entert a főmenübe való visszatéréshez!");
            Console.ReadLine();
        }

        #region Menu2 - Hozzáadás
        static void Menu2(ref Noveny[] novenyek)
        {
            Noveny local = new Noveny();
            Console.Write("Növény neve: ");
            local.Nev = Console.ReadLine();
            local.Fenyigeny = FenyigenyBeolvas();
            Console.Write("Ennyi naponta kell öntözni: ");
            local.Vizigeny = int.Parse(Console.ReadLine());
            local.UtolsoOntozes = DatumBeolvas();
            local.Szuros = SzurosBeolvas();
            Hozzaad(ref novenyek, local);
            Console.WriteLine("Növény hozzáadva! (Üss netert továbblépéshez)");
            Console.ReadLine();
        }

        static int FenyigenyBeolvas()
        {
            int local;

            do
            {
                Console.Write("Növény fényigénye (0 - Árnyékkedvelő (sciophyta), 1 - Fény -és árnykedvelő (heliosciophyta), 2 - Fénykedvelő (heliophyta)): ");
                local = int.Parse(Console.ReadLine());
                if (!(local == 0 || local == 1 || local == 2))
                {
                    Console.WriteLine("Helytelen bevitel, próbálja újra. üss entert a továbblépéshez!");
                    Console.ReadLine();
                }
            } while (!(local == 0 || local == 1 || local == 2));
            return local;
        }

        static bool SzurosBeolvas()
        {
            Console.Write("Szúrós? (igen/nem): ");
            string be;           
            bool ki = false;
            do
            {
                be = Console.ReadLine();
                if (be != "igen" && be != "nem")
                    Console.Write("Helytelen bevitel, próbálja újra.\nSzúrós? (igen/nem): ");
                else if(be == "igen")
                    ki = true;
            } while (be != "igen" && be != "nem");
            return ki;
        }

        static void Hozzaad (ref Noveny[] novenyek, Noveny ujNoveny)
        {
            Noveny[] novenyTemp = novenyek;
            novenyek = new Noveny[novenyek.Length + 1];
            for (int i = 0; i < novenyek.Length-1; i++)
            {
                novenyek[i] = novenyTemp[i];
            }
            novenyek[novenyek.Length-1] = ujNoveny;
        }
        #endregion

        #region Menu3 - Törlés
        static void Menu3(ref Noveny[] novenyek)
        {
            int index = KeresNev(novenyek);
            Torles(ref novenyek, index);
            Console.WriteLine("Növény törölve! A főmenübe való visszalépéshez nyomjon entert!");
            Console.ReadLine();
        }
        
        static void Torles(ref Noveny[] novenyek, int index)
        {

            for (int i = index; i < novenyek.Length - 1; i++)
            {
                novenyek[index + i] = novenyek[index + i + 1];
            }
            Noveny[] tempNovenyek = novenyek;
            novenyek = new Noveny[novenyek.Length - 1];
            for (int i = 0; i < novenyek.Length; i++)
            {
                novenyek[i] = tempNovenyek[i];
            }
        }
        #endregion

        #region Menu4 - Adatmódosítás

        static void Menu4(ref Noveny[] novenyek, int index)
        {
            if (index == -1)
                index = KeresNev(novenyek);
            Console.WriteLine("Mit szeretnél módosítani a növényen?\n1 - Név\n2 - fényigény\n3 - vízigény\n4 - Legutóbbi öntözés\n5 - Szúrós-e");
            char command = Console.ReadKey().KeyChar;

            switch (command)
            {
                case '1':
                    NevMod(ref novenyek, index);
                    break;
                case '2':
                    FenyigenyMod(ref novenyek, index);
                    break;
                case '3':
                    VizigenyMod(ref novenyek, index);
                    break;
                case '4':
                    novenyek[index].UtolsoOntozes = DatumBeolvas();
                    break;
                case '5':
                    SzurosMod(ref novenyek, index);
                    break;
                default:
                    Console.WriteLine("Ez a menüpont (" + command + ") nem létezik, válassz másikat! (Üss entert a továbblépéshez)");
                    Console.ReadLine();
                    Menu4(ref novenyek, index);
                    break;
            }

            string posibbleCommands = "12345";
            if (posibbleCommands.Contains(command))
            {
                Console.WriteLine("Módosítás megtörtént! Főmenübe való visszatéréshez nyomj entert!");
                Console.ReadLine();
            }
        }

        static void NevMod(ref Noveny[] novenyek, int index)
        {
            Console.Write("Új név: ");
            novenyek[index].Nev = Console.ReadLine();
        }
        static void FenyigenyMod(ref Noveny[] novenyek, int index)
        {
            Console.Write("Új fényigény (0 - Árnyékkedvelő (sciophyta), 1 - Fény -és árnykedvelő (heliosciophyta), 2 - Fénykedvelő (heliophyta): ");
            novenyek[index].Fenyigeny = int.Parse(Console.ReadLine());
        }
        static void VizigenyMod(ref Noveny[] novenyek, int index)
        {
            Console.Write("Új vízigény (hány naponta kell öntözni): ");
            novenyek[index].Vizigeny = int.Parse(Console.ReadLine());
        }
        static void SzurosMod(ref Noveny[] novenyek, int index)
        {
            if (novenyek[index].Szuros)
            {
                novenyek[index].Szuros = false;
                Console.WriteLine("Növény mostantól nem szúrós!");
            }
            else
            {
                novenyek[index].Szuros = true;
                Console.WriteLine("Növény mostantól szúrós!");
            }
        }

        #endregion 

        #region Menu5 - Öntözés

        static void Menu5(ref Noveny[] novenyek)
        {
            int[] ontozendok = OntozendoLista(novenyek);
            Console.WriteLine("Öntözendő növények: ");
            for (int i = 0; i < ontozendok.Length; i++)
            {
                if (novenyek[ontozendok[i]].Szuros)
                {
                    Console.Write((i + 1) + ". " + novenyek[ontozendok[i]].Nev);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" - Csak védőkesztyűben közelítsd meg!");
                    Console.ResetColor();
                }
                else
                    Console.WriteLine((i + 1) + ". " + novenyek[ontozendok[i]].Nev);
            }

            string possibleCommands = "12";
            char command;
            do
            {
                Console.WriteLine("\nVálassz az opciók közül:\n1 - A növények utolsó öntözési dátumának átírása a mai napra\n2 - Kilépés a főmenübe");
                command = Console.ReadKey().KeyChar;
                if (!possibleCommands.Contains(command))
                {
                    Console.WriteLine("Nincs ilyen menüpont (" + command + "), válassz a felsoroltak közül");
                }
            } while (!possibleCommands.Contains(command));

            if (command == '1')
                UjOntozes(ref novenyek, ontozendok);
        }

        static int[] OntozendoLista(Noveny[] novenyek)
        {
            int[] local = new int[0];
            for (int i = 0; i < novenyek.Length; i++)
            {
                if (novenyek[i].Ontozendo())
                {
                    int[] tempLocal = local;
                    local = new int[local.Length + 1];
                    for (int j = 0; j < tempLocal.Length; j++)
                    {
                        local[j] = tempLocal[j];
                    }
                    local[local.Length - 1] = i;
                }
            }
            return local;
        }

        static void UjOntozes(ref Noveny[] novenyek, int[] ontozendok)
        {
            for (int i = 0; i < ontozendok.Length; i++)
            {
                novenyek[ontozendok[i]].UtolsoOntozes = DateTime.Today;
            }
            Console.WriteLine("Utolsó öntözések átírva! Üss entert a főmenübe való visszalépéshez!");
            Console.ReadLine();
        }

        #endregion

        static Noveny[] Inditas(string file)
        {
            StreamReader fileIn = new StreamReader(file, Encoding.Default); //név, fényigény, vízigény, legutóbbi_öntözés(x, y, z), szúrós //első sor = növények száma
            Noveny[] novenyek = new Noveny[int.Parse(fileIn.ReadLine())];
                
            for (int i = 0; i < novenyek.Length; i++)
            {
                string[] temp = fileIn.ReadLine().Split(',');
                string[] temp_date = temp[3].Split('/');
                novenyek[i] = new Noveny(temp[0], int.Parse(temp[1]), int.Parse(temp[2]), new DateTime(int.Parse(temp_date[0]), int.Parse(temp_date[1]), int.Parse(temp_date[2])), bool.Parse(temp[4]));
            }

            fileIn.Close();
            return novenyek;
        }

        static void Kilepes(Noveny[] novenyek, string file)
        {
            StreamWriter fileOut = new StreamWriter(file);
            fileOut.WriteLine(novenyek.Length);
            for (int i = 0; i < novenyek.Length; i++)
            {
                fileOut.WriteLine(novenyek[i].KiSor());
            }
            fileOut.Close();
            System.Environment.Exit(0);
        }
    }
}
