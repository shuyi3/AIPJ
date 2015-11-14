using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_610 : SimTemplate //explosivetrap
	{
        //todo secret
//    geheimnis:/ wenn euer held angegriffen wird, erleiden alle feinde $2 schaden.
        public override void onSecretPlay(Playfield p, bool ownplay, int number)
        {
            int dmg = p.getSpellDamageDamage(2, ownplay);
            p.allMinionOfASideGetDamage(!ownplay, dmg);
        }

	}

}