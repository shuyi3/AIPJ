using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_087 : SimTemplate //Steamwheedle Sniper
    {

        //  Your Hero Power can target minions. 

        public override void onAuraStarts(Playfield p, Minion m)
        {
            if (m.own) 
            {
                if (p.playerFirst.ownHeroName == HeroEnum.hunter) p.playerFirst.weHaveSteamwheedleSniper = true;
            }
            else
            {
                if (p.playerSecond.ownHeroName == HeroEnum.hunter) p.playerSecond.weHaveSteamwheedleSniper = true;
            }
        }

        public override void  onAuraEnds(Playfield p, Minion m)
        {
            if (m.own && p.playerFirst.ownHeroName == HeroEnum.hunter)
            {
                bool hasss = false;
                foreach (Minion mnn in p.playerFirst.ownMinions)
                {
                    if (!mnn.silenced && m.name == CardDB.cardName.steamwheedlesniper)
                    {
                        hasss = true;
                    }
                }
                p.playerFirst.weHaveSteamwheedleSniper = hasss;
               
            }
            if (!m.own && p.playerSecond.ownHeroName == HeroEnum.hunter)
            {
                bool hasss = false;
                foreach (Minion mnn in p.playerSecond.ownMinions)
                {
                    if (!mnn.silenced && m.name == CardDB.cardName.steamwheedlesniper)
                    {
                        hasss = true;
                    }
                }
                p.playerSecond.weHaveSteamwheedleSniper = hasss;
            }
        }


    }

}