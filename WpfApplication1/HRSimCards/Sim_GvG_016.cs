using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_016 : SimTemplate //Fel Reaver
    {

        //    Whenever your opponent plays a card, discard the top 3 cards of your deck.

        public override void onCardIsGoingToBePlayed(Playfield p, CardDB.Card c, bool wasOwnCard, Minion triggerEffectMinion)
        {
            if (wasOwnCard == triggerEffectMinion.own) return; //owner of card = owner of minion -> no effect

            if (triggerEffectMinion.own)
            {
                p.playerFirst.ownDeckSize = Math.Max(0, p.playerFirst.ownDeckSize - 3);
                p.getArandomCardFromDeck(true);
                p.getArandomCardFromDeck(true);
                p.getArandomCardFromDeck(true);
            }
            else
            {
                p.playerSecond.ownDeckSize = Math.Max(0, p.playerSecond.ownDeckSize - 3);
                p.getArandomCardFromDeck(false);
                p.getArandomCardFromDeck(false);
                p.getArandomCardFromDeck(false);
            }
        }


    }

}