using System;

namespace D0JAQL_feleves
{
    class Noveny
    {
        public string Nev { get; set; }
        public int Fenyigeny { get; set; } //0 - Árnyékkedvelő (sciophyta), 1 - Fény -és árnykedvelő (heliosciophyta), 2 - Fénykedvelő (heliophyta)
        public int Vizigeny { get; set; }
        public DateTime UtolsoOntozes { get; set; }
        public bool Szuros { get; set; }

        public Noveny(string nev, int fenyigeny, int vizigeny, DateTime utolsoOntozes, bool szuros)
        {
            Nev = nev;
            Fenyigeny = fenyigeny;
            Vizigeny = vizigeny;
            UtolsoOntozes = utolsoOntozes;
            Szuros = szuros;
        }

        public Noveny()
        {
        }

        public string Informacio()
        {
            string local = "";
            if (Fenyigeny == 0)
            {
                local = $"Árnyékkedvelő (sciophyta)\n{Vizigeny} naponta szükséges öntözni\nLegutóbb megöntözve {UtolsoOntozes.Year}/{UtolsoOntozes.Month}/{UtolsoOntozes.Day} napon volt\n";
                if (Szuros)
                {
                    local += "Szúrós!";
                }
                else
                {
                    local += "Nem szúrós";
                }
            }
            else if (Fenyigeny == 1)
            {
                local = $"Fény -és árnykedvelő (heliosciophyta)\n{Vizigeny} naponta szükséges öntözni\nLegutóbb megöntözve {UtolsoOntozes.Year}/{UtolsoOntozes.Month}/{UtolsoOntozes.Day} napon volt\n";
                if (Szuros)
                {
                    local += "Szúrós!";
                }
                else
                {
                    local += "Nem szúrós";
                }
            }
            else
            {
                local = $"Fénykedvelő (heliophyta)\n{Vizigeny} naponta szükséges öntözni\nLegutóbb megöntözve {UtolsoOntozes.Year}/{UtolsoOntozes.Month}/{UtolsoOntozes.Day} napon volt\n";
                if (Szuros)
                {
                    local += "Szúrós!";
                }
                else
                {
                    local += "Nem szúrós";
                }
            }
            return local;
        }

        public bool Ontozendo()
        {
            return (UtolsoOntozes.AddDays(Vizigeny)).CompareTo(DateTime.Today) <= 0;
        }

        public  string KiSor()
        {
            string local = "";
            local += Nev + ",";
            local += Fenyigeny + ",";
            local += Vizigeny + ",";
            local += UtolsoOntozes.Year + "/" + UtolsoOntozes.Month + "/" + UtolsoOntozes.Day + ",";
            local += Szuros;
            return local;
        }
    }
}
