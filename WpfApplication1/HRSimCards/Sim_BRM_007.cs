using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_BRM_007 : SimTemplate //* Gang Up
    {
        // Choose a minion. Shuffle 3 copies of it into your deck.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            if (ownplay)
            {
                p.playerFirst.ownDeckSize += 3;
                p.homeDeck.Add(target.handcard.card);
                p.homeDeck.Add(target.handcard.card);
                p.homeDeck.Add(target.handcard.card);
                //p.addCardToBucket(target.handcard.card, true);
            }
            else
            {
                p.playerSecond.ownDeckSize += 3;
                p.awayDeck.Add(target.handcard.card);
                p.awayDeck.Add(target.handcard.card);
                p.awayDeck.Add(target.handcard.card);
                //p.addCardToBucket(target.handcard.card, false);
            }
        }
    }
}