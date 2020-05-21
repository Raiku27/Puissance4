using Puissance4.classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace Puissance4.window
{
	public partial class StatsWindow : Window
	{
		private int nbrParties;
		public StatsWindow(MainWindow newOwner)
		{
			InitializeComponent();
			Owner = newOwner;
			int nbrVictoires = 0;
			int nbrPartiesGagne = 0;
			int nbrFoisJoueeEnPremier = 0;
			
			nbrParties = Directory.GetFiles(((MainWindow)Owner).PathParties, "*", SearchOption.AllDirectories).Length - 1;

			for(int i = 0; i < nbrParties; i++)
			{
				try
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(Partie));
					XmlReader xmlReader = XmlReader.Create(((MainWindow)Owner).PathParties + i.ToString() + ".xml");
					Partie partie = (Partie)xmlSerializer.Deserialize(xmlReader);

					//JoueurPlusVictoires
					int res1 = getNbrVictoire(partie.Joueur1.Nom, partie.Joueur1.Prenom);
					int res2 = getNbrVictoire(partie.Joueur2.Nom, partie.Joueur2.Prenom);
					if (res1 > nbrVictoires)
					{
						nbrVictoires = res1;
						JoueurPlusVictoires.Content = partie.Joueur1.Nom + " " + partie.Joueur2.Prenom + " " + res1.ToString();
					}	
					else if (res2 > nbrVictoires)
					{
						nbrVictoires = res2;
						JoueurPlusVictoires.Content = partie.Joueur2.Nom + " " + partie.Joueur2.Prenom + " " + res2.ToString();
					}

					//JoueurPlusPartiesJouees
					int res3 = getNbrPartiesJouees(partie.Joueur1.Nom, partie.Joueur1.Prenom);
					int res4 = getNbrPartiesJouees(partie.Joueur2.Nom, partie.Joueur2.Prenom);
					if (res3 > nbrPartiesGagne)
					{
						nbrPartiesGagne = res3;
						JoueurPlusParties.Content = partie.Joueur1.Nom + " " + partie.Joueur1.Prenom + " " + res3.ToString();
					}
					else if (res4 > nbrPartiesGagne)
					{
						nbrPartiesGagne = res4;
						JoueurPlusParties.Content = partie.Joueur2.Nom + " " + partie.Joueur2.Prenom + " "  + res4.ToString();
					}

					//CaseLaPlusJoueeEnPremier
					int res5 = getNbrFoisJoueeEnPremier(partie.FirstLigne,partie.FirstColonne);
					if(res5 > nbrFoisJoueeEnPremier)
					{
						nbrFoisJoueeEnPremier = res5;
						CasePlusJouee.Content = string.Format("L: {0} C: {1} => {2}",partie.FirstLigne,partie.FirstColonne,res5);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Puissance4 - Erreur");
				}
			}
		}

		private void BouttonOk_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		public int getNbrVictoire(string Nom,string Prenom)
		{
			int result = 0;

			for (int i = 0; i < nbrParties; i++)
			{
				try
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(Partie));
					XmlReader xmlReader = XmlReader.Create(((MainWindow)Owner).PathParties + i.ToString() + ".xml");
					Partie partie = (Partie)xmlSerializer.Deserialize(xmlReader);

					if (partie.Gagant == 1 && partie.Joueur1.Nom.Equals(Nom) && partie.Joueur1.Prenom.Equals(Prenom))
					{
						result++;
					}
					else if (partie.Gagant == 2 && partie.Joueur2.Nom.Equals(Nom) && partie.Joueur2.Prenom.Equals(Prenom))
					{
						result++;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Puissance4 - Erreur");
				}
			}
			return result;
		}

		public int getNbrPartiesJouees(string Nom, string Prenom)
		{
			int result = 0;

			for (int i = 0; i < nbrParties; i++)
			{
				try
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(Partie));
					XmlReader xmlReader = XmlReader.Create(((MainWindow)Owner).PathParties + i.ToString() + ".xml");
					Partie partie = (Partie)xmlSerializer.Deserialize(xmlReader);

					if (partie.Joueur1.Nom.Equals(Nom) && partie.Joueur1.Prenom.Equals(Prenom))
					{
						result++;
					}
					else if (partie.Joueur2.Nom.Equals(Nom) && partie.Joueur2.Prenom.Equals(Prenom))
					{
						result++;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Puissance4 - Erreur");
				}
			}
			return result;
		}

		public int getNbrFoisJoueeEnPremier(int ligne,int colonne)
		{
			int result = 0;

			for (int i = 0; i < nbrParties; i++)
			{
				try
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(Partie));
					XmlReader xmlReader = XmlReader.Create(((MainWindow)Owner).PathParties + i.ToString() + ".xml");
					Partie partie = (Partie)xmlSerializer.Deserialize(xmlReader);

					if (partie.FirstLigne == ligne && partie.FirstColonne == colonne)
					{
						result++;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Puissance4 - Erreur");
				}
			}
			return result;
		}
	}
}
