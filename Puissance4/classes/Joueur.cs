using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Puissance4.classes
{
	[Serializable]
	public class Joueur : INotifyPropertyChanged
	{
		#region Variables Membres
		private string _nom;
		private string _prenom;
		private string _image;
		private string _motDePasse;
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Constructeurs
		public Joueur() : this("","","","")
		{

		}
		public Joueur(string newNom,string newPrenom,string newImage,string newMotDePAsse)
		{
			Nom = newNom;
			Prenom = newPrenom;
			Image = newImage;
			MotDePasse = newMotDePAsse;
		}
		#endregion

		#region Proprietes
		public string Nom
		{
			set
			{
				if (value != null)
					_nom = value;
				OnPropertyChanged();
			}
			get
			{
				return _nom;
			}
		}
		public string Prenom
		{
			set
			{
				if (value != null)
					_prenom = value;
				OnPropertyChanged();
			}
			get
			{
				return _prenom;
			}
		}
		public string Image
		{
			set
			{
				if (value != null)
					_image = value;
				OnPropertyChanged();
			}
			get
			{
				return _image;
			}
		}
		public string MotDePasse
		{
			set
			{
				if (value != null)
					_motDePasse = value;
				OnPropertyChanged();
			}
			get
			{
				return _motDePasse;
			}
		}



		#endregion

		#region Méthodes

		#endregion

		#region Intefaces
		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
		#endregion

		#region Surcharge Operateurs
		public override string ToString()
		{
			return string.Format("Nom: {0} Prénom: {1}",Nom,Prenom);
		}
		#endregion
	}
}
