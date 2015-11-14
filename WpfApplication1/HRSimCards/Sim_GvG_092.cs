using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_092 : SimTemplate //Gnomish Experimenter
    {

        //  Battlecry: Draw a card. If it's a minion, transform it into a Chicken. 

        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            CardDB.Card randomCard = CardDB.Instance.getCardData(p.getArandomCardFromDeck(own.own));
            if (randomCard.type == CardDB.cardtype.MOB) {
                randomCard = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.Mekka4t);
            }
            p.drawACard(randomCard.name, own.own);
        }

    }

}