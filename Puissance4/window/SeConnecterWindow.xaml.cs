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
using System.Xml.Serialization;

namespace Puissance4.window
{
	public partial class SeConnecterWindow : Window
	{
		public SeConnecterWindow(MainWindow newOwner)
		{
			InitializeComponent();
			Owner = newOwner;
			CheckBoxJoueur1.IsChecked = true;
			if (((MainWindow)Owner).Joueur1 != null)
			{
				CheckBoxJoueur1.IsChecked = false;
				CheckBoxJoueur2.IsChecked = true;
			}
		}

		private void BouttonAnnuler_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void BouttonSeConnecter_Click(object sender, RoutedEventArgs e)
		{
			if (TextBoxNom.Text.Length == 0 || TextBoxPrenom.Text.Length == 0 || TextBoxMotDePasse.Text.Length == 0)
			{
				MessageBox.Show("Impossible de se connecter, une case est vide!", "Puissance4 - Erreur");
				return;
			}
			
			if (File.Exists(((MainWindow)Owner).PathJoueur + TextBoxNom.Text + TextBoxPrenom.Text + ".xml"))
			{
				//Charger le joueur
				try
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(Joueur));
					FileStream fileStream = File.OpenRead(((MainWindow)Owner).PathJoueur + TextBoxNom.Text + TextBoxPrenom.Text + ".xml");
					Joueur joueur = (Joueur)xmlSerializer.Deserialize(fileStream);
					fileStream.Close();
					if (joueur.MotDePasse.CompareTo(TextBoxMotDePasse.Text) == 0)
					{
						if (CheckBoxJoueur1.IsChecked == true)
						{
							if(((MainWindow)Owner).Joueur2 != null && ((MainWindow)Owner).Joueur2.Nom.CompareTo(joueur.Nom) == 0 && ((MainWindow)Owner).Joueur2.Prenom.CompareTo(joueur.Prenom) == 0)
							{
								MessageBox.Show("Les 2 joueurs ne peuvent pas êtres les mêmes!", "Puissance4 - Erreur");
								return;
							}
							else
							{
								((MainWindow)Owner).Joueur1 = joueur;
							}
						}

						else if (CheckBoxJoueur2.IsChecked == true)
						{
							if (((MainWindow)Owner).Joueur1 != null && ((MainWindow)Owner).Joueur1.Nom.CompareTo(joueur.Nom) == 0 && ((MainWindow)Owner).Joueur1.Prenom.CompareTo(joueur.Prenom) == 0)
							{
								MessageBox.Show("Les 2 joueurs ne peuvent pas êtres les mêmes!", "Puissance4 - Erreur");
								return;
							}
							else
							{
								((MainWindow)Owner).Joueur2 = joueur;
							}
						}
						Close();
					}
					else
					{
						MessageBox.Show("Mot de passe incorrecte!", "Puissance4 - Erreur");
						return;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Puissance4 - Erreur");
					return;
				}
			}
			else
			{
				MessageBox.Show("Impossible de se connecter, aucuns joueur trouver!", "Puissance4 - Erreur");
				return;
			}
			
		}

		private void CheckBoxJoueur1_Click(object sender, RoutedEventArgs e)
		{
			CheckBoxJoueur1.IsChecked = true;
			CheckBoxJoueur2.IsChecked = false;
		}

		private void CheckBoxJoueur2_Click(object sender, RoutedEventArgs e)
		{
			CheckBoxJoueur2.IsChecked = true;
			CheckBoxJoueur1.IsChecked = false;
		}
	}
}
