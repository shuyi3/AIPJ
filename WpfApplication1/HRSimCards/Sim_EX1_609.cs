using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_609 : SimTemplate //snipe
	{
        //todo secret
//    geheimnis:/ wenn euer gegner einen diener ausspielt, werden diesem $4 schaden zugefügt.

        public override void onSecretPlay(Playfield p, bool ownplay, Minion target, int number)
        {
            int dmg = p.getSpellDamageDamage(4, ownplay);

            p.minionGetDamageOrHeal(target, dmg);
        }

	}

}
