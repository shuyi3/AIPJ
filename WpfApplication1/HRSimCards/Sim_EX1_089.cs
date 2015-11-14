using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_089 : SimTemplate //arcanegolem
	{

//    ansturm/. kampfschrei:/ gebt eurem gegner 1 manakristall.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            if (own.own)
            {
                p.playerSecond.ownMaxMana = Math.Min(10, p.playerSecond.ownMaxMana+1);
            }
            else
            {
                p.playerFirst.ownMaxMana = Math.Min(10, p.playerFirst.ownMaxMana + 1);
            }
		}


	}
}