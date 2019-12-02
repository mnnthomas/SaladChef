using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
    /// <summary>
    /// A powerup spawn class to spawn random powerup in the defined powerup spawn area
    /// </summary>
    public class PowerupSpawn : MonoBehaviour
    {
        [SerializeField] private List<GameObject> m_Powerups = new List<GameObject>();
        [SerializeField] private BoxCollider m_Collider = default;
 
        private Vector3 GetRandomSpawnPoint()
        {
            if(m_Collider.bounds != null)
            {
               return new Vector3(Random.Range(m_Collider.bounds.min.x, m_Collider.bounds.max.x), 0.5f, Random.Range(m_Collider.bounds.min.z, m_Collider.bounds.max.z));
            }
            return Vector3.zero;
        }

        public void SpawnPowerup(PlayerController player)
        {
            int powerupIndex = Random.Range(0, m_Powerups.Count);
            GameObject powerupObj = Instantiate(m_Powerups[powerupIndex], GetRandomSpawnPoint(), Quaternion.identity, transform);
            powerupObj.GetComponent<Powerup>().SetPlayer(player);
        }
    }
}

