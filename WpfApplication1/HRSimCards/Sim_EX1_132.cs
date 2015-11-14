using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_132 : SimTemplate //eyeforaneye
    {
        //todo secret
        //    geheimnis:/ wenn euer held schaden erleidet, wird dem feindlichen helden ebenso viel schaden zugefügt.
        public override void onSecretPlay(Playfield p, bool ownplay, int number)
        {
            int dmg = p.getSpellDamageDamage(number, ownplay);

            p.minionGetDamageOrHeal(ownplay ? p.playerSecond.ownHero : p.playerFirst.ownHero, dmg);
        }

    }

}