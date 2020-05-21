using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Puissance4.classes
{
	[Serializable]
	[XmlRoot(ElementName = "Partie")]
	public class Partie
	{
		#region Variables Membres
		private Joueur _joueur1 = null;
		private Joueur _joueur2 = null;
		private String _couleurJoueur1 = null;
		private String _couleurJoueur2 = null;
		private int _gagnant = -1;
		private int[,] _tableau = new int[6,7];
		private int _firstLigne = -1;
		private int _firstColonne = -1;
		//On ne peus pas serialiser un int[,] mais List<int> oui
		[XmlArrayItem("Tableau")]
		public List<int> _data = new List<int>();
		#endregion

		#region Constructeurs
		public Partie()
		{
			for (int i = 0; i < _tableau.GetLength(0); i++)
			{
				for (int j = 0; j < _tableau.GetLength(1); j++)
				{
					_tableau[i, j] = -1;
				}
			}
		}

		#endregion

		#region Propriétés
		public Joueur Joueur1
		{
			set
			{
				if (value != null)
					_joueur1 = value;
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
				if (value != null)
					_joueur2 = value;
			}
			get
			{
				return _joueur2;
			}
		}

		public string CouleurJoueur1
		{
			set
			{
				if (value != null)
					_couleurJoueur1 = value;
			}
			get
			{
				return _couleurJoueur1;
			}
		}
		public string CouleurJoueur2
		{
			set
			{
				if (value != null)
					_couleurJoueur2 = value;
			}
			get
			{
				return _couleurJoueur2;
			}
		}
		public int Gagant
		{
			set
			{
				_gagnant = value;
			}
			get
			{
				return _gagnant;
			}
		}
		public int[,] Tableau
		{
			get
			{
				return _tableau;
			}
		}
		public int FirstLigne
		{
			set
			{
				_firstLigne = value;
			}
			get
			{
				return _firstLigne;
			}
		}
		public int FirstColonne
		{
			set
			{
				_firstColonne = value;
			}
			get
			{
				return _firstColonne;
			}
		}
		#endregion

		#region Méthodes
		public void prepareSerialise()
		{
			for (int i = 0; i < _tableau.GetLength(0); i++)
			{
				for (int j = 0; j < _tableau.GetLength(1); j++)
				{
					_data.Add(_tableau[i, j]);
				}
			}
		}

		public void afterDeserialise()
		{
			for (int i = 0; i < _tableau.GetLength(0); i++)
			{
				for (int j = 0; j < _tableau.GetLength(1); j++)
				{
					_tableau[i, j] = _data[i*j];
				}
			}
		}
		#endregion

		#region Interfaces

		#endregion

		#region Surcharge Opérateurs

		#endregion
	}
}
