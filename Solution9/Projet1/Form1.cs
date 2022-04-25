// Théme : affichage des distances entre les villes françaises
// réalisé par JM CARTRON le 27/10/2020

using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;          // pour accéder aux classes de gestion des fichiers (IO = Input Output)

namespace Projet1
{
    public partial class Form1 : Form
    {
        // Constantes globales
        const int NBR_VILLES = 10;     // nombre de villes

        // Variables globales
        static String[] lesVilles = new String[NBR_VILLES];
        static double[] lesLatitudes = new double[NBR_VILLES];
        static double[] lesLongitudes = new double[NBR_VILLES];
        static double[,] lesDistances = new double[NBR_VILLES, NBR_VILLES];

        // constructeur du formulaire
        public Form1()
        {
            InitializeComponent();
        }

        // traitement initial après le chargement du formulaire
        private void Form1_Load(object sender, EventArgs e)
        {
            bool ok = remplirTableaux();    // appel de la fonction de remplissage des 3 tableaux
            if (ok)
            {
                // calcul des distances entre chaque ville
                calculDistances();
                // affichage du tableau
                afficherTableau();
            }
        } // fin de la fonction Form1_Load

        // fonction de remplissage des tableaux lesVilles, lesLatitudes et lesLongitudes
        // les tableaux sont remplis à partir d'un fichier texte contenant les villes et leur position
        static bool remplirTableaux()
        {
            String nomFic = "villes.txt";       // le nom du fichier contenant les villes et leur position
            StreamReader flux;                  // pour lire dans le fichier
            String ligneLue;                    // contient une ligne lue dans le fichier
            int position = 0;                   // position d'une case du tableau
            String msg;

            // tester si le fichier existe :
            if (File.Exists(nomFic) == false)
            {
                msg = "Impossible de trouver le fichier " + nomFic;
                MessageBox.Show(msg, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {   // ouverture du fichier en lecture
                flux = File.OpenText(nomFic);
                // boucle de traitement des lignes du fichier
                while (flux.Peek() != -1)
                {   // lit le nom de la ville et le range dans le tableau correspondant
                    ligneLue = flux.ReadLine();
                    lesVilles[position] = ligneLue.Trim();

                    // lit la latitude et la range dans le tableau correspondant
                    ligneLue = flux.ReadLine();
                    lesLatitudes[position] = Convert.ToDouble(ligneLue.Trim());

                    // lit la longitude et la range dans le tableau correspondant
                    ligneLue = flux.ReadLine();
                    lesLongitudes[position] = Convert.ToDouble(ligneLue.Trim());

                    // incrémente la position de la prochaine case à remplir
                    position++;
                }
                // ferme le fichier
                flux.Close();
                return true;
            }
        } // fin de la fonction remplirTableaux

        // affiche le tableau en plaçant une série de contrôles étiquettes (Label) dans un contrôle Panel
        private void afficherTableau()
        {
            Label uneCase;                  // chaque case est représentée par une étiquette (contrôle Label)
            int largeurCase, hauteurCase;   // largeur et hauteur des cases du panneau central
            int lig, col;                   // numéros de ligne et de colonne

            largeurCase = this.panelCentral.Width / NBR_VILLES;
            hauteurCase = this.panelCentral.Height / NBR_VILLES;

            // affichage des noms de villes dans panelHaut
            for (col = 0; col < NBR_VILLES; col++)
            {
                uneCase = new Label();									    // on crée le contrôle, et on règle...
                uneCase.Width = largeurCase;							    // sa largeur
                uneCase.Height = hauteurCase;							    // sa hauteur
                uneCase.Top = 0;			                                // sa position haute
                uneCase.Left = col * largeurCase;                           // sa position gauche
                uneCase.Text = lesVilles[col];		                        // son contenu
                uneCase.TextAlign = ContentAlignment.MiddleCenter;		    // l'alignement du contenu
                uneCase.BorderStyle = BorderStyle.FixedSingle;			    // sa bordure
                uneCase.BackColor = Color.Aquamarine;                       // sa couleur
                this.panelHaut.Controls.Add(uneCase);                       // et on l'ajoute au contrôle panelHaut
            }

            // affichage des noms de villes dans panelGauche
            for (lig = 0; lig < NBR_VILLES; lig++)
            {
                uneCase = new Label();									    // on crée le contrôle, et on règle...
                uneCase.Width = largeurCase;							    // sa largeur
                uneCase.Height = hauteurCase;							    // sa hauteur
                uneCase.Top = lig * hauteurCase;                            // sa position haute
                uneCase.Left = 0;			                                // sa position gauche
                uneCase.Text = lesVilles[lig];		                        // son contenu
                uneCase.TextAlign = ContentAlignment.MiddleCenter;		    // l'alignement du contenu
                uneCase.BorderStyle = BorderStyle.FixedSingle;			    // sa bordure
                uneCase.BackColor = Color.Aquamarine;                       // sa couleur
                this.panelGauche.Controls.Add(uneCase);                     // et on l'ajoute au contrôle panelGauche
            }

            // affichage des distances dans panelCentral
            for (lig = 0; lig < NBR_VILLES; lig++)
            {
                for (col = 0; col < NBR_VILLES; col++)
                {
                    uneCase = new Label();									// on crée le contrôle, et on règle...
                    uneCase.Width = largeurCase;							// sa largeur
                    uneCase.Height = hauteurCase;							// sa hauteur
                    uneCase.Top = lig * hauteurCase;                        // sa position haute
                    uneCase.Left = col * largeurCase;                       // sa position gauche
                    uneCase.Text = lesDistances[lig, col].ToString("0");    // son contenu
                    uneCase.TextAlign = ContentAlignment.MiddleCenter;		// l'alignement du contenu
                    uneCase.BorderStyle = BorderStyle.FixedSingle;			// sa bordure
                    uneCase.BackColor = Color.Aqua;                         // sa couleur
                    this.panelCentral.Controls.Add(uneCase);                // et on l'ajoute au contrôle panelCentral
                    
                    if (lesDistances[lig, col] == 0)
                    {
                        uneCase.BackColor = Color.White;
                    }
                    else
                    {
                        uneCase.BackColor = Color.Aqua;
                    }
                }
            }
        }
        // Calcul de la distance (en Km) entre 2 points géographiques.
        // paramètre latitude1  : latitude point 1 (en degrés décimaux)
        // paramètre longitude1 : longitude point 1 (en degrés décimaux)
        // paramètre latitude2  : latitude point 2 (en degrés décimaux)
        // paramètre longitude2 : longitude point 2 (en degrés décimaux)
        // retourne           : la distance (en Km) entre les 2 points
        static double getDistanceBetween(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            if (latitude1 == latitude2 && longitude1 == longitude2) return 0;

            double a = Math.PI / 180;
            latitude1 = latitude1 * a;
            latitude2 = latitude2 * a;
            longitude1 = longitude1 * a;
            longitude2 = longitude2 * a;
            double t1 = Math.Sin(latitude1) * Math.Sin(latitude2);
            double t2 = Math.Cos(latitude1) * Math.Cos(latitude2);
            double t3 = Math.Cos(longitude1 - longitude2);
            double t4 = t2 * t3;
            double t5 = t1 + t4;
            double rad_dist = Math.Atan(-t5 / Math.Sqrt(-t5 * t5 + 1)) + 2 * Math.Atan(1);
            return (rad_dist * 3437.74677 * 1.1508) * 1.6093470878864446;
        }

        // calcul des distances entre les villes et remplissage du tableau lesDistances
        static void calculDistances()
        {
            int lig, col;
            double latitude1, longitude1, latitude2, longitude2;
            for (lig = 0; lig <= NBR_VILLES - 1; lig++)
            {
                latitude1 = lesLatitudes[lig];
                longitude1 = lesLongitudes[lig];
                for (col = 0; col <= NBR_VILLES - 1; col++)
                {
                    latitude2 = lesLatitudes[col];
                    longitude2 = lesLongitudes[col];
                    lesDistances[lig, col] = getDistanceBetween(lesLatitudes[lig], lesLongitudes[lig], lesLatitudes[col], lesLongitudes[col]);
                }
            }
        } // fin de la fonction calculDistances
    } // fin de la classe
} // fin du namespace
