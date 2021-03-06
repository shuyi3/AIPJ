using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Sim_BRM_010a : SimTemplate //* Firecat Form
	{
		// Transform into a 5/2 minion.
		
        CardDB.Card cat = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.BRM_010t);
        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            p.minionTransform(own, cat);
        }
	}
}