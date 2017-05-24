using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
    class Sim_GVG_068 : SimTemplate //Burly Rockjaw Trogg
    {

        //   Whenever your opponent casts a spell, gain +2 Attack.

        public override void onCardIsGoingToBePlayed(Playfield p, CardDB.Card c, bool wasOwnCard, Minion triggerEffectMinion)
        {
            if (c.type == CardDB.cardtype.SPELL && wasOwnCard != triggerEffectMinion.own)
            {
                p.minionGetBuffed(triggerEffectMinion, 2, 0);
            }
        }


    }

}