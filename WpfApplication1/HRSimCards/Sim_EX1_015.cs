using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_015 : SimTemplate //noviceengineer
	{

//    kampfschrei:/ zieht eine karte.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            p.drawACard(p.getArandomCardFromDeck(own.own), own.own);
		}


	}
}