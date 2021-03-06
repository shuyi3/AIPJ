using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_056 : SimTemplate //lifetap
	{

//    heldenfähigkeit/\nzieht eine karte und erleidet 2 schaden.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);

            int dmg = 2;
            if (ownplay)
            {
                if (p.playerFirst.doublepriest >= 1) dmg *= (2 * p.playerFirst.doublepriest);
                p.minionGetDamageOrHeal(p.playerFirst.ownHero, dmg);
            }
            else
            {
                if (p.playerSecond.doublepriest >= 1) dmg *= (2 * p.playerSecond.doublepriest);
                p.minionGetDamageOrHeal(p.playerSecond.ownHero, dmg);
            }
        }


	}
}