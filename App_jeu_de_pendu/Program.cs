using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsciiArt;
using System.IO;

namespace App_jeu_de_pendu
{
    class Program
    {
        static void AfficherMot(string mot, List<char> lettres)
        {
            string motCacher = "";
            foreach(var caractere in mot)
            {
                if (lettres.Contains(caractere))
                {
                    motCacher += " " + caractere;
                }
                else
                {
                    motCacher += " _";
                }
            }

            Console.WriteLine(motCacher);
        }

        static bool ToutesLetrresDevinees(string mot, List<char> lettres)
        {
            foreach(var lettre in mot)
            {
                if (lettres.Contains(lettre))
                {
                    mot = mot.Replace(lettre.ToString(),"");
                }
            }

            if(mot == "")
            {
                return true;
            }
            return false;
        }

        static char DemanderUneLettre( string message = "Rentrez une lettre : ")
        {
            string lettre = "";
            int num;
            while (true)
            {
                Console.Write(message);
                lettre = Console.ReadLine();
                bool isNumber = int.TryParse(lettre, out num);

                if(lettre.Length == 1 && !isNumber)
                {
                    lettre = lettre.ToUpper();
                    return lettre[0];
                }
                Console.WriteLine("ERREUR : Vous devez rentrer une lettre\n");
            }
        }

        static void DevinerMot(string mot)
        {
            //AfficherMot(mot);
            var lettresDevinees = new List<char>();
            var lettresExclus = new List<char>();
            const int NB_VIE = 6;
            int viesRestantes = NB_VIE;

            while (viesRestantes > 0)
            {
                Console.WriteLine(Ascii.PENDU[NB_VIE - viesRestantes]);
                Console.WriteLine();
                AfficherMot(mot, lettresDevinees);
                Console.WriteLine();
                var lettre = DemanderUneLettre();
                Console.Clear();
                if(mot.Contains(lettre)){
                    Console.WriteLine("Cette lettre est dans le mot ");
                    lettresDevinees.Add(lettre);
                    if(ToutesLetrresDevinees(mot, lettresDevinees))
                    {
                        break;
                    }
                }
                else
                {
                    if (!lettresExclus.Contains(lettre))
                    {
                        lettresExclus.Add(lettre);
                        viesRestantes--;
                    }
                    Console.WriteLine("Nombre de vie restante : " + viesRestantes);

                }

                if (lettresExclus.Count > 0)
                {
                    Console.WriteLine("Le mot ne contient pas les lettres : " + string.Join(", ", lettresExclus));
                }
                Console.WriteLine();
            }

            Console.WriteLine(Ascii.PENDU[NB_VIE - viesRestantes]);

            if (viesRestantes == 0)
            {
                Console.WriteLine("PERDU ! le mot était : " + mot);
            }
            else
            {
                AfficherMot(mot, lettresDevinees);
                Console.WriteLine();
                Console.WriteLine("GAGNÉ ! Toutes les lettres ont été trouvées");
            }
        }

        static string[] ChargerLesMots(string nomFichier)
        {
            try
            {
                return File.ReadAllLines(nomFichier);
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERREUR de lecture du fichier : " + nomFichier + " (" + ex.Message + ")");
            }

            return null;
            
        }

        static bool DemanderDeRejouer()
        {
            var response = DemanderUneLettre("Voulez-vous rejouer (o/n)");
            if(response == 'o' || response == 'O')
            {
                return true;
            }else if (response == 'n' || response == 'N')
            {
                return false;
            }
            else
            {
                Console.WriteLine("ERREUR ! Vous devez répondre avec 'o' ou 'n' ");
                return DemanderDeRejouer();
            }
        }

        static void Main(string[] args)
        {
            var r = new Random();
            var mots = ChargerLesMots("mots.txt");
            if((mots == null) || (mots.Length == 0))
            {
                Console.WriteLine("La liste des mot est vide");
            }
            else
            {
                while (true)
                {
                    int rand = r.Next(mots.Length);
                    string mot = mots[rand].Trim().ToUpper();
                    DevinerMot(mot);
                    if (!DemanderDeRejouer())
                    {
                        break;
                    }
                    Console.Clear();
                }
                Console.WriteLine("Merci et à bientôt.");
                
            }
            Console.ReadKey();
        }
    }
}
