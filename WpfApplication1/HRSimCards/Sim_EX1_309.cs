using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_309 : SimTemplate//Siphon Soul
    {
        //Vernichtet einen Diener. Stellt bei Eurem Helden #3 Leben wieder her.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            p.minionGetDestroyed(target);
            int heal = p.getSpellHeal(3, ownplay);

            p.minionGetDamageOrHeal(ownplay ? p.playerFirst.ownHero : p.playerSecond.ownHero, -heal);
        }

    }
}
