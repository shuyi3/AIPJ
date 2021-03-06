using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_BRM_010b : SimTemplate //* Fire Hawk Form
	{
		// Transform into a 2/5 minion.
		
        CardDB.Card hawk = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.BRM_010t2);
        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            p.minionTransform(own, hawk);
        }
	}
}