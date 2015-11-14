using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_050 : SimTemplate //coldlightoracle
	{

//    kampfschrei:/ jeder spieler zieht 2 karten.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            if (p.isOwnTurn)
            {
                p.drawACard(p.getArandomCardFromDeck(true), true);
                p.drawACard(p.getArandomCardFromDeck(true), true);
                p.drawACard(p.getArandomCardFromDeck(false), false);
                p.drawACard(p.getArandomCardFromDeck(false), false);
            }
            else {
                p.drawACard(p.getArandomCardFromDeck(false), false);
                p.drawACard(p.getArandomCardFromDeck(false), false);
                p.drawACard(p.getArandomCardFromDeck(true), true);
                p.drawACard(p.getArandomCardFromDeck(true), true);        
            }

		}


	}
}