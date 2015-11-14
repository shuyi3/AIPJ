using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_CS2_075 : SimTemplate //sinisterstrike
    {

        //    fügt dem feindlichen helden $3 schaden zu.
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(3, ownplay);

            p.minionGetDamageOrHeal(ownplay ? p.playerSecond.ownHero : p.playerFirst.ownHero, dmg);

        }

    }
}