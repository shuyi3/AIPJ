using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
	class Sim_BRM_019 : SimTemplate //* Grim Patron
	{
		// Whenever this minion survives damage, summon another Grim Patron.
		CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.BRM_019);//Grim Patron
		
        public override void onMinionGotDmgTrigger(Playfield p, Minion m, bool ownDmgdmin)
        {
            if (m.anzGotDmg>=1 && m.Hp > 0)
            {
                for (int i = 0; i < m.anzGotDmg; i++)
                {
                    p.callKid(kid, m.zonepos, m.own);
                }
                m.anzGotDmg = 0;
            }
        }
	}
}