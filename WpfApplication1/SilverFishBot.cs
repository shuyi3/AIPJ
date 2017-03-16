using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRSim;

namespace HREngine.Bots
{
    public class Bot
    {
        private int concedeLvl = 5; // the rank, till you want to concede
        //PenalityManager penman = PenalityManager.Instance;
        //DateTime starttime = DateTime.Now;
        Silverfish sf;
        Behavior behave;

        public Bot(Silverfish sf)
        {
            //starttime = DateTime.Now;
            //Settings set = Settings.Instance;
            //this.sf = sf;
            //set.setSettings();
            //sf.setnewLoggFile();

            //bool teststuff = true;
            //bool printstuff = false;

            //if (teststuff)
            //{
            //    Helpfunctions.Instance.logg("teststuff");
            //    Playfield p = new Playfield();
            //    Ai.Instance.autoTester( printstuff);
            //}
            //Helpfunctions.Instance.ErrorLog("wait for board...");
        }

        //public void doData(string data)
        //{
        //    Ai.Instance.updateTwoTurnSim();
        //    Ai.Instance.autoTester(false, data);
        //    HRSim.Helpfunctions.Instance.resetBuffer();
        //    HRSim.Helpfunctions.Instance.writeToBuffer("board " + Ai.Instance.currentCalculatedBoard);
        //    HRSim.Helpfunctions.Instance.writeToBuffer("value " + Ai.Instance.bestmoveValue);
        //    if (Ai.Instance.bestmove != null)
        //    {
        //        Ai.Instance.bestmove.print(true);
        //        foreach (Action a in Ai.Instance.bestActions)
        //        {
        //            a.print(true);
        //        }
        //    }
        //    HRSim.Helpfunctions.Instance.writeBufferToActionFile();
        //    Ai.Instance.currentCalculatedBoard = "1";
        //    HRSim.Helpfunctions.Instance.ErrorLog("wait for next board...");

        //    //sf.readActionFile();
        //}

        public void testing(int start)
        {
            //for (int i = start; i < 900; i++)
            //{
            //    homeHandManager.anzcards = 1;
            //    handmanager.handCards.Clear();
            //    Handmanager.Handcard hc = new Handmanager.Handcard();
            //    hc.manacost = 1;
            //    hc.position = 1;
            //    hc.entity = 122;
            //    hc.card = CardDB.Instance.getCardDataFromID((CardDB.cardIDEnum)i);
            //    handmanager.handCards.Add(hc);
            //    Helpfunctions.Instance.ErrorLog("test " + i + " " + hc.card.name + " " + hc.card.cardIDenum);
            //    if (hc.card.sim_card == null)
            //    {
            //        Helpfunctions.Instance.logg("cant test " + i + " " + hc.card.name);
            //    }
            //    else
            //    {
            //        Ai.Instance.dosomethingclever(this.behave);
            //    }
            //}
        }

    }

    public class Silverfish : HRSim.PlayerAgent
    {
        Behavior botbase;

        //public Ai ai;
        //public Settings Settings;
        //public Helpfunctions Helpfunctions;
        //public Movegenerator Movegenerator;
        //public Hrtprozis Hrtprozis;
        //public Handmanager Handmanager;
        //public ComboBreaker Combobreaker;
        //public PenalityManager PenalityManager;
        //public Probabilitymaker Probabilitymaker;
        //public Mulligan Mulligan; // read the mulligan list
        public Ai ai;
        public Settings settings;
        public Helpfunctions helpfunctions;
        public Movegenerator movegenerator;
        public Hrtprozis hrtprozis;
        public Handmanager handmanager;
        public ComboBreaker combobreaker;
        public PenalityManager penalityManager;
        public Probabilitymaker probabilitymaker;
        public Mulligan mulligan; // read the mulligan list

        public string versionnumber = "115.0";
        private bool singleLog = false;
        private string botbehave = "rush";

        //Settings settings = Settings.Instance;

        List<Minion> ownMinions = new List<Minion>();
        List<Minion> enemyMinions = new List<Minion>();
        List<Handmanager.Handcard> handCards = new List<Handmanager.Handcard>();
        int ownPlayerController = 0;
        List<string> ownSecretList = new List<string>();
        int enemySecretCount = 0;

        int currentMana = 0;
        int ownMaxMana = 0;
        int cardsPlayedThisTurn = 0;
        int ueberladung = 0;

        int enemyMaxMana = 0;

        string ownHeroWeapon = "";
        int heroWeaponAttack = 0;
        int heroWeaponDurability = 0;

        string enemyHeroWeapon = "";
        int enemyWeaponAttack = 0;
        int enemyWeaponDurability = 0;

        string heroname = "";
        string enemyHeroname = "";

        CardDB.Card heroAbility = new CardDB.Card();
        bool ownAbilityisReady = false;
        CardDB.Card enemyAbility = new CardDB.Card();

        int anzcards = 0;
        int enemyAnzCards = 0;

        int ownHeroFatigue = 0;
        int enemyHeroFatigue = 0;
        int ownDecksize = 0;
        int enemyDecksize = 0;

        Minion ownHero;
        Minion enemyHero;

        //difference
        private Playfield lastpf;
        private int numOptionPlayedThisTurn = 0;
        private int numMinionsPlayedThisTurn = 0;
        private List<int> enemySecretList = new List<int>();

        //private static Silverfish instance;

        public override void updateState(HRSim.Playfield playfield)
        {
            this.updateEverything(playfield, playfield.isOwnTurn);
        }

        public override HRSim.Action getMove()
        {
            if (Ai.bestmove == null) return null;
            return new HRSim.Action(Ai.bestmove);
        }

        public Ai Ai
        {
            get
            {
                return ai ?? (ai = new Ai(this));
            }
        }

        public Settings Settings
        {
            get
            {
                return settings ?? (settings = new Settings(this));
            }
        }

        public Helpfunctions Helpfunctions
        {
            get
            {
                return helpfunctions ?? (helpfunctions = new Helpfunctions(this));
            }
        }

        public Movegenerator Movegenerator
        {
            get
            {
                return movegenerator ?? (movegenerator = new Movegenerator(this));
            }
        }

        public Hrtprozis Hrtprozis
        {
            get
            {
                return hrtprozis ?? (hrtprozis = new Hrtprozis(this));
            }
        }

        public Handmanager Handmanager
        {
            get
            {
                return handmanager ?? (handmanager = new Handmanager(this));
            }
        }

        public ComboBreaker Combobreaker
        {
            get
            {
                return combobreaker ?? (combobreaker = new ComboBreaker(this));
            }
        }

        public PenalityManager PenalityManager
        {
            get
            {
                return penalityManager ?? (penalityManager = new PenalityManager(this));
            }
        }

        public Probabilitymaker Probabilitymaker
        {
            get
            {
                return probabilitymaker ?? (probabilitymaker = new Probabilitymaker(this));
            }
        }

        public Mulligan Mulligan
        {
            get
            {
                return mulligan ?? (mulligan = new Mulligan(this));
            }
        }



        public Silverfish()
        {
            botbase = new BehaviorControl(this);
            //Ai = new Ai(this);
            //Settings = new Settings(this);
            //Helpfunctions = new Helpfunctions(this);
            //Movegenerator = new Movegenerator(this);
            //Hrtprozis = new Hrtprozis(this);
            //Handmanager = new Handmanager(this);
            //Combobreaker = new ComboBreaker(this);
            //PenalityManager = new PenalityManager(this);
            //Probabilitymaker = new Probabilitymaker(this);
            //Mulligan = new Mulligan(this);

            this.singleLog = Settings.writeToSingleFile;
            HRSim.Helpfunctions.Instance.ErrorLog("init Silverfish");
            string path = "";
            //System.IO.Directory.CreateDirectory(path);
            Settings.setFilePath("C:\\Code\\ConsoleApplication1\\ConsoleApplication1\\");

            if (!singleLog)
            {
                Settings.setLoggPath(path);
            }
            else
            {
                Settings.setLoggPath("");
                Settings.setLoggFile("UILogg.txt");
                try
                {
                    HRSim.Helpfunctions.Instance.createNewLoggfile();
                }
                catch
                {
                }
            }
            PenalityManager.setCombos();
        }

        public void updateHeroStuff(HRSim.Playfield p, bool own)
        {
            HRSim.Player mPlayer, ePlayer;
            if (own)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerSecond;
                ePlayer = p.playerFirst;
            }

            int ownheroentity = mPlayer.ownHero.entitiyID;
            int enemyheroentity = ePlayer.ownHero.entitiyID;
            this.ownHero = new Minion(mPlayer.ownHero);
            this.enemyHero = new Minion(ePlayer.ownHero);

            this.currentMana = mPlayer.mana;
            this.ownMaxMana = mPlayer.ownMaxMana;
            this.enemyMaxMana = ePlayer.ownMaxMana;

            //secret
            this.ownSecretList = new List<string>();
            foreach (CardDB.cardIDEnum cardIDenum in mPlayer.ownSecretsIDList)
            {
                this.ownSecretList.Add(cardIDenum.ToString());
            }
            enemySecretCount = ePlayer.ownSecretsIDList.Count;

            //minions
            this.numMinionsPlayedThisTurn = mPlayer.mobsplayedThisTurn;
            this.cardsPlayedThisTurn = mPlayer.cardsPlayedThisTurn;
            //ueberladung = TritonHs.RecallOwed;

            //get weapon stuff
            this.ownHeroWeapon = mPlayer.ownWeaponName.ToString();
            this.heroWeaponAttack = mPlayer.ownWeaponAttack;
            this.heroWeaponDurability = mPlayer.ownWeaponDurability;

            this.ownHeroFatigue = mPlayer.ownHeroFatigue;
            this.enemyHeroFatigue = ePlayer.ownHeroFatigue;

            //this.ownDecksize = mPlayer.ownDeckSize;
            //this.enemyDecksize = ePlayer.ownDeckSize;

            //own hero stuff###########################
            int heroAtk = mPlayer.ownHero.Angr;
            int heroHp = mPlayer.ownHero.Hp;
            int heroDefence = mPlayer.ownHero.armor;
            this.heroname = "mage"; //hard coded
            //this.heroname = "warlock"; //hard coded

            //enmey weapon hardcoded for mage
            this.enemyHeroname = "mage";
            //this.enemyHeroname = "warlock";
            this.enemyHeroWeapon = ePlayer.ownWeaponName.ToString();
            this.enemyWeaponAttack = ePlayer.ownWeaponAttack;
            this.enemyWeaponDurability = ePlayer.ownWeaponDurability;

            //own hero ablity stuff###########################################################
            //hard coded for mage

            this.heroAbility = CardDB.Instance.getCardData(CardDB.Instance.cardNamestringToEnum("fireblast"));
            //this.heroAbility =
            //    CardDB.Instance.getCardData(CardDB.Instance.cardNamestringToEnum("lifetap"));
            this.ownAbilityisReady = mPlayer.ownAbilityReady;
            this.enemyAbility = CardDB.Instance.getCardData(CardDB.Instance.cardNamestringToEnum("fireblast"));
            //this.enemyAbility =
            //    CardDB.Instance.getCardData(CardDB.Instance.cardNamestringToEnum("lifetap"));

            //generate Heros
            this.ownHero = new Minion(mPlayer.ownHero);
            this.enemyHero = new Minion(ePlayer.ownHero);
            this.enemyHero.Ready = false;

            this.ownHero.updateReadyness();

            //load enchantments of the heros
            //How to do this?

            //fastmode weapon correction:
            if (this.ownHero.Angr < this.heroWeaponAttack) this.ownHero.Angr = this.heroWeaponAttack;
            if (this.enemyHero.Angr < this.enemyWeaponAttack) this.enemyHero.Angr = this.enemyWeaponAttack;

        }

        public void updateMinions(HRSim.Playfield p, bool own)
        {
            HRSim.Player mPlayer, ePlayer;
            if (own)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerSecond;
                ePlayer = p.playerFirst;
            }

            this.ownMinions.Clear();
            this.enemyMinions.Clear();
            foreach (HRSim.Minion m in mPlayer.ownMinions)
            {
                Minion mMinion = new Minion(m);
                mMinion.own = true;
                this.ownMinions.Add(mMinion);
            }
            foreach (HRSim.Minion m in ePlayer.ownMinions)
            {
                Minion mMinion = new Minion(m);
                mMinion.own = false;
                this.enemyMinions.Add(mMinion);
            }
        }

        private void updateHandcards(HRSim.Playfield p, bool own)
        {
            HRSim.Player mPlayer, ePlayer;
            if (own)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerSecond;
                ePlayer = p.playerFirst;
            }

            this.handCards.Clear();
            this.anzcards = 0;
            this.anzcards = mPlayer.owncards.Count;
            foreach (HRSim.Handmanager.Handcard hc in mPlayer.owncards)
            {
                Handmanager.Handcard mHandcard = new Handmanager.Handcard(hc);
                this.handCards.Add(mHandcard);
            }
            enemyAnzCards = ePlayer.owncards.Count;
        }

        private void updateDecks(HRSim.Playfield p, bool own)
        {
            HRSim.Player mPlayer, ePlayer;
            if (own)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerSecond;
                ePlayer = p.playerFirst;
            }

            List<CardDB.cardIDEnum> ownCards = new List<CardDB.cardIDEnum>();
            List<CardDB.cardIDEnum> enemyCards = new List<CardDB.cardIDEnum>();
            List<GraveYardItem> graveYard = new List<GraveYardItem>();

            //own hands
            foreach (HRSim.Handmanager.Handcard c in mPlayer.owncards)
            {
                CardDB.cardIDEnum mCardID = (CardDB.cardIDEnum)Enum.Parse(typeof(CardDB.cardIDEnum), c.card.cardIDenum.ToString());
                ownCards.Add(mCardID);
            }
            //own minions
            foreach (HRSim.Minion m in mPlayer.ownMinions)
            {
                CardDB.cardIDEnum mCardID = (CardDB.cardIDEnum)Enum.Parse(typeof(CardDB.cardIDEnum), m.handcard.card.cardIDenum.ToString());
                ownCards.Add(mCardID);
            }
            //own secret
            foreach (HRSim.CardDB.cardIDEnum c in mPlayer.ownSecretsIDList)
            {
                CardDB.cardIDEnum mCardID = (CardDB.cardIDEnum)Enum.Parse(typeof(CardDB.cardIDEnum), c.ToString());
                ownCards.Add(mCardID);
            }
            //own weapon
            CardDB.cardName weaponName = (CardDB.cardName)Enum.Parse(typeof(CardDB.cardName), mPlayer.ownWeaponName.ToString());
            ownCards.Add(CardDB.Instance.getCardData(weaponName).cardIDenum);

            //playerSecond.own minions
            foreach (HRSim.Minion m in p.playerSecond.ownMinions)
            {
                Minion mMinion = new Minion(m);
                ownCards.Add(mMinion.handcard.card.cardIDenum);
            }
            //playerSecond.own weapon
            weaponName = (CardDB.cardName)Enum.Parse(typeof(CardDB.cardName), p.playerSecond.ownWeaponName.ToString());
            ownCards.Add(CardDB.Instance.getCardData(weaponName).cardIDenum);

            //grave yard
            foreach (HRSim.GraveYardItem g in p.graveYard)
            {
                CardDB.cardIDEnum graveCardID = (CardDB.cardIDEnum)Enum.Parse(typeof(CardDB.cardIDEnum), g.cardid.ToString());
                GraveYardItem mGraveItem = new GraveYardItem(graveCardID, g.entity, g.own);
                graveYard.Add(mGraveItem);
            }

            Probabilitymaker.setOwnCards(ownCards);
            Probabilitymaker.setEnemyCards(enemyCards);
            bool isTurnStart = false;
            if (Ai.nextMoveGuess.mana == -100)
            {
                isTurnStart = true;
                Ai.updateTwoTurnSim();
            }
            Probabilitymaker.setGraveYard(graveYard, isTurnStart);

        }


        private void updateBehaveString(Behavior botbase)
        {
            this.botbehave = "rush";
            if (botbase is BehaviorControl) this.botbehave = "control";
            if (botbase is BehaviorMana) this.botbehave = "mana";
            this.botbehave += " " + Ai.maxwide;
            this.botbehave += " face " + Combobreaker.attackFaceHP;
            if (Settings.secondTurnAmount > 0)
            {
                if (Ai.nextMoveGuess.mana == -100)
                {
                    Ai.updateTwoTurnSim();
                }
                this.botbehave += " twoturnsim " + Settings.secondTurnAmount + " ntss " +
                                  Settings.nextTurnDeep + " " + Settings.nextTurnMaxWide + " " +
                                  Settings.nextTurnTotalBoards;
            }

            if (Settings.playarround)
            {
                this.botbehave += " playaround";
                this.botbehave += " " + Settings.playaroundprob + " " + Settings.playaroundprob2;
            }

            this.botbehave += " ets " + Settings.enemyTurnMaxWide;

            if (Settings.simEnemySecondTurn)
            {
                this.botbehave += " ets2 " + Settings.enemyTurnMaxWideSecondTime;
                this.botbehave += " ents " + Settings.enemySecondTurnMaxWide;
            }

            if (Settings.useSecretsPlayArround)
            {
                this.botbehave += " secret";
            }

            if (Settings.secondweight != 0.5f)
            {
                this.botbehave += " weight " + (int)(Settings.secondweight * 100f);
            }

            if (Settings.simulatePlacement)
            {
                this.botbehave += " plcmnt";
            }


        }

        public bool updateEverything(HRSim.Playfield mp, bool own)
        {
            //HRSim.Helpfunctions.Instance.startTimer();
            HRSim.Player mPlayer, ePlayer;
            if (own)
            {
                mPlayer = mp.playerFirst;
                ePlayer = mp.playerSecond;
            }
            else
            {
                mPlayer = mp.playerSecond;
                ePlayer = mp.playerFirst;
            }

            this.updateBehaveString(botbase);

            updateHeroStuff(mp, own);
            updateMinions(mp, own);
            updateHandcards(mp, own);
            updateDecks(mp, own);

            // send ai the data:
            Hrtprozis.clearAll();
            Handmanager.clearAll();

            Hrtprozis.setOwnPlayer(mPlayer.ownController);
            Handmanager.setOwnPlayer(mPlayer.ownController);

            this.numOptionPlayedThisTurn = 0;
            this.numOptionPlayedThisTurn += this.cardsPlayedThisTurn + this.ownHero.numAttacksThisTurn;
            foreach (Minion m in this.ownMinions)
            {
                if (m.Hp >= 1) this.numOptionPlayedThisTurn += m.numAttacksThisTurn;
            }

            Hrtprozis.updatePlayer(this.ownMaxMana, this.currentMana, this.cardsPlayedThisTurn,
                this.numMinionsPlayedThisTurn, this.numOptionPlayedThisTurn, this.ueberladung, mPlayer.ownHero.entitiyID,
                mp.playerSecond.ownHero.entitiyID);
            Hrtprozis.updateSecretStuff(this.ownSecretList, this.enemySecretCount);

            Hrtprozis.updateOwnHero(this.ownHeroWeapon, this.heroWeaponAttack, this.heroWeaponDurability,
                this.heroname, this.heroAbility, this.ownAbilityisReady, this.ownHero);
            Hrtprozis.updateEnemyHero(this.enemyHeroWeapon, this.enemyWeaponAttack, this.enemyWeaponDurability,
                this.enemyHeroname, this.enemyMaxMana, this.enemyAbility, this.enemyHero);

            Hrtprozis.updateMinions(this.ownMinions, this.enemyMinions);
            Handmanager.setHandcards(this.handCards, this.anzcards, this.enemyAnzCards);

            Hrtprozis.updateFatigueStats(this.ownDecksize, this.ownHeroFatigue, this.enemyDecksize,
                this.enemyHeroFatigue);

            Hrtprozis.updateMaxEntity(mp.getNextEntity()); //change here!!

            Probabilitymaker.getEnemySecretGuesses(this.enemySecretList,
                Hrtprozis.enemyHeroStartClass);
            //learnmode :D
            //HRSim.Helpfunctions.Instance.logTime("start to finish update");

            Playfield p = new Playfield(this);

            if (lastpf != null)
            {
                if (lastpf.isEqualf(p))
                {
                    return false;
                }

                //board changed we update secrets!
                //if(Ai.nextMoveGuess!=null) Probabilitymaker.Instance.updateSecretList(Ai.nextMoveGuess.enemySecretList);
                Probabilitymaker.updateSecretList(p, lastpf);
                lastpf = p;
            }
            else
            {
                lastpf = p;
            }

            p = new Playfield(this); //secrets have updated :D

            p.printBoard();

            // calculate stuff
            HRSim.Helpfunctions.Instance.ErrorLog("calculating stuff... " + DateTime.Now.ToString("HH:mm:ss.ffff"));
            //if (runExtern)
            //{
            //    HRSim.Helpfunctions.Instance.logg("recalc-check###########");
            //    if (p.isEqual(Ai.nextMoveGuess, true))
            //    {
            //        //printstuff(false);
            //        Debug.WriteLine("equal");
            //        Ai.doNextCalcedMove();
            //    }
            //    else
            //    {
            //        //printstuff(true);
            //        Debug.WriteLine("not equal");
            //        //readActionFile(passiveWait);
            //    }
            //}
            //else
            //{
            // Drew: This prevents the freeze during AI updates, but no API functions may be called
            // during this time!
            //using (TritonHs.Memory.ReleaseFrame(true))
            //{
            //    printstuff(false);
            Ai.dosomethingclever(botbase);
            //}
            //}
            //HRSim.Helpfunctions.Instance.logTime("end of do sth clever");

            //Ai.bestmove = Movegenerator.getRandomMove(new Playfield(p), true, false, true);

            HRSim.Helpfunctions.Instance.ErrorLog("calculating ended! " + DateTime.Now.ToString("HH:mm:ss.ffff"));
            return true;
        }

        public void setnewLoggFile(int number)
        {
            if (!singleLog)
            {
                Settings.setLoggFile("UILogg_" + number + "_" + DateTime.Now.ToString("_yyyy-MM-dd_HH-mm-ss") + ".txt");
                HRSim.Helpfunctions.Instance.createNewLoggfile();
                HRSim.Helpfunctions.Instance.ErrorLog("#######################################################");
                HRSim.Helpfunctions.Instance.ErrorLog("fight is logged in: " + Settings.logpath + Settings.logfile);
                HRSim.Helpfunctions.Instance.ErrorLog("#######################################################");
            }
            else
            {
                Settings.setLoggFile("UILogg.txt");
            }
        }

        //public void readActionFile(bool passiveWaiting = false)
        //{
        //    Ai.nextMoveGuess = new Playfield(this);
        //    bool readed = true;
        //    List<string> alist = new List<string>();
        //    float value = 0f;
        //    string boardnumm = "-1";
        //    while (readed)
        //    {
        //        try
        //        {
        //            string data = System.IO.File.ReadAllText(Settings.path + "actionstodo.txt");
        //            if (data != "" && data != "<EoF>" && data.EndsWith("<EoF>"))
        //            {
        //                data = data.Replace("<EoF>", "");
        //                //Helpfunctions.Instance.ErrorLog(data);
        //                HRSim.Helpfunctions.Instance.resetBuffer();
        //                HRSim.Helpfunctions.Instance.writeBufferToActionFile();
        //                alist.AddRange(data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
        //                string board = alist[0];
        //                if (board.StartsWith("board "))
        //                {
        //                    boardnumm = (board.Split(' ')[1].Split(' ')[0]);
        //                    alist.RemoveAt(0);
        //                    /*if (boardnumm != Ai.currentCalculatedBoard)
        //                    {
        //                        if (passiveWaiting)
        //                        {
        //                            System.Threading.Thread.Sleep(10);
        //                            return;
        //                        }
        //                        continue;
        //                    }*/
        //                }
        //                string first = alist[0];
        //                if (first.StartsWith("value "))
        //                {
        //                    value = float.Parse((first.Split(' ')[1].Split(' ')[0]));
        //                    alist.RemoveAt(0);
        //                }
        //                readed = false;
        //            }
        //            else
        //            {
        //                System.Threading.Thread.Sleep(10);
        //                if (passiveWaiting)
        //                {
        //                    return;
        //                }
        //            }

        //        }
        //        catch
        //        {
        //            System.Threading.Thread.Sleep(10);
        //        }

        //    }
        //    HRSim.Helpfunctions.Instance.logg("received " + boardnumm + " actions to do:");
        //    Ai.currentCalculatedBoard = "0";
        //    Playfield p = new Playfield(this);
        //    List<Action> aclist = new List<Action>();

        //    foreach (string a in alist)
        //    {
        //        aclist.Add(new Action(a, p));
        //        HRSim.Helpfunctions.Instance.logg(a);
        //    }

        //    Ai.setBestMoves(aclist, value);

        //}

        public static int getLastAffected(int entityid)
        {
            return 0;
        }

        public static int getCardTarget(int entityid)
        {
            return 0;
        }


    }


    // the ai :D
    //please ask/write me if you use this in your project
    public class Helpfunctions
    {
        Silverfish sf;
        public static Random rng = null;

        public static List<T> TakeList<T>(IEnumerable<T> source, int limit)
        {
            List<T> retlist = new List<T>();
            int i = 0;

            foreach (T item in source)
            {
                retlist.Add(item);
                i++;

                if (i >= limit) break;
            }
            return retlist;
        }


        public bool runningbot = false;


        //private static Helpfunctions instance;

        //public static Helpfunctions Instance
        //{
        //    get
        //    {
        //        return instance ?? (instance = new Helpfunctions());
        //    }
        //}
        public static Random getRNG()
        {
            if (rng == null)
            {
                //int seed = Environment.TickCount;
                int seed = -1622430187;
                HRSim.Helpfunctions.Instance.logg("sf seed = " + seed);
                rng = new Random(seed);
            }
            return rng;
        }

        public Helpfunctions(Silverfish sf)
        {
            //foreach (Window window in Application.Current.Windows)
            //{
            //    if (window.GetType() == typeof(HRSim.MainWindow))
            //    {
            //        this.window = (HRSim.MainWindow)window;
            //    }
            //}
            this.sf = sf;
            //System.IO.File.WriteAllText(sf.Settings.logpath + sf.Settings.logfile, "");
        }

        private bool writelogg = true;
        public void loggonoff(bool onoff)
        {
            writelogg = onoff;
        }

        public void createNewLoggfile()
        {
            //System.IO.File.WriteAllText(sf.Settings.logpath + sf.Settings.logfile, "");
        }

        public void logg(string s)
        {


            //if (!writelogg) return;
            //try
            //{
            //    using (StreamWriter sw = File.AppendText(sf.Settings.logpath + sf.Settings.logfile))
            //    {
            //        sw.WriteLine(s);
            //    }
            //}
            //catch { }
            //Console.WriteLine(s);
            //ErrorLog(s);
        }

        public DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public void ErrorLog(string s)
        {
            //HREngine.API.Utilities.HRLog.Write(s);
            //Console.WriteLine(s);
            //window.appendLog(s);
        }

        string sendbuffer = "";
        public void resetBuffer()
        {
            this.sendbuffer = "";
        }

        public void writeToBuffer(string data)
        {
            this.sendbuffer += "\r\n" + data;
        }

        public void writeBufferToFile()
        {
            bool writed = true;
            //this.sendbuffer += "<EoF>";
            while (writed)
            {
                try
                {
                    System.IO.File.AppendAllText(sf.Settings.path + "crrntbrd.txt", this.sendbuffer);
                    writed = false;
                }
                catch
                {
                    writed = true;
                }
            }
            this.sendbuffer = "";
        }

        public void writeBufferToActionFile()
        {
            bool writed = true;
            this.sendbuffer += "<EoF>";
            this.ErrorLog("write to action file: " + sendbuffer);
            while (writed)
            {
                try
                {
                    System.IO.File.WriteAllText(sf.Settings.path + "actionstodo.txt", this.sendbuffer);
                    writed = false;
                }
                catch
                {
                    writed = true;
                }
            }
            this.sendbuffer = "";

        }
    }


}

