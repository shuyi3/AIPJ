using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_031 : SimTemplate //* Recycle
    {
        //   Shuffle an enemy minion into your opponent's deck.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            List<Minion> temp = (ownplay) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;
            target.handcard.card.sim_card.onAuraEnds(p, target);
            temp.Remove(target);

            if (ownplay)
            {
                p.tempTrigger.enemyMininsChanged = true;
                p.awayDeck.Add(target.handcard.card);
            }
            else
            {
                p.tempTrigger.ownMinionsChanged = true;
                p.homeDeck.Add(target.handcard.card);
            }
        }
    }
}