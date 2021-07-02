using System;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Mana : MonoBehaviour
    {
        [Tooltip("How much mana will regen per second")] [SerializeField] float manaRegenRate = 4;

        float mana;

        BaseStats baseStats;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Start()
        {
            mana = baseStats.GetStat(Stat.Mana);
        }

        private void Update()
        {
            if (mana < baseStats.GetStat(Stat.Mana))
            {
                mana += manaRegenRate * Time.deltaTime;
            }
            if (mana > baseStats.GetStat(Stat.Mana)) mana = baseStats.GetStat(Stat.Mana);
        }
        
        public float GetMaxMana()
        {
            return baseStats.GetStat(Stat.Mana);
        }

        public float GetMana()
        {
            return mana;
        }

        public bool UseMana(float manaToUse)
        {
            if (manaToUse > mana)
            {
                return false;
            }
            
            mana -= manaToUse;
            return true;
        }
    }
}