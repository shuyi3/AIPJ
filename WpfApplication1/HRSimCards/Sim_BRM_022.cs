using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_BRM_022 : SimTemplate //* Dragon Egg
	{
		// Whenever this minion takes damage, summon a 2/1 Whelp.
		CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.BRM_022t);//Black Whelp
		
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