using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_DS1_233 : SimTemplate //mindblast
    {

        //    fügt dem feindlichen helden $5 schaden zu.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(5, ownplay);

            p.minionGetDamageOrHeal(ownplay ? p.playerSecond.ownHero : p.playerFirst.ownHero, dmg);
        }

    }
}