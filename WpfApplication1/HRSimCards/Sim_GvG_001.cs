using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_001 : SimTemplate //Flamecannon
    {

        //    Deal $4 damage to a random enemy minion.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            // optimistic

            List<Minion> temp = (ownplay) ? p.playerSecond.ownMinions : p.playerFirst.ownMinions;
            int times = p.getSpellDamageDamage(4, ownplay);
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