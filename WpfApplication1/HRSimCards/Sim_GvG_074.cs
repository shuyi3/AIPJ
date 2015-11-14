using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_074 : SimTemplate //Kezan Mystic //TODO£ºrework
    {
        //todo better!
        //  Battlecry: Take control of a random enemy Secret;. 

        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            if (own.own)
            {
                if (p.playerSecond.ownSecretsIDList.Count >= 1)
                {
                    if (p.playerSecond.ownHeroStartClass == TAG_CLASS.HUNTER) p.playerFirst.ownSecretsIDList.Add(CardDB.cardIDEnum.EX1_610);
                    if (p.playerSecond.ownHeroStartClass == TAG_CLASS.MAGE) p.playerFirst.ownSecretsIDList.Add(CardDB.cardIDEnum.EX1_594);
                    if (p.playerSecond.ownHeroStartClass == TAG_CLASS.PALADIN) p.playerFirst.ownSecretsIDList.Add(CardDB.cardIDEnum.EX1_130);

                    if (p.playerSecond.ownHeroStartClass != TAG_CLASS.HUNTER && p.playerSecond.ownHeroStartClass != TAG_CLASS.MAGE && p.playerSecond.ownHeroStartClass != TAG_CLASS.PALADIN) p.playerFirst.ownSecretsIDList.Add(CardDB.cardIDEnum.EX1_130);
                    
                    p.playerSecond.ownSecretsIDList.RemoveAt(0);
                }
            }
            else
            {
                if (p.playerFirst.ownSecretsIDList.Count >= 1)
                {
                    p.playerFirst.ownSecretsIDList.RemoveAt(0);
                    SecretItem s = new SecretItem();
                    s.canBe_avenge = false;
                    s.canBe_counterspell = false;
                    s.canBe_duplicate = false;
                    s.canBe_explosive = false;
                    s.canBe_eyeforaneye = false;
                    s.canBe_freezing = false;
                    s.canBe_icebarrier = false;
                    s.canBe_iceblock = false;
                    s.canBe_mirrorentity = false;
                    s.canBe_missdirection = false;
                    s.canBe_noblesacrifice = false;
                    s.canBe_redemption = false;
                    s.canBe_repentance = false;
                    s.canBe_snaketrap = false;
                    s.canBe_snipe = false;
                    s.canBe_spellbender = false;
                    s.canBe_vaporize = false;

                    s.entityId = 1050;
                    s.canBe_explosive=true;

                    //p.playerSecond.ownSecretsIDList.Add(s);
                }
            }
        }


    }

}