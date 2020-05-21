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
using Path = System.IO.Path;

namespace Puissance4.window
{
	public partial class CreeUnCompteWindow : Window
	{
		private List<string> BufferImages = new List<string>();
		private int selectedImage = 0;

		public CreeUnCompteWindow(MainWindow newOwner)
		{
			InitializeComponent();
			Owner = newOwner;
			//Charger les images dans la liste
			string[] files = Directory.GetFiles(((MainWindow)Owner).PathImage);
			if (files.Length == 0)
			{
				Console.WriteLine("Erreur: Aucunes images trouver au chemin {0}", ((MainWindow)Owner).PathImage);
				Application.Current.Shutdown();
			}
			foreach (string filename in files)
			{
				string file = Path.GetFileName(filename);
				BufferImages.Add(file);
			}
			ImageLogo.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(((MainWindow)Owner).PathImage + BufferImages[selectedImage]);
		}

        private void BouttonAnnuler_Click(object sender, RoutedEventArgs e)
        {
			Close();
        }

        private void BouttonOk_Click(object sender, RoutedEventArgs e)
        {
			//Cree le joueur
			if(TextBoxNom.Text.Length == 0 || TextBoxPrenom.Text.Length == 0 || TextBoxMotDePasse.Text.Length == 0)
			{
				MessageBox.Show("Impossible de créer un compte, une case est vide!","Puissance4 - Erreur");
				return;
			}
			//Procedure de cree le compte avec un fichier
			Joueur joueur = new Joueur(TextBoxNom.Text, TextBoxPrenom.Text, BufferImages[selectedImage], TextBoxMotDePasse.Text);
			
			if(File.Exists(((MainWindow)Owner).PathJoueur + joueur.Nom + joueur.Prenom + ".xml"))
			{
				MessageBox.Show("Un joueur existe deja avec ce nom et prenom!","Puissance4 - Erreur");
				return;
			}
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(Joueur));
				FileStream fileStream = File.Create(((MainWindow)Owner).PathJoueur + joueur.Nom + joueur.Prenom + ".xml");
				xmlSerializer.Serialize(fileStream,joueur);
				fileStream.Close();
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message,"Puissance4 - Erreur");
			}
        }

		private void BouttonPrecedent_Click(object sender, RoutedEventArgs e)
		{
			if (selectedImage == 0)
				selectedImage = BufferImages.Count - 1;
			else
				selectedImage--;
			ImageLogo.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(((MainWindow)Owner).PathImage + BufferImages[selectedImage]);
		}

		private void BouttonSuivant_Click(object sender, RoutedEventArgs e)
		{
			if (selectedImage == BufferImages.Count - 1)
				selectedImage = 0;
			else
				selectedImage++;
			ImageLogo.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(((MainWindow)Owner).PathImage + BufferImages[selectedImage]);
		}
	}
}
