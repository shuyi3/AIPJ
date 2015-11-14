using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_099 : SimTemplate //Bomb Lobber
    {

        // Battlecry: Deal 4 damage to a random enemy minion.  

        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            List<Minion> temp = (own.own) ? p.playerSecond.ownMinions : p.playerFirst.ownMinions;
            int times = p.getSpellDamageDamage(4, own.own);

            if (temp.Count >= 1)
            {
                //search Minion with lowest hp
                Minion enemy = temp[0];
                int minhp = 10000;
                foreach (Minion m in temp)
                {
                    if (m.Hp >= times + 1 && minhp > m.Hp)
                    {
                        enemy = m;
                        minhp = m.Hp;
                    }
                }

                p.minionGetDamageOrHeal(enemy, times);

            } 
        }

    }

}