using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_038 : SimTemplate //Crackle
    {

        //    Deal $3-$6 damage.Overload: (1)

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(4, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
            if(ownplay) p.ueberladung+=1;
        }


    }

}