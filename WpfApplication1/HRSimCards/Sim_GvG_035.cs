using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_035 : SimTemplate //Malorne
    {

        //    Deathrattle:&lt;/b&gt; Shuffle this minion into your deck.

        public override void onDeathrattle(Playfield p, Minion m)
        {
            if (m.own)
            {
                CardDB.Card card = CardDB.Instance.getDeckCardData(CardDB.cardName.malorne);
                p.homeDeck.Add(card);
            }
            else
            {
                CardDB.Card card = CardDB.Instance.getDeckCardData(CardDB.cardName.malorne);
                p.awayDeck.Add(card);
            }
        }


    }

}