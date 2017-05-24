using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
    class Sim_BRM_027pH : SimTemplate //DIE, INSECTS!
    {

        //   Hero Power: Deal 8 damage to a random enemy. TWICE.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {

            int count = (ownplay) ? p.enemyMinions.Count : p.ownMinions.Count;
            if (count >= 1)
            {
                List<Minion> temp2 = (ownplay) ? new List<Minion>(p.enemyMinions) : new List<Minion>(p.ownMinions);
                temp2.Sort((a, b) => a.Hp.CompareTo(b.Hp));//damage the lowest
                foreach (Minion mins in temp2)
                {
                    p.minionGetDamageOrHeal(mins, 8);
                    break;
                }
            }
            else
            {
                p.minionGetDamageOrHeal(ownplay ? p.enemyHero : p.ownHero, 8);
            }

            p.doDmgTriggers();

            count = (ownplay) ? p.enemyMinions.Count : p.ownMinions.Count;
            if (count >= 1)
            {
                List<Minion> temp2 = (ownplay) ? new List<Minion>(p.enemyMinions) : new List<Minion>(p.ownMinions);
                temp2.Sort((a, b) => a.Hp.CompareTo(b.Hp));//damage the lowest
                foreach (Minion mins in temp2)
                {
                    p.minionGetDamageOrHeal(mins, 8);
                    break;
                }
            }
            else
            {
                p.minionGetDamageOrHeal(ownplay ? p.enemyHero : p.ownHero, 8);
            }

        }



    }

}