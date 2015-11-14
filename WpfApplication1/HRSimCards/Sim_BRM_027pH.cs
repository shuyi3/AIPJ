using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_BRM_027pH : SimTemplate //DIE, INSECTS!
    {

        //   Hero Power: Deal 8 damage to a random enemy. TWICE.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {

            int count = (ownplay) ? p.playerSecond.ownMinions.Count : p.playerFirst.ownMinions.Count;
            if (count >= 1)
            {
                List<Minion> temp2 = (ownplay) ? new List<Minion>(p.playerSecond.ownMinions) : new List<Minion>(p.playerFirst.ownMinions);
                temp2.Sort((a, b) => a.Hp.CompareTo(b.Hp));//damage the lowest
                foreach (Minion mins in temp2)
                {
                    p.minionGetDamageOrHeal(mins, 8);
                    break;
                }
            }
            else
            {
                p.minionGetDamageOrHeal(ownplay ? p.playerSecond.ownHero : p.playerFirst.ownHero, 8);
            }

            p.doDmgTriggers();

            count = (ownplay) ? p.playerSecond.ownMinions.Count : p.playerFirst.ownMinions.Count;
            if (count >= 1)
            {
                List<Minion> temp2 = (ownplay) ? new List<Minion>(p.playerSecond.ownMinions) : new List<Minion>(p.playerFirst.ownMinions);
                temp2.Sort((a, b) => a.Hp.CompareTo(b.Hp));//damage the lowest
                foreach (Minion mins in temp2)
                {
                    p.minionGetDamageOrHeal(mins, 8);
                    break;
                }
            }
            else
            {
                p.minionGetDamageOrHeal(ownplay ? p.playerSecond.ownHero : p.playerFirst.ownHero, 8);
            }

        }



    }

}