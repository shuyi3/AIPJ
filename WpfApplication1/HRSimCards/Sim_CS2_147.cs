using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_147 : SimTemplate //gnomishinventor
	{

//    kampfschrei:/ zieht eine karte.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            p.drawACard(p.getArandomCardFromDeck(own.own), own.own);
		}


	}
}