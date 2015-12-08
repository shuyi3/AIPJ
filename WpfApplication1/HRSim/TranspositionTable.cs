namespace HRSim
{
    using System.Collections.Generic;
    using System;

	public class TranspositionTable
	{
        private Dictionary<float, List<Playfield>> TTable;

        public TranspositionTable() { 
            TTable = new Dictionary<float, List<Playfield>>();
        }

        public bool addToMap(Playfield state)
        {
            float key = getHashkey(state);
            //Helpfunctions.Instance.logg("key:" + key);
            //if ("" + key == "4140817") 
            //{
            //    int debug = 1;
            //    state.debugMinions();
            //}
            if (TTable.ContainsKey(key))
            {
                foreach (Playfield existState in TTable[key])
                {
                    if (existState.isEqual(state, true)) {
                        return true;
                    }
                }
                TTable[key].Add(state);
                return false;
                //state.printBoard();
            }
            else {
                TTable.Add(key, new List<Playfield>());
                TTable[key].Add(state);
            }
            return false;
        }

        public List<Playfield> getListByKey(float key) 
        {
            List<Playfield> list = null;
            TTable.TryGetValue(key, out list);
            return list;
        }

        public void clearTable()
        {
            TTable.Clear();
        }

        public float getHashkey(Playfield p) 
        {
            float ownMinionValue = 0f;
            float enemyMinionValue = 0f;
            float ownHandValue = 0f;
            float enemyHandValue = 0f;

            foreach (Minion m in p.playerFirst.ownMinions)
            {
                ownMinionValue += (m.Angr != 0) ? (float)Math.Sqrt(m.Angr) * m.Hp * (int)(m.name) : 0.5f * m.Hp * (int)(m.name);
            }
            foreach (Minion m in p.playerSecond.ownMinions)
            {
                enemyMinionValue += (m.Angr != 0) ? (float)Math.Sqrt(m.Angr) * m.Hp * (int)(m.name) : 0.5f * m.Hp * (int)(m.name);
            }
            foreach (Handmanager.Handcard hc in p.playerFirst.owncards)
            {
                if (hc.card.type == CardDB.cardtype.MOB)
                {
                    ownHandValue += hc.card.cost * (int)(hc.card.name);
                }
                else 
                {
                    ownHandValue += (float)Math.Sqrt(hc.card.cost) * (int)(hc.card.name);
                }
            }
            foreach (Handmanager.Handcard hc in p.playerSecond.owncards)
            {
                if (hc.card.type == CardDB.cardtype.MOB)
                {
                    enemyHandValue += hc.card.cost * (int)(hc.card.name);
                }
                else
                {
                    enemyHandValue += (float)Math.Sqrt(hc.card.cost) * (int)(hc.card.name);
                }
            }

            //Helpfunctions.Instance.logg("own hero hp " + p.playerFirst.ownHero.Hp + "enemy hero hp " + p.playerSecond.ownHero.Hp);
            float value = 10 * ownMinionValue + 100 * enemyMinionValue + 1000 * p.playerFirst.mana + 10000 * p.playerFirst.ownHero.Hp + 100000 * p.playerSecond.ownHero.Hp + ownHandValue + enemyHandValue + p.playerFirst.ownHero.Angr + p.playerFirst.ownHero.armor + p.playerFirst.ownWeaponAttack + p.playerSecond.ownWeaponDurability;
            if ("" + value == "4149265")
            {
                Helpfunctions.Instance.logg("10 *" + ownMinionValue + " + 100 *" + enemyMinionValue + " + 1000 *" + p.playerFirst.mana + " + 10000 *" + p.playerFirst.ownHero.Hp + " + 100000 *" + p.playerSecond.ownHero.Hp + " + " + ownHandValue + " + " + enemyHandValue + " + " + p.playerFirst.ownHero.Angr + " + " + p.playerFirst.ownHero.armor + " + " + p.playerFirst.ownWeaponAttack + " + " + p.playerSecond.ownWeaponDurability);
                int debug = 1;
            }
            return value;
        }
	}
}
