using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_NEW1_026 : SimTemplate//Violet Teacher
    {
        public CardDB.Card card = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.NEW1_026t);
        
        public override void onCardIsGoingToBePlayed(Playfield p, CardDB.Card c, bool wasOwnCard, Minion triggerEffectMinion)
        {
            if (wasOwnCard == triggerEffectMinion.own && c.type == CardDB.cardtype.SPELL)
            {
                int place = (wasOwnCard)? p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;
                p.callKid(card, place, wasOwnCard);
            }
        }

    }

}
