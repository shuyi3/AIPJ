using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_116 : SimTemplate //* Mekgineer Thermaplugg
    {
        //   Whenever an enemy minion dies, summon a Leper Gnome.
		CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_029);//lepergnome
		
        public override void onMinionDiedTrigger(Playfield p, Minion m, Minion diedMinion)
        {
			int diedMinions = (m.own) ? p.tempTrigger.enemyMinionsDied : p.tempTrigger.ownMinionsDied;
			if (diedMinions >= 1)
			{
				for (int i = 0; i < diedMinions; i++)
				{
					int pos = (m.own) ? p.ownMinions.Count : p.enemyMinions.Count;
					p.callKid(kid, pos, m.own);
				}
            }
        }
    }
}