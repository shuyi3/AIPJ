using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HREngine.Bots;
using Newtonsoft.Json;

namespace HRSim
{
    public class Message
    {
        public int result { get; set; }
        public double[][] features { get; set; }
    }

    public class GameRecord
    {
        public int result { get; set; }
        public List<StateKeyInfo> playSec { get; set; }
    }

    public class StateKeyInfo
    {
        public int turnSt { get; set; }
        public PlayerKeyInfo attackPlayer { get; set; }
        public PlayerKeyInfo defensePlayer { get; set; }
        public bool isRandom { get; set; }
        public double stateValue { get; set; }
        public int nextEntity { get; set; }

        [JsonConstructor]
        public StateKeyInfo(int nextEntity, PlayerKeyInfo ap, PlayerKeyInfo dp, bool isRandom, double stateVal)
        {
            this.nextEntity = nextEntity;
            this.turnSt = 0;
            this.attackPlayer = ap;
            this.defensePlayer = dp;
            this.isRandom = isRandom;
            this.stateValue = stateVal;
        }
    }

    public class PlayerKeyInfo
    {

        public class ActionKeyInfo
        {
            public actionEnum actionType { get; set; }
            //public Handmanager.Handcard card;
            public int cardEntitiy { get; set; }
            public int place { get; set; } //= target where card/minion is placed
            public int ownEntity { get; set; } // -10 for none
            public int targetEntity { get; set; } //-10 for none
            //public int druidchoice; // 1 left card, 2 right card
            //public int penalty;
            public int manaCost { get; set; }

            [JsonConstructor]
            public ActionKeyInfo()
            {
            }

            public ActionKeyInfo(Action action)
            {
                this.actionType = action.actionType;
                this.cardEntitiy = (action.card != null) ? action.card.entity : -10;
                this.place = action.place;
                this.ownEntity = (action.own != null) ? action.own.entitiyID : -10;
                this.targetEntity = (action.target != null) ? action.target.entitiyID : -10;
                this.manaCost = action.manaCost;
            }
        }

        public class MinionKeyInfo
        {
            public string name { get; set; }
            public int entity { get; set; }
            public int zonepos { get; set; }
            public int spellpower { get; set; }
            public int Hp { get; set; }
            public int maxHp { get; set; }
            public int armor { get; set; }
            public int Angr { get; set; }
            public bool taunt { get; set; }
            public bool wounded { get; set; }
            public bool divineshild { get; set; }
            public bool windfury { get; set; }
            public bool frozen { get; set; }
            public bool stealth { get; set; }
            public bool immune { get; set; }
            public bool exhausted { get; set; }
            public int charge { get; set; }
            public bool poisonous { get; set; }
            public bool silenced { get; set; }
            public bool deathrattle { get; set; }
            public float offValue { get; set; }
            public float defValue { get; set; }

            [JsonConstructor]
            public MinionKeyInfo()
            {
            }

            public MinionKeyInfo(Minion m)
            {
                name = m.name.ToString();
                entity = m.entitiyID;
                zonepos = m.zonepos;
                spellpower = m.spellpower;
                Hp = m.Hp;
                maxHp = m.maxHp;
                armor = m.armor;
                Angr = m.Angr;
                taunt = m.taunt;
                wounded = m.wounded;
                divineshild = m.divineshild;
                windfury = m.windfury;
                frozen = m.frozen;
                stealth = m.stealth;
                immune = m.immune;
                exhausted = m.exhausted;
                charge = m.charge;
                poisonous = m.poisonous;
                silenced = m.silenced;
                deathrattle = m.handcard.card.deathrattle;
                offValue = m.getOffMinionValue();
                defValue = m.getDefMinionValue();
            }
        }

        public class CardKeyInfo
        {
            //this.permCostChange = hc.permCostChange;
            public int entity { get; set; }
            public int position { get; set; }
            public int manacost { get; set; }
            public string cardName { get; set; }
            public bool playable { get; set; }

            [JsonConstructor]
            public CardKeyInfo()
            {
            }

            public CardKeyInfo(Handmanager.Handcard hc, bool own, Playfield pf)
            {
                entity = hc.entity;
                position = hc.position;
                manacost = hc.getManaCost(pf, own);
                playable = hc.canplayCard(pf, own);
                cardName = hc.card.name.ToString();
            }

            public CardKeyInfo(Handmanager.Handcard hc)
            {
                entity = hc.entity;
                position = hc.position;
                manacost = hc.manacost;
                cardName = hc.card.name.ToString();
            }
        }


        //no weapon
        //no sec
        //current mage only
        public int turn { get; set; } //0 first, 1 second
        public int maxMana { get; set; }
        public MinionKeyInfo heroInfo { get; set; }
        public List<MinionKeyInfo> minionJsonList { get; set; }
        //public List<float> minionValueList { get; set; }
        public List<CardKeyInfo> handcardJsonList { get; set; }
        public List<List<CardKeyInfo>> handcardChange { get; set; }
        public List<int> canPlayHeroPower { get; set; }
        public List<ActionKeyInfo> playedActionJsonList { get; set; }
        public List<CardKeyInfo> playedCardJsonList { get; set; }
        public List<string> ownDeckList { get; set; }
        public List<int> endTurnFeatrueList { get; set; }

        [JsonConstructor]
        public PlayerKeyInfo()
        {
        }

        public PlayerKeyInfo(Player p, List<CardDB.Card> ownDeck, bool ownTurn, Playfield pf)
        {
            this.maxMana = Math.Min(10, p.ownMaxMana);
            this.turn = p.ownController;
            this.heroInfo = new MinionKeyInfo(p.ownHero);
            List<MinionKeyInfo> minionList = new List<MinionKeyInfo>();
            //List<float> minionValueList = new List<float>();
            foreach (Minion m in p.ownMinions)
            {
                MinionKeyInfo minionInfo = new MinionKeyInfo(m);
                //float minionValue = off ? m.getOffMinionValue() : m.getDefMinionValue();
                minionList.Add(minionInfo);
                //minionValueList.Add(minionValue);
            }
            this.minionJsonList = minionList;
            List<CardKeyInfo> hcList = new List<CardKeyInfo>();
            foreach (Handmanager.Handcard hc in p.owncards)
            {
                CardKeyInfo hcInfo = new CardKeyInfo(hc, ownTurn, pf);
                hcList.Add(hcInfo);
            }
            this.handcardJsonList = hcList;
            this.ownDeckList = new List<string>();
            foreach (CardDB.Card card in ownDeck)
            {
                this.ownDeckList.Add(card.name.ToString());
            }
        }

        public void updatePlayedAction(Player p)
        {
            List<ActionKeyInfo> actionList = new List<ActionKeyInfo>();
            foreach (Action action in p.lastTurnActions)
            {
                ActionKeyInfo acKeyInfo = new ActionKeyInfo(action);
                actionList.Add(acKeyInfo);
            }
            this.playedActionJsonList = actionList;
        }

        public void updatePlayedCard(Player p, bool own, Playfield pf)
        {
            List<CardKeyInfo> playedCardList = new List<CardKeyInfo>();
            foreach (Action action in p.lastTurnActions)
            {
                if (action.actionType == actionEnum.playcard)
                {
                    CardKeyInfo pcInfo = new CardKeyInfo(action.card, own, pf);
                    playedCardList.Add(pcInfo);
                }
            }
            this.playedCardJsonList = playedCardList;
        }

        public void updateEndTurnFeature(List<int> endTurnFeature)
        {
            this.endTurnFeatrueList = endTurnFeature;
        }
    }

    public class PlayerAgent
    {

        public virtual void updateState(Playfield playField)
        {
            return;
        }

        public virtual Action getMove()
        {
            return null;
        }

        public virtual bool isRandomMove()
        {
            return true;
        }

        public virtual float getStateValue(Playfield p)
        {
            return 0.0f;
        }
    }

    public class GameManager
    {

        public static int[] bucketMana = { 1, 2, 3, 5, 10 };
        public static int[] bucketManaFor3 = { 2, 5, 10 };

        public static Random rng = null;
        public int turnStartEntity = 1000;

        private Playfield playField;
        public PlayerAgent playerFirst, playerSecond;
        public Bot playerOneBot, playerTwoBot;
        public Behavior playerOneBH, playerTwoBH;
        public Behavior botBase = new BehaviorControl();

        public int moveCount = 0;
        public double myTimer = 0;
        public double sfTimer = 0;
        public int nodeCount = 0;
        //public List<double[]> featureList = new List<double[]>();
        public List<StateKeyInfo> featureList = new List<StateKeyInfo>();
        public List<Playfield> playList = new List<Playfield>();

        private PlayerKeyInfo ownkeyInfo = null;
        private PlayerKeyInfo enemykeyInfo = null;
        public Behavior bh;
        public double P1Time = 0;
        public double P2Time = 0;

        public Playfield mPlayfield
        {

            get { return playField; }

            set { playField = value; }

        }

        private static GameManager instance = null;

        public static GameManager Instance
        {
            get
            {
                return instance;
            }
        }

        public static void Init()
        {

            //Playfield mPlayerField = new Playfield();
            instance = new GameManager();
            instance.initPlayField();
            instance.bh = new BehaviorControl();
            //instance.playField.initDeckFromFile("C:\\Code\\WpfApplication1\\WpfApplication1\\test_mage_deck.txt", true);
            //instance.playField.initDeckFromFile("C:\\Code\\WpfApplication1\\WpfApplication1\\test_mage_deck.txt", false);
            //instance.playField.drawInitCards();
            //instance.playField.isOwnTurn = true;
            //Helpfunctions.Instance.convertPlayfieldToJson(GameManager.Instance.mPlayfield);

        }

        public void initPlayField()
        {
            //Playfield mPlayerField = new Playfield();
            if (this.playField == null || this.playField.getGameResult() != -1)
            {
                this.playField = new Playfield();
                this.playField.initDeckFromFile("C:\\Code\\WpfApplication1\\WpfApplication1\\test_mage_deck.txt", true);
                this.playField.initDeckFromFile("C:\\Code\\WpfApplication1\\WpfApplication1\\test_mage_deck.txt", false);
                this.playField.drawInitCards();
                this.playField.isOwnTurn = true;
            }
        }

        public GameManager()
        {
        }

        public void setPlayer(int number, PlayerAgent player)
        {
            if (number == 0)
            {
                playerFirst = player;
            }
            else
            {
                playerSecond = player;
            }
        }

        public int playMove()
        {
            int gameResult = playField.getGameResult();
            if (gameResult == 0)
            {
                Helpfunctions.Instance.logg("##########PLAYER 1 WON##########");
                return 0;
            }
            else if (gameResult == 1)
            {
                Helpfunctions.Instance.logg("##########PLAYER 2 WON##########");
                return 1;
            }
            Action moveTodo = null;
            if (playField.isOwnTurn)
            {

                Helpfunctions.Instance.logg("PLAYER 1 ##########MOVE##########");
                //DateTime startTime = DateTime.Now;  
                if (playerFirst is LearningPlayer)
                {
                    playerFirst.updateState(playField);
                    GameManager.Instance.mPlayfield = ((LearningPlayer)playerFirst).getBestPlayfield();
                }
                else
                {
                    if (ownkeyInfo == null && enemykeyInfo == null)
                    {
                        ownkeyInfo = new PlayerKeyInfo(playField.playerFirst, playField.homeDeck, true, playField);
                        enemykeyInfo = new PlayerKeyInfo(playField.playerSecond, playField.awayDeck, false, playField);
                    }
                    playerFirst.updateState(playField);
                    moveTodo = playerFirst.getMove();
                    if (moveTodo != null)
                    {
                        moveTodo.print();
                        playField.doAction(moveTodo);
                    }
                    else
                    {
                        List<int> endTurnFeature = GameManager.Instance.bh.getPlayfieldFeature(playField, true);
                        playField.endTurn(false, false);
                        ownkeyInfo.updatePlayedAction(playField.playerFirst);
                        ownkeyInfo.updatePlayedCard(playField.playerFirst, true, GameManager.Instance.mPlayfield);
                        ownkeyInfo.updateEndTurnFeature(endTurnFeature);
                        playField.value = Int32.MinValue;
                        StateKeyInfo stateInfo = new StateKeyInfo(GameManager.Instance.turnStartEntity, ownkeyInfo, enemykeyInfo,
                            playerFirst.isRandomMove(), playerFirst.getStateValue(playField));
                        featureList.Add(stateInfo);
                        ownkeyInfo = null;
                        enemykeyInfo = null;
                        playField.drawTurnStartCard();
                        GameManager.Instance.turnStartEntity = playField.nextEntity;
                        Helpfunctions.Instance.logg("Next eneity: " + GameManager.Instance.turnStartEntity);
                        //if (GameManager.Instance.turnStartEntity == 1072)
                        //{ 
                        //    int debug = 1;
                        //}
                    }
                }
                //DateTime endTime = DateTime.Now;
                //double calcTime = (endTime - startTime).TotalMilliseconds / 1000;
                //if (((GreedyPlayer)playerFirst).isDNNEval())
                //{
                //    GameManager.Instance.P1Time += calcTime;
                //}
                //else
                //{
                //    GameManager.Instance.P2Time += calcTime;
                //}
                //Helpfunctions.Instance.logg("PLAYER 1 calculating time: " + calcTime);
            }
            else
            {
                Helpfunctions.Instance.logg("PLAYER 2 ##########MOVE##########");
                //DateTime startTime = DateTime.Now;  
                if (playerSecond is LearningPlayer)
                {
                    playerSecond.updateState(playField);
                    GameManager.Instance.mPlayfield = ((LearningPlayer)playerSecond).getBestPlayfield();
                }
                else
                {
                    if (ownkeyInfo == null && enemykeyInfo == null)
                    {
                        ownkeyInfo = new PlayerKeyInfo(playField.playerFirst, playField.homeDeck, true, playField);
                        enemykeyInfo = new PlayerKeyInfo(playField.playerSecond, playField.awayDeck, false, playField);
                    }
                    playerSecond.updateState(playField);
                    moveTodo = playerSecond.getMove();
                    if (moveTodo != null)
                    {
                        moveTodo.print();
                        playField.doAction(moveTodo);
                    }
                    else
                    {
                        List<int> endTurnFeature = GameManager.Instance.bh.getPlayfieldFeature(playField, false);
                        playField.endTurn(false, false);
                        enemykeyInfo.updatePlayedAction(playField.playerSecond);
                        enemykeyInfo.updatePlayedCard(playField.playerSecond, false, playField);
                        enemykeyInfo.updateEndTurnFeature(endTurnFeature);
                        playField.value = Int32.MinValue;
                        StateKeyInfo stateInfo = new StateKeyInfo(GameManager.Instance.turnStartEntity, enemykeyInfo, ownkeyInfo,
                            playerSecond.isRandomMove(), playerSecond.getStateValue(playField));
                        featureList.Add(stateInfo);
                        ownkeyInfo = null;
                        enemykeyInfo = null;
                        playField.drawTurnStartCard();
                        GameManager.Instance.turnStartEntity = playField.nextEntity;
                        Helpfunctions.Instance.logg("Next eneity: " + GameManager.Instance.turnStartEntity);
                    }
                }
                //DateTime endTime = DateTime.Now;
                //double calcTime = (endTime - startTime).TotalMilliseconds / 1000;
                //if (((GreedyPlayer)playerFirst).isDNNEval())
                //{
                //    GameManager.Instance.P1Time += calcTime;
                //}
                //else
                //{
                //    GameManager.Instance.P2Time += calcTime;
                //}
                //Helpfunctions.Instance.logg("PLAYER 2 calculating time: " + calcTime);
            }

            gameResult = playField.getGameResult();
            if (gameResult != -1) //game end, so update the last frame
            {
                if (playField.isOwnTurn)
                {
                    List<int> endTurnFeature = GameManager.Instance.bh.getPlayfieldFeature(playField, true);
                    ownkeyInfo.updatePlayedAction(playField.playerFirst);
                    ownkeyInfo.updatePlayedCard(playField.playerFirst, true, playField);
                    ownkeyInfo.updateEndTurnFeature(endTurnFeature);
                    playField.value = Int32.MinValue;
                    StateKeyInfo stateInfo = new StateKeyInfo(GameManager.Instance.turnStartEntity, ownkeyInfo, enemykeyInfo,
                                               playerFirst.isRandomMove(), playerFirst.getStateValue(playField));
                    featureList.Add(stateInfo);
                    ownkeyInfo = null;
                    enemykeyInfo = null;
                }
                else
                {
                    List<int> endTurnFeature = GameManager.Instance.bh.getPlayfieldFeature(playField, false);
                    enemykeyInfo.updatePlayedAction(playField.playerSecond);
                    enemykeyInfo.updatePlayedCard(playField.playerSecond, false, playField);
                    enemykeyInfo.updateEndTurnFeature(endTurnFeature);
                    playField.value = Int32.MinValue;
                    StateKeyInfo stateInfo = new StateKeyInfo(GameManager.Instance.turnStartEntity, enemykeyInfo, ownkeyInfo,
                                                playerSecond.isRandomMove(), playerSecond.getStateValue(playField));
                    featureList.Add(stateInfo);
                    ownkeyInfo = null;
                    enemykeyInfo = null;
                }
            }

            //for debuging
            //if (moveTodo != null && moveTodo.actionType == actionEnum.playcard && moveTodo.card.card.name == CardDB.cardName.loatheb)
            //{
            //    return -2;
            //}
            //UPDATE UI   

            return -1;
        }

        public static void resetSeed(int seed)
        {
            rng = new Random(seed);
        }

        public static void resetSeed()
        {
            int seed = rng.Next();
            rng = new Random(seed);
        }

        public static Random getRNG()
        {
            if (rng == null)
            {
                //int seed = Environment.TickCount;
                //int seed = 564654093;
                int seed = 1220632265;
                rng = new Random(seed);
                Helpfunctions.Instance.logg("seed = " + seed);
            }
            return rng;
        }

    }

}
