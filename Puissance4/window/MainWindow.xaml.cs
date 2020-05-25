using Microsoft.Win32;
using Puissance4.classes;
using Puissance4.window;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace Puissance4
{
	public partial class MainWindow : Window,INotifyPropertyChanged
	{
		#region Variables Membres
		//A modifier avec le registry
		private string _defaultPath = @"C:\Users\Vincent\OneDrive - Enseignement de la Province de Liège\Cours\B2\C#\Puissance4\labo-phase-3-Raiku27\Puissance4\data\images\";
		private string _pathJoueur = @"C:\Users\Vincent\OneDrive - Enseignement de la Province de Liège\Cours\B2\C#\Puissance4\labo-phase-3-Raiku27\Puissance4\data\joueurs\";
		private string _pathParties = @"C:\Users\Vincent\OneDrive - Enseignement de la Province de Liège\Cours\B2\C#\Puissance4\labo-phase-3-Raiku27\Puissance4\data\parties\";
		private string _pathImages;
		private string _keyPuissance4 = "Puissance4";
		private string _keyPath = "Path";
		public Joueur _joueur1 = null;
		public Joueur _joueur2 = null;
		private ImageSource _image1;
		private ImageSource _image2;
		private SolidColorBrush _couleurJoueur1 = null;
		private SolidColorBrush _couleurJoueur2 = null;
		private Partie _partie = null;
		private int _joueurActuel = 1;
		private Boolean isPartieGagne = false;
		private static int nbrParties = 0;
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Variables Statiques

		#endregion

		#region Constructeurs
		public MainWindow()
		{
			InitializeComponent();
			this.SourceInitialized += Window_SourceInitialized;

			DataContext = this;

			#region INIT
			//Recuperer la path des images grace a la registry
			//Teste Puissance4
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(_keyPuissance4);
			if (registryKey == null)
			{
				//Si puisance4 existe pas on le cree
				RegistryKey ret = Registry.CurrentUser.CreateSubKey(_keyPuissance4);
				if (ret == null)
					Console.WriteLine("Erreur: Registry.CurrentUser.CreateSubKey(_keyPuissance4)");
				else
					registryKey = ret;
			}
			//Tester Path de Puissance4
			String res = (String)registryKey.GetValue(_keyPath);
			if (res == null)
			{
				//Si path existe pas on le cree
				RegistryKey ret = Registry.CurrentUser.CreateSubKey(_keyPuissance4);
				if (ret == null)
					Console.WriteLine("Erreur: Registry.CurrentUser.CreateSubKey(_keyPuissance4)");
				else
					registryKey = ret;
				registryKey.SetValue(_keyPath, _defaultPath);
			}
			res = (String)registryKey.GetValue(_keyPath);
			//Charger la cle
			_pathImages = res;
			//Tester si la path existe sinon le cree
			if (!Directory.Exists(_pathImages))
			{
				try
				{
					Directory.CreateDirectory(_pathImages);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					MessageBox.Show(e.Message, "Puissance4 - Erreur");
				}
			}
			#endregion

			//Puissance4
			
			nbrParties = Directory.GetFiles(PathParties, "*", SearchOption.AllDirectories).Length - 1;

			//Stats
			

		}
		#endregion

		#region Proprietes
		public Joueur Joueur1
		{
			set
			{
				if (_joueur1 != value && value != null)
				{
					_joueur1 = value;
					Image1 = (ImageSource)new ImageSourceConverter().ConvertFromString(PathImage + value.Image);
					OnPropertyChanged();
				}
			}
			get
			{
				return _joueur1;
			}
		}
		public Joueur Joueur2
		{
			set
			{
				if (_joueur2 != value && value != null)
				{
					_joueur2 = value;
					Image2 = (ImageSource)new ImageSourceConverter().ConvertFromString(PathImage + value.Image);
					OnPropertyChanged();
				}
			}
			get
			{
				return _joueur2;
			}
		}
		public string PathImage
		{
			get
			{
				return _pathImages;
			}
		}
		public string PathJoueur
		{
			get
			{
				return _pathJoueur;
			}
		}
		public string PathParties
		{
			get
			{
				return _pathParties;
			}
		}
		public ImageSource Image1
		{
			set
			{
				_image1 = value;
				OnPropertyChanged();
			}
			get
			{
				return _image1;
			}
		}
		public ImageSource Image2
		{
			set
			{
				_image2 = value;
				OnPropertyChanged();
			}
			get
			{
				return _image2;
			}
		}
		public SolidColorBrush CouleurJoueur1
		{
			set
			{
				if (value != _couleurJoueur1 && value != null)
				{
					_couleurJoueur1 = value;
					OnPropertyChanged();
				}
			}
			get
			{
				return _couleurJoueur1;
			}
		}
		public SolidColorBrush CouleurJoueur2
		{
			set
			{
				if (value != _couleurJoueur2 && value != null)
				{
					_couleurJoueur2 = value;
					OnPropertyChanged();
				}
			}
			get
			{
				return _couleurJoueur2;
			}
		}


		#endregion

		#region Méthodes

		#region Menu
		private void BouttonCreeUnCompte_Click(object sender, RoutedEventArgs e)
		{
			CreeUnCompteWindow creeUnCompteWindow = new CreeUnCompteWindow(this);
			creeUnCompteWindow.ShowDialog();
		}
		private void BouttonSeConnecter_Click(object sender, RoutedEventArgs e)
		{
			SeConnecterWindow seConnecterWindow = new SeConnecterWindow(this);
			seConnecterWindow.ShowDialog();
		}
		private void BouttonJouer_Click(object sender, RoutedEventArgs e)
		{
			if (Joueur1 == null || Joueur2 == null)
				MessageBox.Show("Impossible de débuter une partie, 2 joueurs sont nécessaire!","Puissance4 - Erreur");
			else if(Joueur1 != null && Joueur2 != null)
			{
				DebutPartie();
			}
		}
		private void BouttonStatistiques_Click(object sender, RoutedEventArgs e)
		{
			StatsWindow statsWindow = new StatsWindow(this);
			statsWindow.ShowDialog();
		}
		private void BouttonQuitter_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
		#endregion

		#region Puissance4

		#region Boutton
		private void Boutton00_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(0, 0);
		}
		private void Boutton01_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(0, 1);
		}
		private void Boutton02_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(0, 2);
		}
		private void Boutton03_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(0, 3);
		}
		private void Boutton04_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(0, 4);
		}
		private void Boutton05_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(0, 5);
		}
		private void Boutton06_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(0, 6);
		}
		private void Boutton10_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(1, 0);
		}
		private void Boutton11_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(1, 1);
		}
		private void Boutton12_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(1, 02);
		}
		private void Boutton13_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(1, 3);
		}
		private void Boutton14_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(1, 4);
		}
		private void Boutton15_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(1, 5);
		}
		private void Boutton16_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(1, 6);
		}
		private void Boutton20_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(2, 0);
		}
		private void Boutton21_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(2, 1);
		}
		private void Boutton22_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(2, 2);
		}
		private void Boutton23_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(2, 3);
		}
		private void Boutton24_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(2, 4);
		}
		private void Boutton25_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(2, 5);
		}
		private void Boutton26_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(2, 6);
		}
		private void Boutton30_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(3, 0);
		}
		private void Boutton31_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(3, 1);
		}
		private void Boutton32_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(3, 2);
		}
		private void Boutton33_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(3, 3);
		}
		private void Boutton34_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(3, 4);
		}
		private void Boutton35_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(3, 5);
		}
		private void Boutton36_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(3, 6);
		}
		private void Boutton40_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(4, 0);
		}
		private void Boutton41_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(4, 1);
		}
		private void Boutton42_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(4, 2);
		}
		private void Boutton43_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(4, 3);
		}
		private void Boutton44_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(4, 4);
		}
		private void Boutton45_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(4, 5);
		}
		private void Boutton46_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(4, 6);
		}
		private void Boutton50_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(5, 0);
		}
		private void Boutton51_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(5, 1);
		}
		private void Boutton52_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(5, 2);
		}
		private void Boutton53_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(5, 3);
		}
		private void Boutton54_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(5, 4);
		}
		private void Boutton55_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(5, 5);
		}
		private void Boutton56_Click(object sender, RoutedEventArgs e)
		{
			ClickBoutton(5, 6);
		}
		#endregion

		public void ClickBoutton(int ligne, int colonne)
		{
			//Tester si le cursor du boutton == hand sinon on prends pas le click
			if (isBouttonCursorNo(ligne,colonne)) return;
			if(isPartieGagne) return;

			for (int i = ligne; i < _partie.Tableau.GetLength(0); i++)
			{
				if (_partie.Tableau[i,colonne] == -1)
				{
					ligne = i;
				}
			}

			if (_joueurActuel == 1)
			{
				setBouttonForeground(ligne,colonne,CouleurJoueur1);
				_partie.Tableau[ligne, colonne] = 1;
			}
			else
			{
				setBouttonForeground(ligne, colonne, CouleurJoueur2);
				_partie.Tableau[ligne, colonne] = 2;
			}

			setBouttonCursor(ligne, colonne, Cursors.No);

			if(_partie.FirstColonne == -1 || _partie.FirstLigne == -1)
			{
				_partie.FirstLigne = ligne;
				_partie.FirstColonne = colonne;
			}

			if (TestGagner(ligne, colonne,_joueurActuel))
			{
				if (_joueurActuel == 1)
					FinPartie(1);
				else
					FinPartie(2);
				return;
			}

			if(_joueurActuel == 1)
			{
				GridJoueur1.Opacity = 0.5;
				GridJoueur2.Opacity = 1;
				_joueurActuel = 2;
			}
			else
			{
				GridJoueur2.Opacity = 0.5;
				GridJoueur1.Opacity = 1;
				_joueurActuel = 1;
			}

			if (isTableauFull())
			{
				//Le tableau est rempli,la partie s'arrete pas de gagnant
				FinPartie(-1);
				return;
			}
		}

		private void BouttonRetourMenu_Click(object sender, RoutedEventArgs e)
		{
			//Reset les boutton
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 7; j++)
				{
					setBouttonCursor(i, j, Cursors.Hand);
					setBouttonForeground(i, j, Brushes.White);
				}
			}

			isPartieGagne = false;
			_partie.Gagant = _joueurActuel;
			_partie.Joueur1 = Joueur1;
			_partie.Joueur2 = Joueur2;
			_partie.CouleurJoueur1 = CouleurJoueur1.ToString();
			_partie.CouleurJoueur2 = CouleurJoueur2.ToString();

			_partie.prepareSerialise();

			//Enregistrer la partie dans un fichier
			try
			{
				nbrParties++;
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(Partie));
				FileStream fileStream = File.Create(PathParties + nbrParties.ToString() + ".xml");
				xmlSerializer.Serialize(fileStream, _partie);
				fileStream.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Puissance4 - Erreur");
			}
			LabelInfo.Visibility = Visibility.Hidden;
			BouttonRetourMenu.Visibility = Visibility.Hidden;
			GridPuissance4.Visibility = Visibility.Hidden;
			GridMenu.Visibility = Visibility.Visible;
		}

		private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			CouleurJoueur1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString(((ComboBoxItem)ComboBox1.SelectedItem).Content.ToString()));
			if(_partie != null)
			{
				//Changer la couleur des bouttons
				for (int i = 0; i < 6; i++)
				{
					for (int j = 0; j < 7; j++)
					{
						if (_partie.Tableau[i, j] == 1)
							setBouttonForeground(i, j, CouleurJoueur1);
					}
				}
			}
		}

		private void ComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			CouleurJoueur2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString(((ComboBoxItem)ComboBox2.SelectedItem).Content.ToString()));
			if(_partie != null)
			{
				//Changer la couleur des bouttons
				for (int i = 0; i < 6; i++)
				{
					for (int j = 0; j < 7; j++)
					{
						if (_partie.Tableau[i, j] == 2)
							setBouttonForeground(i, j, CouleurJoueur2);
					}
				}
			}
		}

		public void DebutPartie()
		{
			GridMenu.Visibility = Visibility.Hidden;
			GridPuissance4.Visibility = Visibility.Visible;
			_partie = new Partie();
			_joueurActuel = 1;
			GridJoueur2.Opacity = 0.5;
			GridJoueur1.Opacity = 1;
		}

		public void FinPartie(int gagnant)
		{
			if (gagnant == -1)
			{
				LabelInfo.Content = "Égalité!";
				BouttonRetourMenu.Background = Brushes.Red;
			}
			else if (gagnant == 1)
			{
				LabelInfo.Content = Joueur1.Prenom + " a gagner!";
				BouttonRetourMenu.Background = Brushes.Green;
			}
			else
			{
				LabelInfo.Content = Joueur2.Prenom + " a gagner!";
				BouttonRetourMenu.Background = Brushes.Green;
			}
			isPartieGagne = true;
			//Enlever le cursor hand des boutton
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 7; j++)
				{
					setBouttonCursor(i, j, Cursors.Arrow);
				}
			}
			LabelInfo.Visibility = Visibility.Visible;
			BouttonRetourMenu.Visibility = Visibility.Visible;
		}

		public bool isTableauFull()
		{
			int count = 0;
			for (int i = 0; i < _partie.Tableau.GetLength(0); i++)
			{
				for (int j = 0; j < _partie.Tableau.GetLength(1); j++)
				{
					if (_partie.Tableau[i, j] == -1)
						count++;
				}
			}
			if (count == 0)
				return true;
			return false;
		}

		public Boolean isBouttonCursorNo(int ligne,int colonne)
		{
			if(ligne == 0 && colonne == 0){if(Boutton00.Cursor.Equals(Cursors.No)){return true;}}
			else if (ligne == 0 && colonne == 1) { if (Boutton01.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 0 && colonne == 2) { if (Boutton02.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 0 && colonne == 3) { if (Boutton03.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 0 && colonne == 4) { if (Boutton04.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 0 && colonne == 5) { if (Boutton05.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 0 && colonne == 6) { if (Boutton06.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 1 && colonne == 0) { if (Boutton10.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 1 && colonne == 1) { if (Boutton11.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 1 && colonne == 2) { if (Boutton12.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 1 && colonne == 3) { if (Boutton13.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 1 && colonne == 4) { if (Boutton14.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 1 && colonne == 5) { if (Boutton15.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 1 && colonne == 6) { if (Boutton16.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 2 && colonne == 0) { if (Boutton20.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 2 && colonne == 1) { if (Boutton21.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 2 && colonne == 2) { if (Boutton22.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 2 && colonne == 3) { if (Boutton23.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 2 && colonne == 4) { if (Boutton24.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 2 && colonne == 5) { if (Boutton25.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 2 && colonne == 6) { if (Boutton26.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 3 && colonne == 0) { if (Boutton30.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 3 && colonne == 1) { if (Boutton31.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 3 && colonne == 2) { if (Boutton32.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 3 && colonne == 3) { if (Boutton33.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 3 && colonne == 4) { if (Boutton34.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 3 && colonne == 5) { if (Boutton35.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 3 && colonne == 6) { if (Boutton36.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 4 && colonne == 0) { if (Boutton40.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 4 && colonne == 1) { if (Boutton41.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 4 && colonne == 2) { if (Boutton42.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 4 && colonne == 3) { if (Boutton43.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 4 && colonne == 4) { if (Boutton44.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 4 && colonne == 5) { if (Boutton45.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 4 && colonne == 6) { if (Boutton46.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 5 && colonne == 0) { if (Boutton50.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 5 && colonne == 1) { if (Boutton51.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 5 && colonne == 2) { if (Boutton52.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 5 && colonne == 3) { if (Boutton53.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 5 && colonne == 4) { if (Boutton54.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 5 && colonne == 5) { if (Boutton55.Cursor.Equals(Cursors.No)) { return true; } }
			else if (ligne == 5 && colonne == 6) { if (Boutton56.Cursor.Equals(Cursors.No)) { return true; } }
			return false;
		}

		public void setBouttonForeground(int ligne,int colonne,SolidColorBrush couleur)
		{
			if (ligne == 0 && colonne == 0) { Boutton00.Foreground = couleur; }
			else if (ligne == 0 && colonne == 1) { Boutton01.Foreground = couleur; }
			else if (ligne == 0 && colonne == 2) { Boutton02.Foreground = couleur; }
			else if (ligne == 0 && colonne == 3) { Boutton03.Foreground = couleur; }
			else if (ligne == 0 && colonne == 4) { Boutton04.Foreground = couleur; }
			else if (ligne == 0 && colonne == 5) { Boutton05.Foreground = couleur; }
			else if (ligne == 0 && colonne == 6) { Boutton06.Foreground = couleur; }
			else if (ligne == 1 && colonne == 0) { Boutton10.Foreground = couleur; }
			else if (ligne == 1 && colonne == 1) { Boutton11.Foreground = couleur; }
			else if (ligne == 1 && colonne == 2) { Boutton12.Foreground = couleur; }
			else if (ligne == 1 && colonne == 3) { Boutton13.Foreground = couleur; }
			else if (ligne == 1 && colonne == 4) { Boutton14.Foreground = couleur; }
			else if (ligne == 1 && colonne == 5) { Boutton15.Foreground = couleur; }
			else if (ligne == 1 && colonne == 6) { Boutton16.Foreground = couleur; }
			else if (ligne == 2 && colonne == 0) { Boutton20.Foreground = couleur; }
			else if (ligne == 2 && colonne == 1) { Boutton21.Foreground = couleur; }
			else if (ligne == 2 && colonne == 2) { Boutton22.Foreground = couleur; }
			else if (ligne == 2 && colonne == 3) { Boutton23.Foreground = couleur; }
			else if (ligne == 2 && colonne == 4) { Boutton24.Foreground = couleur; }
			else if (ligne == 2 && colonne == 5) { Boutton25.Foreground = couleur; }
			else if (ligne == 2 && colonne == 6) { Boutton26.Foreground = couleur; }
			else if (ligne == 3 && colonne == 0) { Boutton30.Foreground = couleur; }
			else if (ligne == 3 && colonne == 1) { Boutton31.Foreground = couleur; }
			else if (ligne == 3 && colonne == 2) { Boutton32.Foreground = couleur; }
			else if (ligne == 3 && colonne == 3) { Boutton33.Foreground = couleur; }
			else if (ligne == 3 && colonne == 4) { Boutton34.Foreground = couleur; }
			else if (ligne == 3 && colonne == 5) { Boutton35.Foreground = couleur; }
			else if (ligne == 3 && colonne == 6) { Boutton36.Foreground = couleur; }
			else if (ligne == 4 && colonne == 0) { Boutton40.Foreground = couleur; }
			else if (ligne == 4 && colonne == 1) { Boutton41.Foreground = couleur; }
			else if (ligne == 4 && colonne == 2) { Boutton42.Foreground = couleur; }
			else if (ligne == 4 && colonne == 3) { Boutton43.Foreground = couleur; }
			else if (ligne == 4 && colonne == 4) { Boutton44.Foreground = couleur; }
			else if (ligne == 4 && colonne == 5) { Boutton45.Foreground = couleur; }
			else if (ligne == 4 && colonne == 6) { Boutton46.Foreground = couleur; }
			else if (ligne == 5 && colonne == 0) { Boutton50.Foreground = couleur; }
			else if (ligne == 5 && colonne == 1) { Boutton51.Foreground = couleur; }
			else if (ligne == 5 && colonne == 2) { Boutton52.Foreground = couleur; }
			else if (ligne == 5 && colonne == 3) { Boutton53.Foreground = couleur; }
			else if (ligne == 5 && colonne == 4) { Boutton54.Foreground = couleur; }
			else if (ligne == 5 && colonne == 5) { Boutton55.Foreground = couleur; }
			else if (ligne == 5 && colonne == 6) { Boutton56.Foreground = couleur; }
		}

		public void setBouttonCursor(int ligne,int colonne, Cursor cursor)
		{
			if (ligne == 0 && colonne == 0) { Boutton00.Cursor = cursor; }
			else if (ligne == 0 && colonne == 1) { Boutton01.Cursor = cursor; }
			else if (ligne == 0 && colonne == 2) { Boutton02.Cursor = cursor; }
			else if (ligne == 0 && colonne == 3) { Boutton03.Cursor = cursor; }
			else if (ligne == 0 && colonne == 4) { Boutton04.Cursor = cursor; }
			else if (ligne == 0 && colonne == 5) { Boutton05.Cursor = cursor; }
			else if (ligne == 0 && colonne == 6) { Boutton06.Cursor = cursor; }
			else if (ligne == 1 && colonne == 0) { Boutton10.Cursor = cursor; }
			else if (ligne == 1 && colonne == 1) { Boutton11.Cursor = cursor; }
			else if (ligne == 1 && colonne == 2) { Boutton12.Cursor = cursor; }
			else if (ligne == 1 && colonne == 3) { Boutton13.Cursor = cursor; }
			else if (ligne == 1 && colonne == 4) { Boutton14.Cursor = cursor; }
			else if (ligne == 1 && colonne == 5) { Boutton15.Cursor = cursor; }
			else if (ligne == 1 && colonne == 6) { Boutton16.Cursor = cursor; }
			else if (ligne == 2 && colonne == 0) { Boutton20.Cursor = cursor; }
			else if (ligne == 2 && colonne == 1) { Boutton21.Cursor = cursor; }
			else if (ligne == 2 && colonne == 2) { Boutton22.Cursor = cursor; }
			else if (ligne == 2 && colonne == 3) { Boutton23.Cursor = cursor; }
			else if (ligne == 2 && colonne == 4) { Boutton24.Cursor = cursor; }
			else if (ligne == 2 && colonne == 5) { Boutton25.Cursor = cursor; }
			else if (ligne == 2 && colonne == 6) { Boutton26.Cursor = cursor; }
			else if (ligne == 3 && colonne == 0) { Boutton30.Cursor = cursor; }
			else if (ligne == 3 && colonne == 1) { Boutton31.Cursor = cursor; }
			else if (ligne == 3 && colonne == 2) { Boutton32.Cursor = cursor; }
			else if (ligne == 3 && colonne == 3) { Boutton33.Cursor = cursor; }
			else if (ligne == 3 && colonne == 4) { Boutton34.Cursor = cursor; }
			else if (ligne == 3 && colonne == 5) { Boutton35.Cursor = cursor; }
			else if (ligne == 3 && colonne == 6) { Boutton36.Cursor = cursor; }
			else if (ligne == 4 && colonne == 0) { Boutton40.Cursor = cursor; }
			else if (ligne == 4 && colonne == 1) { Boutton41.Cursor = cursor; }
			else if (ligne == 4 && colonne == 2) { Boutton42.Cursor = cursor; }
			else if (ligne == 4 && colonne == 3) { Boutton43.Cursor = cursor; }
			else if (ligne == 4 && colonne == 4) { Boutton44.Cursor = cursor; }
			else if (ligne == 4 && colonne == 5) { Boutton45.Cursor = cursor; }
			else if (ligne == 4 && colonne == 6) { Boutton46.Cursor = cursor; }
			else if (ligne == 5 && colonne == 0) { Boutton50.Cursor = cursor; }
			else if (ligne == 5 && colonne == 1) { Boutton51.Cursor = cursor; }
			else if (ligne == 5 && colonne == 2) { Boutton52.Cursor = cursor; }
			else if (ligne == 5 && colonne == 3) { Boutton53.Cursor = cursor; }
			else if (ligne == 5 && colonne == 4) { Boutton54.Cursor = cursor; }
			else if (ligne == 5 && colonne == 5) { Boutton55.Cursor = cursor; }
			else if (ligne == 5 && colonne == 6) { Boutton56.Cursor = cursor; }
		}

		public bool TestGagner(int ligne,int colonne,int numJ)
		{
			int[,] tab = _partie.Tableau;
			//6 lignes 7 colonnes

			//Horizontal
			//OXXX
			//XOXX
			//XXOX
			//XXXO
			if (colonne + 3 <= 6 && tab[ligne, colonne + 1] == numJ && tab[ligne, colonne + 2] == numJ && tab[ligne, colonne + 3] == numJ) { return true; }
			else if (colonne - 1 >= 0 && colonne + 2 <= 6 && tab[ligne, colonne - 1] == numJ && tab[ligne, colonne + 1] == numJ && tab[ligne, colonne + 2] == numJ) { return true; }
			else if (colonne - 2 >= 0 && colonne + 1 <= 6 && tab[ligne, colonne - 2] == numJ && tab[ligne, colonne - 1] == numJ && tab[ligne, colonne + 1] == numJ) { return true; }
			else if (colonne - 3 >= 0 && tab[ligne, colonne - 3] == numJ && tab[ligne, colonne - 2] == numJ && tab[ligne, colonne - 1] == numJ) {  return true; }

			//Vertical
			//OXXX
			//XOXX
			//XXOX
			//XXXO
			if(ligne + 3 <= 5 && tab[ligne + 1,colonne] == numJ && tab[ligne + 2, colonne] == numJ && tab[ligne + 3, colonne] == numJ) {  return true; }
			else if (ligne - 1 >=0 && ligne + 2 <= 5 && tab[ligne - 1, colonne] == numJ && tab[ligne + 1, colonne] == numJ && tab[ligne + 2, colonne] == numJ) {  return true; }
			else if (ligne - 2 >= 0 && ligne + 1 <= 5 && tab[ligne - 2, colonne] == numJ && tab[ligne - 1, colonne] == numJ && tab[ligne + 1, colonne] == numJ) { return true; }
			else if (ligne - 3 >= 0 && tab[ligne - 3, colonne] == numJ && tab[ligne - 2, colonne] == numJ && tab[ligne - 1, colonne] == numJ) {  return true; }

			//Diagonal1
			//0    X    X    X 				
			//   X    O    X    X
			//      X    X    O    X
			//         X    X    X    O
			if(ligne + 3 <= 5 && colonne + 3 <= 6 && tab[ligne + 1,colonne + 1] == numJ && tab[ligne + 2, colonne + 2] == numJ && tab[ligne + 3, colonne + 3] == numJ) { return true; }
			else if (ligne - 1 >= 0 && ligne + 2 <= 5 && colonne - 1 >= 0 && colonne + 2 <= 6 && tab[ligne - 1, colonne - 1] == numJ && tab[ligne + 1, colonne + 1] == numJ && tab[ligne + 2, colonne + 2] == numJ) {  return true; }
			else if (ligne - 2 >= 0 && ligne + 1 <= 5 && colonne - 2 >= 0 && colonne + 1 <= 6 && tab[ligne - 2, colonne - 2] == numJ && tab[ligne - 1, colonne - 1] == numJ && tab[ligne + 1, colonne + 1] == numJ) {  return true; }
			else if (ligne - 3 >= 0 && colonne - 3 >= 0 && tab[ligne - 3, colonne - 3] == numJ && tab[ligne - 2, colonne - 2] == numJ && tab[ligne - 1, colonne - 1] == numJ) {  return true; }

			//Diagonal2
			//            X     X     X     0
			//        X     X     O     X
			//    X     O     X     X
			//0     X     X     X
			if (ligne - 3 >= 0 && colonne + 3 <= 6 && tab[ligne - 1, colonne + 1] == numJ && tab[ligne - 2, colonne + 2] == numJ && tab[ligne - 3, colonne + 3] == numJ) {  return true; }
			else if (ligne - 2 >= 0 && ligne + 1 <= 5 && colonne - 1 >= 0 && colonne + 2 <= 6 && tab[ligne + 1, colonne - 1] == numJ && tab[ligne - 1, colonne + 1] == numJ && tab[ligne - 2, colonne + 2] == numJ) {  return true; }
			else if (ligne - 1 >= 0 && ligne + 2 <= 5 && colonne - 2 >= 0 && colonne + 1 <= 6 && tab[ligne + 2, colonne - 2] == numJ && tab[ligne + 1, colonne + 1] == numJ && tab[ligne - 1, colonne + 1] == numJ) {  return true; }
			else if (ligne + 3 <= 5 && colonne - 3 >= 0 && tab[ligne + 3, colonne - 3] == numJ && tab[ligne + 2, colonne - 2] == numJ && tab[ligne + 1, colonne - 1] == numJ) { return true; }

			return false;
		}

		#endregion

		#region Stats
		#endregion

		#endregion

		#region Intefaces
		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		#endregion

		#region ResizeMode
		//https://stackoverflow.com/questions/386484/how-can-i-only-allow-uniform-resizing-in-a-wpf-window/696266#696266
		private static double _aspectRatio = 1.7777; //16/9
		private bool? _adjustingHeight = null;

		[StructLayout(LayoutKind.Sequential)]
		internal struct WINDOWPOS
		{
			public IntPtr hwnd;
			public IntPtr hwndInsertAfter;
			public int x;
			public int y;
			public int cx;
			public int cy;
			public int flags;
		}

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetCursorPos(ref Win32Point pt);

		[StructLayout(LayoutKind.Sequential)]
		internal struct Win32Point
		{
			public Int32 X;
			public Int32 Y;
		};

		internal enum SWP
		{
			NOMOVE = 0x0002
		}
		internal enum WM
		{
			WINDOWPOSCHANGING = 0x0046,
			EXITSIZEMOVE = 0x0232,
		}

		public static Point GetMousePosition() // mouse position relative to screen
		{
			Win32Point w32Mouse = new Win32Point();
			GetCursorPos(ref w32Mouse);
			return new Point(w32Mouse.X, w32Mouse.Y);
		}


		private void Window_SourceInitialized(object sender, EventArgs ea)
		{
			HwndSource hwndSource = (HwndSource)HwndSource.FromVisual((Window)sender);
			hwndSource.AddHook(DragHook);

			_aspectRatio = this.Width / this.Height;
		}

		private IntPtr DragHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			switch ((WM)msg)
			{
				case WM.WINDOWPOSCHANGING:
					{
						WINDOWPOS pos = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));

						if ((pos.flags & (int)SWP.NOMOVE) != 0)
							return IntPtr.Zero;

						Window wnd = (Window)HwndSource.FromHwnd(hwnd).RootVisual;
						if (wnd == null)
							return IntPtr.Zero;

						// determine what dimension is changed by detecting the mouse position relative to the 
						// window bounds. if gripped in the corner, either will work.
						if (!_adjustingHeight.HasValue)
						{
							Point p = GetMousePosition();

							double diffWidth = Math.Min(Math.Abs(p.X - pos.x), Math.Abs(p.X - pos.x - pos.cx));
							double diffHeight = Math.Min(Math.Abs(p.Y - pos.y), Math.Abs(p.Y - pos.y - pos.cy));

							_adjustingHeight = diffHeight > diffWidth;
						}

						if (_adjustingHeight.Value)
							pos.cy = (int)(pos.cx / _aspectRatio); // adjusting height to width change
						else
							pos.cx = (int)(pos.cy * _aspectRatio); // adjusting width to heigth change

						Marshal.StructureToPtr(pos, lParam, true);
						handled = true;
					}
					break;
				case WM.EXITSIZEMOVE:
					_adjustingHeight = null; // reset adjustment dimension and detect again next time window is resized
					break;
			}
			return IntPtr.Zero;
		}


		#endregion

		
	}
}
