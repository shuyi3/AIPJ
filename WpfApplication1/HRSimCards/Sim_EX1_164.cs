using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_164 : SimTemplate //* nourish
    {

        //    Choose One - Gain 2 Mana Crystals or Draw 3 cards.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            if (choice == 1)
            {
				if (ownplay)
				{
					p.playerFirst.mana = Math.Min(10, p.playerFirst.mana+2);
					p.playerFirst.ownMaxMana = Math.Min(10, p.playerFirst.ownMaxMana+2);
				}
				else
				{
					p.playerFirst.mana = Math.Min(10, p.playerFirst.mana+2);
					p.playerSecond.ownMaxMana = Math.Min(10, p.playerSecond.ownMaxMana+2);
				}
            }
            if (choice == 2)
            {
                //this.owncarddraw+=3;
                p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
                p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
                p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
            }
        }

    }


}