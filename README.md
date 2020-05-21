# Labo Partie 3 C# Vincent GERARD 2226

# Proposition du projet:
## Puissance 4
<p>
J'aimerais réaliser le jeu Puissance 4. Je l'ai déjà fait en console l'année passée mais ne connaissant pas encore le WPF, je n'ai pas réussi à faire une interface correcte. Le jeu aurait une fonctionnalité de statistique pour voir quelles sont les meilleurs joueurs et établir un classement.
</p>


### Les Données
  0. Une classe générique dont toutes les autres classes hériteront pour faire load et save.
  1. Une classe Joueur qui contient le nom, prénom, age, date de derniere connexion et une image de profile.
  2. Une classe Partie qui contient 2 joueurs, le nombre de tours et le score.
  3. Une classe Statistique qui va stocker différents informations tels que:
  * La case la plus jouée au premier tour
  * La case de la grille le plus souvent remplie
  * La direction qui gagne le plus souvent (Horizontal, Vertical et Diagonal)
  * Le nombre de tours moyen pour gagner
  
### Persistance des Donnés
<p> 
La classe Joueur sera stocké en XML car elle est plus facilement visualisable et les classes Partie et Statistique seront stocké en binaire.
</p>

### Interface Utilisateur
1. Login
2. Fenêtre de Jeu
3. Fenêtre de Statistique Globale (tous les joueurs)
4. Fenêtre de Statistique Personnelle (chaque joueur)
5. Fenêtre des Parties Joués (chaque joueur)

### Données de la Registry
<p>
Sauvegarder les liens des images de profile des joueurs.
</p>

### Info Complémentaires
<p>
Si le projet n’est pas assez long, une notion de tournoi peut être ajoutée.
<p/>

### Commentaie du Prof
<p>
Registry: Stocker le liens vers le dossier contenant les images
</p>

