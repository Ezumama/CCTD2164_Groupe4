using UnityEngine;
using System.Collections.Generic;
using System; 

public class PathManager : MonoBehaviour
{
    public static PathManager instance;

    [Serializable]
    public class SpawnPointData
    {
        [Tooltip("Point de départ de l'ennemi (où il est instancié).")]
        public Transform spawnPointTransform;

        [Tooltip("Liste des Nœuds Pères (Node 1A, Node 1B, etc.). Chaque Node est un chemin complet. Un seul sera choisi au hasard.")]
        public Transform[] waypoints;
    }

    [Tooltip("Liste de tous les SpawnPoints disponibles et de leurs chemins associés.")]
    public SpawnPointData[] spawnPointsData;

    void Awake()
    {
        if (instance != null)
        {
            //Destroy(gameObject);
            return;
        }
        instance = this;
    }
}



