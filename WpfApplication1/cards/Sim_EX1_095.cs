using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Sim_EX1_095 : SimTemplate //gadgetzanauctioneer
	{

//    zieht jedes mal eine karte, wenn ihr einen zauber wirkt.

        public override void onCardIsGoingToBePlayed(Playfield p, CardDB.Card c, bool wasOwnCard, Minion triggerEffectMinion)
        {
            if (c.type == CardDB.cardtype.SPELL && wasOwnCard == triggerEffectMinion.own)
            {
                p.drawACard(CardDB.cardName.unknown, wasOwnCard);
            }

        }

	}
}