using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_032 : SimTemplate //* Grove Tender
    {

        //    Choose One - Give each player a Mana Crystal; or Each player draws a card.

        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            if (choice == 1)
            {
				p.playerFirst.mana = Math.Min(10, p.playerFirst.mana+1);
				p.playerFirst.ownMaxMana = Math.Min(10, p.playerFirst.ownMaxMana+1);
				p.playerSecond.ownMaxMana = Math.Min(10, p.playerSecond.ownMaxMana+1);
            }

            if (choice == 2)//TODO: need order here?
            {
                p.drawACard(p.getArandomCardFromDeck(own.own), own.own);
                p.drawACard(p.getArandomCardFromDeck(!own.own), !own.own);
            }
        }


    }

}