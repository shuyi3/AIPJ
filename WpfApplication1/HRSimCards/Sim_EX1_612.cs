using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_612 : SimTemplate //kirintormage
	{

//    kampfschrei:/ das nächste geheimnis/, das ihr in diesem zug ausspielt, kostet (0).

		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            if (own.own) p.playerFirst.playedmagierinderkirintor = true;
		}


	}
}