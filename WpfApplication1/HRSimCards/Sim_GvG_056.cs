using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_056 : SimTemplate //Iron Juggernaut
    {

        //   Battlecry:&lt;/b&gt; Shuffle a Mine into your opponent's deck. When drawn, it explodes for 10 damage.

        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            if (own.own)
            {
                p.playerSecond.ownDeckSize++;
                if (p.playerSecond.ownDeckSize <= 6)
                {
                    p.minionGetDamageOrHeal(p.playerSecond.ownHero, Math.Min(10, p.playerSecond.ownHero.Hp-1), true);
                    p.evaluatePenality -= 6;
                }
                else
                {
                    if (p.playerSecond.ownDeckSize <= 16)
                    {
                        p.minionGetDamageOrHeal(p.playerSecond.ownHero, Math.Min(5, p.playerSecond.ownHero.Hp - 1), true);
                        p.evaluatePenality -= 8;
                    }
                    else
                    {
                        if (p.playerSecond.ownDeckSize <= 26)
                        {
                            p.minionGetDamageOrHeal(p.playerSecond.ownHero, Math.Min(2, p.playerSecond.ownHero.Hp - 1), true);
                            p.evaluatePenality -= 10;
                        }
                    }
                }
            }
            else
            {
                p.playerFirst.ownDeckSize++;
            }
        }


    }

}