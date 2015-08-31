using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_559 : SimTemplate //archmageantonidas
	{

//    erhaltet jedes mal einen „feuerball“-zauber auf eure hand, wenn ihr einen zauber wirkt.

        public override void onCardIsGoingToBePlayed(Playfield p, CardDB.Card c, bool wasOwnCard, Minion triggerEffectMinion)
        {
            if (wasOwnCard == triggerEffectMinion.own && c.type == CardDB.cardtype.SPELL)
            {
                p.drawACard(CardDB.cardName.fireball, wasOwnCard, true);
            }
        }

	}
}