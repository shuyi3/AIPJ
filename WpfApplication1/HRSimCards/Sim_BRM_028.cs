using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_BRM_028 : SimTemplate //* Emperor Thaurissan
	{
		// At the end of your turn, reduce the Cost of cards in your hand by (1).
		
        public override void onTurnEndsTrigger(Playfield p, Minion m, bool turnEndOfOwner)
        {
            if (m.own == turnEndOfOwner)
            {
				foreach (Handmanager.Handcard hc in p.playerFirst.owncards)
                {
                    if (hc.manacost >= 1) hc.manacost--;
                    hc.permCostChange--;     
                }
            }
        }
	}
}