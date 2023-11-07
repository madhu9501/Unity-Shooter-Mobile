using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDM
{
    public class Objects : MonoBehaviour
    {
        [SerializeField]
        public float HitPoints;
        public void TakeDamage(float damage)
        {
            HitPoints -= damage;
            Debug.Log(HitPoints);
            if(HitPoints <= 0f)
            {
                // if (this.tag == "Player")
                // {
                //     PlayerDeath();
                // }else if (this.tag == "Enemy")
                // {
                //     PlayerDeath();
                // }else
                // {
                //     DestroyObject();
                // } 

                DestroyObject();
            }
        }

        void DestroyObject()
        {
            Destroy(gameObject);
        }
 
    }
}

