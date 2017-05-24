using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Sim_BRM_006 : SimTemplate //* Imp Gang Boss
	{
		// Whenever this minion takes damage, summon a 1/1 Imp.
		CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.BRM_006t);//imp
		
        public override void onMinionGotDmgTrigger(Playfield p, Minion m, bool ownDmgdmin)
        {
            if (m.anzGotDmg>=1)
            {
                for (int i = 0; i < m.anzGotDmg; i++)
                {
					int pos = m.zonepos;
					p.callKid(kid, pos, m.own);
                }
                m.anzGotDmg = 0;
            }
        }
	}
}