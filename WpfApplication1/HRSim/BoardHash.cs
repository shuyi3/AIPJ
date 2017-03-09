using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using System.Diagnostics;

namespace HRSim
{
    class BoardHash
    {

        HashSet<int> featureSparseSet = new HashSet<int>();
        int offset = 0;

        static string[] minionCardArray = {"damagedgolem", "clockworkgnome", "boombot", "manawyrm", "cogmaster", "annoyotron", "mechwarper",
               "snowchugger", "harvestgolem", "spidertank", "tinkertowntechnician", "mechanicalyeti",
               "goblinblastmage", "loatheb", "archmageantonidas", "drboom", "unknown"};

        static string[] cardArray = {"damagedgolem", "clockworkgnome", "boombot", "manawyrm", "cogmaster", "annoyotron", "mechwarper",
               "snowchugger", "harvestgolem", "spidertank", "tinkertowntechnician", "mechanicalyeti",
               "goblinblastmage", "loatheb", "archmageantonidas", "drboom", "unknown",
                "thecoin", "armorplating", "fireblast", "frostbolt", "fireball", "flamestrike"};
        
        static Dictionary<CardDB.cardName, int> cardIdxDict = new Dictionary<CardDB.cardName,int>()
                                                               {
                                                                {CardDB.cardName.damagedgolem, 0},                                                              
                                                                {CardDB.cardName.clockworkgnome, 1},
                                                                {CardDB.cardName.boombot, 2},
                                                                {CardDB.cardName.manawyrm, 3},
                                                                {CardDB.cardName.cogmaster, 4},
                                                                {CardDB.cardName.annoyotron, 5},
                                                                {CardDB.cardName.mechwarper, 6},
                                                                {CardDB.cardName.snowchugger, 7},
                                                                {CardDB.cardName.harvestgolem, 8},
                                                                {CardDB.cardName.spidertank, 9},
                                                                {CardDB.cardName.tinkertowntechnician, 10},
                                                                {CardDB.cardName.mechanicalyeti, 11},
                                                                {CardDB.cardName.goblinblastmage, 12},
                                                                {CardDB.cardName.loatheb, 13},
                                                                {CardDB.cardName.archmageantonidas, 14},
                                                                {CardDB.cardName.drboom, 15},
                                                                {CardDB.cardName.unknown, 16},
                                                                {CardDB.cardName.thecoin, 17},
                                                                {CardDB.cardName.armorplating, 18},
                                                                {CardDB.cardName.fireblast, 19},
                                                                {CardDB.cardName.frostbolt, 20},
                                                                {CardDB.cardName.fireball, 21},
                                                                {CardDB.cardName.flamestrike, 22}
                                                               };

        private static BoardHash instance;

        private BoardHash()
        {
        }

        public static BoardHash Instance
        {
            get
            {
                return instance ?? (instance = new BoardHash());
            }
        }

        public float[] encodePlayfield(Playfield p)
        {

            Player mPlayer, ePlayer;
            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                ePlayer = p.playerFirst;
                mPlayer = p.playerSecond;
            }

            float[] encode = new float[4 + cardArray.Length * 2 + minionCardArray.Length * 2];

            encode[0] = mPlayer.ownMaxMana;
            encode[1] = mPlayer.ownHero.Hp + mPlayer.ownHero.armor;
            encode[2] = ePlayer.ownMaxMana;
            encode[3] = ePlayer.ownHero.Hp + ePlayer.ownHero.armor;

            offset = 4;

            foreach (Minion m in mPlayer.ownMinions)
            {
                encode[cardIdxDict[m.name] + offset] += m.getOffMinionValue();
            }

            offset += minionCardArray.Length;

            foreach (Minion m in ePlayer.ownMinions)
            {
                encode[cardIdxDict[m.name] + offset] += m.getDefMinionValue();
            }

            offset += minionCardArray.Length;

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                encode[cardIdxDict[hc.card.name] + offset] += 1;
            }

            offset += cardArray.Length;

            foreach (Handmanager.Handcard hc in ePlayer.owncards)
            {
                encode[cardIdxDict[hc.card.name] + offset] += 1;
            }

            return encode;

        }

        public List<int> encodeAction(Playfield p)
        {
            List<Action> actions = p.getActions();
            List<int> playcardEntity = new List<int>();
            foreach (Action action in actions)
            {
                if (action.actionType == actionEnum.playcard)
                {
                    playcardEntity.Add(action.card.entity);
                }
            }
            return playcardEntity;
        }

        public int getActionHash(Playfield p)
        {
            List<int> encode = encodeAction(p);
            return string.Join(",", encode).GetHashCode();
        }

        public int hashPlayfield(Playfield p)
        {

            float[] encode = encodePlayfield(p);
            return string.Join(",", encode).GetHashCode();
        }
    }
}
