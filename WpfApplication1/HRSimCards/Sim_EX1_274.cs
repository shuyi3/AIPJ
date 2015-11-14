using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_274 : SimTemplate //etherealarcanist
	{

//    erhält +2/+2, wenn ihr am ende eures zuges über ein aktives geheimnis/ verfügt.
        public override void onTurnEndsTrigger(Playfield p, Minion triggerEffectMinion, bool turnEndOfOwner)
        {
            if (triggerEffectMinion.own == turnEndOfOwner)
            {
                int b = (turnEndOfOwner) ? p.playerFirst.ownSecretsIDList.Count : p.playerSecond.ownSecretsIDList.Count;
                if (b >= 1) p.minionGetBuffed(triggerEffectMinion, 2, 2);
 
            }
        }

	}
}