using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_NEW1_003 : SimTemplate //sacrificialpact
    {

        //    vernichtet einen dämon. stellt bei eurem helden #5 leben wieder her.
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            p.minionGetDestroyed(target);
            int heal = p.getSpellHeal(5, ownplay);
            p.minionGetDamageOrHeal(ownplay ? p.playerFirst.ownHero : p.playerSecond.ownHero, -heal);
        }

    }
}