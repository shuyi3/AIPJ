using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading;

using System.IO;
using System.Text.RegularExpressions;
using HREngine.Bots;
using System.Diagnostics;
using System.IO.Pipes;

using System.ComponentModel;

namespace HRSim
{

    public partial class MainWindow : Window
    {

        HRSim.Playfield testField;
        public Silverfish playerOne, playerTwo;
        private string _logText;

        public event PropertyChangedEventHandler LogChanged;

        public string LogText
        {
            get
            {
                return _logText;
            }

            set
            {
                if (value == _logText)
                    return;

                _logText = value;

                if (LogChanged != null)
                    LogChanged(this, new PropertyChangedEventArgs("LogText"));
            }
        }

        private string _ownHeroText;
        public event PropertyChangedEventHandler ownHeroTextChanged;

        public string OwnHeroText
        {
            get
            {
                return _ownHeroText;
            }

            set
            {
                if (value == _ownHeroText)
                    return;

                _ownHeroText = value;

                if (ownHeroTextChanged != null)
                    ownHeroTextChanged(this, new PropertyChangedEventArgs("OwnHeroText"));
            }
        }

        private string _enemyHeroText;
        public event PropertyChangedEventHandler enemyHeroTextChanged;

        public string EnemyHeroText
        {
            get
            {
                return _enemyHeroText;
            }

            set
            {
                if (value == _enemyHeroText)
                    return;

                _enemyHeroText = value;

                if (enemyHeroTextChanged != null)
                    enemyHeroTextChanged(this, new PropertyChangedEventArgs("EnemyHeroText"));
            }
        }
        
        public class FileInfo
        {
            public CardDB.cardName Name { get; set; }
            public BitmapImage Image { get; set; }
            public FileInfo(HRSim.CardDB.cardName name, HRSim.CardDB.cardIDEnum cardID)
            {
                Name = name;
                Image = new BitmapImage(new Uri("C:\\Code\\WpfApplication1\\WpfApplication1\\hs-images\\" + cardID + ".png", UriKind.RelativeOrAbsolute));
            }

        }

        ObservableCollection<FileInfo> ownHandCards = new ObservableCollection<FileInfo>();
        ObservableCollection<FileInfo> enemyHandCards = new ObservableCollection<FileInfo>();
        ObservableCollection<FileInfo> ownMinions = new ObservableCollection<FileInfo>();
        ObservableCollection<FileInfo> enemyMinions = new ObservableCollection<FileInfo>();


        public ObservableCollection<FileInfo> OwnHandCards
        {
            get
            {
                return ownHandCards;
            }
        }

        public ObservableCollection<FileInfo> EnemyHandCards
        {
            get
            {
                return enemyHandCards;
            }
        }

        public ObservableCollection<FileInfo> OwnMinions
        {
            get
            {
                return ownMinions;
            }
        }

        public ObservableCollection<FileInfo> EnemyMinions
        {
            get
            {
                return enemyMinions;
            }
        }

        private BackgroundWorker bw = new BackgroundWorker();
        bool isInit = false;

        public MainWindow()
        {
            DataContext = this;          
            InitializeComponent();
            Init();
            InitPipe();
            //bw.WorkerReportsProgress = true;
            //bw.WorkerSupportsCancellation = true;
            //bw.DoWork += BackgroundWorkerOnDoWork;
            //bw.ProgressChanged += BackgroundWorkerOnProgressChanged;
            //bw.RunWorkerAsync();
        }

        private void BackgroundWorkerOnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            object userObject = e.UserState;
            int percentage = e.ProgressPercentage;
        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            while (!worker.CancellationPending)
            {
                //Do your stuff here
                Thread.Sleep(4000);
                if (!isInit)
                {
                    Init();
                    InitPipe();
                    isInit = true;
                }
                worker.ReportProgress(0, "AN OBJECT TO PASS TO THE UI-THREAD");
            }
        }

        private static int numThreads = 1;

        public void InitPipe()
        {
            int i;
            Thread server = new Thread(ServerThread);
            server.Start();
            //Thread[] servers = new Thread[numThreads];

            //Console.WriteLine("\n*** Named pipe server stream with impersonation example ***\n");
            //Console.WriteLine("Waiting for client connect...\n");
            //for (i = 0; i < numThreads; i++)
            //{
            //    servers[i] = new Thread(ServerThread);
            //    servers[i].Start();
            //}
            //Thread.Sleep(250);
            //while (i > 0)
            //{
            //    for (int j = 0; j < numThreads; j++)
            //    {
            //        if (servers[j] != null)
            //        {
            //            if (servers[j].Join(250))
            //            {
            //                Console.WriteLine("Server thread[{0}] finished.", servers[j].ManagedThreadId);
            //                servers[j] = null;
            //                i--;    // decrement the thread watch count
            //            }
            //        }
            //    }
            //}
            //Console.WriteLine("\nServer threads exhausted, exiting.");
        }

        private void ServerThread(object data)
        {
            NamedPipeServerStream pipeServer =
                new NamedPipeServerStream("testpipe", PipeDirection.InOut, numThreads);

            int threadId = Thread.CurrentThread.ManagedThreadId;

            // Wait for a client to connect
            pipeServer.WaitForConnection();

            Console.WriteLine("Client connected on thread[{0}].", threadId);

            StreamString ss = new StreamString(pipeServer);
            ss.WriteString("I am the one true server!");

            while (true) { 
                if (ss.ReadString() == "hi there"){
                    //update board
                    displayMinions(testField);                          
                }
            }       
            pipeServer.Close();
        }

// Defines the data protocol for reading and writing strings on our stream 
    public class StreamString
{
    private Stream ioStream;
    private UnicodeEncoding streamEncoding;

    public StreamString(Stream ioStream)
    {
        this.ioStream = ioStream;
        streamEncoding = new UnicodeEncoding();
    }

    public string ReadString()
    {
        int len = 0;

        len = ioStream.ReadByte() * 256;
        len += ioStream.ReadByte();
        byte[] inBuffer = new byte[len];
        ioStream.Read(inBuffer, 0, len);

        return streamEncoding.GetString(inBuffer);
    }

    public int WriteString(string outString)
    {
        byte[] outBuffer = streamEncoding.GetBytes(outString);
        int len = outBuffer.Length;
        if (len > UInt16.MaxValue)
        {
            len = (int)UInt16.MaxValue;
        }
        ioStream.WriteByte((byte)(len / 256));
        ioStream.WriteByte((byte)(len & 255));
        ioStream.Write(outBuffer, 0, len);
        ioStream.Flush();

        return outBuffer.Length + 2;
    }
}

        public void displayHandCards(HRSim.Playfield p, bool shwoWithImage)
        {
            HRSim.Helpfunctions.Instance.ErrorLog("Own Handcards: ");
            foreach (HRSim.Handmanager.Handcard c in p.owncards)
            {
                if (shwoWithImage)
                    ownHandCards.Add(new FileInfo(c.card.name, c.card.cardIDenum));
                HRSim.Helpfunctions.Instance.ErrorLog("pos " + c.position + " " + c.card.name + " " + c.manacost + " entity " + c.entity + " " + c.card.cardIDenum + " " + c.addattack);
            }
            HRSim.Helpfunctions.Instance.ErrorLog("Enemy Handcards: ");
            foreach (HRSim.Handmanager.Handcard c in p.enemycards)
            {
                if (shwoWithImage)
                    enemyHandCards.Add(new FileInfo(c.card.name, c.card.cardIDenum));
                HRSim.Helpfunctions.Instance.ErrorLog("pos " + c.position + " " + c.card.name + " " + c.manacost + " entity " + c.entity + " " + c.card.cardIDenum + " " + c.addattack);

            }
        }

        public void displayMinions(HRSim.Playfield p)
        {
            foreach (Minion m in p.ownMinions)
            {
                App.Current.Dispatcher.Invoke((System.Action)delegate // <--- HERE
                {
                    ownMinions.Add(new FileInfo(m.handcard.card.name, m.handcard.card.cardIDenum));
                });
            }
            foreach (Minion m in p.enemyMinions)
            {
                App.Current.Dispatcher.Invoke((System.Action)delegate // <--- HERE
                {
                    enemyMinions.Add(new FileInfo(m.handcard.card.name, m.handcard.card.cardIDenum));
                });
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ThreadPool.QueueUserWorkItem((x) =>
            //{
            //    while (true)
            //    {
            //        Dispatcher.BeginInvoke((Action)(() =>
            //        {
            //            mFileNames.Add(new FileInfo("X"));
            //        }));
            //        Thread.Sleep(500);
            //    }
            //});
        }

        public void appendLog(string log)
        {
            LogText += Environment.NewLine + log;
        }

        public void updateHeroState(HRSim.Playfield p)
        {
            OwnHeroText = "" + p.ownHeroName + " || ";
            OwnHeroText += "ownherohp: " + p.ownHero.Hp + " + " + p.ownHero.armor + " || ";
            OwnHeroText += "mana " + p.mana + "/" + p.ownMaxMana + " || ";
            OwnHeroText += "ownheroattac: " + p.ownHero.Angr + " || ";
            OwnHeroText += "ownheroweapon: " + p.ownWeaponAttack + " " + p.ownWeaponDurability + " " + p.ownWeaponName + " || ";
            OwnHeroText += "ownherostatus: frozen" + p.ownHero.frozen + " || ";
            //Helpfunctions.Instance.logg("enemyherohp: " + this.enemyHero.Hp + " + " + this.enemyHero.armor + ((this.enemyHero.immune) ? " immune" : ""));

            //if (this.enemySecretCount >= 1) Helpfunctions.Instance.logg("enemySecrets: " + Probabilitymaker.Instance.getEnemySecretData(this.enemySecretList));
            //foreach (Action a in this.playactions)
            //{
            //    a.print();
            //}
            EnemyHeroText = "" + p.enemyHeroName + " || ";
            EnemyHeroText += "enemyherohp: " + p.enemyHero.Hp + " + " + p.enemyHero.armor + " || ";
            EnemyHeroText += "mana " + p.enemyMaxMana + "/" + p.enemyMaxMana + " || ";
            EnemyHeroText += "enemyheroattac: " + p.enemyHero.Angr + " || ";
            EnemyHeroText += "enemyherowwapon: " + p.enemyWeaponAttack + " " + p.enemyWeaponDurability + " " + p.enemyWeaponName + " || ";
            EnemyHeroText += "ownherostatus: frozen" + p.enemyHero.frozen + " || ";
        }

        public void InitAISettings() 
        {
           
            bool concede = false;

            // play with these settings###################################
            int enfacehp = 15;  // hp of enemy when your hero is allowed to attack the enemy face with his weapon
            int mxwde = 3000;   // numer of boards which are taken to the next deep-lvl
            int twotsamount = 0;          // number of boards where the next turn is simulated
            bool enemySecondTurnSim = false; // if he simulates the next players-turn, he also simulates the enemys respons

            bool playaround = false;  //play around some enemys aoe-spells?
            //these two probs are >= 0 and <= 100
            int playaroundprob = 50;    //probability where the enemy plays the aoe-spell, but your minions will not die through it
            int playaroundprob2 = 80;   // probability where the enemy plays the aoe-spell, and your minions can die!
            bool useExternalProcess = false; // use silver.exe for calculations a lot faster than turning it off (true = recomended)

            int amountBoardsInEnemyTurnSim = 40;
            int amountBoardsInEnemyTurnSimSecondStepp = 200;
            int amountBoardsInEnemySecondTurnSim = 20;

            int nextturnsimDeep = 6;
            int nextturnsimMaxWidth = 12;
            int nexttunsimMaxBoards = 80;
            int ImprovedCalculations = 1;

            bool secrets = false; // playing arround enemys secrets

            int alpha = 50; // weight of the second turn in calculation (0<= alpha <= 100)

            HREngine.Bots.Settings.Instance.simulatePlacement = false;  //rudiment!!! set this true, and ai will simulate all placements, whether you have a alpha/flametongue/argus
            //use it only with useExternalProcess = true !!!!

            //###########################################################


            Mulligan.Instance.setAutoConcede(concede);

            //Silverfish.Instance.setnewLoggFile();

            HRSim.Helpfunctions.Instance.ErrorLog("set enemy-face-hp to: " + enfacehp);
            ComboBreaker.Instance.attackFaceHP = enfacehp;

            Ai.Instance.setMaxWide(mxwde);
            HRSim.Helpfunctions.Instance.ErrorLog("set maxwide to: " + mxwde);

            Ai.Instance.setTwoTurnSimulation(false, twotsamount);
            HRSim.Helpfunctions.Instance.ErrorLog("calculate the second turn of the " + twotsamount + " best boards");
            if (twotsamount >= 1)
            {
                //Ai.Instance.nextTurnSimulator.setEnemyTurnsim(enemySecondTurnSim);
                HREngine.Bots.Settings.Instance.simEnemySecondTurn = enemySecondTurnSim;
                if (enemySecondTurnSim) Helpfunctions.Instance.ErrorLog("simulates the enemy turn on your second turn");
            }

            if (secrets)
            {

                HREngine.Bots.Settings.Instance.useSecretsPlayArround = secrets;
                HRSim.Helpfunctions.Instance.ErrorLog("playing arround secrets is " + secrets);
            }


            if (playaround)
            {
                HREngine.Bots.Settings.Instance.playarround = playaround;
                HREngine.Bots.Settings.Instance.playaroundprob = playaroundprob;
                HREngine.Bots.Settings.Instance.playaroundprob2 = playaroundprob2;
                Ai.Instance.setPlayAround();
                HRSim.Helpfunctions.Instance.ErrorLog("activated playaround");
            }


            HREngine.Bots.Settings.Instance.setWeights(alpha);


            bool teststuff = false;
            // set to true, to run a testfile (requires test.txt file in filder where _cardDB.txt file is located)
            bool printstuff = false; // if true, the best board of the tested file is printet stepp by stepp

            HREngine.Bots.Settings.Instance.enemyTurnMaxWide = amountBoardsInEnemyTurnSim;
            HREngine.Bots.Settings.Instance.enemySecondTurnMaxWide = amountBoardsInEnemySecondTurnSim;

            HREngine.Bots.Settings.Instance.nextTurnDeep = nextturnsimDeep;
            HREngine.Bots.Settings.Instance.nextTurnMaxWide = nextturnsimMaxWidth;
            HREngine.Bots.Settings.Instance.nextTurnTotalBoards = nexttunsimMaxBoards;
           // HREngine.Bots.Settings.Instance.ImprovedCalculations = ImprovedCalculations;

            HRSim.Helpfunctions.Instance.ErrorLog("----------------------------");
            HRSim.Helpfunctions.Instance.ErrorLog("you are running uai V" + playerOne.versionnumber);
            HRSim.Helpfunctions.Instance.ErrorLog("----------------------------");

            //if (this.useExternalProcess) Helpfunctions.Instance.ErrorLog("YOU USE SILVER.EXE FOR CALCULATION, MAKE SURE YOU STARTED IT!");
            //if (this.useExternalProcess) Helpfunctions.Instance.ErrorLog("SILVER.EXE IS LOCATED IN: " + HREngine.Bots.Settings.Instance.path);

            //if (teststuff)
            //{
                //Ai.Instance.autoTester(printstuff);
            //}
        }

        public void Init()
        {

            playerOne = new Silverfish();
            playerOne.setnewLoggFile();

            HREngine.Bots.Bot b = new HREngine.Bots.Bot(playerOne);

            //implementation
            //string[] deckLines;
            //try
            //{
            //    deckLines = System.IO.File.ReadAllLines("C:\\Code\\ConsoleApplication1\\ConsoleApplication1\\test_mage_deck.txt");
            //    foreach (string line in deckLines)
            //    {
            //        string cardNameString = Regex.Replace(line.ToLower(), @"[^a-z]", "");
            //        CardDB.Card card = CardDB.Instance.getCardData(CardDB.Instance.cardNamestringToEnum(cardNameString));
            //        //CardDB.cardName cardName = CardDB.Instance.cardNamestringToEnum(cardNameString);
            //        Console.WriteLine(card.name);
            //    } 
            //}
            //catch
            //{

            //}

            testField = new HRSim.Playfield();

            testField.initDeckFromFile("C:\\Code\\WpfApplication1\\WpfApplication1\\test_mage_deck.txt", true);
            testField.initDeckFromFile("C:\\Code\\WpfApplication1\\WpfApplication1\\test_mage_deck.txt", false);

            testField.drawInitCards();

            InitAISettings();

            displayHandCards(testField, false);

            //Handmanager.Handcard cardToPlay = testField.owncards[0];

            //HREngine.Bots.Action playLeftMinion = new HREngine.Bots.Action(actionEnum.playcard, cardToPlay, null, 1, null, 0, 0);

            //testField.doAction(playLeftMinion);

            //testField.endTurn(false, false, false, Settings.Instance.playaroundprob, Settings.Instance.playaroundprob2);

            //Handmanager.Handcard enemyCardToPlay = testField.enemycards[0];

            //HREngine.Bots.Action playEnemyLeftMinion = new HREngine.Bots.Action(actionEnum.playcard, enemyCardToPlay, null, 1, null, 0, 0);

            //testField.doAction(playEnemyLeftMinion);  

            Behavior behave = new BehaviorControl();//change this to new BehaviorRush() for rush mode
            HRSim.Action moveTodo = null;
            //testField.printBoard();
            for (; ; )
            {
                bool templearn = playerOne.updateEverything(behave, testField);
                if (Ai.Instance.bestmove == null) break;
                moveTodo = new HRSim.Action(Ai.Instance.bestmove);
                HRSim.Helpfunctions.Instance.logg("##########MOVE##########");
                moveTodo.print();
                HRSim.Helpfunctions.Instance.logg("##########MOVE##########");
                testField.doAction(moveTodo);

            }



            //testField.printBoard();

            //displayMinions(testField);

            //displayHandCards(testField, true);

            //updateHeroState(testField);


        }

        public static ImageSource LoadImage(string fileName)
        {
            var image = new BitmapImage();

            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
            }

            return image;
        }
    }
}

namespace HRSim {

    public class Playfield
    {
        //Todo: delete all new list<minion>
        //TODO: graveyard change (list <card,owner>)
        //Todo: vanish clear all auras/buffs (NEW1_004)

        public bool logging = false;
        public bool complete = false;

        public int nextEntity = 70;

        public triggerCounter tempTrigger = new triggerCounter();

        //aura minions##########################
        //todo reduce buffing vars
        public int anzOwnRaidleader = 0;
        public int anzEnemyRaidleader = 0;
        public int anzOwnStormwindChamps = 0;
        public int anzEnemyStormwindChamps = 0;
        public int anzOwnTundrarhino = 0;
        public int anzEnemyTundrarhino = 0;
        public int anzOwnTimberWolfs = 0;
        public int anzEnemyTimberWolfs = 0;
        public int anzMurlocWarleader = 0;
        public int anzGrimscaleOracle = 0;
        public int anzOwnAuchenaiSoulpriest = 0;
        public int anzEnemyAuchenaiSoulpriest = 0;
        public int anzOwnsorcerersapprentice = 0;
        public int anzOwnsorcerersapprenticeStarted = 0;
        public int anzEnemysorcerersapprentice = 0;
        public int anzEnemysorcerersapprenticeStarted = 0;
        public int anzOwnSouthseacaptain = 0;
        public int anzEnemySouthseacaptain = 0;
        public int anzOwnMalGanis = 0;
        public int anzEnemyMalGanis = 0;
        public int anzOwnChromaggus = 0;
        public int anzEnemyChromaggus = 0;
        public int anzOwnDragonConsort = 0;
        public int anzOwnDragonConsortStarted = 0;

        public int anzOwnMechwarper = 0;
        public int anzOwnMechwarperStarted = 0;
        public int anzEnemyMechwarper = 0;
        public int anzEnemyMechwarperStarted = 0;

        public int anzEnemyTaunt = 0;
        public int ownMinionsDiedTurn = 0;
        public int enemyMinionsDiedTurn = 0;

        public bool feugenDead = false;
        public bool stalaggDead = false;

        public bool weHavePlayedMillhouseManastorm = false;
        public bool weHaveSteamwheedleSniper = false;
        public bool enemyHaveSteamwheedleSniper = false;

        public bool needGraveyard = true;


        public int doublepriest = 0;
        public int enemydoublepriest = 0;


        public int ownBaronRivendare = 0;
        public int enemyBaronRivendare = 0;
        //#########################################

        public int tempanzOwnCards = 0; // for Goblin Sapper
        public int tempanzEnemyCards = 0;// for Goblin Sapper

        public bool isOwnTurn = true; // its your turn?
        public int turnCounter = 0;
        public bool sEnemTurn = false;//should the enemy turn be simulated?

        public bool attacked = false;
        public int attackFaceHP = 15;

        public int evaluatePenality = 0;
        public int ownController = 0;

        //public int ownHeroEntity = -1;
        //public int enemyHeroEntity = -1;

        public int hashcode = 0;
        public float value = Int32.MinValue;
        public int guessingHeroHP = 30;

        public int mana = 0;
        public int manaTurnEnd = 0;

        public List<CardDB.cardIDEnum> ownSecretsIDList = new List<CardDB.cardIDEnum>();
        public List<SecretItem> enemySecretList = new List<SecretItem>();

        public int enemySecretCount = 0;

        public Minion ownHero;
        public Minion enemyHero;
        public HeroEnum ownHeroName = HeroEnum.druid;
        public HeroEnum enemyHeroName = HeroEnum.druid;
        public TAG_CLASS ownHeroStartClass = TAG_CLASS.INVALID;
        public TAG_CLASS enemyHeroStartClass = TAG_CLASS.INVALID;

        public CardDB.cardName ownWeaponName = CardDB.cardName.unknown;
        public int ownWeaponAttack = 0;
        public int ownWeaponDurability = 0;

        public CardDB.cardName enemyWeaponName = CardDB.cardName.unknown;
        public int enemyWeaponAttack = 0;
        public int enemyWeaponDurability = 0;

        public List<Minion> ownMinions = new List<Minion>();
        public List<Minion> enemyMinions = new List<Minion>();
        public List<GraveYardItem> diedMinions = null;

        public List<Handmanager.Handcard> owncards = new List<Handmanager.Handcard>();
        public List<Handmanager.Handcard> enemycards = new List<Handmanager.Handcard>();
        public int owncarddraw = 0;

        public List<Action> playactions = new List<Action>();

        public int enemycarddraw = 0;
        public int enemyAnzCards = 0;

        public int spellpower = 0;
        public int enemyspellpower = 0;

        public bool playedmagierinderkirintor = false;
        public bool playedPreparation = false;

        public bool loatheb = false;
        public int winzigebeschwoererin = 0;
        public int startedWithWinzigebeschwoererin = 0;
        public int managespenst = 0;
        public int startedWithManagespenst = 0;
        public int soeldnerDerVenture = 0;
        public int startedWithsoeldnerDerVenture = 0;
        public int beschwoerungsportal = 0;
        public int startedWithbeschwoerungsportal = 0;
        public int nerubarweblord = 0;
        public int startedWithnerubarweblord = 0;

        public bool startedWithDamagedMinions = false; // needed for manacalculation of the spell "Crush"

        public int ownWeaponAttackStarted = 0;
        public int ownMobsCountStarted = 0;
        public int ownCardsCountStarted = 0;
        public int enemyCardsCountStarted = 0;
        public int ownHeroHpStarted = 30;
        public int enemyHeroHpStarted = 30;

        public int mobsplayedThisTurn = 0;
        public int startedWithMobsPlayedThisTurn = 0;

        public int optionsPlayedThisTurn = 0;
        public int cardsPlayedThisTurn = 0;
        public int ueberladung = 0; //=recall

        public int enemyOptionsDoneThisTurn = 0;

        public int ownMaxMana = 0;
        public int enemyMaxMana = 0;

        public int lostDamage = 0;
        public int lostHeal = 0;
        public int lostWeaponDamage = 0;

        public int ownDeckSize = 30;
        public int enemyDeckSize = 30;
        public int ownHeroFatigue = 0;
        public int enemyHeroFatigue = 0;

        public bool ownAbilityReady = false;
        public Handmanager.Handcard ownHeroAblility;

        public bool enemyAbilityReady = false;
        public Handmanager.Handcard enemyHeroAblility;

        // just for saving which minion to revive with secrets (=the first one that died);
        public CardDB.cardIDEnum revivingOwnMinion = CardDB.cardIDEnum.None;
        public CardDB.cardIDEnum revivingEnemyMinion = CardDB.cardIDEnum.None;
        public CardDB.cardIDEnum OwnLastDiedMinion = CardDB.cardIDEnum.None;

        public int shadowmadnessed = 0; //minions has switched controllers this turn.
        //Helpfunctions help = Helpfunctions.Instance;

        //implementation
        public MyHandManager homeHandManager = new MyHandManager();
        public MyHandManager awayHandManager = new MyHandManager();
        public static Random rng = null;

        public List<CardDB.Card> homeDeck = new List<CardDB.Card>();
        public List<CardDB.Card> awayDeck = new List<CardDB.Card>();

        public List<GraveYardItem> graveYard = new List<GraveYardItem>();

        public static Random getRNG()
        {
            if (rng == null)
            {
                rng = new Random();
            }
            return rng;
        }

        public void initDeckFromFile(string filePath, bool isHome)
        {
            string[] deckLines;
            try
            {
                deckLines = System.IO.File.ReadAllLines(filePath);
                foreach (string line in deckLines)
                {
                    string cardNameString = Regex.Replace(line.ToLower(), @"[^a-z]", "");
                    CardDB.Card card = CardDB.Instance.getDeckCardData(CardDB.Instance.cardNamestringToEnum(cardNameString));
                    if (isHome)
                    {
                        homeDeck.Add(card);
                    }
                    else
                    {
                        awayDeck.Add(card);
                    }
                }
            }
            catch
            {
                Console.WriteLine("init deck exception");
            }
        }

        public CardDB.Card getArandomCardFromDeck(bool isHome)
        {
            CardDB.Card cardToDraw;
            if (isHome)
            {
                int r = getRNG().Next(homeDeck.Count);
                cardToDraw = homeDeck[r];
                homeDeck.RemoveAt(r);
            }
            else
            {
                int r = getRNG().Next(awayDeck.Count);
                cardToDraw = awayDeck[r];
                awayDeck.RemoveAt(r);
            }
            return cardToDraw;
        }

        public void drawInitCards()
        {
            //decide who's go first
            //int r = getRNG().Next(2);
            int r = 0;
            if (r == 0)
            { //home go first
                //here is some questions
                List<Handmanager.Handcard> handList = new List<Handmanager.Handcard>();
                for (int i = 0; i < 4; i++)
                {
                    Handmanager.Handcard card = new Handmanager.Handcard();
                    card.card = getArandomCardFromDeck(true);
                    card.entity = getNextEntity();
                    card.addattack = 0;
                    card.manacost = card.card.cost;
                    card.position = i + 1;
                    handList.Add(card);
                }
                homeHandManager.setHandcards(handList, handList.Count, handList.Count + 1);
                addCardsReal(handList);

                handList.Clear();
                for (int i = 0; i < 5; i++)
                {
                    Handmanager.Handcard card = new Handmanager.Handcard();
                    card.card = getArandomCardFromDeck(false);
                    card.entity = getNextEntity();
                    card.addattack = 0;
                    card.manacost = mana;
                    card.position = i + 1;
                    handList.Add(card);
                }
                awayHandManager.setHandcards(handList, handList.Count, handList.Count + 1);
                addEnemyCardsReal(handList);
            }
            else
            {//home go second, with a coin




            }
            //
        }

        //end of implementation

        private void addMinionsReal(List<Minion> source, List<Minion> trgt)
        {
            foreach (Minion m in source)
            {
                trgt.Add(new Minion(m));
            }

        }

        private void addCardsReal(List<Handmanager.Handcard> source)
        {

            foreach (Handmanager.Handcard m in source)
            {
                this.owncards.Add(new Handmanager.Handcard(m));
            }

        }

        private void addEnemyCardsReal(List<Handmanager.Handcard> source)
        {

            foreach (Handmanager.Handcard m in source)
            {
                this.enemycards.Add(new Handmanager.Handcard(m));
            }

        }

        public void createHero()
        {


        }

        public Playfield()
        {
            this.nextEntity = 1000;
            //this.simulateEnemyTurn = Ai.Instance.simulateEnemyTurn;
            this.ownController = 1;

            //this.ownHeroEntity = Hrtprozis.Instance.ownHeroEntity;
            //this.enemyHeroEntity = Hrtprozis.Instance.enemyHeroEntitiy;

            this.mana = 10;
            this.manaTurnEnd = this.mana;
            this.ownMaxMana = 10;
            this.enemyMaxMana = 0;
            this.evaluatePenality = 0;
            //this.ownSecretsIDList.AddRange(Hrtprozis.Instance.ownSecretList);
            this.enemySecretCount = 0;


            this.attackFaceHP = 15;

            this.complete = false;

            //addMinionsReal(Hrtprozis.Instance.ownMinions, ownMinions);
            //addMinionsReal(Hrtprozis.Instance.enemyMinions, enemyMinions);
            this.ownHero = new Minion(Hrtprozis.Instance.ownHero);
            this.enemyHero = new Minion(Hrtprozis.Instance.enemyHero);
            this.ownHero.cardClass = TAG_CLASS.MAGE;
            this.enemyHero.cardClass = TAG_CLASS.MAGE;

            //implementation
            this.ownHero.isHero = true;
            this.enemyHero.isHero = true;
            this.ownHero.own = true;
            this.enemyHero.own = false;
            this.ownHero.maxHp = 30;
            this.enemyHero.maxHp = 30;
            this.ownHero.entitiyID = 1;
            this.enemyHero.entitiyID = 2;

            this.ownHero.Angr = 0;
            this.ownHero.Hp = 30;
            this.ownHero.armor = 0;
            this.ownHero.frozen = false;
            this.ownHero.immuneWhileAttacking = false;
            this.ownHero.immune = false;
            this.ownHero.numAttacksThisTurn = 0;
            this.ownHero.windfury = false;

            this.enemyHero.Angr = 0;
            this.enemyHero.Hp = 30;
            this.enemyHero.armor = 0;
            this.enemyHero.frozen = false;
            this.enemyHero.immuneWhileAttacking = false;
            this.enemyHero.immune = false;
            this.enemyHero.numAttacksThisTurn = 0;
            this.enemyHero.windfury = false;

            //end of implementation

            //addCardsReal(homeHandManager.handCards);

            this.ownHeroName = HeroEnum.mage;
            this.enemyHeroName = HeroEnum.mage;
            this.ownHeroStartClass = TAG_CLASS.MAGE;
            this.enemyHeroStartClass = TAG_CLASS.MAGE;

            /*
            this.enemyHeroHp = Hrtprozis.Instance.enemyHp;
            this.ownHeroHp = Hrtprozis.Instance.heroHp;
            this.ownHeroReady = Hrtprozis.Instance.ownheroisread;
            this.ownHeroWindfury = Hrtprozis.Instance.ownHeroWindfury;
            this.ownHeroNumAttackThisTurn = Hrtprozis.Instance.ownHeroNumAttacksThisTurn;
            this.ownHeroFrozen = Hrtprozis.Instance.herofrozen;
            this.enemyHeroFrozen = Hrtprozis.Instance.enemyfrozen;
            this.ownheroAngr = Hrtprozis.Instance.heroAtk;
            this.heroImmuneWhileAttacking = Hrtprozis.Instance.heroImmuneToDamageWhileAttacking;
            this.ownHeroDefence = Hrtprozis.Instance.heroDefence;
            this.enemyHeroDefence = Hrtprozis.Instance.enemyDefence;
             */

            //####buffs#############################

            this.anzOwnRaidleader = 0;
            this.anzEnemyRaidleader = 0;
            this.anzOwnStormwindChamps = 0;
            this.anzEnemyStormwindChamps = 0;
            this.anzOwnTundrarhino = 0;
            this.anzEnemyTundrarhino = 0;
            this.anzOwnTimberWolfs = 0;
            this.anzEnemyTimberWolfs = 0;
            this.anzMurlocWarleader = 0;
            this.anzGrimscaleOracle = 0;
            this.anzOwnAuchenaiSoulpriest = 0;
            this.anzEnemyAuchenaiSoulpriest = 0;
            this.anzOwnsorcerersapprentice = 0;
            this.anzOwnsorcerersapprenticeStarted = 0;
            this.anzEnemysorcerersapprentice = 0;
            this.anzEnemysorcerersapprenticeStarted = 0;
            this.anzOwnSouthseacaptain = 0;
            this.anzEnemySouthseacaptain = 0;
            this.anzOwnDragonConsortStarted = 0;

            this.anzEnemyTaunt = 0;
            this.ownMinionsDiedTurn = 0;
            this.enemyMinionsDiedTurn = 0;

            this.feugenDead = false;
            this.stalaggDead = false;

            this.weHavePlayedMillhouseManastorm = false;

            this.doublepriest = 0;
            this.enemydoublepriest = 0;

            this.ownBaronRivendare = 0;
            this.enemyBaronRivendare = 0;
            //#########################################

            this.ownWeaponDurability = 0;
            this.ownWeaponAttack = 0;
            this.ownWeaponName = CardDB.cardName.unknown;
            this.owncarddraw = 0;

            this.enemyWeaponAttack = 0;//dont know jet
            this.enemyWeaponName = CardDB.cardName.unknown;
            this.enemyWeaponDurability = 0;
            this.enemycarddraw = 0;

            this.enemyAnzCards = homeHandManager.enemyAnzCards;

            this.ownAbilityReady = true;
            this.ownHeroAblility = new Handmanager.Handcard(CardDB.Instance.getCardData(CardDB.Instance.cardNamestringToEnum("fireblast")));
            this.enemyHeroAblility = new Handmanager.Handcard(CardDB.Instance.getCardData(CardDB.Instance.cardNamestringToEnum("fireblast")));
            this.enemyAbilityReady = false;


            this.mobsplayedThisTurn = 0;
            this.startedWithMobsPlayedThisTurn = 0;// only change mobsplayedthisturm
            this.cardsPlayedThisTurn = 0;
            //todo:
            this.optionsPlayedThisTurn = 0;

            this.ueberladung = 0;

            this.ownHeroFatigue = 0;
            this.enemyHeroFatigue = 0;
            this.ownDeckSize = 30;
            this.enemyDeckSize = 30;

            //need the following for manacost-calculation
            this.ownHeroHpStarted = this.ownHero.Hp;
            this.enemyHeroHpStarted = this.enemyHero.Hp;
            this.ownWeaponAttackStarted = this.ownWeaponAttack;
            this.ownCardsCountStarted = this.owncards.Count;
            this.enemyCardsCountStarted = this.enemyAnzCards;
            this.ownMobsCountStarted = this.ownMinions.Count + this.enemyMinions.Count;

            this.playedmagierinderkirintor = false;
            this.playedPreparation = false;

            this.winzigebeschwoererin = 0;
            this.managespenst = 0;
            this.soeldnerDerVenture = 0;
            this.beschwoerungsportal = 0;
            this.nerubarweblord = 0;

            this.startedWithnerubarweblord = 0;
            this.startedWithbeschwoerungsportal = 0;
            this.startedWithManagespenst = 0;
            this.startedWithWinzigebeschwoererin = 0;
            this.startedWithsoeldnerDerVenture = 0;

            this.ownBaronRivendare = 0;
            this.enemyBaronRivendare = 0;

            needGraveyard = false;
            this.loatheb = false;
            this.spellpower = 0;
            this.enemyspellpower = 0;

            this.startedWithDamagedMinions = false;

            foreach (Minion m in this.ownMinions)
            {
                if (m.Hp < m.maxHp && m.Hp >= 1) this.startedWithDamagedMinions = true;
                if (m.playedThisTurn && m.name == CardDB.cardName.loatheb) this.loatheb = true;

                spellpower = spellpower + m.spellpower;
                if (m.silenced) continue;
                spellpower += m.handcard.card.spellpowervalue;
                if (m.name == CardDB.cardName.prophetvelen) this.doublepriest++;


                if (m.name == CardDB.cardName.pintsizedsummoner)
                {
                    this.winzigebeschwoererin++;
                    this.startedWithWinzigebeschwoererin++;
                }

                if (m.name == CardDB.cardName.manawraith)
                {
                    this.managespenst++;
                    this.startedWithManagespenst++;
                }
                if (m.name == CardDB.cardName.nerubarweblord)
                {
                    this.nerubarweblord++;
                    this.startedWithnerubarweblord++;
                }
                if (m.name == CardDB.cardName.venturecomercenary)
                {
                    this.soeldnerDerVenture++;
                    this.startedWithsoeldnerDerVenture++;
                }
                if (m.name == CardDB.cardName.summoningportal)
                {
                    this.beschwoerungsportal++;
                    this.startedWithbeschwoerungsportal++;
                }

                if (m.handcard.card.name == CardDB.cardName.baronrivendare)
                {
                    this.ownBaronRivendare++;
                }
                if (m.handcard.card.name == CardDB.cardName.kelthuzad)
                {
                    this.needGraveyard = true;
                }

                if (m.name == CardDB.cardName.raidleader) this.anzOwnRaidleader++;
                if (m.name == CardDB.cardName.malganis) this.anzOwnMalGanis++;
                if (m.name == CardDB.cardName.stormwindchampion) this.anzOwnStormwindChamps++;
                if (m.name == CardDB.cardName.tundrarhino) this.anzOwnTundrarhino++;
                if (m.name == CardDB.cardName.timberwolf) this.anzOwnTimberWolfs++;
                if (m.name == CardDB.cardName.murlocwarleader) this.anzMurlocWarleader++;
                if (m.name == CardDB.cardName.grimscaleoracle) this.anzGrimscaleOracle++;
                if (m.name == CardDB.cardName.auchenaisoulpriest) this.anzOwnAuchenaiSoulpriest++;
                if (m.name == CardDB.cardName.sorcerersapprentice)
                {
                    this.anzOwnsorcerersapprentice++;
                    this.anzOwnsorcerersapprenticeStarted++;
                }
                if (m.name == CardDB.cardName.southseacaptain) this.anzOwnSouthseacaptain++;
                if (m.name == CardDB.cardName.chromaggus) this.anzOwnChromaggus++;
                if (m.name == CardDB.cardName.dragonconsort && anzOwnDragonConsort > 0) this.anzOwnDragonConsortStarted++;
                if (m.name == CardDB.cardName.mechwarper)
                {
                    this.anzOwnMechwarper++;
                    this.anzOwnMechwarperStarted++;
                }
                if (m.name == CardDB.cardName.steamwheedlesniper && this.ownHeroName == HeroEnum.hunter)
                {
                    this.weHaveSteamwheedleSniper = true;
                }

            }

            foreach (Handmanager.Handcard hc in this.owncards)
            {

                if (hc.card.name == CardDB.cardName.kelthuzad)
                {
                    this.needGraveyard = true;
                }
            }

            foreach (Minion m in this.enemyMinions)
            {
                this.enemyspellpower = this.enemyspellpower + m.spellpower;
                enemyspellpower += m.handcard.card.spellpowervalue;
                if (m.silenced) continue;
                if (m.taunt) anzEnemyTaunt++;
                if (m.name == CardDB.cardName.prophetvelen) this.enemydoublepriest++;
                if (m.name == CardDB.cardName.manawraith)
                {
                    this.managespenst++;
                    this.startedWithManagespenst++;
                }
                if (m.name == CardDB.cardName.nerubarweblord)
                {
                    this.nerubarweblord++;
                    this.startedWithnerubarweblord++;
                }
                if (m.handcard.card.name == CardDB.cardName.baronrivendare)
                {
                    this.enemyBaronRivendare++;
                }
                if (m.handcard.card.name == CardDB.cardName.kelthuzad)
                {
                    this.needGraveyard = true;
                }

                if (m.name == CardDB.cardName.raidleader) this.anzEnemyRaidleader++;
                if (m.name == CardDB.cardName.malganis) this.anzEnemyMalGanis++;
                if (m.name == CardDB.cardName.stormwindchampion) this.anzEnemyStormwindChamps++;
                if (m.name == CardDB.cardName.tundrarhino) this.anzEnemyTundrarhino++;
                if (m.name == CardDB.cardName.timberwolf) this.anzEnemyTimberWolfs++;
                if (m.name == CardDB.cardName.murlocwarleader) this.anzMurlocWarleader++;
                if (m.name == CardDB.cardName.grimscaleoracle) this.anzGrimscaleOracle++;
                if (m.name == CardDB.cardName.auchenaisoulpriest) this.anzEnemyAuchenaiSoulpriest++;
                if (m.name == CardDB.cardName.sorcerersapprentice)
                {
                    this.anzEnemysorcerersapprentice++;
                    this.anzEnemysorcerersapprenticeStarted++;
                }
                if (m.name == CardDB.cardName.southseacaptain) this.anzEnemySouthseacaptain++;
                if (m.name == CardDB.cardName.chromaggus) this.anzEnemyChromaggus++;
                if (m.name == CardDB.cardName.mechwarper)
                {
                    this.anzEnemyMechwarper++;
                    this.anzEnemyMechwarperStarted++;
                }
                if (m.name == CardDB.cardName.steamwheedlesniper && this.enemyHeroName == HeroEnum.hunter)
                {
                    this.enemyHaveSteamwheedleSniper = true;
                }
            }
            if (this.enemySecretCount >= 1) this.needGraveyard = true;
            if (this.needGraveyard) this.diedMinions = new List<GraveYardItem>(Probabilitymaker.Instance.turngraveyard);

            this.tempanzOwnCards = this.owncards.Count;
            this.tempanzEnemyCards = this.enemyAnzCards;


        }

        public Playfield(Playfield p)
        {

            //implementation
            this.homeHandManager = p.homeHandManager;
            this.awayHandManager = p.awayHandManager;

            this.homeDeck = p.homeDeck;
            this.awayDeck = p.awayDeck;

            this.graveYard = p.graveYard;
            //end of implementation

            this.nextEntity = p.nextEntity;

            this.isOwnTurn = p.isOwnTurn;
            this.turnCounter = p.turnCounter;

            this.attacked = p.attacked;
            this.sEnemTurn = p.sEnemTurn;
            this.ownController = p.ownController;
            //this.ownHeroEntity = p.ownHeroEntity;
            //this.enemyHeroEntity = p.enemyHeroEntity;

            this.evaluatePenality = p.evaluatePenality;
            this.ownSecretsIDList.AddRange(p.ownSecretsIDList);

            this.enemySecretCount = p.enemySecretCount;

            this.enemySecretList.Clear();
            if (Settings.Instance.useSecretsPlayArround)
            {
                foreach (SecretItem si in p.enemySecretList)
                {
                    this.enemySecretList.Add(new SecretItem(si));
                }
            }

            this.mana = p.mana;
            this.manaTurnEnd = p.manaTurnEnd;
            this.ownMaxMana = p.ownMaxMana;
            this.enemyMaxMana = p.enemyMaxMana;
            addMinionsReal(p.ownMinions, ownMinions);
            addMinionsReal(p.enemyMinions, enemyMinions);
            this.ownHero = new Minion(p.ownHero);
            this.enemyHero = new Minion(p.enemyHero);
            addCardsReal(p.owncards);
            addEnemyCardsReal(p.enemycards);

            this.ownHeroName = p.ownHeroName;
            this.enemyHeroName = p.enemyHeroName;

            this.playactions.AddRange(p.playactions);
            this.complete = false;

            this.attackFaceHP = p.attackFaceHP;

            this.owncarddraw = p.owncarddraw;

            this.enemyWeaponAttack = p.enemyWeaponAttack;
            this.enemyWeaponDurability = p.enemyWeaponDurability;
            this.enemyWeaponName = p.enemyWeaponName;
            this.enemycarddraw = p.enemycarddraw;
            this.enemyAnzCards = p.enemyAnzCards;

            this.ownWeaponDurability = p.ownWeaponDurability;
            this.ownWeaponAttack = p.ownWeaponAttack;
            this.ownWeaponName = p.ownWeaponName;

            this.lostDamage = p.lostDamage;
            this.lostWeaponDamage = p.lostWeaponDamage;
            this.lostHeal = p.lostHeal;

            this.ownAbilityReady = p.ownAbilityReady;
            this.enemyAbilityReady = p.enemyAbilityReady;
            this.ownHeroAblility = new Handmanager.Handcard(p.ownHeroAblility);
            this.enemyHeroAblility = new Handmanager.Handcard(p.enemyHeroAblility);

            this.spellpower = 0;
            this.mobsplayedThisTurn = p.mobsplayedThisTurn;
            this.startedWithMobsPlayedThisTurn = p.startedWithMobsPlayedThisTurn;
            this.optionsPlayedThisTurn = p.optionsPlayedThisTurn;
            this.cardsPlayedThisTurn = p.cardsPlayedThisTurn;
            this.ueberladung = p.ueberladung;

            this.ownDeckSize = p.ownDeckSize;
            this.enemyDeckSize = p.enemyDeckSize;
            this.ownHeroFatigue = p.ownHeroFatigue;
            this.enemyHeroFatigue = p.enemyHeroFatigue;

            //need the following for manacost-calculation
            this.ownHeroHpStarted = p.ownHeroHpStarted;
            this.ownWeaponAttackStarted = p.ownWeaponAttackStarted;
            this.ownCardsCountStarted = p.ownCardsCountStarted;
            this.enemyCardsCountStarted = p.enemyCardsCountStarted;
            this.ownMobsCountStarted = p.ownMobsCountStarted;

            this.startedWithWinzigebeschwoererin = p.startedWithWinzigebeschwoererin;
            this.playedmagierinderkirintor = p.playedmagierinderkirintor;

            this.startedWithWinzigebeschwoererin = p.startedWithWinzigebeschwoererin;
            this.startedWithManagespenst = p.startedWithManagespenst;
            this.startedWithsoeldnerDerVenture = p.startedWithsoeldnerDerVenture;
            this.startedWithbeschwoerungsportal = p.startedWithbeschwoerungsportal;
            this.startedWithnerubarweblord = p.startedWithnerubarweblord;

            this.startedWithDamagedMinions = p.startedWithDamagedMinions;

            this.nerubarweblord = p.nerubarweblord;
            this.winzigebeschwoererin = p.winzigebeschwoererin;
            this.managespenst = p.managespenst;
            this.soeldnerDerVenture = p.soeldnerDerVenture;
            this.loatheb = p.loatheb;

            this.spellpower = p.spellpower;
            this.enemyspellpower = p.enemyspellpower;

            this.needGraveyard = p.needGraveyard;
            if (p.needGraveyard) this.diedMinions = new List<GraveYardItem>(p.diedMinions);
            this.OwnLastDiedMinion = p.OwnLastDiedMinion;

            //####buffs#############################

            this.anzOwnRaidleader = p.anzOwnRaidleader;
            this.anzEnemyRaidleader = p.anzEnemyRaidleader;
            this.anzOwnMalGanis = p.anzOwnMalGanis;
            this.anzEnemyMalGanis = p.anzEnemyMalGanis;
            this.anzOwnStormwindChamps = p.anzOwnStormwindChamps;
            this.anzEnemyStormwindChamps = p.anzEnemyStormwindChamps;
            this.anzOwnTundrarhino = p.anzOwnTundrarhino;
            this.anzEnemyTundrarhino = p.anzEnemyTundrarhino;
            this.anzOwnTimberWolfs = p.anzOwnTimberWolfs;
            this.anzEnemyTimberWolfs = p.anzEnemyTimberWolfs;
            this.anzMurlocWarleader = p.anzMurlocWarleader;
            this.anzGrimscaleOracle = p.anzGrimscaleOracle;
            this.anzOwnAuchenaiSoulpriest = p.anzOwnAuchenaiSoulpriest;
            this.anzEnemyAuchenaiSoulpriest = p.anzEnemyAuchenaiSoulpriest;
            this.anzOwnsorcerersapprentice = p.anzOwnsorcerersapprentice;
            this.anzOwnsorcerersapprenticeStarted = p.anzOwnsorcerersapprenticeStarted;
            this.anzEnemysorcerersapprentice = p.anzEnemysorcerersapprentice;
            this.anzEnemysorcerersapprenticeStarted = p.anzEnemysorcerersapprenticeStarted;
            this.anzOwnSouthseacaptain = p.anzOwnSouthseacaptain;
            this.anzEnemySouthseacaptain = p.anzEnemySouthseacaptain;
            this.anzOwnMechwarper = p.anzOwnMechwarper;
            this.anzOwnMechwarperStarted = p.anzOwnMechwarperStarted;
            this.anzEnemyMechwarper = p.anzEnemyMechwarper;
            this.anzEnemyMechwarperStarted = p.anzEnemyMechwarperStarted;
            this.anzOwnChromaggus = p.anzOwnChromaggus;
            this.anzEnemyChromaggus = p.anzEnemyChromaggus;
            this.anzOwnDragonConsort = p.anzOwnDragonConsort;
            this.anzOwnDragonConsortStarted = p.anzOwnDragonConsortStarted;

            this.anzEnemyTaunt = p.anzEnemyTaunt;
            this.ownMinionsDiedTurn = p.ownMinionsDiedTurn;
            this.enemyMinionsDiedTurn = p.enemyMinionsDiedTurn;

            this.feugenDead = p.feugenDead;
            this.stalaggDead = p.stalaggDead;

            this.weHavePlayedMillhouseManastorm = p.weHavePlayedMillhouseManastorm;

            this.doublepriest = p.doublepriest;
            this.enemydoublepriest = p.enemydoublepriest;

            this.ownBaronRivendare = p.ownBaronRivendare;
            this.enemyBaronRivendare = p.enemyBaronRivendare;

            this.weHaveSteamwheedleSniper = p.weHaveSteamwheedleSniper;
            this.enemyHaveSteamwheedleSniper = p.enemyHaveSteamwheedleSniper;
            //#########################################


            this.tempanzOwnCards = this.owncards.Count;
            this.tempanzEnemyCards = this.enemyAnzCards;

        }

        public void copyValuesFrom(Playfield p)
        {

        }

        //public bool isEqual(Playfield p, bool logg)
        //{
        //    if (logg)
        //    {
        //        if (this.value != p.value) return false;
        //    }
        //    if (this.enemySecretCount != p.enemySecretCount)
        //    {

        //        if (logg) Helpfunctions.Instance.logg("enemy secrets changed ");
        //        return false;
        //    }

        //    if (this.enemySecretCount >= 1)
        //    {
        //        for (int i = 0; i < this.enemySecretList.Count; i++)
        //        {
        //            if (!this.enemySecretList[i].isEqual(p.enemySecretList[i]))
        //            {
        //                if (logg) Helpfunctions.Instance.logg("enemy secrets changed! ");
        //                return false;
        //            }
        //        }
        //    }

        //    if (this.mana != p.mana || this.enemyMaxMana != p.enemyMaxMana || this.ownMaxMana != p.ownMaxMana)
        //    {
        //        if (logg) Helpfunctions.Instance.logg("mana changed " + this.mana + " " + p.mana + " " + this.enemyMaxMana + " " + p.enemyMaxMana + " " + this.ownMaxMana + " " + p.ownMaxMana);
        //        return false;
        //    }

        //    if (this.ownDeckSize != p.ownDeckSize || this.enemyDeckSize != p.enemyDeckSize || this.ownHeroFatigue != p.ownHeroFatigue || this.enemyHeroFatigue != p.enemyHeroFatigue)
        //    {
        //        if (logg) Helpfunctions.Instance.logg("deck/fatigue changed " + this.ownDeckSize + " " + p.ownDeckSize + " " + this.enemyDeckSize + " " + p.enemyDeckSize + " " + this.ownHeroFatigue + " " + p.ownHeroFatigue + " " + this.enemyHeroFatigue + " " + p.enemyHeroFatigue);
        //    }

        //    if (this.cardsPlayedThisTurn != p.cardsPlayedThisTurn || this.mobsplayedThisTurn != p.mobsplayedThisTurn || this.ueberladung != p.ueberladung || this.ownAbilityReady != p.ownAbilityReady)
        //    {
        //        if (logg) Helpfunctions.Instance.logg("stuff changed " + this.cardsPlayedThisTurn + " " + p.cardsPlayedThisTurn + " " + this.mobsplayedThisTurn + " " + p.mobsplayedThisTurn + " " + this.ueberladung + " " + p.ueberladung + " " + this.ownAbilityReady + " " + p.ownAbilityReady);
        //        return false;
        //    }

        //    if (this.ownHeroName != p.ownHeroName || this.enemyHeroName != p.enemyHeroName)
        //    {
        //        if (logg) Helpfunctions.Instance.logg("hero name changed ");
        //        return false;
        //    }

        //    if (this.ownHero.Hp != p.ownHero.Hp || this.ownHero.Angr != p.ownHero.Angr || this.ownHero.armor != p.ownHero.armor || this.ownHero.frozen != p.ownHero.frozen || this.ownHero.immuneWhileAttacking != p.ownHero.immuneWhileAttacking || this.ownHero.immune != p.ownHero.immune)
        //    {
        //        if (logg) Helpfunctions.Instance.logg("ownhero changed " + this.ownHero.Hp + " " + p.ownHero.Hp + " " + this.ownHero.Angr + " " + p.ownHero.Angr + " " + this.ownHero.armor + " " + p.ownHero.armor + " " + this.ownHero.frozen + " " + p.ownHero.frozen + " " + this.ownHero.immuneWhileAttacking + " " + p.ownHero.immuneWhileAttacking + " " + this.ownHero.immune + " " + p.ownHero.immune);
        //        return false;
        //    }
        //    if (this.ownHero.Ready != p.ownHero.Ready || this.ownWeaponAttack != p.ownWeaponAttack || this.ownWeaponDurability != p.ownWeaponDurability || this.ownHero.numAttacksThisTurn != p.ownHero.numAttacksThisTurn || this.ownHero.windfury != p.ownHero.windfury)
        //    {
        //        if (logg) Helpfunctions.Instance.logg("weapon changed " + this.ownHero.Ready + " " + p.ownHero.Ready + " " + this.ownWeaponAttack + " " + p.ownWeaponAttack + " " + this.ownWeaponDurability + " " + p.ownWeaponDurability + " " + this.ownHero.numAttacksThisTurn + " " + p.ownHero.numAttacksThisTurn + " " + this.ownHero.windfury + " " + p.ownHero.windfury);
        //        return false;
        //    }
        //    if (this.enemyHero.Hp != p.enemyHero.Hp || this.enemyWeaponAttack != p.enemyWeaponAttack || this.enemyHero.armor != p.enemyHero.armor || this.enemyWeaponDurability != p.enemyWeaponDurability || this.enemyHero.frozen != p.enemyHero.frozen || this.enemyHero.immune != p.enemyHero.immune)
        //    {
        //        if (logg) Helpfunctions.Instance.logg("enemyhero changed " + this.enemyHero.Hp + " " + p.enemyHero.Hp + " " + this.enemyWeaponAttack + " " + p.enemyWeaponAttack + " " + this.enemyHero.armor + " " + p.enemyHero.armor + " " + this.enemyWeaponDurability + " " + p.enemyWeaponDurability + " " + this.enemyHero.frozen + " " + p.enemyHero.frozen + " " + this.enemyHero.immune + " " + p.enemyHero.immune);
        //        return false;
        //    }

        //    /*if (this.auchenaiseelenpriesterin != p.auchenaiseelenpriesterin || this.winzigebeschwoererin != p.winzigebeschwoererin || this.zauberlehrling != p.zauberlehrling || this.managespenst != p.managespenst || this.soeldnerDerVenture != p.soeldnerDerVenture || this.beschwoerungsportal != p.beschwoerungsportal || this.doublepriest != p.doublepriest)
        //    {
        //        Helpfunctions.Instance.logg("special minions changed " + this.auchenaiseelenpriesterin + " " + p.auchenaiseelenpriesterin + " " + this.winzigebeschwoererin + " " + p.winzigebeschwoererin + " " + this.zauberlehrling + " " + p.zauberlehrling + " " + this.managespenst + " " + p.managespenst + " " + this.soeldnerDerVenture + " " + p.soeldnerDerVenture + " " + this.beschwoerungsportal + " " + p.beschwoerungsportal + " " + this.doublepriest + " " + p.doublepriest);
        //        return false;
        //    }*/

        //    if (this.ownHeroAblility.card.name != p.ownHeroAblility.card.name)
        //    {
        //        if (logg) Helpfunctions.Instance.logg("hero ability changed ");
        //        return false;
        //    }

        //    if (this.spellpower != p.spellpower)
        //    {
        //        if (logg) Helpfunctions.Instance.logg("spellpower changed");
        //        return false;
        //    }

        //    if (this.ownMinions.Count != p.ownMinions.Count || this.enemyMinions.Count != p.enemyMinions.Count || this.owncards.Count != p.owncards.Count)
        //    {
        //        if (logg) Helpfunctions.Instance.logg("minions count or hand changed");
        //        return false;
        //    }

        //    bool minionbool = true;
        //    for (int i = 0; i < this.ownMinions.Count; i++)
        //    {
        //        Minion dis = this.ownMinions[i]; Minion pis = p.ownMinions[i];
        //        //if (dis.entitiyID == 0) dis.entitiyID = pis.entitiyID;
        //        //if (pis.entitiyID == 0) pis.entitiyID = dis.entitiyID;
        //        if (dis.name != pis.name) minionbool = false;
        //        if (dis.Angr != pis.Angr || dis.Hp != pis.Hp || dis.maxHp != pis.maxHp || dis.numAttacksThisTurn != pis.numAttacksThisTurn) minionbool = false;
        //        if (dis.Ready != pis.Ready) minionbool = false; // includes frozen, exhaunted
        //        if (dis.playedThisTurn != pis.playedThisTurn) minionbool = false;
        //        if (dis.silenced != pis.silenced || dis.stealth != pis.stealth || dis.taunt != pis.taunt || dis.windfury != pis.windfury || dis.zonepos != pis.zonepos) minionbool = false;
        //        if (dis.divineshild != pis.divineshild || dis.cantLowerHPbelowONE != pis.cantLowerHPbelowONE || dis.immune != pis.immune) minionbool = false;
        //        if (dis.ownBlessingOfWisdom != pis.ownBlessingOfWisdom || dis.enemyBlessingOfWisdom != pis.enemyBlessingOfWisdom) minionbool = false;
        //        if (dis.destroyOnEnemyTurnStart != pis.destroyOnEnemyTurnStart || dis.destroyOnEnemyTurnEnd != pis.destroyOnEnemyTurnEnd || dis.destroyOnOwnTurnEnd != pis.destroyOnOwnTurnEnd || dis.destroyOnOwnTurnStart != pis.destroyOnOwnTurnStart) minionbool = false;
        //        if (dis.ancestralspirit != pis.ancestralspirit || dis.souloftheforest != pis.souloftheforest) minionbool = false;

        //    }
        //    if (minionbool == false)
        //    {
        //        if (logg) Helpfunctions.Instance.logg("ownminions changed");
        //        return false;
        //    }

        //    for (int i = 0; i < this.enemyMinions.Count; i++)
        //    {
        //        Minion dis = this.enemyMinions[i]; Minion pis = p.enemyMinions[i];
        //        //if (dis.entitiyID == 0) dis.entitiyID = pis.entitiyID;
        //        //if (pis.entitiyID == 0) pis.entitiyID = dis.entitiyID;
        //        if (dis.name != pis.name) minionbool = false;
        //        if (dis.Angr != pis.Angr || dis.Hp != pis.Hp || dis.maxHp != pis.maxHp || dis.numAttacksThisTurn != pis.numAttacksThisTurn) minionbool = false;
        //        if (dis.Ready != pis.Ready) minionbool = false; // includes frozen, exhaunted
        //        if (dis.playedThisTurn != pis.playedThisTurn) minionbool = false;
        //        if (dis.silenced != pis.silenced || dis.stealth != pis.stealth || dis.taunt != pis.taunt || dis.windfury != pis.windfury || dis.zonepos != pis.zonepos) minionbool = false;
        //        if (dis.divineshild != pis.divineshild || dis.cantLowerHPbelowONE != pis.cantLowerHPbelowONE || dis.immune != pis.immune) minionbool = false;
        //        if (dis.ownBlessingOfWisdom != pis.ownBlessingOfWisdom || dis.enemyBlessingOfWisdom != pis.enemyBlessingOfWisdom) minionbool = false;
        //        if (dis.destroyOnEnemyTurnStart != pis.destroyOnEnemyTurnStart || dis.destroyOnEnemyTurnEnd != pis.destroyOnEnemyTurnEnd || dis.destroyOnOwnTurnEnd != pis.destroyOnOwnTurnEnd || dis.destroyOnOwnTurnStart != pis.destroyOnOwnTurnStart) minionbool = false;
        //        if (dis.ancestralspirit != pis.ancestralspirit || dis.souloftheforest != pis.souloftheforest) minionbool = false;
        //    }
        //    if (minionbool == false)
        //    {
        //        if (logg) Helpfunctions.Instance.logg("enemyminions changed");
        //        return false;
        //    }

        //    for (int i = 0; i < this.owncards.Count; i++)
        //    {
        //        Handmanager.Handcard dishc = this.owncards[i]; Handmanager.Handcard pishc = p.owncards[i];
        //        if (dishc.position != pishc.position || dishc.entity != pishc.entity || dishc.getManaCost(this) != pishc.getManaCost(p))
        //        {
        //            if (logg) Helpfunctions.Instance.logg("handcard changed: " + dishc.card.name);
        //            return false;
        //        }
        //    }

        //    for (int i = 0; i < this.ownMinions.Count; i++)
        //    {
        //        Minion dis = this.ownMinions[i]; Minion pis = p.ownMinions[i];
        //        if (dis.entitiyID != pis.entitiyID) Ai.Instance.updateEntitiy(pis.entitiyID, dis.entitiyID);

        //    }

        //    for (int i = 0; i < this.enemyMinions.Count; i++)
        //    {
        //        Minion dis = this.enemyMinions[i]; Minion pis = p.enemyMinions[i];
        //        if (dis.entitiyID != pis.entitiyID) Ai.Instance.updateEntitiy(pis.entitiyID, dis.entitiyID);

        //    }
        //    if (this.ownSecretsIDList.Count != p.ownSecretsIDList.Count)
        //    {
        //        if (logg) Helpfunctions.Instance.logg("secretsCount changed");
        //        return false;
        //    }
        //    for (int i = 0; i < this.ownSecretsIDList.Count; i++)
        //    {
        //        if (this.ownSecretsIDList[i] != p.ownSecretsIDList[i])
        //        {
        //            if (logg) Helpfunctions.Instance.logg("secrets changed");
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //public bool isEqualf(Playfield p)
        //{
        //    if (this.value != p.value) return false;

        //    if (this.ownMinions.Count != p.ownMinions.Count || this.enemyMinions.Count != p.enemyMinions.Count || this.owncards.Count != p.owncards.Count) return false;

        //    if (this.cardsPlayedThisTurn != p.cardsPlayedThisTurn || this.mobsplayedThisTurn != p.mobsplayedThisTurn || this.ueberladung != p.ueberladung || this.ownAbilityReady != p.ownAbilityReady) return false;

        //    if (this.mana != p.mana || this.enemyMaxMana != p.enemyMaxMana || this.ownMaxMana != p.ownMaxMana) return false;

        //    if (this.ownHeroName != p.ownHeroName || this.enemyHeroName != p.enemyHeroName || this.enemySecretCount != p.enemySecretCount) return false;

        //    if (this.ownHero.Hp != p.ownHero.Hp || this.ownHero.Angr != p.ownHero.Angr || this.ownHero.armor != p.ownHero.armor || this.ownHero.frozen != p.ownHero.frozen || this.ownHero.immuneWhileAttacking != p.ownHero.immuneWhileAttacking || this.ownHero.immune != p.ownHero.immune) return false;

        //    if (this.ownHero.Ready != p.ownHero.Ready || this.ownWeaponAttack != p.ownWeaponAttack || this.ownWeaponDurability != p.ownWeaponDurability || this.ownHero.numAttacksThisTurn != p.ownHero.numAttacksThisTurn || this.ownHero.windfury != p.ownHero.windfury) return false;

        //    if (this.enemyHero.Hp != p.enemyHero.Hp || this.enemyWeaponAttack != p.enemyWeaponAttack || this.enemyHero.armor != p.enemyHero.armor || this.enemyWeaponDurability != p.enemyWeaponDurability || this.enemyHero.frozen != p.enemyHero.frozen || this.enemyHero.immune != p.enemyHero.immune) return false;

        //    if (this.ownHeroAblility.card.name != p.ownHeroAblility.card.name || this.spellpower != p.spellpower) return false;

        //    bool minionbool = true;
        //    for (int i = 0; i < this.ownMinions.Count; i++)
        //    {
        //        Minion dis = this.ownMinions[i]; Minion pis = p.ownMinions[i];
        //        //if (dis.entitiyID == 0) dis.entitiyID = pis.entitiyID;
        //        //if (pis.entitiyID == 0) pis.entitiyID = dis.entitiyID;
        //        if (dis.entitiyID != pis.entitiyID) minionbool = false;
        //        if (dis.Angr != pis.Angr || dis.Hp != pis.Hp || dis.maxHp != pis.maxHp || dis.numAttacksThisTurn != pis.numAttacksThisTurn) minionbool = false;
        //        if (dis.Ready != pis.Ready) minionbool = false; // includes frozen, exhaunted
        //        if (dis.playedThisTurn != pis.playedThisTurn) minionbool = false;
        //        if (dis.silenced != pis.silenced || dis.stealth != pis.stealth || dis.taunt != pis.taunt || dis.windfury != pis.windfury || dis.zonepos != pis.zonepos) minionbool = false;
        //        if (dis.divineshild != pis.divineshild || dis.cantLowerHPbelowONE != pis.cantLowerHPbelowONE || dis.immune != pis.immune) minionbool = false;
        //        if (dis.ownBlessingOfWisdom != pis.ownBlessingOfWisdom || dis.enemyBlessingOfWisdom != pis.enemyBlessingOfWisdom) minionbool = false;
        //        if (dis.destroyOnEnemyTurnStart != pis.destroyOnEnemyTurnStart || dis.destroyOnEnemyTurnEnd != pis.destroyOnEnemyTurnEnd || dis.destroyOnOwnTurnEnd != pis.destroyOnOwnTurnEnd || dis.destroyOnOwnTurnStart != pis.destroyOnOwnTurnStart) minionbool = false;
        //        if (dis.ancestralspirit != pis.ancestralspirit || dis.souloftheforest != pis.souloftheforest) minionbool = false;
        //        if (minionbool == false) break;
        //    }
        //    if (minionbool == false)
        //    {

        //        return false;
        //    }

        //    for (int i = 0; i < this.enemyMinions.Count; i++)
        //    {
        //        Minion dis = this.enemyMinions[i]; Minion pis = p.enemyMinions[i];
        //        //if (dis.entitiyID == 0) dis.entitiyID = pis.entitiyID;
        //        //if (pis.entitiyID == 0) pis.entitiyID = dis.entitiyID;
        //        if (dis.entitiyID != pis.entitiyID) minionbool = false;
        //        if (dis.Angr != pis.Angr || dis.Hp != pis.Hp || dis.maxHp != pis.maxHp || dis.numAttacksThisTurn != pis.numAttacksThisTurn) minionbool = false;
        //        if (dis.Ready != pis.Ready) minionbool = false; // includes frozen, exhaunted
        //        if (dis.playedThisTurn != pis.playedThisTurn) minionbool = false;
        //        if (dis.silenced != pis.silenced || dis.stealth != pis.stealth || dis.taunt != pis.taunt || dis.windfury != pis.windfury || dis.zonepos != pis.zonepos) minionbool = false;
        //        if (dis.divineshild != pis.divineshild || dis.cantLowerHPbelowONE != pis.cantLowerHPbelowONE || dis.immune != pis.immune) minionbool = false;
        //        if (dis.ownBlessingOfWisdom != pis.ownBlessingOfWisdom || dis.enemyBlessingOfWisdom != pis.enemyBlessingOfWisdom) minionbool = false;
        //        if (dis.destroyOnEnemyTurnStart != pis.destroyOnEnemyTurnStart || dis.destroyOnEnemyTurnEnd != pis.destroyOnEnemyTurnEnd || dis.destroyOnOwnTurnEnd != pis.destroyOnOwnTurnEnd || dis.destroyOnOwnTurnStart != pis.destroyOnOwnTurnStart) minionbool = false;
        //        if (dis.ancestralspirit != pis.ancestralspirit || dis.souloftheforest != pis.souloftheforest) minionbool = false;
        //        if (minionbool == false) break;
        //    }
        //    if (minionbool == false)
        //    {
        //        return false;
        //    }

        //    for (int i = 0; i < this.owncards.Count; i++)
        //    {
        //        Handmanager.Handcard dishc = this.owncards[i]; Handmanager.Handcard pishc = p.owncards[i];
        //        if (dishc.position != pishc.position || dishc.entity != pishc.entity || dishc.manacost != pishc.manacost)
        //        {
        //            return false;
        //        }
        //    }

        //    if (this.enemySecretCount >= 1)
        //    {
        //        for (int i = 0; i < this.enemySecretList.Count; i++)
        //        {
        //            if (!this.enemySecretList[i].isEqual(p.enemySecretList[i]))
        //            {
        //                return false;
        //            }
        //        }
        //    }

        //    if (this.ownSecretsIDList.Count != p.ownSecretsIDList.Count)
        //    {
        //        return false;
        //    }
        //    for (int i = 0; i < this.ownSecretsIDList.Count; i++)
        //    {
        //        if (this.ownSecretsIDList[i] != p.ownSecretsIDList[i])
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        public override int GetHashCode()
        {
            int retval = 0;
            retval += 10000 * this.ownMinions.Count + 100 * this.enemyMinions.Count + 1000 * this.mana + 100000 * (this.ownHero.Hp + this.enemyHero.Hp) + this.owncards.Count + this.enemycarddraw + this.cardsPlayedThisTurn + this.mobsplayedThisTurn + this.ownHero.Angr + this.ownHero.armor + this.ownWeaponAttack + this.enemyWeaponDurability;
            return retval;
        }


        //stuff for playing around enemy aoes
        //public void enemyPlaysAoe(int pprob, int pprob2)
        //{
        //    if (!this.loatheb)
        //    {
        //        MyPlayfield p = new MyPlayfield(this);
        //        float oldval = Ai.Instance.botBase.getPlayfieldValue(p);
        //        p.value = int.MinValue;
        //        p.EnemyCardPlaying(p.enemyHeroStartClass, p.mana, p.enemyAnzCards, pprob, pprob2);
        //        float newval = Ai.Instance.botBase.getPlayfieldValue(p);
        //        p.value = int.MinValue;
        //        if (oldval > newval) // new board is better for enemy (value is smaller)
        //        {
        //            this.copyValuesFrom(p);
        //        }
        //    }
        //}

        //public int EnemyCardPlaying(TAG_CLASS enemyHeroStrtClass, int currmana, int cardcount, int playAroundProb, int pap2)
        //{
        //    int mana = currmana;
        //    if (cardcount == 0) return currmana;

        //    bool useAOE = false;
        //    int mobscount = 0;
        //    foreach (Minion min in this.ownMinions)
        //    {
        //        if (min.maxHp >= 2 && min.Angr >= 2) mobscount++;
        //    }

        //    if (mobscount >= 3) useAOE = true;

        //    if (enemyHeroStrtClass == TAG_CLASS.WARRIOR)
        //    {
        //        bool usewhirlwind = true;
        //        foreach (Minion m in this.enemyMinions)
        //        {
        //            if (m.Hp == 1) usewhirlwind = false;
        //        }
        //        if (this.ownMinions.Count <= 3) usewhirlwind = false;

        //        if (usewhirlwind)
        //        {
        //            mana = EnemyPlaysACard(CardDB.cardName.whirlwind, mana, playAroundProb, pap2);
        //        }
        //    }

        //    if (!useAOE) return mana;

        //    if (enemyHeroStrtClass == TAG_CLASS.MAGE)
        //    {
        //        mana = EnemyPlaysACard(CardDB.cardName.flamestrike, mana, playAroundProb, pap2);
        //        mana = EnemyPlaysACard(CardDB.cardName.blizzard, mana, playAroundProb, pap2);
        //    }

        //    if (enemyHeroStrtClass == TAG_CLASS.HUNTER)
        //    {
        //        mana = EnemyPlaysACard(CardDB.cardName.unleashthehounds, mana, playAroundProb, pap2);
        //    }

        //    if (enemyHeroStrtClass == TAG_CLASS.PRIEST)
        //    {
        //        mana = EnemyPlaysACard(CardDB.cardName.holynova, mana, playAroundProb, pap2);
        //    }

        //    if (enemyHeroStrtClass == TAG_CLASS.SHAMAN)
        //    {
        //        mana = EnemyPlaysACard(CardDB.cardName.lightningstorm, mana, playAroundProb, pap2);
        //    }

        //    if (enemyHeroStrtClass == TAG_CLASS.PALADIN)
        //    {
        //        mana = EnemyPlaysACard(CardDB.cardName.consecration, mana, playAroundProb, pap2);
        //    }

        //    if (enemyHeroStrtClass == TAG_CLASS.DRUID)
        //    {
        //        mana = EnemyPlaysACard(CardDB.cardName.swipe, mana, playAroundProb, pap2);
        //    }



        //    return mana;
        //}

        //public int EnemyPlaysACard(CardDB.cardName cardname, int currmana, int playAroundProb, int pap2)
        //{

        //    //todo manacosts
        //    if (cardname == CardDB.cardName.flamestrike && currmana >= 7)
        //    {
        //        bool dontkill = false;
        //        int prob = Probabilitymaker.Instance.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.CS2_032, this.enemyAnzCards, this.enemyDeckSize);
        //        if (playAroundProb > prob) return currmana;
        //        if (pap2 > prob) dontkill = true;

        //        List<Minion> temp = this.ownMinions;
        //        int damage = getEnemySpellDamageDamage(4);
        //        foreach (Minion enemy in temp)
        //        {
        //            enemy.cantLowerHPbelowONE = dontkill;
        //            this.minionGetDamageOrHeal(enemy, damage);
        //            enemy.cantLowerHPbelowONE = false;
        //        }

        //        currmana -= 7;
        //        return currmana;
        //    }

        //    if (cardname == CardDB.cardName.blizzard && currmana >= 6)
        //    {
        //        bool dontkill = false;
        //        int prob = Probabilitymaker.Instance.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.CS2_028, this.enemyAnzCards, this.enemyDeckSize);
        //        if (playAroundProb > prob) return currmana;
        //        if (pap2 > prob) dontkill = true;

        //        List<Minion> temp = this.ownMinions;
        //        int damage = getEnemySpellDamageDamage(2);
        //        foreach (Minion enemy in temp)
        //        {
        //            enemy.frozen = true;
        //            enemy.cantLowerHPbelowONE = dontkill;
        //            this.minionGetDamageOrHeal(enemy, damage);
        //            enemy.cantLowerHPbelowONE = false;
        //        }

        //        currmana -= 6;
        //        return currmana;
        //    }


        //    if (cardname == CardDB.cardName.unleashthehounds && currmana >= 5)
        //    {
        //        bool dontkill = false;
        //        int prob = Probabilitymaker.Instance.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.EX1_538, this.enemyAnzCards, this.enemyDeckSize);
        //        if (playAroundProb > prob) return currmana;
        //        if (pap2 > prob) dontkill = true;

        //        int anz = this.ownMinions.Count;
        //        int posi = this.enemyMinions.Count - 1;
        //        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_538t);//hound
        //        for (int i = 0; i < anz; i++)
        //        {
        //            callKid(kid, posi, false);
        //        }
        //        currmana -= 5;
        //        return currmana;
        //    }





        //    if (cardname == CardDB.cardName.holynova && currmana >= 5)
        //    {
        //        bool dontkill = false;
        //        int prob = Probabilitymaker.Instance.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.CS1_112, this.enemyAnzCards, this.enemyDeckSize);
        //        if (playAroundProb > prob) return currmana;
        //        if (pap2 > prob) dontkill = true;

        //        List<Minion> temp = this.enemyMinions;
        //        int heal = getEnemySpellHeal(2);
        //        int damage = getEnemySpellDamageDamage(2);
        //        foreach (Minion enemy in temp)
        //        {
        //            this.minionGetDamageOrHeal(enemy, -heal);
        //        }
        //        this.minionGetDamageOrHeal(this.enemyHero, -heal);
        //        temp = this.ownMinions;
        //        foreach (Minion enemy in temp)
        //        {
        //            enemy.cantLowerHPbelowONE = dontkill;
        //            this.minionGetDamageOrHeal(enemy, damage);
        //            enemy.cantLowerHPbelowONE = false;
        //        }
        //        this.minionGetDamageOrHeal(this.ownHero, damage);
        //        currmana -= 5;
        //        return currmana;
        //    }




        //    if (cardname == CardDB.cardName.lightningstorm && currmana >= 4)//3
        //    {
        //        bool dontkill = false;
        //        int prob = Probabilitymaker.Instance.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.EX1_259, this.enemyAnzCards, this.enemyDeckSize);
        //        if (playAroundProb > prob) return currmana;
        //        if (pap2 > prob) dontkill = true;

        //        List<Minion> temp = this.ownMinions;
        //        int damage = getEnemySpellDamageDamage(3);
        //        foreach (Minion enemy in temp)
        //        {
        //            enemy.cantLowerHPbelowONE = dontkill;
        //            this.minionGetDamageOrHeal(enemy, damage);
        //            enemy.cantLowerHPbelowONE = false;
        //        }
        //        currmana -= 3;
        //        return currmana;
        //    }



        //    if (cardname == CardDB.cardName.whirlwind && currmana >= 3)//1
        //    {
        //        bool dontkill = false;
        //        int prob = Probabilitymaker.Instance.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.EX1_400, this.enemyAnzCards, this.enemyDeckSize);
        //        if (playAroundProb > prob) return currmana;
        //        if (pap2 > prob) dontkill = true;

        //        List<Minion> temp = this.enemyMinions;
        //        int damage = getEnemySpellDamageDamage(1);
        //        foreach (Minion enemy in temp)
        //        {
        //            this.minionGetDamageOrHeal(enemy, damage);
        //        }
        //        temp = this.ownMinions;
        //        foreach (Minion enemy in temp)
        //        {
        //            enemy.cantLowerHPbelowONE = dontkill;
        //            this.minionGetDamageOrHeal(enemy, damage);
        //            enemy.cantLowerHPbelowONE = false;
        //        }
        //        currmana -= 1;
        //        return currmana;
        //    }



        //    if (cardname == CardDB.cardName.consecration && currmana >= 4)
        //    {
        //        bool dontkill = false;
        //        int prob = Probabilitymaker.Instance.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.CS2_093, this.enemyAnzCards, this.enemyDeckSize);
        //        if (playAroundProb > prob) return currmana;
        //        if (pap2 > prob) dontkill = true;

        //        List<Minion> temp = this.ownMinions;
        //        int damage = getEnemySpellDamageDamage(2);
        //        foreach (Minion enemy in temp)
        //        {
        //            enemy.cantLowerHPbelowONE = dontkill;
        //            this.minionGetDamageOrHeal(enemy, damage);
        //            enemy.cantLowerHPbelowONE = false;
        //        }

        //        this.minionGetDamageOrHeal(this.ownHero, damage);
        //        currmana -= 4;
        //        return currmana;
        //    }



        //    if (cardname == CardDB.cardName.swipe && currmana >= 4)
        //    {
        //        bool dontkill = false;
        //        int prob = Probabilitymaker.Instance.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.CS2_012, this.enemyAnzCards, this.enemyDeckSize);
        //        if (playAroundProb > prob) return currmana;
        //        if (pap2 > prob) dontkill = true;

        //        int damage = getEnemySpellDamageDamage(4);
        //        // all others get 1 spelldamage
        //        int damage1 = getEnemySpellDamageDamage(1);

        //        List<Minion> temp = this.ownMinions;
        //        Minion target = null;
        //        foreach (Minion mnn in temp)
        //        {
        //            if (mnn.Hp <= damage || mnn.handcard.card.isSpecialMinion || target == null)
        //            {
        //                target = mnn;
        //            }
        //        }
        //        foreach (Minion mnn in temp.ToArray())
        //        {
        //            if (mnn.entitiyID == target.entitiyID)
        //            {
        //                mnn.cantLowerHPbelowONE = dontkill;
        //                this.minionGetDamageOrHeal(mnn, damage1);
        //                mnn.cantLowerHPbelowONE = false;
        //            }
        //            else
        //            {
        //                mnn.cantLowerHPbelowONE = dontkill;
        //                this.minionGetDamageOrHeal(mnn, damage);
        //                mnn.cantLowerHPbelowONE = false;
        //            }
        //        }
        //        currmana -= 4;
        //        return currmana;
        //    }





        //    return currmana;
        //}

        public int getNextEntity()
        {
            //i dont trust return this.nextEntity++; !!!
            int retval = this.nextEntity;
            this.nextEntity++;
            return retval;
        }


        // get all minions which are attackable
        public List<Minion> getAttackTargets(bool own, bool isLethalCheck)
        {
            List<Minion> trgts = new List<Minion>();
            List<Minion> trgts2 = new List<Minion>();

            List<Minion> temp = (own) ? this.enemyMinions : this.ownMinions;
            bool hasTaunts = false;
            foreach (Minion m in temp)
            {
                if (m.stealth) continue; // cant target stealth

                if (m.taunt)
                {
                    hasTaunts = true;
                    trgts.Add(m);
                }
                else
                {
                    trgts2.Add(m);
                }
            }
            if (hasTaunts) return trgts;

            if (isLethalCheck) trgts2.Clear(); // only target enemy hero during Lethal check!

            if (own && !this.enemyHero.immune) trgts2.Add(this.enemyHero);
            else if (!own && !this.ownHero.immune) trgts2.Add(this.ownHero);
            return trgts2;
        }

        //public int getBestPlace(CardDB.Card card, bool lethal)
        //{
        //    //we return the zonepos!
        //    if (card.type != CardDB.cardtype.MOB) return 1;
        //    if (this.ownMinions.Count == 0) return 1;
        //    if (this.ownMinions.Count == 1) return 2;

        //    int[] places = new int[this.ownMinions.Count];
        //    int i = 0;
        //    int tempval = 0;
        //    if (lethal && (card.name == CardDB.cardName.defenderofargus || card.name == CardDB.cardName.flametonguetotem || card.name == CardDB.cardName.direwolfalpha || card.name == CardDB.cardName.ancientmage))
        //    {
        //        i = 0;
        //        foreach (Minion m in this.ownMinions)
        //        {

        //            places[i] = 0;
        //            tempval = 0;
        //            if (m.Ready)
        //            {
        //                tempval -= m.Angr - 1;
        //                if (m.windfury) tempval -= m.Angr - 1;
        //            }
        //            places[i] = tempval;

        //            i++;
        //        }


        //        i = 0;
        //        int bestpl = 7;
        //        int bestval = 10000;
        //        foreach (Minion m in this.ownMinions)
        //        {
        //            int prev = 0;
        //            int next = 0;
        //            if (i >= 1) prev = places[i - 1];
        //            next = places[i];
        //            if (bestval > prev + next)
        //            {
        //                bestval = prev + next;
        //                bestpl = i;
        //            }
        //            i++;
        //        }
        //        return bestpl + 1;
        //    }
        //    if (card.name == CardDB.cardName.sunfuryprotector || card.name == CardDB.cardName.defenderofargus) // bestplace, if right and left minions have no taunt + lots of hp, dont make priority-minions to taunt
        //    {
        //        if (lethal) return 1;

        //        i = 0;
        //        foreach (Minion m in this.ownMinions)
        //        {

        //            places[i] = 0;
        //            tempval = 0;
        //            if (!m.taunt)
        //            {
        //                tempval -= m.Hp;
        //            }
        //            else
        //            {
        //                tempval -= m.Hp - 2;
        //            }

        //            if (m.windfury)
        //            {
        //                tempval += 2;
        //            }

        //            tempval += PenalityManager.Instance.getValueOfUsefulNeedKeepPriority(m.handcard.card.name);
        //            places[i] = tempval;
        //            i++;
        //        }


        //        i = 0;
        //        int bestpl = 7;
        //        int bestval = 10000;
        //        foreach (Minion m in this.ownMinions)
        //        {
        //            int prev = 0;
        //            int next = 0;
        //            if (i >= 1) prev = places[i - 1];
        //            next = places[i];
        //            if (bestval > prev + next)
        //            {
        //                bestval = prev + next;
        //                bestpl = i;
        //            }
        //            i++;
        //        }
        //        return bestpl + 1;
        //    }

        //    int cardIsBuffer = 0;
        //    bool placebuff = false;
        //    if (card.name == CardDB.cardName.flametonguetotem || card.name == CardDB.cardName.direwolfalpha)
        //    {
        //        placebuff = true;
        //        if (card.name == CardDB.cardName.flametonguetotem) cardIsBuffer = 2;
        //        if (card.name == CardDB.cardName.direwolfalpha) cardIsBuffer = 1;
        //    }
        //    bool commander = false;
        //    foreach (Minion m in this.ownMinions)
        //    {
        //        if (m.handcard.card.name == CardDB.cardName.warsongcommander) commander = true;
        //        if (m.handcard.card.name == CardDB.cardName.flametonguetotem || m.handcard.card.name == CardDB.cardName.direwolfalpha) placebuff = true;
        //    }
        //    //attackmaxing :D
        //    if (placebuff)
        //    {


        //        int cval = 0;
        //        if (card.Charge || (card.Attack <= 3 && commander))
        //        {
        //            cval = card.Attack;
        //            if (card.windfury) cval = card.Attack;
        //        }
        //        if (card.name == CardDB.cardName.nerubianegg)
        //        {
        //            cval += 1;
        //        }
        //        cval++;
        //        i = 0;
        //        int[] buffplaces = new int[this.ownMinions.Count];
        //        int[] whirlwindplaces = new int[this.ownMinions.Count];
        //        int gesval = 0;
        //        int minionsBefore = 0;
        //        foreach (Minion m in this.ownMinions)
        //        {
        //            buffplaces[i] = 0;
        //            whirlwindplaces[i] = 1;
        //            places[i] = 0;
        //            tempval = -1;
        //            if (!m.Ready && m.Angr == 0 && !m.playedThisTurn) tempval = 0;
        //            if (m.Ready)
        //            {
        //                tempval = m.Angr;
        //                if (m.windfury && m.numAttacksThisTurn == 0)
        //                {
        //                    tempval += m.Angr;
        //                    whirlwindplaces[i] = 2;
        //                }


        //            }
        //            if (m.handcard.card.name == CardDB.cardName.flametonguetotem)
        //            {
        //                buffplaces[i] = 2;
        //                minionsBefore = i;
        //            }
        //            if (m.handcard.card.name == CardDB.cardName.direwolfalpha)
        //            {
        //                buffplaces[i] = 1;
        //                minionsBefore = i;
        //            }
        //            tempval++;
        //            places[i] = tempval;
        //            gesval += tempval;
        //            i++;
        //        }
        //        //gesval = whole possible damage
        //        int bplace = 0;
        //        int bvale = 0;
        //        int somebplace = 0;
        //        int somebvale = 0;
        //        tempval = 0;
        //        i = 0;
        //        for (int j = 0; j <= this.ownMinions.Count; j++)
        //        {
        //            tempval = gesval;
        //            int current = cval;
        //            int prev = 0;
        //            int next = 0;
        //            if (i >= 1)
        //            {
        //                tempval -= places[i - 1];
        //                prev = places[i - 1];
        //                if (prev >= 0) prev += whirlwindplaces[i - 1] * cardIsBuffer;
        //                if (current > 0) current += buffplaces[i - 1];

        //                if (i < this.ownMinions.Count)
        //                {
        //                    prev -= whirlwindplaces[i - 1] * buffplaces[i];
        //                }
        //            }
        //            if (i < this.ownMinions.Count)
        //            {
        //                tempval -= places[i];
        //                next = places[i];
        //                if (next >= 0) next += whirlwindplaces[i] * cardIsBuffer;
        //                if (current > 0) current += buffplaces[i];
        //                if (i >= 1)
        //                {
        //                    next -= whirlwindplaces[i] * buffplaces[i - 1];
        //                }
        //            }
        //            tempval += current + prev + next;
        //            if (tempval > bvale)
        //            {
        //                bplace = i;
        //                bvale = tempval;
        //                somebplace = i;
        //                somebvale = tempval;
        //            }
        //            else if (tempval == somebvale)
        //            {
        //                somebplace = i;
        //            }

        //            i++;
        //        }
        //        if ((bplace != somebplace) && (minionsBefore > this.ownMinions.Count - somebplace)) bplace = somebplace;
        //        return bplace + 1;

        //    }

        //    // normal placement
        //    int cardvalue = card.Attack * 2 + card.Health;
        //    if (card.tank)
        //    {
        //        cardvalue += 5;
        //        cardvalue += card.Health;
        //    }

        //    cardvalue += PenalityManager.Instance.getValueOfUsefulNeedKeepPriority(card.name); //  add a custom placement rules (replace it)
        //    cardvalue += 1;

        //    i = 0;
        //    foreach (Minion m in this.ownMinions)
        //    {
        //        places[i] = 0;
        //        tempval = m.Angr * 2 + m.maxHp;
        //        if (m.taunt)
        //        {
        //            tempval += 6;
        //            tempval += m.maxHp;
        //        }
        //        if (!m.silenced)
        //        {
        //            tempval += PenalityManager.Instance.getValueOfUsefulNeedKeepPriority(m.handcard.card.name);
        //            if (m.stealth) tempval += 40;
        //        }
        //        places[i] = tempval;

        //        i++;
        //    }

        //    //bigminion if >=10
        //    int bestplace = 0;
        //    int bestvale = 0;
        //    tempval = 0;
        //    i = 0;
        //    for (int j = 0; j <= this.ownMinions.Count; j++)
        //    {
        //        int prev = cardvalue;
        //        int next = cardvalue;
        //        if (i >= 1) prev = places[i - 1];
        //        if (i < this.ownMinions.Count) next = places[i];


        //        if (cardvalue >= prev && cardvalue >= next)
        //        {
        //            tempval = 2 * cardvalue - prev - next;
        //            if (tempval > bestvale)
        //            {
        //                bestplace = i;
        //                bestvale = tempval;
        //            }
        //        }
        //        if (cardvalue <= prev && cardvalue <= next)
        //        {
        //            tempval = -2 * cardvalue + prev + next;
        //            if (tempval > bestvale)
        //            {
        //                bestplace = i;
        //                bestvale = tempval;
        //            }
        //        }

        //        i++;
        //    }

        //    return bestplace + 1;
        //}

        public void guessHeroDamage()
        {
            int ghd = 0;
            foreach (Minion m in this.enemyMinions)
            {
                if (m.frozen) continue;
                if (m.name == CardDB.cardName.ancientwatcher && !m.silenced)
                {
                    continue;
                }
                ghd += m.Angr;
                if (m.windfury) ghd += m.Angr;
            }

            if (!this.enemyHero.frozen)
            {
                if (this.enemyWeaponAttack >= 1)
                {
                    ghd += enemyWeaponAttack;
                    if (this.enemyHero.windfury || this.enemyWeaponName == CardDB.cardName.doomhammer) ghd += enemyWeaponAttack;
                }
                else
                {
                    if (this.enemyHeroName == HeroEnum.druid) ghd++;
                    if (this.enemyHeroName == HeroEnum.thief) ghd++;
                }
            }

            if (this.enemyHeroName == HeroEnum.mage) ghd++;
            if (this.enemyHeroName == HeroEnum.hunter) ghd += 2;
            if (this.enemyHeroName == HeroEnum.ragnarosthefirelord) ghd += 8;


            foreach (Minion m in this.ownMinions)
            {
                if (m.taunt) ghd -= m.Hp;
                if (m.taunt && m.divineshild) ghd -= 1;
            }

            int guessingHeroDamage = Math.Max(0, ghd);
            if (this.ownHero.immune) guessingHeroDamage = 0;
            this.guessingHeroHP = this.ownHero.Hp + this.ownHero.armor - guessingHeroDamage;
        }

        public void simulateTraps()
        {
            //todo rework this
            // DONT KILL ENEMY HERO (cause its only guessing)
            foreach (CardDB.cardIDEnum secretID in this.ownSecretsIDList)
            {
                //hunter secrets############
                if (secretID == CardDB.cardIDEnum.EX1_554) //snaketrap
                {

                    //call 3 snakes (if possible)
                    int posi = this.ownMinions.Count - 1;
                    CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_554t);//snake
                    callKid(kid, posi, true);
                    callKid(kid, posi, true);
                    callKid(kid, posi, true);
                }
                if (secretID == CardDB.cardIDEnum.EX1_609) //snipe
                {
                    //kill weakest minion of enemy
                    List<Minion> temp = new List<Minion>(this.enemyMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    minionGetDamageOrHeal(m, 4);
                }
                if (secretID == CardDB.cardIDEnum.EX1_610) //explosive trap
                {
                    //take 2 damage to each enemy
                    List<Minion> temp = this.enemyMinions;
                    foreach (Minion m in temp)
                    {
                        minionGetDamageOrHeal(m, 2);
                    }
                    attackEnemyHeroWithoutKill(2);
                }
                if (secretID == CardDB.cardIDEnum.EX1_611) //freezing trap
                {
                    //return weakest enemy minion to hand
                    List<Minion> temp = new List<Minion>(this.enemyMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    minionReturnToHand(m, false, 0);
                }
                if (secretID == CardDB.cardIDEnum.EX1_533) // missdirection
                {
                    // first damage to your hero is nulled -> lower guessingHeroDamage
                    List<Minion> temp = new List<Minion>(this.enemyMinions);
                    temp.Sort((a, b) => -a.Angr.CompareTo(b.Angr));//take the strongest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    m.Angr = 0;
                    this.evaluatePenality -= this.enemyMinions.Count;// the more the enemy minions has on board, the more the posibility to destroy something other :D
                }

                //mage secrets############
                if (secretID == CardDB.cardIDEnum.EX1_287) //counterspell
                {
                    // what should we do?
                    this.evaluatePenality -= 8;
                }

                if (secretID == CardDB.cardIDEnum.EX1_289) //ice barrier
                {
                    this.ownHero.armor += 8;
                }

                if (secretID == CardDB.cardIDEnum.EX1_295) //ice block
                {
                    //set the guessed Damage to zero
                    this.ownHero.immune = true;
                }

                if (secretID == CardDB.cardIDEnum.EX1_294) //mirror entity
                {
                    //summon snake ( a weak minion)
                    int posi = this.ownMinions.Count - 1;
                    CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_554t);//snake
                    callKid(kid, posi, true);
                }
                if (secretID == CardDB.cardIDEnum.tt_010) //spellbender
                {
                    //whut???
                    // add 2 to your defence (most attack-buffs give +2, lots of damage spells too)
                    this.evaluatePenality -= 4;
                }
                if (secretID == CardDB.cardIDEnum.EX1_594) // vaporize
                {
                    // first damage to your hero is nulled -> lower guessingHeroDamage and destroy weakest minion
                    List<Minion> temp = new List<Minion>(this.enemyMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    minionGetDestroyed(m);
                }
                if (secretID == CardDB.cardIDEnum.FP1_018) // duplicate
                {
                    // first damage to your hero is nulled -> lower guessingHeroDamage and destroy weakest minion
                    List<Minion> temp = new List<Minion>(this.ownMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    drawACard(m.name, true);
                    drawACard(m.name, true);
                }

                //pala secrets############
                if (secretID == CardDB.cardIDEnum.EX1_132) // eye for an eye
                {
                    // enemy takes one damage
                    List<Minion> temp = new List<Minion>(this.enemyMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    attackEnemyHeroWithoutKill(m.Angr);
                }
                if (secretID == CardDB.cardIDEnum.EX1_130) // noble sacrifice
                {
                    //spawn a 2/1 taunt!
                    int posi = this.ownMinions.Count - 1;
                    CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_130a);
                    callKid(kid, posi, true);

                }

                if (secretID == CardDB.cardIDEnum.EX1_136) // redemption
                {
                    // we give our weakest minion a divine shield :D
                    List<Minion> temp = new List<Minion>(this.ownMinions);
                    temp.Sort((a, b) => a.Hp.CompareTo(b.Hp));//take the weakest
                    if (temp.Count == 0) continue;
                    foreach (Minion m in temp)
                    {
                        if (m.divineshild) continue;
                        m.divineshild = true;
                        break;
                    }
                }

                if (secretID == CardDB.cardIDEnum.EX1_379) // repentance
                {
                    // set his current lowest hp minion to x/1
                    List<Minion> temp = new List<Minion>(this.enemyMinions);
                    temp.Sort((a, b) => a.Hp.CompareTo(b.Hp));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    m.Hp = 1;
                    m.maxHp = 1;
                }

                if (secretID == CardDB.cardIDEnum.FP1_020) // avenge
                {
                    // we give our weakest minion +3/+2 :D
                    List<Minion> temp = new List<Minion>(this.ownMinions);
                    temp.Sort((a, b) => a.Hp.CompareTo(b.Hp));//take the weakest
                    if (temp.Count == 0) continue;
                    foreach (Minion m in temp)
                    {
                        minionGetBuffed(m, 3, 2);
                        break;
                    }
                }
            }

            this.doDmgTriggers();
        }



        //old one, will be replaced soon
        public void endTurn(bool simulateTwoTurns, bool playaround, bool print = false, int pprob = 0, int pprob2 = 0)
        {
            this.value = int.MinValue;
            if (this.turnCounter == 0) this.manaTurnEnd = this.mana;
            this.turnCounter++;
            //penalty for destroying combo

            //this.evaluatePenality += ComboBreaker.Instance.checkIfComboWasPlayed(this.playactions, this.ownWeaponName, this.ownHeroName);

            if (this.complete) return;
            this.triggerEndTurn(this.isOwnTurn);
            this.isOwnTurn = !this.isOwnTurn;
            this.triggerStartTurn(this.isOwnTurn);
            this.optionsPlayedThisTurn = 0;
            if (!this.isOwnTurn) simulateTraps();

            if (!sEnemTurn)
            {
                guessHeroDamage();
                this.triggerEndTurn(false);
                this.triggerStartTurn(true);
                this.complete = true;
            }
            else
            {
                guessHeroDamage();
                /*
                if (this.guessingHeroHP >= 1)
                {
                    //simulateEnemysTurn(simulateTwoTurns, playaround, print, pprob, pprob2);
                    this.prepareNextTurn(this.isOwnTurn);

                    if (this.turnCounter >= 2)
                        Ai.Instance.enemySecondTurnSim.simulateEnemysTurn(this, simulateTwoTurns, playaround, print, pprob, pprob2);
                    else
                        Ai.Instance.enemyTurnSim.simulateEnemysTurn(this, simulateTwoTurns, playaround, print, pprob, pprob2);
                }
                this.complete = true;*/
            }

        }

        //prepares the turn for the next player
        public void prepareNextTurn(bool own)
        {
            //call this after start turn trigger!

            if (own)
            {
                this.ownMaxMana = Math.Min(10, this.ownMaxMana + 1);
                this.mana = this.ownMaxMana - this.ueberladung;
                foreach (Minion m in ownMinions)
                {
                    m.numAttacksThisTurn = 0;
                    m.playedThisTurn = false;
                    m.updateReadyness();

                    if (m.concedal)
                    {
                        m.concedal = false;
                        m.stealth = false;
                    }

                }
                //unfreeze the enemy minions
                foreach (Minion m in enemyMinions)
                {
                    m.frozen = false;
                }
                this.enemyHero.frozen = false;


                this.ownHero.Angr = this.ownWeaponAttack;
                this.ownHero.numAttacksThisTurn = 0;
                this.ownAbilityReady = true;
                this.ownHero.updateReadyness();
                this.cardsPlayedThisTurn = 0;
                this.mobsplayedThisTurn = 0;
                this.playedPreparation = false;
                this.playedmagierinderkirintor = false;
                this.optionsPlayedThisTurn = 0;
                this.owncarddraw = 0;
                this.sEnemTurn = false;
            }
            else
            {

                this.enemyMaxMana = Math.Min(10, this.enemyMaxMana + 1);
                this.mana = this.enemyMaxMana; ;//todo enemy overload -this.ueberladung;

                foreach (Minion m in enemyMinions)
                {
                    m.numAttacksThisTurn = 0;
                    m.playedThisTurn = false;
                    m.updateReadyness();
                    if (m.concedal)
                    {
                        m.concedal = false;
                        m.stealth = false;
                    }
                }
                //unfreeze the own minions
                foreach (Minion m in ownMinions)
                {
                    m.frozen = false;
                }
                this.ownHero.frozen = false;

                this.enemyHero.Angr = this.enemyWeaponAttack;
                this.enemyHero.numAttacksThisTurn = 0;
                this.enemyAbilityReady = true;
                this.enemyHero.updateReadyness();
                this.enemyOptionsDoneThisTurn = 0;

                this.cardsPlayedThisTurn = 0;
                this.mobsplayedThisTurn = 0;
                this.playedPreparation = false;
                this.playedmagierinderkirintor = false;

                this.sEnemTurn = false;
            }


            this.ueberladung = 0;
            this.complete = false;

            this.value = int.MinValue;
            if (this.diedMinions != null) this.diedMinions.Clear();//contains only the minions that died in this turn! //implementation
        }

        public void endEnemyTurn()
        {
            this.triggerEndTurn(false);
            this.turnCounter++;
            this.isOwnTurn = true;
            this.triggerStartTurn(true);
            this.complete = true;
            //Ai.Instance.botBase.getPlayfieldValue(this);

        }

        //spelldamage calculation---------------------------------------------------
        public int getSpellDamageDamage(int dmg)
        {
            int retval = dmg;
            retval += this.spellpower;
            if (this.doublepriest >= 1) retval *= (2 * this.doublepriest);
            return retval;
        }

        public int getSpellHeal(int heal)
        {
            int retval = heal;
            if (this.anzOwnAuchenaiSoulpriest >= 1)
            {
                retval *= -1;
                retval -= this.spellpower;
            }
            if (this.doublepriest >= 1) retval *= (2 * this.doublepriest);
            return retval;
        }


        public int getMinionHeal(int heal)
        {
            return (this.anzOwnAuchenaiSoulpriest >= 1) ? -heal : heal;
        }

        public int getEnemySpellDamageDamage(int dmg)
        {
            int retval = dmg;
            retval += this.enemyspellpower;
            if (this.enemydoublepriest >= 1) retval *= (2 * this.enemydoublepriest);
            return retval;
        }

        public int getEnemySpellHeal(int heal)
        {
            int retval = heal;
            if (this.anzOwnAuchenaiSoulpriest >= 1)
            {
                retval *= -1;
                retval -= this.enemyspellpower;
            }
            if (this.doublepriest >= 1) retval *= (2 * this.doublepriest);
            return retval;
        }

        public int getEnemyMinionHeal(int heal)
        {
            return (this.anzEnemyAuchenaiSoulpriest >= 1) ? -heal : heal;
        }


        // do the action--------------------------------------------------------------

        public void doAction(Action aa)
        {
            Helpfunctions.Instance.ErrorLog("Do action");
            //CREATE NEW MINIONS (cant use a.target or a.own) (dont belong to this board)
            Minion trgt = null;
            Minion o = null;
            Handmanager.Handcard ha = null;
            if (aa.target != null)
            {
                foreach (Minion m in this.ownMinions)
                {
                    if (aa.target.entitiyID == m.entitiyID)
                    {
                        trgt = m;
                        break;
                    }
                }
                foreach (Minion m in this.enemyMinions)
                {
                    if (aa.target.entitiyID == m.entitiyID)
                    {
                        trgt = m;
                        break;
                    }
                }
                if (aa.target.entitiyID == this.ownHero.entitiyID) trgt = this.ownHero;
                if (aa.target.entitiyID == this.enemyHero.entitiyID) trgt = this.enemyHero;
            }
            if (aa.own != null)
            {
                foreach (Minion m in this.ownMinions)
                {
                    if (aa.own.entitiyID == m.entitiyID)
                    {
                        o = m;
                        break;
                    }
                }
                foreach (Minion m in this.enemyMinions)
                {
                    if (aa.own.entitiyID == m.entitiyID)
                    {
                        o = m;
                        break;
                    }
                }
                if (aa.own.entitiyID == this.ownHero.entitiyID) o = this.ownHero;
                if (aa.own.entitiyID == this.enemyHero.entitiyID) o = this.enemyHero;
            }

            if (aa.card != null)
            {
                foreach (Handmanager.Handcard hh in this.owncards)
                {
                    if (hh.entity == aa.card.entity)
                    {
                        ha = hh;
                        break;
                    }
                }
                foreach (Handmanager.Handcard hh in this.enemycards)
                {
                    if (hh.entity == aa.card.entity)
                    {
                        ha = hh;
                        break;
                    }
                }
                if (aa.actionType == actionEnum.useHeroPower)
                {
                    ha = this.isOwnTurn ? this.ownHeroAblility : this.enemyHeroAblility;
                }
            }
            // create and execute the action------------------------------------------------------------------------
            Action a = new Action(aa.actionType, ha, o, aa.place, trgt, aa.penalty, aa.druidchoice);



            //save the action if its our first turn
            if (this.turnCounter == 0) this.playactions.Add(a);
            //if (this.isOwnTurn) this.playactions.Add(a);

            // its a minion attack--------------------------------
            if (a.actionType == actionEnum.attackWithMinion)
            {
                this.evaluatePenality += a.penalty;
                Minion target = a.target;
                //secret stuff
                int newTarget = this.secretTrigger_CharIsAttacked(a.own, target);

                if (newTarget >= 1)
                {
                    //search new target!
                    foreach (Minion m in this.ownMinions)
                    {
                        if (m.entitiyID == newTarget)
                        {
                            target = m;
                            break;
                        }
                    }
                    foreach (Minion m in this.enemyMinions)
                    {
                        if (m.entitiyID == newTarget)
                        {
                            target = m;
                            break;
                        }
                    }
                    if (this.ownHero.entitiyID == newTarget) target = this.ownHero;
                    if (this.enemyHero.entitiyID == newTarget) target = this.enemyHero;
                    //Helpfunctions.Instance.ErrorLog("missdirection target = " + target.entitiyID);
                }
                if (a.own.Hp >= 1) minionAttacksMinion(a.own, target);
            }
            else
            {
                // its an hero attack--------------------------------
                if (a.actionType == actionEnum.attackWithHero)
                {
                    //secret trigger is inside
                    attackWithWeapon(a.own, a.target, a.penalty);
                }
                else
                {
                    // its an playing-card--------------------------------
                    if (a.actionType == actionEnum.playcard)
                    {
                        if (this.isOwnTurn)
                        {
                            playACard(a.card, a.target, a.place, a.druidchoice, a.penalty);
                        }
                        else
                        {
                            enemyplaysACard(a.card, a.target, a.place, a.druidchoice, a.penalty);
                        }
                    }
                    else
                    {
                        // its using the hero power--------------------------------
                        if (a.actionType == actionEnum.useHeroPower)
                        {
                            playHeroPower(a.target, a.penalty, this.isOwnTurn);
                        }
                    }
                }
            }







            if (this.isOwnTurn)
            {
                this.optionsPlayedThisTurn++;
            }
            else
            {
                this.enemyOptionsDoneThisTurn++;
            }

        }

        //minion attacks a minion

        //dontcount = betrayal effect!
        public void minionAttacksMinion(Minion attacker, Minion defender, bool dontcount = false)
        {

            if (attacker.isHero)
            {
                if (defender.isHero)
                {

                    defender.getDamageOrHeal(attacker.Angr, this, true, false);
                }
                else
                {

                    int enem_attack = defender.Angr;

                    defender.getDamageOrHeal(attacker.Angr, this, true, false);

                    if (!this.ownHero.immuneWhileAttacking)
                    {
                        int oldhp = attacker.Hp;
                        attacker.getDamageOrHeal(enem_attack, this, true, false);
                        if (!defender.silenced && oldhp > attacker.Hp)
                        {
                            if (defender.handcard.card.name == CardDB.cardName.waterelemental || defender.handcard.card.name == CardDB.cardName.snowchugger)
                            {
                                attacker.frozen = true;
                            }

                            this.triggerAMinionDealedDmg(defender, oldhp - attacker.Hp);
                        }
                    }
                }
                doDmgTriggers();
                return;
            }

            if (!dontcount)
            {
                attacker.numAttacksThisTurn++;
                attacker.stealth = false;
                if ((attacker.windfury && attacker.numAttacksThisTurn == 2) || !attacker.windfury)
                {
                    attacker.Ready = false;
                }

            }


            if (logging) Helpfunctions.Instance.logg(".attck with" + attacker.name + " A " + attacker.Angr + " H " + attacker.Hp);

            int attackerAngr = attacker.Angr;
            int defAngr = defender.Angr;

            //trigger attack ---------------------------
            this.triggerAMinionIsGoingToAttack(attacker);
            //------------------------------------------

            if (defender.isHero)//target is enemy hero
            {

                int oldhp = defender.Hp;
                defender.getDamageOrHeal(attacker.Angr, this, true, false);
                if (!attacker.silenced && oldhp > defender.Hp) // attacker did dmg
                {

                    if (attacker.handcard.card.name == CardDB.cardName.waterelemental || attacker.handcard.card.name == CardDB.cardName.snowchugger) defender.frozen = true;

                    this.triggerAMinionDealedDmg(attacker, oldhp - defender.Hp);
                }
                doDmgTriggers();
                return;
            }



            //defender gets dmg
            int oldHP = defender.Hp;
            defender.getDamageOrHeal(attackerAngr, this, true, false);
            if (!attacker.silenced && oldHP > defender.Hp && (attacker.handcard.card.name == CardDB.cardName.waterelemental || attacker.handcard.card.name == CardDB.cardName.snowchugger)) defender.frozen = true;
            bool defenderGotDmg = oldHP > defender.Hp;

            bool attackerGotDmg = false;

            //attacker gets dmg
            if (!dontcount)//betrayal effect :D
            {
                oldHP = attacker.Hp;
                attacker.getDamageOrHeal(defAngr, this, true, false);

                if (!defender.silenced && oldHP > attacker.Hp)
                {
                    if (defender.handcard.card.name == CardDB.cardName.waterelemental || defender.handcard.card.name == CardDB.cardName.snowchugger) attacker.frozen = true;

                    this.triggerAMinionDealedDmg(defender, oldHP - attacker.Hp);
                }
                attackerGotDmg = oldHP > attacker.Hp;
            }


            //trigger poisonous effect of attacker + defender (even if they died due to attacking/defending)

            if (defenderGotDmg && !attacker.silenced && attacker.handcard.card.poisionous && !defender.isHero)
            {
                minionGetDestroyed(defender);
            }

            if (attackerGotDmg && !defender.silenced && defender.handcard.card.poisionous && !attacker.isHero)
            {
                minionGetDestroyed(attacker);
            }

            if (!dontcount) //foe reaper reaps!
            {
                if (attacker.name == CardDB.cardName.foereaper4000 && !attacker.silenced)
                {
                    List<Minion> temp = (attacker.own) ? this.enemyMinions : this.ownMinions;
                    foreach (Minion mnn in temp)
                    {
                        if (mnn.zonepos + 1 == defender.zonepos || mnn.zonepos - 1 == defender.zonepos)
                        {
                            this.minionAttacksMinion(attacker, mnn, true);
                        }
                    }
                }
            }

            this.doDmgTriggers();



        }

        //a hero attacks a minion
        public void attackWithWeapon(Minion hero, Minion target, int penality)
        {
            bool own = hero.own;
            this.attacked = true;
            this.evaluatePenality += penality;
            hero.numAttacksThisTurn++;

            //hero will end his readyness
            hero.updateReadyness();

            //heal whether truesilverchampion equipped
            if (own)
            {
                if (this.ownWeaponName == CardDB.cardName.truesilverchampion)
                {
                    int heal = this.getMinionHeal(2);//minionheal because it's ignoring spellpower
                    this.minionGetDamageOrHeal(hero, -heal);
                    doDmgTriggers();
                }
            }
            else
            {
                if (this.enemyWeaponName == CardDB.cardName.truesilverchampion)
                {
                    int heal = this.getEnemyMinionHeal(2);
                    this.minionGetDamageOrHeal(hero, -heal);
                    doDmgTriggers();
                }
            }

            if (logging) Helpfunctions.Instance.logg("attck with weapon trgt: " + target.entitiyID);

            // hero attacks enemy----------------------------------------------------------------------------------

            if (target.isHero)// trigger secret and change target if necessary
            {
                int newTarget = this.secretTrigger_CharIsAttacked(hero, target);
                if (newTarget >= 1)
                {
                    //search new target!
                    foreach (Minion m in this.ownMinions)
                    {
                        if (m.entitiyID == newTarget)
                        {
                            target = m;
                            break;
                        }
                    }
                    foreach (Minion m in this.enemyMinions)
                    {
                        if (m.entitiyID == newTarget)
                        {
                            target = m;
                            break;
                        }
                    }
                    if (this.ownHero.entitiyID == newTarget) target = this.ownHero;
                    if (this.enemyHero.entitiyID == newTarget) target = this.enemyHero;
                }

            }
            this.minionAttacksMinion(hero, target);
            //-----------------------------------------------------------------------------------------------------

            //gorehowl is not killed if he attacks minions
            if (own)
            {
                if (ownWeaponName == CardDB.cardName.gorehowl && !target.isHero)
                {
                    this.ownWeaponAttack--;
                    hero.Angr--;
                }
                else
                {
                    this.lowerWeaponDurability(1, true);
                }
            }
            else
            {
                if (enemyWeaponName == CardDB.cardName.gorehowl && !target.isHero)
                {
                    this.enemyWeaponAttack--;
                    hero.Angr--;
                }
                else
                {
                    this.lowerWeaponDurability(1, false);
                }
            }

        }

        //play a minion trigger stuff:
        // 1 whenever you play a card whatever triggers
        // 2 Auras
        // 5 whenever you summon a minion triggers (like starving buzzard)
        // 3 battlecry
        // 3.5 place minion
        // 3.75 dmg/died/dthrttl triggers
        // 4 secret: minion is played
        // 4.5 dmg/died/dthrttl triggers
        // 5 after you summon a minion triggers
        // 5.5 dmg/died/dthrttl triggers
        public void playACard(Handmanager.Handcard hc, Minion target, int position, int choice, int penality)
        {
            Helpfunctions.Instance.ErrorLog("play a card");
            CardDB.Card c = hc.card;
            this.evaluatePenality += penality;
            this.mana = this.mana - hc.getManaCost(this);
            removeCard(hc);// remove card from hand

            this.triggerCardsChanged(true);

            if (c.type == CardDB.cardtype.SPELL)
            {
                this.playedPreparation = false;
                if (target != null && target.name == CardDB.cardName.dragonkinsorcerer && target.own) this.minionGetBuffed(target, 1, 1);
            }

            if (c.Secret)
            {
                this.ownSecretsIDList.Add(c.cardIDenum);
                this.playedmagierinderkirintor = false;
            }


            //Helpfunctions.Instance.logg("play crd " + c.name + " entitiy# " + cardEntity + " mana " + hc.getManaCost(this) + " trgt " + target);
            if (logging) Helpfunctions.Instance.logg("play crd " + c.name + " entitiy# " + hc.entity + " mana " + hc.getManaCost(this) + " trgt " + target);


            this.triggerACardWillBePlayed(hc, true);
            int newTarget = secretTrigger_SpellIsPlayed(target, c.type == CardDB.cardtype.SPELL);
            if (newTarget >= 1)
            {
                //search new target!
                foreach (Minion m in this.ownMinions)
                {
                    if (m.entitiyID == newTarget)
                    {
                        target = m;
                        break;
                    }
                }
                foreach (Minion m in this.enemyMinions)
                {
                    if (m.entitiyID == newTarget)
                    {
                        target = m;
                        break;
                    }
                }
                if (this.ownHero.entitiyID == newTarget) target = this.ownHero;
                if (this.enemyHero.entitiyID == newTarget) target = this.enemyHero;
            }
            if (newTarget != -2) // trigger spell-secrets!
            {

                if (c.type == CardDB.cardtype.MOB)
                {
                    this.placeAmobSomewhere(hc, true, target, choice, position);
                    this.mobsplayedThisTurn++;
                }
                else
                {

                    c.sim_card.onCardPlay(this, true, target, choice);
                    this.doDmgTriggers();
                    //secret trigger? do here
                    GraveYardItem gyi = new GraveYardItem(c.cardIDenum, hc.entity, true);
                    graveYard.Add(gyi);
                }
            }

            //this.ueberladung += c.recallValue;
            this.cardsPlayedThisTurn++;

        }

        public void enemyplaysACard(Handmanager.Handcard hc, Minion target, int position, int choice, int penality)
        {

            //Handmanager.Handcard hc = new Handmanager.Handcard(c);
            //hc.entity = this.getNextEntity();
            //Helpfunctions.Instance.logg("play crd " + c.name + " entitiy# " + cardEntity + " mana " + hc.getManaCost(this) + " trgt " + target);
            CardDB.Card c = hc.card;
            if (logging) Helpfunctions.Instance.logg("enemy play crd " + c.name + " trgt " + target);

            this.enemyAnzCards--;//might be deleted if he got a real hand

            this.triggerACardWillBePlayed(hc, false);
            this.triggerCardsChanged(false);

            int newTarget = secretTrigger_SpellIsPlayed(target, c.type == CardDB.cardtype.SPELL);
            if (newTarget >= 1)
            {
                //search new target!
                foreach (Minion m in this.ownMinions)
                {
                    if (m.entitiyID == newTarget)
                    {
                        target = m;
                        break;
                    }
                }
                foreach (Minion m in this.enemyMinions)
                {
                    if (m.entitiyID == newTarget)
                    {
                        target = m;
                        break;
                    }
                }
                if (this.ownHero.entitiyID == newTarget) target = this.ownHero;
                if (this.enemyHero.entitiyID == newTarget) target = this.enemyHero;
            }
            if (newTarget != -2) // trigger spell-secrets!
            {
                if (c.type == CardDB.cardtype.MOB)
                {
                    //todo mob playing
                    this.placeAmobSomewhere(hc, false, target, choice, position);
                }
                else
                {
                    c.sim_card.onCardPlay(this, false, target, choice);
                    this.doDmgTriggers();
                    //secret trigger? do here
                    GraveYardItem gyi = new GraveYardItem(c.cardIDenum, hc.entity, false);
                    graveYard.Add(gyi);
                }
            }
        }


        public void playHeroPower(Minion target, int penality, bool ownturn)
        {

            CardDB.Card c = (ownturn) ? this.ownHeroAblility.card : this.enemyHeroAblility.card;

            if (ownturn) this.ownAbilityReady = false;
            else this.enemyAbilityReady = false;

            this.evaluatePenality += penality;
            this.mana = this.mana - 2;

            //Helpfunctions.Instance.logg("play crd " + c.name + " entitiy# " + cardEntity + " mana " + hc.getManaCost(this) + " trgt " + target);
            if (logging) Helpfunctions.Instance.logg("play crd " + c.name + " trgt " + target);

            c.sim_card.onCardPlay(this, ownturn, target, 0);
            this.doDmgTriggers();
        }


        //lower durability of weapon + destroy them (deathrattle) 
        //todo: test death's bite's dearthrattle
        public void lowerWeaponDurability(int value, bool own)
        {

            if (own)
            {
                if (this.ownWeaponDurability <= 0) return;
                this.ownWeaponDurability -= value;
                if (this.ownWeaponDurability <= 0)
                {
                    //todo deathrattle deathsbite

                    if (this.ownWeaponName == CardDB.cardName.powermace && this.ownMinions.Count >= 1)
                    {
                        int sum = 1000;
                        Minion t = null;

                        foreach (Minion m in ownMinions)
                        {
                            if ((TAG_RACE)m.handcard.card.race == TAG_RACE.MECHANICAL)
                            {
                                int s = m.maxHp + m.Angr;
                                if (s < sum)
                                {
                                    t = m;
                                    sum = s;
                                }
                            }

                        }

                        if (t != null && sum < 999)
                        {
                            this.minionGetBuffed(t, 2, 2);
                        }
                    }

                    if (this.ownWeaponName == CardDB.cardName.deathsbite)
                    {
                        int anz = 1;
                        if (this.ownBaronRivendare >= 1) anz = 2;
                        int dmg = getSpellDamageDamage(1);
                        foreach (Minion m in this.ownMinions)
                        {
                            this.minionGetDamageOrHeal(m, anz * dmg);
                        }
                        foreach (Minion m in this.enemyMinions)
                        {
                            this.minionGetDamageOrHeal(m, anz * dmg);
                        }
                        this.doDmgTriggers();

                    }


                    this.ownHero.Angr -= this.ownWeaponAttack;
                    this.ownWeaponDurability = 0;
                    this.ownWeaponAttack = 0;
                    this.ownWeaponName = CardDB.cardName.unknown;
                    this.ownHero.windfury = false;

                    foreach (Minion m in this.ownMinions)
                    {
                        if (m.playedThisTurn && m.name == CardDB.cardName.southseadeckhand)
                        {
                            m.charge--;
                            m.updateReadyness();
                        }
                    }
                    this.ownHero.updateReadyness();
                }
            }
            else
            {
                if (this.enemyWeaponDurability <= 0) return;
                this.enemyWeaponDurability -= value;
                if (this.enemyWeaponDurability <= 0)
                {
                    //deathrattle deathsbite

                    if (this.ownWeaponName == CardDB.cardName.powermace && this.enemyMinions.Count >= 1)
                    {
                        int sum = 1000;
                        Minion t = null;

                        foreach (Minion m in enemyMinions)
                        {
                            if ((TAG_RACE)m.handcard.card.race == TAG_RACE.MECHANICAL)
                            {
                                int s = m.maxHp + m.Angr;
                                if (s < sum)
                                {
                                    t = m;
                                    sum = s;
                                }
                            }

                        }

                        if (t != null && sum < 999)
                        {
                            this.minionGetBuffed(t, 2, 2);
                        }
                    }

                    if (this.enemyWeaponName == CardDB.cardName.deathsbite)
                    {
                        int anz = 1;
                        if (this.enemyBaronRivendare >= 1) anz = 2;
                        int dmg = getEnemySpellDamageDamage(1);
                        foreach (Minion m in this.ownMinions)
                        {
                            this.minionGetDamageOrHeal(m, anz * dmg);
                        }
                        foreach (Minion m in this.enemyMinions)
                        {
                            this.minionGetDamageOrHeal(m, anz * dmg);
                        }
                        this.doDmgTriggers();
                    }

                    this.enemyHero.Angr -= this.enemyWeaponAttack;
                    this.enemyWeaponDurability = 0;
                    this.enemyWeaponAttack = 0;
                    this.enemyWeaponName = CardDB.cardName.unknown;
                    this.enemyHero.windfury = false;
                    this.enemyHero.updateReadyness();
                }
            }
        }



        public void doDmgTriggers()
        {
            //we do the these trigger manualy (to less minions) (we could trigger them with m.handcard.card.sim_card.ontrigger...)
            if (this.tempTrigger.charsGotHealed >= 1)
            {
                triggerACharGotHealed();//possible effects: gain attack
                this.tempTrigger.charsGotHealed = 0;
            }

            if (this.tempTrigger.minionsGotHealed >= 1)
            {
                triggerAMinionGotHealed();//possible effects: draw card
                this.tempTrigger.minionsGotHealed = 0;
            }


            if (this.tempTrigger.ownMinionsGotDmg + this.tempTrigger.enemyMinionsGotDmg >= 1)
            {
                triggerAMinionGotDmg(); //possible effects: draw card, gain armor, gain attack
                this.tempTrigger.ownMinionsGotDmg = 0;
                this.tempTrigger.enemyMinionsGotDmg = 0;
                this.tempTrigger.ownHeroGotDmg = 0;
                this.tempTrigger.enemyHeroGotDmg = 0;
            }

            if (this.tempTrigger.ownMinionsDied + this.tempTrigger.enemyMinionsDied >= 1)
            {
                triggerAMinionDied(); //possible effects: draw card, gain attack + hp
                if (this.tempTrigger.ownMinionsDied >= 1) this.tempTrigger.ownMinionsChanged = true;
                if (this.tempTrigger.enemyMinionsDied >= 1) this.tempTrigger.enemyMininsChanged = true;
                this.tempTrigger.ownMinionsDied = 0;
                this.tempTrigger.enemyMinionsDied = 0;
                this.tempTrigger.ownBeastDied = 0;
                this.tempTrigger.enemyBeastDied = 0;
                this.tempTrigger.ownMurlocDied = 0;
                this.tempTrigger.enemyMurlocDied = 0;
                this.tempTrigger.ownMechanicDied = 0;
                this.tempTrigger.enemyMechanicDied = 0;
            }

            updateBoards();
            if (this.tempTrigger.charsGotHealed + this.tempTrigger.minionsGotHealed + this.tempTrigger.ownMinionsGotDmg + this.tempTrigger.enemyMinionsGotDmg + this.tempTrigger.ownMinionsDied + this.tempTrigger.enemyMinionsDied >= 1)
            {
                doDmgTriggers();
            }
        }

        public void triggerACharGotHealed()
        {
            foreach (Minion mnn in this.ownMinions)
            {
                if (mnn.silenced) continue;
                if (mnn.handcard.card.name == CardDB.cardName.lightwarden)
                {
                    minionGetBuffed(mnn, 2 * this.tempTrigger.charsGotHealed, 0);
                }
                if (mnn.handcard.card.name == CardDB.cardName.shadowboxer)
                {
                    for (int i = 0; i < this.tempTrigger.charsGotHealed; i++)
                    {
                        Minion t = this.searchRandomMinion(this.enemyMinions, searchmode.searchHighestHP);
                        if (t != null)
                        {
                            this.minionGetDamageOrHeal(t, 1);
                        }
                        else
                        {
                            this.minionGetDamageOrHeal(this.enemyHero, 1);
                        }
                    }
                }
            }
            foreach (Minion mnn in this.enemyMinions)
            {
                if (mnn.silenced) continue;
                if (mnn.handcard.card.name == CardDB.cardName.lightwarden)
                {
                    minionGetBuffed(mnn, 2 * this.tempTrigger.charsGotHealed, 0);
                }
                if (mnn.handcard.card.name == CardDB.cardName.shadowboxer)
                {
                    for (int i = 0; i < this.tempTrigger.charsGotHealed; i++)
                    {
                        Minion t = this.searchRandomMinion(this.ownMinions, searchmode.searchHighestHP);
                        if (t != null)
                        {
                            this.minionGetDamageOrHeal(t, 1);
                        }
                        else
                        {
                            this.minionGetDamageOrHeal(this.ownHero, 1);
                        }
                    }
                }
            }
        }

        public void triggerAMinionGotHealed()
        {
            //also dead minions trigger this
            foreach (Minion mnn in this.ownMinions)
            {
                if (mnn.silenced) continue;
                if (mnn.handcard.card.name == CardDB.cardName.northshirecleric)
                {
                    for (int i = 0; i < this.tempTrigger.minionsGotHealed; i++)
                    {
                        this.drawACard(CardDB.cardName.unknown, true);
                    }
                }
            }

            foreach (Minion mnn in this.enemyMinions)
            {
                if (mnn.silenced) continue;
                if (mnn.handcard.card.name == CardDB.cardName.northshirecleric)
                {
                    for (int i = 0; i < this.tempTrigger.minionsGotHealed; i++)
                    {
                        this.drawACard(CardDB.cardName.unknown, false);
                    }
                }
            }
        }

        public void triggerAMinionGotDmg()
        {
            foreach (Minion m in this.ownMinions.ToArray())
            {
                if (m.silenced) { m.anzGotDmg = 0; continue; }
                m.handcard.card.sim_card.onMinionGotDmgTrigger(this, m, true);
            }

            foreach (Minion m in this.enemyMinions.ToArray())
            {
                if (m.silenced) { m.anzGotDmg = 0; continue; }
                m.handcard.card.sim_card.onMinionGotDmgTrigger(this, m, false);
            }
            this.ownHero.anzGotDmg = 0;
            this.enemyHero.anzGotDmg = 0;
        }

        public void triggerAMinionDied()
        {
            this.ownMinionsDiedTurn += this.tempTrigger.ownMinionsDied;
            this.enemyMinionsDiedTurn += this.tempTrigger.enemyMinionsDied;

            foreach (Minion m in this.ownMinions.ToArray())
            {
                if (m.silenced) continue;
                if (m.Hp <= 0) continue;
                m.handcard.card.sim_card.onMinionDiedTrigger(this, m, m); //onMinionDiedTrigger(Playfield p, Minion triggerEffectMinion, Minion diedMinion) where "diedMinion" - not implemented and is not currently needed
            }
            foreach (Minion m in this.enemyMinions.ToArray())
            {
                if (m.silenced) continue;
                if (m.Hp <= 0) continue;
                m.handcard.card.sim_card.onMinionDiedTrigger(this, m, m);
            }

            foreach (Handmanager.Handcard hc in this.owncards)
            {
                if (hc.card.name == CardDB.cardName.bolvarfordragon) hc.addattack += this.tempTrigger.ownMinionsDied;
            }

        }

        public void triggerAMinionIsGoingToAttack(Minion m)
        {
            //todo trigger secret her too
            //blessing of wisdom (truesilver is located in attackWithWeapon(...))
            if (m.ownBlessingOfWisdom >= 1)
            {
                for (int i = 0; i < m.ownBlessingOfWisdom; i++)
                {
                    this.drawACard(CardDB.cardName.unknown, true);
                }
            }
            if (m.enemyBlessingOfWisdom >= 1)
            {
                for (int i = 0; i < m.enemyBlessingOfWisdom; i++)
                {
                    this.drawACard(CardDB.cardName.unknown, false);
                }
            }

        }

        public void triggerAMinionDealedDmg(Minion m, int dmgDone)
        {
            //only GVG_018 has such an trigger!
            if (m.name == CardDB.cardName.mistressofpain && dmgDone >= 1)
            {
                if (m.own)
                {
                    if (this.anzOwnAuchenaiSoulpriest >= 1) // you have a soulpriest? lol you die!!!
                    {
                        this.ownHero.Hp = 0;
                    }
                    else
                    {
                        this.minionGetDamageOrHeal(this.ownHero, -dmgDone);
                    }
                }
                else
                {
                    if (this.anzEnemyAuchenaiSoulpriest >= 1) // you have a soulpriest? lol you die!!!
                    {
                        this.enemyHero.Hp = 0;
                    }
                    else
                    {
                        this.minionGetDamageOrHeal(this.enemyHero, -dmgDone);
                    }
                }
            }

        }

        public void triggerACardWillBePlayed(Handmanager.Handcard hc, bool own)
        {
            if (own)
            {
                if (anzOwnDragonConsort > 0 && (TAG_RACE)hc.card.race == TAG_RACE.DRAGON) anzOwnDragonConsort = 0;
                int violetteacher = 0; //we count violetteacher to avoid copying ownminions
                int illidan = 0;
                int burly = 0;
                foreach (Minion m in this.ownMinions)
                {
                    if (m.silenced) continue;

                    if (own && m.name == CardDB.cardName.illidanstormrage)
                    {
                        illidan++;
                        continue;
                    }

                    if (own && m.name == CardDB.cardName.violetteacher)
                    {
                        if (hc.card.type == CardDB.cardtype.SPELL)
                        {
                            violetteacher++;
                        }
                        continue;
                    }
                    if (own && m.name == CardDB.cardName.hobgoblin)
                    {
                        if (hc.card.type == CardDB.cardtype.MOB && hc.card.Attack == 1)
                        {
                            hc.addattack += 2;
                            hc.addHp += 2;
                        }
                        continue;
                    }

                    m.handcard.card.sim_card.onCardIsGoingToBePlayed(this, hc.card, own, m);
                }

                foreach (Minion m in this.enemyMinions)
                {
                    if (m.name == CardDB.cardName.troggzortheearthinator)
                    {
                        burly++;
                    }
                    if (m.name == CardDB.cardName.felreaver)
                    {
                        m.handcard.card.sim_card.onCardIsGoingToBePlayed(this, hc.card, own, m);
                    }
                }

                for (int i = 0; i < violetteacher; i++)
                {
                    int pos = this.ownMinions.Count;
                    this.callKid(CardDB.Instance.teacherminion, pos, own);
                }

                for (int i = 0; i < illidan; i++)
                {
                    int pos = this.ownMinions.Count;
                    this.callKid(CardDB.Instance.illidanminion, pos, own);
                }

                for (int i = 0; i < burly; i++)//summon for enemy !
                {
                    int pos = this.enemyMinions.Count;
                    this.callKid(CardDB.Instance.burlyrockjaw, pos, !own);
                }


            }
            else
            {
                int violetteacher = 0; //we count violetteacher to avoid copying ownminions
                int illidan = 0;
                int burly = 0;
                foreach (Minion m in this.enemyMinions)
                {
                    if (m.silenced) continue;
                    if (!own && m.name == CardDB.cardName.illidanstormrage)
                    {
                        illidan++;
                        continue;
                    }
                    if (!own && m.name == CardDB.cardName.violetteacher)
                    {
                        if (hc.card.type == CardDB.cardtype.SPELL)
                        {
                            violetteacher++;
                        }
                        continue;
                    }
                    if (!own && m.name == CardDB.cardName.hobgoblin)
                    {
                        if (hc.card.type == CardDB.cardtype.MOB && hc.card.Attack == 1)
                        {
                            hc.addattack += 2;
                            hc.addHp += 2;
                        }
                        continue;
                    }

                    m.handcard.card.sim_card.onCardIsGoingToBePlayed(this, hc.card, own, m);
                }
                foreach (Minion m in this.ownMinions)
                {
                    if (m.name == CardDB.cardName.troggzortheearthinator)
                    {
                        burly++;
                    }
                    if (m.name == CardDB.cardName.felreaver)
                    {
                        m.handcard.card.sim_card.onCardIsGoingToBePlayed(this, hc.card, own, m);
                    }
                }
                for (int i = 0; i < violetteacher; i++)
                {
                    int pos = this.enemyMinions.Count;
                    this.callKid(CardDB.Instance.teacherminion, pos, own);
                }
                for (int i = 0; i < illidan; i++)
                {
                    int pos = this.enemyMinions.Count;
                    this.callKid(CardDB.Instance.illidanminion, pos, own);
                }

                for (int i = 0; i < burly; i++)//summon for us
                {
                    int pos = this.ownMinions.Count;
                    this.callKid(CardDB.Instance.burlyrockjaw, pos, own);
                }
            }

        }

        // public void triggerACardWasPlayed(CardDB.Card c, bool own) {        }

        public void triggerAMinionIsSummoned(Minion m)
        {
            if (m.own)
            {
                foreach (Minion mnn in this.ownMinions)
                {
                    if (mnn.silenced) continue;
                    mnn.handcard.card.sim_card.onMinionIsSummoned(this, mnn, m);
                }

                foreach (Minion mnn in this.enemyMinions)
                {
                    if (mnn.silenced) continue;
                    if (mnn.name == CardDB.cardName.murloctidecaller) mnn.handcard.card.sim_card.onMinionIsSummoned(this, mnn, m);
                }

                if (this.ownWeaponName == CardDB.cardName.swordofjustice)
                {
                    this.minionGetBuffed(m, 1, 1);
                    this.lowerWeaponDurability(1, true);
                }
            }
            else
            {
                foreach (Minion mnn in this.enemyMinions)
                {
                    if (mnn.silenced) continue;
                    mnn.handcard.card.sim_card.onMinionIsSummoned(this, mnn, m);
                }

                foreach (Minion mnn in this.ownMinions)
                {
                    if (mnn.silenced) continue;
                    if (mnn.name == CardDB.cardName.murloctidecaller) mnn.handcard.card.sim_card.onMinionIsSummoned(this, mnn, m);
                }
                if (this.enemyWeaponName == CardDB.cardName.swordofjustice)
                {
                    this.minionGetBuffed(m, 1, 1);
                    this.lowerWeaponDurability(1, false);
                }
            }

        }

        public void triggerAMinionWasSummoned(Minion mnn)
        {
            if (mnn.own)
            {
                foreach (Minion m in this.ownMinions)
                {
                    if (m.silenced) continue;
                    if (m.name == CardDB.cardName.knifejuggler)
                    {
                        m.handcard.card.sim_card.onMinionWasSummoned(this, m, mnn);
                    }
                }
            }
            else
            {
                foreach (Minion m in this.enemyMinions)
                {
                    if (m.silenced) continue;
                    if (m.name == CardDB.cardName.knifejuggler)
                    {
                        m.handcard.card.sim_card.onMinionWasSummoned(this, m, mnn);
                    }
                }
            }

        }

        public void triggerEndTurn(bool ownturn)
        {
            foreach (Minion m in this.ownMinions.ToArray())
            {
                if (!m.silenced)
                {
                    m.handcard.card.sim_card.onTurnEndsTrigger(this, m, ownturn);
                }
                if (ownturn && m.destroyOnOwnTurnEnd) this.minionGetDestroyed(m);
            }
            foreach (Minion m in this.enemyMinions.ToArray())
            {
                if (!m.silenced)
                {
                    m.handcard.card.sim_card.onTurnEndsTrigger(this, m, ownturn);
                }
                if (!ownturn && m.destroyOnEnemyTurnEnd) this.minionGetDestroyed(m);
            }

            this.doDmgTriggers();

            //shadowmadness
            if (this.shadowmadnessed >= 1)
            {
                List<Minion> ownm = (ownturn) ? this.ownMinions : this.enemyMinions;
                foreach (Minion m in ownm.ToArray())
                {
                    if (m.shadowmadnessed)
                    {
                        m.shadowmadnessed = false;
                        this.minionGetControlled(m, !m.own, false);
                    }
                }
                this.shadowmadnessed = 0;
                updateBoards();
            }

            this.playedmagierinderkirintor = false;

            foreach (Minion m in this.ownMinions)
            {
                this.minionGetTempBuff(m, -m.tempAttack, 0);
                m.immune = false;
                m.cantLowerHPbelowONE = false;
            }
            foreach (Minion m in this.enemyMinions)
            {
                this.minionGetTempBuff(m, -m.tempAttack, 0);
                m.immune = false;
                m.cantLowerHPbelowONE = false;
            }
        }

        public void triggerStartTurn(bool ownturn)
        {

            List<Minion> ownm = (ownturn) ? this.ownMinions : this.enemyMinions;
            foreach (Minion m in ownm.ToArray())
            {
                m.playedThisTurn = false;
                m.numAttacksThisTurn = 0;
                m.updateReadyness();
                if (!m.silenced)
                {
                    m.handcard.card.sim_card.onTurnStartTrigger(this, m, ownturn);
                }
                if (ownturn && m.destroyOnOwnTurnStart) this.minionGetDestroyed(m);
                if (!ownturn && m.destroyOnEnemyTurnStart) this.minionGetDestroyed(m);
            }

            List<Minion> enemm = (ownturn) ? this.enemyMinions : this.ownMinions;
            foreach (Minion m in enemm.ToArray())
            {
                if (!m.silenced)
                {
                    if (m.name == CardDB.cardName.micromachine) m.handcard.card.sim_card.onTurnStartTrigger(this, m, ownturn);
                }
                if (ownturn && m.destroyOnOwnTurnStart) this.minionGetDestroyed(m);
                if (!ownturn && m.destroyOnEnemyTurnStart) this.minionGetDestroyed(m);
            }

            this.doDmgTriggers();

            Handmanager.Handcard card = new Handmanager.Handcard();

            this.drawACard(getArandomCardFromDeck(ownturn).name, ownturn);
            this.doDmgTriggers();
        }

        public void triggerAHeroGotArmor(bool ownHero)
        {
            foreach (Minion m in ((ownHero) ? this.ownMinions : this.enemyMinions))
            {
                if (m.name == CardDB.cardName.siegeengine)
                {
                    this.minionGetBuffed(m, 1, 0);
                }
            }
        }

        public void triggerCardsChanged(bool own)
        {
            if (own)
            {
                if (this.tempanzOwnCards >= 6 && this.owncards.Count <= 5)
                {
                    //delete effect of enemy Goblin Sapper
                    foreach (Minion m in this.enemyMinions)
                    {
                        if (m.name == CardDB.cardName.goblinsapper && !m.silenced)
                        {
                            this.minionGetBuffed(m, -4, 0);
                        }
                    }
                }
                if (this.owncards.Count >= 6 && this.tempanzOwnCards <= 5)
                {
                    //add effect of enemy Goblin Sapper
                    foreach (Minion m in this.enemyMinions)
                    {
                        if (m.name == CardDB.cardName.goblinsapper && !m.silenced)
                        {
                            this.minionGetBuffed(m, 4, 0);
                        }
                    }
                }

                this.tempanzOwnCards = this.owncards.Count;
            }
            else
            {
                if (this.tempanzEnemyCards >= 6 && this.enemyAnzCards <= 5)
                {
                    //delete effect of own Goblin Sapper
                    foreach (Minion m in this.ownMinions)
                    {
                        if (m.name == CardDB.cardName.goblinsapper && !m.silenced)
                        {
                            this.minionGetBuffed(m, -4, 0);
                        }
                    }
                }
                if (this.enemyAnzCards >= 6 && this.tempanzEnemyCards <= 5)
                {
                    //add effect of own Goblin Sapper
                    foreach (Minion m in this.ownMinions)
                    {
                        if (m.name == CardDB.cardName.goblinsapper && !m.silenced)
                        {
                            this.minionGetBuffed(m, 4, 0);
                        }
                    }
                }

                this.tempanzEnemyCards = this.enemyAnzCards;
            }
        }



        public int secretTrigger_CharIsAttacked(Minion attacker, Minion defender)
        {
            int newTarget = 0;
            int triggered = 0;
            if (this.isOwnTurn && this.enemySecretCount >= 1)
            {

                if (defender.isHero && !defender.own)
                {
                    foreach (SecretItem si in this.enemySecretList.ToArray())
                    {
                        if (si.canBe_explosive)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_610).sim_card.onSecretPlay(this, false, 0);
                            doDmgTriggers();
                            //Helpfunctions.Instance.ErrorLog("trigger explosive" + attacker.Hp);
                            si.usedTrigger_CharIsAttacked(true, attacker.isHero);
                            foreach (SecretItem sii in this.enemySecretList)
                            {
                                sii.canBe_explosive = false;
                            }
                        }

                        if (!attacker.isHero && si.canBe_vaporize)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_594).sim_card.onSecretPlay(this, false, attacker, 0);
                            doDmgTriggers();

                            si.usedTrigger_CharIsAttacked(true, attacker.isHero);
                            foreach (SecretItem sii in this.enemySecretList)
                            {
                                sii.canBe_vaporize = false;
                            }
                        }

                        if (si.canBe_missdirection)
                        {
                            if (!(attacker.isHero && this.ownMinions.Count + this.enemyMinions.Count == 0))
                            {
                                triggered++;
                                CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_533).sim_card.onSecretPlay(this, false, attacker, defender, out newTarget);
                                si.usedTrigger_CharIsAttacked(true, attacker.isHero);
                                //Helpfunctions.Instance.ErrorLog("trigger miss " + attacker.Hp);
                                foreach (SecretItem sii in this.enemySecretList)
                                {
                                    sii.canBe_missdirection = false;
                                }
                            }
                        }

                        if (si.canBe_icebarrier)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_289).sim_card.onSecretPlay(this, false, defender, 0);
                            si.usedTrigger_CharIsAttacked(true, attacker.isHero);
                            foreach (SecretItem sii in this.enemySecretList)
                            {
                                sii.canBe_icebarrier = false;
                            }
                        }

                    }

                }

                if (!defender.isHero && !defender.own)
                {
                    foreach (SecretItem si in this.enemySecretList)
                    {

                        if (si.canBe_snaketrap)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_554).sim_card.onSecretPlay(this, false, 0);
                            si.usedTrigger_CharIsAttacked(false, attacker.isHero);
                            foreach (SecretItem sii in this.enemySecretList)
                            {
                                sii.canBe_snaketrap = false;
                            }
                        }
                    }
                }

                if (!attacker.isHero && attacker.own) // minion attacks
                {
                    foreach (SecretItem si in this.enemySecretList)
                    {
                        if (si.canBe_freezing)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_611).sim_card.onSecretPlay(this, false, attacker, 0);
                            si.usedTrigger_CharIsAttacked(defender.isHero, attacker.isHero);
                            //Helpfunctions.Instance.ErrorLog("trigger freeze " + attacker.Hp);
                            foreach (SecretItem sii in this.enemySecretList)
                            {
                                sii.canBe_freezing = false;
                            }
                        }
                    }
                }

                foreach (SecretItem si in this.enemySecretList)
                {

                    if (si.canBe_noblesacrifice)
                    {
                        //triggered++;
                        bool ishero = defender.isHero;
                        //CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_130).sim_card.onSecretPlay(this, false, attacker, defender, out newTarget);
                        si.usedTrigger_CharIsAttacked(ishero, attacker.isHero);
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_noblesacrifice = false;
                        }
                    }
                }


            }

            if (turnCounter == 0)
            {
                this.evaluatePenality -= triggered * 50;
            }

            return newTarget;
        }

        public void secretTrigger_HeroGotDmg(bool own, int dmg)
        {
            int triggered = 0;
            if (own != this.isOwnTurn)
            {
                if (this.isOwnTurn && this.enemySecretCount >= 1)
                {
                    foreach (SecretItem si in this.enemySecretList)
                    {
                        if (si.canBe_eyeforaneye)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_132).sim_card.onSecretPlay(this, false, dmg);
                            si.usedTrigger_HeroGotDmg();
                            foreach (SecretItem sii in this.enemySecretList)
                            {
                                sii.canBe_eyeforaneye = false;
                            }
                        }

                        if (si.canBe_iceblock && this.enemyHero.Hp <= 0)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_295).sim_card.onSecretPlay(this, false, this.enemyHero, dmg);
                            si.usedTrigger_HeroGotDmg(true);
                            foreach (SecretItem sii in this.enemySecretList)
                            {
                                sii.canBe_iceblock = false;
                            }

                        }
                    }
                }
            }

            if (turnCounter == 0)
            {
                this.evaluatePenality -= triggered * 50;
            }

        }

        public void secretTrigger_MinionIsPlayed(Minion playedMinion)
        {
            int triggered = 0;
            if (this.isOwnTurn && playedMinion.own && this.enemySecretCount >= 1)
            {
                foreach (SecretItem si in this.enemySecretList.ToArray())
                {
                    if (si.canBe_snipe)
                    {
                        triggered++;
                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_609).sim_card.onSecretPlay(this, false, playedMinion, 0);
                        doDmgTriggers();
                        si.usedTrigger_MinionIsPlayed();
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_snipe = false;
                        }
                    }

                    if (si.canBe_mirrorentity)
                    {
                        triggered++;
                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_294).sim_card.onSecretPlay(this, false, playedMinion, 0);
                        si.usedTrigger_MinionIsPlayed();
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_mirrorentity = false;
                        }

                    }

                    if (si.canBe_repentance)
                    {
                        //triggered++;
                        //CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_379).sim_card.onSecretPlay(this, false, playedMinion, 0);
                        si.usedTrigger_MinionIsPlayed();
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_repentance = false;
                        }
                    }
                }
            }

            if (turnCounter == 0)
            {
                this.evaluatePenality -= triggered * 50;
            }

        }

        public int secretTrigger_SpellIsPlayed(Minion target, bool isSpell)
        {
            int triggered = 0;
            if (this.isOwnTurn && isSpell && this.enemySecretCount >= 1) //actual secrets need a spell played!
            {
                foreach (SecretItem si in this.enemySecretList)
                {

                    if (si.canBe_counterspell)
                    {
                        triggered++;
                        // dont use spell!
                        si.usedTrigger_SpellIsPlayed(false);
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_counterspell = false;
                        }

                        if (turnCounter == 0)
                        {
                            this.evaluatePenality -= triggered * 50;
                        }
                        return -2;//spellbender will NEVER trigger
                    }
                }



                foreach (SecretItem si in this.enemySecretList)
                {

                    if (si.canBe_spellbender && target != null && !target.isHero)
                    {
                        triggered++;
                        int retval = 0;
                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.tt_010).sim_card.onSecretPlay(this, false, null, target, out retval);
                        si.usedTrigger_SpellIsPlayed(true);
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_spellbender = false;
                        }

                        if (turnCounter == 0)
                        {
                            this.evaluatePenality -= triggered * 50;
                        }
                        return retval;// the new target
                    }




                }

            }

            if (turnCounter == 0)
            {
                this.evaluatePenality -= triggered * 50;
            }

            return 0;

        }

        public void secretTrigger_MinionDied(bool own)
        {
            int triggered = 0;

            if (this.isOwnTurn && !own && this.enemySecretCount >= 1)
            {

                foreach (SecretItem si in this.enemySecretList)
                {
                    if (si.canBe_duplicate)
                    {
                        triggered++;
                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.FP1_018).sim_card.onSecretPlay(this, false, 0);
                        si.usedTrigger_MinionDied();
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_duplicate = false;
                        }
                    }

                    if (si.canBe_redemption)
                    {
                        //triggered++;
                        //CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_136).sim_card.onSecretPlay(this, false, 0);
                        si.usedTrigger_MinionDied();
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_redemption = false;
                        }
                    }

                    if (si.canBe_avenge)
                    {
                        //triggered++;
                        //CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.FP1_020).sim_card.onSecretPlay(this, false, 0);
                        si.usedTrigger_MinionDied();
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_avenge = false;
                        }
                    }


                }
            }

            if (turnCounter == 0)
            {
                this.evaluatePenality -= triggered * 50;
            }

        }



        public void doDeathrattles(List<Minion> deathrattles)
        {
            //todo sort them from oldest to newest (first played, first deathrattle)
            //https://www.youtube.com/watch?v=2WrbqsOSbhc
            foreach (Minion m in deathrattles)
            {
                if (!m.silenced && m.handcard.card.deathrattle) m.handcard.card.sim_card.onDeathrattle(this, m);

                for (int i = 0; i < m.souloftheforest; i++)
                {
                    CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_158t);//Treant
                    int pos = (m.own) ? this.ownMinions.Count : this.enemyMinions.Count;
                    callKid(kid, pos, m.own);
                }

                for (int i = 0; i < m.ancestralspirit; i++)
                {
                    CardDB.Card kid = m.handcard.card;
                    int pos = (m.own) ? this.ownMinions.Count : this.enemyMinions.Count;
                    callKid(kid, pos, m.own);
                }

                //baron rivendare ??
                if ((m.own && this.ownBaronRivendare >= 1) || (!m.own && this.enemyBaronRivendare >= 1))
                {
                    if (!m.silenced && m.handcard.card.deathrattle) m.handcard.card.sim_card.onDeathrattle(this, m);

                    for (int i = 0; i < m.souloftheforest; i++)
                    {
                        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_158t);//Treant
                        int pos = (m.own) ? this.ownMinions.Count : this.enemyMinions.Count;
                        callKid(kid, pos, m.own);
                    }

                    for (int i = 0; i < m.ancestralspirit; i++)
                    {
                        CardDB.Card kid = m.handcard.card;
                        int pos = (m.own) ? this.ownMinions.Count : this.enemyMinions.Count;
                        callKid(kid, pos, m.own);
                    }

                }
            }


        }


        public void updateBoards()
        {
            if (!this.tempTrigger.ownMinionsChanged && !this.tempTrigger.enemyMininsChanged) return;
            List<Minion> deathrattles = new List<Minion>();

            bool minionOwnReviving = false;
            bool minionEnemyReviving = false;

            if (this.tempTrigger.ownMinionsChanged)
            {
                this.tempTrigger.ownMinionsChanged = false;
                List<Minion> temp = new List<Minion>();
                int i = 1;
                foreach (Minion m in this.ownMinions)
                {
                    //delete adjacent buffs
                    this.minionGetAdjacentBuff(m, -m.AdjacentAngr, 0);
                    m.cantBeTargetedBySpellsOrHeroPowers = false;
                    if ((m.name == CardDB.cardName.faeriedragon || m.name == CardDB.cardName.spectralknight || m.name == CardDB.cardName.laughingsister || m.name == CardDB.cardName.arcanenullifierx21) && !m.silenced)
                    {
                        m.cantBeTargetedBySpellsOrHeroPowers = true;
                    }

                    //kill it!
                    if (m.Hp <= 0)
                    {
                        this.OwnLastDiedMinion = m.handcard.card.cardIDenum;
                        if (this.revivingOwnMinion == CardDB.cardIDEnum.None)
                        {
                            this.revivingOwnMinion = m.handcard.card.cardIDenum;
                            minionOwnReviving = true;
                        }

                        if ((!m.silenced && m.handcard.card.deathrattle) || m.ancestralspirit >= 1 || m.souloftheforest >= 1)
                        {
                            deathrattles.Add(m);
                        }
                        // end aura of minion m
                        m.handcard.card.sim_card.onAuraEnds(this, m);

                        /*if (m.handcard.card.name == CardDB.cardName.cairnebloodhoof || m.handcard.card.name == CardDB.cardName.harvestgolem || m.ancestralspirit>=1)
                        {
                            this.evaluatePenality -= Ai.Instance.botBase.getEnemyMinionValue(m, this) - 1;
                        }*/

                    }
                    else
                    {
                        m.zonepos = i;
                        temp.Add(m);
                        i++;
                    }

                }
                this.ownMinions = temp;
                this.updateAdjacentBuffs(true);
            }

            if (this.tempTrigger.enemyMininsChanged)
            {
                this.tempTrigger.enemyMininsChanged = false;
                List<Minion> temp = new List<Minion>();
                int i = 1;
                foreach (Minion m in this.enemyMinions)
                {
                    //delete adjacent buffs
                    this.minionGetAdjacentBuff(m, -m.AdjacentAngr, 0);
                    m.cantBeTargetedBySpellsOrHeroPowers = false;
                    if ((m.name == CardDB.cardName.faeriedragon || m.name == CardDB.cardName.spectralknight || m.name == CardDB.cardName.laughingsister || m.name == CardDB.cardName.arcanenullifierx21) && !m.silenced)
                    {
                        m.cantBeTargetedBySpellsOrHeroPowers = true;
                    }

                    //kill it!
                    if (m.Hp <= 0)
                    {
                        if (this.revivingEnemyMinion == CardDB.cardIDEnum.None)
                        {
                            this.revivingEnemyMinion = m.handcard.card.cardIDenum;
                            minionEnemyReviving = true;
                        }

                        if ((!m.silenced && m.handcard.card.deathrattle) || m.ancestralspirit >= 1 || m.souloftheforest >= 1)
                        {
                            deathrattles.Add(m);
                        }
                        m.handcard.card.sim_card.onAuraEnds(this, m);

                        if ((!m.silenced && (m.handcard.card.name == CardDB.cardName.cairnebloodhoof || m.handcard.card.name == CardDB.cardName.harvestgolem)) || m.ancestralspirit >= 1)
                        {
                            //this.evaluatePenality -= Ai.Instance.botBase.getEnemyMinionValue(m, this) - 1;
                        }
                    }
                    else
                    {
                        m.zonepos = i;
                        temp.Add(m);
                        i++;
                    }

                }
                this.enemyMinions = temp;
                this.updateAdjacentBuffs(false);
            }


            if (deathrattles.Count >= 1) this.doDeathrattles(deathrattles);

            if (minionOwnReviving)
            {
                this.secretTrigger_MinionDied(true);
                this.revivingOwnMinion = CardDB.cardIDEnum.None;
            }

            if (minionEnemyReviving)
            {
                this.secretTrigger_MinionDied(false);
                this.revivingEnemyMinion = CardDB.cardIDEnum.None;
            }
            //update buffs
        }

        public void minionGetOrEraseAllAreaBuffs(Minion m, bool get)
        {
            if (m.isHero) return;
            int angr = 0;
            int vert = 0;

            if (m.handcard.card.race == 14)
            {
                angr += 2 * anzMurlocWarleader + anzGrimscaleOracle;
                vert += anzMurlocWarleader;

            }

            if (!m.silenced) // if they are not silenced, these minions will give a buff, but cant buff themselfes
            {
                if (m.name == CardDB.cardName.raidleader || m.name == CardDB.cardName.leokk || m.name == CardDB.cardName.timberwolf) angr--;
                if (m.name == CardDB.cardName.stormwindchampion || m.name == CardDB.cardName.southseacaptain)
                {
                    angr--;
                    vert--;
                }
                if (m.name == CardDB.cardName.murlocwarleader)
                {
                    angr -= 2;
                    vert--;
                }
                if (m.name == CardDB.cardName.grimscaleoracle)
                {
                    angr--;
                }
            }


            if (m.own)
            {
                // todo charge:  m.charge -= anzOwnTundrarhino;
                if (get) m.charge += anzOwnTundrarhino;
                else m.charge -= anzOwnTundrarhino;
                angr += anzOwnRaidleader;
                angr += anzOwnStormwindChamps;
                vert += anzOwnStormwindChamps;
                if (m.handcard.card.race == 20)
                {
                    angr += anzOwnTimberWolfs;
                }
                if (m.handcard.card.race == 23)
                {
                    angr += anzOwnSouthseacaptain;
                    vert += anzOwnSouthseacaptain;

                }
                if (m.handcard.card.race == 15)
                {
                    angr += anzOwnMalGanis * 2;
                    vert += anzOwnMalGanis * 2;

                }

            }
            else
            {
                if (get) m.charge += anzEnemyTundrarhino;
                else m.charge -= anzEnemyTundrarhino;
                angr += anzEnemyRaidleader;
                angr += anzEnemyStormwindChamps;
                vert += anzEnemyStormwindChamps;

                if (m.handcard.card.race == 20)
                {
                    angr += anzEnemyTimberWolfs;
                }
                if (m.handcard.card.race == 23)
                {
                    angr += anzEnemySouthseacaptain;
                    vert += anzEnemySouthseacaptain;
                }
                if (m.handcard.card.race == 15)
                {
                    angr += anzEnemyMalGanis * 2;
                    vert += anzEnemyMalGanis * 2;

                }
            }

            if (get) this.minionGetBuffed(m, angr, vert);
            else this.minionGetBuffed(m, -angr, -vert);

        }

        public void updateAdjacentBuffs(bool own)
        {
            //only call this after update board
            if (own)
            {
                int anz = this.ownMinions.Count;
                for (int i = 0; i < anz; i++)
                {
                    Minion m = this.ownMinions[i];
                    if (!m.silenced)
                    {
                        if (m.name == CardDB.cardName.direwolfalpha)
                        {
                            if (i > 0) this.minionGetAdjacentBuff(this.ownMinions[i - 1], 1, 0);
                            if (i < anz - 1) this.minionGetAdjacentBuff(this.ownMinions[i + 1], 1, 0);
                        }

                        if (m.name == CardDB.cardName.flametonguetotem)
                        {
                            if (i > 0) this.minionGetAdjacentBuff(this.ownMinions[i - 1], 2, 0);
                            if (i < anz - 1) this.minionGetAdjacentBuff(this.ownMinions[i + 1], 2, 0);
                        }

                        if (m.name == CardDB.cardName.weespellstopper)
                        {
                            if (i > 0) this.ownMinions[i - 1].cantBeTargetedBySpellsOrHeroPowers = true;
                            if (i < anz - 1) this.ownMinions[i + 1].cantBeTargetedBySpellsOrHeroPowers = true;
                        }
                    }
                }
            }
            else
            {
                int anz = this.enemyMinions.Count;
                for (int i = 0; i < anz; i++)
                {
                    Minion m = this.enemyMinions[i];
                    if (!m.silenced)
                    {
                        if (m.name == CardDB.cardName.direwolfalpha)
                        {
                            if (i > 0) this.minionGetAdjacentBuff(this.enemyMinions[i - 1], 1, 0);
                            if (i < anz - 1) this.minionGetAdjacentBuff(this.enemyMinions[i + 1], 1, 0);
                        }

                        if (m.name == CardDB.cardName.flametonguetotem)
                        {
                            if (i > 0) this.minionGetAdjacentBuff(this.enemyMinions[i - 1], 2, 0);
                            if (i < anz - 1) this.minionGetAdjacentBuff(this.enemyMinions[i + 1], 2, 0);
                        }

                        if (m.name == CardDB.cardName.weespellstopper)
                        {
                            if (i > 0) this.enemyMinions[i - 1].cantBeTargetedBySpellsOrHeroPowers = true;
                            if (i < anz - 1) this.enemyMinions[i + 1].cantBeTargetedBySpellsOrHeroPowers = true;
                        }
                    }
                }
            }
        }

        public Minion createNewMinion(Handmanager.Handcard hc, int zonepos, bool own)
        {
            Minion m = new Minion();
            Handmanager.Handcard handc = new Handmanager.Handcard(hc);
            //Handmanager.Handcard handc = hc; // new Handcard(hc)?
            m.handcard = handc;
            m.own = own;
            m.isHero = false;
            m.entitiyID = hc.entity;
            m.Angr = hc.card.Attack + hc.addattack;
            m.Hp = hc.card.Health + hc.addHp;

            hc.addattack = 0;
            hc.addHp = 0;

            m.maxHp = hc.card.Health;
            m.name = hc.card.name;
            m.playedThisTurn = true;
            m.numAttacksThisTurn = 0;
            m.zonepos = zonepos;
            m.windfury = hc.card.windfury;
            m.taunt = hc.card.tank;
            m.charge = (hc.card.Charge) ? 1 : 0;
            m.divineshild = hc.card.Shield;
            m.poisonous = hc.card.poisionous;
            m.stealth = hc.card.Stealth;

            m.updateReadyness();

            if (m.name == CardDB.cardName.lightspawn)
            {
                m.Angr = m.Hp;
            }


            //trigger on summon effect!
            this.triggerAMinionIsSummoned(m);
            //activate onAura effect
            m.handcard.card.sim_card.onAuraStarts(this, m);
            //buffs minion
            this.minionGetOrEraseAllAreaBuffs(m, true);
            return m;
        }

        public void placeAmobSomewhere(Handmanager.Handcard hc, bool own, Minion target, int choice, int zonepos)
        {
            int mobplace = zonepos;

            //create the new minion + trigger Summon effects + buffs it
            Minion m = createNewMinion(hc, mobplace, own);


            //trigger the battlecry!
            m.handcard.card.sim_card.getBattlecryEffect(this, m, target, choice);

            //add minion to list + do triggers + do secret trigger +  minion was played trigger
            addMinionToBattlefield(m);

            secretTrigger_MinionIsPlayed(m);


            if (logging) Helpfunctions.Instance.logg("added " + m.handcard.card.name);
        }

        public void addMinionToBattlefield(Minion m, bool isSummon = true)
        {
            List<Minion> temp = (m.own) ? this.ownMinions : this.enemyMinions;
            if (temp.Count >= m.zonepos && m.zonepos >= 1)
            {
                temp.Insert(m.zonepos - 1, m);
            }
            else
            {
                temp.Add(m);
            }
            if (m.own) this.tempTrigger.ownMinionsChanged = true;
            else this.tempTrigger.enemyMininsChanged = true;
            doDmgTriggers();


            //minion was played secrets? trigger here---- (+ do triggers)


            //trigger a minion was summoned
            triggerAMinionWasSummoned(m);
            doDmgTriggers();

        }

        public void equipWeapon(CardDB.Card c, bool own)
        {
            Minion hero = (own) ? this.ownHero : this.enemyHero;
            if (own)
            {
                if (this.ownWeaponDurability >= 1)
                {
                    this.lostWeaponDamage += this.ownWeaponDurability * this.ownWeaponAttack * this.ownWeaponAttack;
                    this.lowerWeaponDurability(1000, true);
                    hero.Angr -= this.ownWeaponAttack;
                }
                this.ownWeaponAttack = c.Attack;
                this.ownWeaponDurability = c.Durability;
                this.ownWeaponName = c.name;
            }
            else
            {
                if (this.enemyWeaponDurability >= 1)
                {
                    hero.Angr -= this.enemyWeaponAttack;
                }
                this.enemyWeaponAttack = c.Attack;
                this.enemyWeaponDurability = c.Durability;
                this.enemyWeaponName = c.name;
            }



            hero.Angr += c.Attack;

            hero.windfury = (c.name == CardDB.cardName.doomhammer);

            hero.updateReadyness();

            hero.immuneWhileAttacking = (c.name == CardDB.cardName.gladiatorslongbow);

            List<Minion> temp = (own) ? this.ownMinions : this.enemyMinions;
            foreach (Minion m in temp)
            {
                if (m.playedThisTurn && m.name == CardDB.cardName.southseadeckhand)
                {
                    minionGetCharge(m);
                }
            }

        }


        //todo 4th param
        public void callKid(CardDB.Card c, int zonepos, bool own, bool spawnKid = false, bool oneMoreIsAllowed = false)
        {
            //spawnKid = true if its a minion spawned with another one (battlecry)
            int allowed = 7;
            allowed += (oneMoreIsAllowed) ? 1 : 0;
            allowed -= (spawnKid) ? 1 : 0;

            if (own)
            {
                if (this.ownMinions.Count >= allowed)
                {
                    if (spawnKid) this.evaluatePenality += 20;
                    return;
                }
            }
            else
            {
                if (this.enemyMinions.Count >= allowed)
                {
                    if (spawnKid) this.evaluatePenality -= 20;
                    return;
                }
            }
            int mobplace = zonepos + 1;//todo check this?

            //create minion (+triggers)
            Handmanager.Handcard hc = new Handmanager.Handcard(c) { entity = this.getNextEntity() };
            Minion m = createNewMinion(hc, mobplace, own);
            //put it on battle field (+triggers)
            addMinionToBattlefield(m);

        }



        public void minionGetSilenced(Minion m)
        {
            //minion cant die due to silencing!
            m.becomeSilence(this);

        }

        public void allMinionsGetSilenced(bool own)
        {
            List<Minion> temp = (own) ? this.ownMinions : this.enemyMinions;
            foreach (Minion m in temp)
            {
                m.becomeSilence(this);
            }
        }

        public void drawACard(CardDB.cardName ss, bool own, bool nopen = false)
        {
            CardDB.cardName s = ss;

            // cant hold more than 10 cards
            if (own)
            {

                if (s == CardDB.cardName.unknown && !nopen) // draw a card from deck :D
                {
                    if (ownDeckSize == 0)
                    {
                        this.ownHeroFatigue++;
                        this.ownHero.getDamageOrHeal(this.ownHeroFatigue, this, false, true);
                        return;
                    }
                    else
                    {
                        this.ownDeckSize--;
                        if (this.owncards.Count >= 10)
                        {
                            this.evaluatePenality += 15;
                            return;
                        }
                        this.owncarddraw++;
                    }

                }
                else
                {
                    if (this.owncards.Count >= 10)
                    {
                        this.evaluatePenality += 5;
                        return;
                    }
                    this.owncarddraw++;

                }


            }
            else
            {
                if (s == CardDB.cardName.unknown && !nopen) // draw a card from deck :D
                {
                    if (enemyDeckSize == 0)
                    {
                        this.enemyHeroFatigue++;
                        this.enemyHero.getDamageOrHeal(this.enemyHeroFatigue, this, false, true);
                        return;
                    }
                    else
                    {
                        this.enemyDeckSize--;
                        if (this.enemyAnzCards >= 10)
                        {
                            this.evaluatePenality -= 50;
                            return;
                        }
                        this.enemycarddraw++;
                        this.enemyAnzCards++;
                    }

                }
                else
                {
                    if (this.enemyAnzCards >= 10)
                    {
                        this.evaluatePenality -= 50;
                        return;
                    }
                    this.enemycarddraw++;
                    this.enemyAnzCards++;
                }
                this.triggerCardsChanged(false);

                if (anzEnemyChromaggus > 0 && s == CardDB.cardName.unknown && !nopen)
                {
                    for (int i = 1; i <= anzEnemyChromaggus; i++)
                    {
                        if (this.enemyAnzCards >= 10)
                        {
                            this.evaluatePenality -= 50;
                            return;
                        }
                        this.enemycarddraw++;
                        this.enemyAnzCards++;
                        this.triggerCardsChanged(false);

                    }
                }
                return;
            }

            if (s == CardDB.cardName.unknown)
            {
                CardDB.Card plchldr = new CardDB.Card { name = CardDB.cardName.unknown };
                Handmanager.Handcard hc = new Handmanager.Handcard { card = plchldr, position = this.owncards.Count + 1, manacost = 1000, entity = this.getNextEntity() };
                this.owncards.Add(hc);
                this.triggerCardsChanged(true);
            }
            else
            {
                CardDB.Card c = CardDB.Instance.getCardData(s);
                Handmanager.Handcard hc = new Handmanager.Handcard { card = c, position = this.owncards.Count + 1, manacost = c.calculateManaCost(this), entity = this.getNextEntity() };
                this.owncards.Add(hc);
                this.triggerCardsChanged(true);
            }

            if (anzOwnChromaggus > 0 && s == CardDB.cardName.unknown && !nopen) // only draw a card from deck
            {
                CardDB.Card plchldr = new CardDB.Card { name = CardDB.cardName.unknown };
                for (int i = 1; i <= anzOwnChromaggus; i++)
                {
                    if (this.owncards.Count >= 10)
                    {
                        this.evaluatePenality += 15;
                        return;
                    }
                    this.owncarddraw++;

                    Handmanager.Handcard hc = new Handmanager.Handcard { card = plchldr, position = this.owncards.Count + 1, manacost = 1000, entity = this.getNextEntity() };
                    this.owncards.Add(hc);
                    this.triggerCardsChanged(true);
                }
            }

        }

        public void drawACard(CardDB.cardIDEnum ss, bool own, bool nopen = false)
        {
            CardDB.cardIDEnum s = ss;

            // cant hold more than 10 cards

            if (own)
            {
                if (s == CardDB.cardIDEnum.None && !nopen) // draw a card from deck :D
                {
                    if (ownDeckSize == 0)
                    {
                        this.ownHeroFatigue++;
                        this.ownHero.getDamageOrHeal(this.ownHeroFatigue, this, false, true);
                        return;
                    }
                    else
                    {
                        this.ownDeckSize--;
                        if (this.owncards.Count >= 10)
                        {
                            this.evaluatePenality += 15;
                            return;
                        }
                        this.owncarddraw++;
                    }

                }
                else
                {
                    if (this.owncards.Count >= 10)
                    {
                        this.evaluatePenality += 5;
                        return;
                    }
                    this.owncarddraw++;
                }
            }
            else
            {
                if (s == CardDB.cardIDEnum.None && !nopen) // draw a card from deck :D
                {
                    if (enemyDeckSize == 0)
                    {
                        this.enemyHeroFatigue++;
                        this.enemyHero.getDamageOrHeal(this.enemyHeroFatigue, this, false, true);
                        return;
                    }
                    else
                    {
                        this.enemyDeckSize--;
                        if (this.enemyAnzCards >= 10)
                        {
                            this.evaluatePenality -= 50;
                            return;
                        }
                        this.enemycarddraw++;
                        this.enemyAnzCards++;
                    }
                }
                else
                {
                    if (this.enemyAnzCards >= 10)
                    {
                        this.evaluatePenality -= 50;
                        return;
                    }
                    this.enemycarddraw++;
                    this.enemyAnzCards++;

                }
                this.triggerCardsChanged(false);

                if (anzEnemyChromaggus > 0 && s == CardDB.cardIDEnum.None && !nopen)
                {
                    for (int i = 1; i <= anzEnemyChromaggus; i++)
                    {
                        if (this.enemyAnzCards >= 10)
                        {
                            this.evaluatePenality -= 50;
                            return;
                        }
                        this.enemycarddraw++;
                        this.enemyAnzCards++;
                        this.triggerCardsChanged(false);
                    }
                }
                return;
            }

            if (s == CardDB.cardIDEnum.None)
            {
                CardDB.Card plchldr = new CardDB.Card { name = CardDB.cardName.unknown };
                Handmanager.Handcard hc = new Handmanager.Handcard { card = plchldr, position = this.owncards.Count + 1, manacost = 1000, entity = this.getNextEntity() };
                this.owncards.Add(hc);
                this.triggerCardsChanged(true);
            }
            else
            {
                CardDB.Card c = CardDB.Instance.getCardDataFromID(s);
                Handmanager.Handcard hc = new Handmanager.Handcard { card = c, position = this.owncards.Count + 1, manacost = c.calculateManaCost(this), entity = this.getNextEntity() };
                this.owncards.Add(hc);
                this.triggerCardsChanged(true);
            }

            if (anzOwnChromaggus > 0 && s == CardDB.cardIDEnum.None && !nopen) // only draw a card from deck
            {
                CardDB.Card plchldr = new CardDB.Card { name = CardDB.cardName.unknown };
                for (int i = 1; i <= anzOwnChromaggus; i++)
                {
                    if (this.owncards.Count >= 10)
                    {
                        this.evaluatePenality += 15;
                        return;
                    }
                    this.owncarddraw++;

                    Handmanager.Handcard hc = new Handmanager.Handcard { card = plchldr, position = this.owncards.Count + 1, manacost = 1000, entity = this.getNextEntity() };
                    this.owncards.Add(hc);
                    this.triggerCardsChanged(true);
                }
            }

        }


        public void removeCard(Handmanager.Handcard hcc)
        {
            int cardPos = 1;
            int cardNum = 0;
            Handmanager.Handcard hcTmp = null;
            foreach (Handmanager.Handcard hc in this.owncards)
            {
                if (hc.entity == hcc.entity)
                {
                    Helpfunctions.Instance.ErrorLog("find in hand");
                    hcTmp = hc;
                    cardNum++;
                    continue;
                }
                this.owncards[cardNum].position = cardPos;
                cardNum++;
                cardPos++;
            }
            if (hcTmp != null) this.owncards.Remove(hcTmp);
        }


        // some helpfunctions 


        public void attackEnemyHeroWithoutKill(int dmg)
        {
            this.enemyHero.cantLowerHPbelowONE = true;
            this.minionGetDamageOrHeal(this.enemyHero, dmg);
            this.enemyHero.cantLowerHPbelowONE = false;
        }

        public void minionGetDestroyed(Minion m)
        {
            if (m.Hp > 0)
            {
                m.Hp = 0;
                m.minionDied(this);
                GraveYardItem gyi = new GraveYardItem(m.handcard.card.cardIDenum, m.entitiyID, m.own);
                diedMinions.Add(gyi);
            }
        }

        public void allMinionsGetDestroyed()
        {
            foreach (Minion m in this.ownMinions)
            {
                minionGetDestroyed(m);
            }
            foreach (Minion m in this.enemyMinions)
            {
                minionGetDestroyed(m);
            }
        }


        public void minionGetArmor(Minion m, int armor)
        {
            m.armor += armor;
            this.triggerAHeroGotArmor(m.own);
        }

        public void minionReturnToHand(Minion m, bool own, int manachange)
        {
            List<Minion> temp = (own) ? this.ownMinions : this.enemyMinions;
            m.handcard.card.sim_card.onAuraEnds(this, m);
            temp.Remove(m);

            if (own)
            {
                CardDB.Card c = m.handcard.card;
                Handmanager.Handcard hc = new Handmanager.Handcard { card = c, position = this.owncards.Count + 1, entity = m.entitiyID, manacost = c.calculateManaCost(this) + manachange };
                if (this.owncards.Count < 10)
                {
                    this.owncards.Add(hc);
                    this.triggerCardsChanged(true);
                }
                else
                {
                    this.drawACard(CardDB.cardName.unknown, true);
                }
                this.tempTrigger.ownMinionsChanged = true;
            }
            else
            {
                this.drawACard(CardDB.cardName.unknown, true);
                this.tempTrigger.enemyMininsChanged = true;
            }

        }

        public void minionTransform(Minion m, CardDB.Card c)
        {
            m.handcard.card.sim_card.onAuraEnds(this, m);//end aura of the minion

            Handmanager.Handcard hc = new Handmanager.Handcard(c) { entity = m.entitiyID };
            int ancestral = m.ancestralspirit;
            if (m.handcard.card.name == CardDB.cardName.cairnebloodhoof || m.handcard.card.name == CardDB.cardName.harvestgolem || ancestral >= 1)
            {
                //this.evaluatePenality -= Ai.Instance.botBase.getEnemyMinionValue(m, this) - 1;
            }

            //necessary???
            /*Minion tranform = createNewMinion(hc, m.zonepos, m.own);
            Minion temp = new Minion();
            temp.setMinionTominion(m);
            m.setMinionTominion(tranform);*/

            m.setMinionTominion(createNewMinion(hc, m.zonepos, m.own));

            m.handcard.card.sim_card.onAuraStarts(this, m);
            this.minionGetOrEraseAllAreaBuffs(m, true);

            if (m.own)
            {
                this.tempTrigger.ownMinionsChanged = true;
            }
            else
            {
                this.tempTrigger.enemyMininsChanged = true;
            }

            if (logging) Helpfunctions.Instance.logg("minion got sheep" + m.name + " " + m.Angr);
        }

        public void minionGetControlled(Minion m, bool newOwner, bool canAttack)
        {
            List<Minion> newOwnerList = (newOwner) ? this.ownMinions : this.enemyMinions;
            List<Minion> oldOwnerList = (newOwner) ? this.enemyMinions : this.ownMinions;



            if (newOwnerList.Count >= 7) return;

            this.tempTrigger.ownMinionsChanged = true;
            this.tempTrigger.enemyMininsChanged = true;
            if (m.taunt && newOwner) this.anzEnemyTaunt--;

            //end buffs/aura
            m.handcard.card.sim_card.onAuraEnds(this, m);
            this.minionGetOrEraseAllAreaBuffs(m, false);

            //remove minion from list
            oldOwnerList.Remove(m);

            //change site (and minion is played in this turn)
            m.playedThisTurn = true;
            m.own = !m.own;

            // add minion to new list + new buffs
            newOwnerList.Add(m);
            m.handcard.card.sim_card.onAuraStarts(this, m);
            this.minionGetOrEraseAllAreaBuffs(m, true);

            if (m.charge >= 1 || canAttack) // minion can attack if its shadowmadnessed (canAttack = true) or it has charge
            {
                this.minionGetCharge(m);
            }
            m.updateReadyness();

        }



        public void minionGetWindfurry(Minion m)
        {
            if (m.windfury) return;
            m.windfury = true;
            m.updateReadyness();
        }

        public void minionGetCharge(Minion m)
        {
            m.charge++;
            m.updateReadyness();
        }

        public void minionLostCharge(Minion m)
        {
            m.charge--;
            m.updateReadyness();
        }



        public void minionGetTempBuff(Minion m, int tempAttack, int tempHp)
        {
            if (!m.silenced && m.name == CardDB.cardName.lightspawn) return;
            if (tempAttack < 0 && -tempAttack > m.Angr)
            {
                tempAttack = -m.Angr;
            }
            m.tempAttack += tempAttack;
            m.Angr += tempAttack;
        }

        public void minionGetAdjacentBuff(Minion m, int angr, int vert)
        {
            if (!m.silenced && m.name == CardDB.cardName.lightspawn) return;
            m.Angr += angr;
            m.AdjacentAngr += angr;
        }

        public void minionGetBuffed(Minion m, int attackbuff, int hpbuff)
        {
            m.Angr = Math.Max(0, m.Angr + attackbuff);

            if (hpbuff >= 1)
            {
                m.Hp = m.Hp + hpbuff;
                m.maxHp = m.maxHp + hpbuff;
            }
            else
            {
                //debuffing hp, lower only maxhp (unless maxhp < hp)
                m.maxHp = m.maxHp + hpbuff;
                if (m.maxHp < m.Hp)
                {
                    m.Hp = m.maxHp;
                }
            }


            m.wounded = (m.maxHp != m.Hp);

            if (m.name == CardDB.cardName.lightspawn && !m.silenced)
            {
                m.Angr = m.Hp;
            }

        }

        public void minionSetAngrToOne(Minion m)
        {
            if (!m.silenced && m.name == CardDB.cardName.lightspawn) return;
            m.Angr = 1;
            m.tempAttack = 0;
            this.minionGetOrEraseAllAreaBuffs(m, true);
        }

        public void minionSetLifetoOne(Minion m)
        {
            minionGetOrEraseAllAreaBuffs(m, false);
            m.Hp = 1;
            m.maxHp = 1;
            if (m.wounded && !m.silenced) m.handcard.card.sim_card.onEnrageStop(this, m);
            m.wounded = false;
            minionGetOrEraseAllAreaBuffs(m, true);
        }

        public void minionSetAngrToHP(Minion m)
        {
            m.Angr = m.Hp;
            m.tempAttack = 0;
            this.minionGetOrEraseAllAreaBuffs(m, true);

        }

        public void minionSwapAngrAndHP(Minion m)
        {
            this.minionGetOrEraseAllAreaBuffs(m, false);
            bool woundedbef = m.wounded;
            int temp = m.Angr;
            m.Angr = m.Hp;
            m.Hp = temp;
            m.maxHp = temp;
            m.wounded = false;
            if (woundedbef) m.handcard.card.sim_card.onEnrageStop(this, m);
            if (m.Hp <= 0)
            {
                if (m.own) this.tempTrigger.ownMinionsDied++;
                else this.tempTrigger.enemyMinionsDied++;
            }

            this.minionGetOrEraseAllAreaBuffs(m, true);
        }

        public void minionGetDamageOrHeal(Minion m, int dmgOrHeal, bool dontDmgLoss = false)
        {
            m.getDamageOrHeal(dmgOrHeal, this, false, dontDmgLoss);
        }



        public void allMinionOfASideGetDamage(bool own, int damages, bool frozen = false)
        {
            List<Minion> temp = (own) ? this.ownMinions : this.enemyMinions;
            foreach (Minion m in temp)
            {
                if (frozen) m.frozen = true;
                minionGetDamageOrHeal(m, damages, true);
            }
        }

        public void allCharsOfASideGetDamage(bool own, int damages)
        {
            //ALL CHARS get same dmg
            List<Minion> temp = (own) ? this.ownMinions : this.enemyMinions;
            foreach (Minion m in temp)
            {
                minionGetDamageOrHeal(m, damages, true);
            }

            this.minionGetDamageOrHeal(own ? this.ownHero : this.enemyHero, damages);
        }

        public void allCharsGetDamage(int damages)
        {
            foreach (Minion m in this.ownMinions)
            {
                minionGetDamageOrHeal(m, damages, true);
            }
            foreach (Minion m in this.enemyMinions)
            {
                minionGetDamageOrHeal(m, damages, true);
            }
            minionGetDamageOrHeal(this.ownHero, damages);
            minionGetDamageOrHeal(this.enemyHero, damages);
        }

        public void allMinionsGetDamage(int damages)
        {
            foreach (Minion m in this.ownMinions)
            {
                minionGetDamageOrHeal(m, damages, true);
            }
            foreach (Minion m in this.enemyMinions)
            {
                minionGetDamageOrHeal(m, damages, true);
            }
        }

        public enum searchmode
        {
            searchLowestHP,
            searchHighestHP,
            searchLowestAttack,
            searchHighestAttack,
            searchHighAttackLowHP,
            searchHighHPLowAttack,
        }

        public Minion searchRandomMinion(List<Minion> minions, searchmode mode)
        {
            //get = 0 -> get lowest hp
            //get = 1 -> get highest hp
            //get = 2 -> get lowest attack
            //get = 3 -> get highest attack
            //get = 4 -> get max attack/hp ratio
            //get = 5 -> get max hp/attack ratio
            int get = (int)mode;

            if (minions.Count == 0) return null;
            Minion ret = minions[0];
            int value = ret.Hp;
            if (get >= 2) value = ret.Angr;
            if (get >= 4) value = 0;
            foreach (Minion m in minions)
            {
                if (m.Hp <= 0) continue;

                switch (get)
                {
                    case 1:
                        if (m.Hp > value)
                        {
                            ret = m;
                            value = m.Hp;
                        }
                        break;
                    case 2:
                        if (m.Angr < value)
                        {
                            ret = m;
                            value = m.Angr;
                        }
                        break;
                    case 3:
                        if (m.Angr > value)
                        {
                            ret = m;
                            value = m.Angr;
                        }
                        break;
                    case 4:
                        if (m.Angr / m.Hp > value)
                        {
                            ret = m;
                            value = m.Angr / m.Hp;
                        }
                        break;
                    case 5:
                        if (m.Hp / m.Angr > value)
                        {
                            ret = m;
                            value = m.Hp / m.Angr;
                        }
                        break;
                    default:
                        if (m.Hp < value)
                        {
                            ret = m;
                            value = m.Hp;
                        }
                        break;
                }
            }
            if (ret.Hp <= 0) return null;
            return ret;
        }


        public void debugMinions()
        {
            Helpfunctions.Instance.logg("OWN MINIONS################");

            foreach (Minion m in this.ownMinions)
            {
                Helpfunctions.Instance.logg("name,ang, hp, maxhp: " + m.name + ", " + m.Angr + ", " + m.Hp + ", " + m.maxHp);
            }

            Helpfunctions.Instance.logg("ENEMY MINIONS############");
            foreach (Minion m in this.enemyMinions)
            {
                Helpfunctions.Instance.logg("name,ang, hp: " + m.name + ", " + m.Angr + ", " + m.Hp);
            }
        }

        public void printBoard()
        {
            Helpfunctions.Instance.logg("board/hash: " + value + "  /  " + this.hashcode + " ++++++++++++++++++++++");
            Helpfunctions.Instance.logg("pen " + this.evaluatePenality);
            Helpfunctions.Instance.logg("mana " + this.mana + "/" + this.ownMaxMana);
            Helpfunctions.Instance.logg("cardsplayed: " + this.cardsPlayedThisTurn + " handsize: " + this.owncards.Count + " eh " + this.enemyAnzCards);

            Helpfunctions.Instance.logg("ownhero: ");
            Helpfunctions.Instance.logg("ownherohp: " + this.ownHero.Hp + " + " + this.ownHero.armor);
            Helpfunctions.Instance.logg("ownheroattac: " + this.ownHero.Angr);
            Helpfunctions.Instance.logg("ownheroweapon: " + this.ownWeaponAttack + " " + this.ownWeaponDurability + " " + this.ownWeaponName);
            Helpfunctions.Instance.logg("ownherostatus: frozen" + this.ownHero.frozen + " ");
            Helpfunctions.Instance.logg("enemyherohp: " + this.enemyHero.Hp + " + " + this.enemyHero.armor + ((this.enemyHero.immune) ? " immune" : ""));

            if (this.enemySecretCount >= 1) Helpfunctions.Instance.logg("enemySecrets: " + Probabilitymaker.Instance.getEnemySecretData(this.enemySecretList));
            foreach (Action a in this.playactions)
            {
                a.print();
            }
            Helpfunctions.Instance.logg("OWN MINIONS################");

            foreach (Minion m in this.ownMinions)
            {
                Helpfunctions.Instance.logg("name,ang, hp: " + m.name + ", " + m.Angr + ", " + m.Hp + " " + m.entitiyID);
            }

            Helpfunctions.Instance.logg("ENEMY MINIONS############");
            foreach (Minion m in this.enemyMinions)
            {
                Helpfunctions.Instance.logg("name,ang, hp: " + m.name + ", " + m.Angr + ", " + m.Hp + " " + m.entitiyID);
            }


            Helpfunctions.Instance.logg("");
        }

        public void printBoardDebug()
        {
            Helpfunctions.Instance.logg("hero " + this.ownHero.Hp + " " + this.ownHero.armor + " " + this.ownHero.entitiyID);
            Helpfunctions.Instance.logg("ehero " + this.enemyHero.Hp + " " + this.enemyHero.armor + " " + this.enemyHero.entitiyID);
            foreach (Minion m in ownMinions)
            {
                Helpfunctions.Instance.logg(m.name + " " + m.entitiyID);
            }
            Helpfunctions.Instance.logg("-");
            foreach (Minion m in enemyMinions)
            {
                Helpfunctions.Instance.logg(m.name + " " + m.entitiyID);
            }
            Helpfunctions.Instance.logg("-");
            foreach (Handmanager.Handcard hc in this.owncards)
            {
                Helpfunctions.Instance.logg(hc.position + " " + hc.card.name + " " + hc.entity);
            }
        }

        public Action getNextAction()
        {
            if (this.playactions.Count >= 1) return this.playactions[0];
            return null;
        }

        public void printActions(bool toBuffer = false)
        {
            foreach (Action a in this.playactions)
            {
                a.print(toBuffer);
                Helpfunctions.Instance.logg("");
            }
        }

        public void printActionforDummies(Action a)
        {
            if (a.actionType == actionEnum.playcard)
            {
                Helpfunctions.Instance.ErrorLog("play " + a.card.card.name);
                if (a.druidchoice >= 1)
                {
                    string choose = (a.druidchoice == 1) ? "left card" : "right card";
                    Helpfunctions.Instance.ErrorLog("choose the " + choose);
                }
                if (a.place >= 1)
                {
                    Helpfunctions.Instance.ErrorLog("on position " + a.place);
                }
                if (a.target != null)
                {
                    if (!a.target.own && !a.target.isHero)
                    {
                        string ename = "" + a.target.name;
                        Helpfunctions.Instance.ErrorLog("and target to the enemy " + ename);
                    }

                    if (a.target.own && !a.target.isHero)
                    {
                        string ename = "" + a.target.name;
                        Helpfunctions.Instance.ErrorLog("and target to your own" + ename);
                    }

                    if (a.target.own && a.target.isHero)
                    {
                        Helpfunctions.Instance.ErrorLog("and target your own hero");
                    }

                    if (!a.target.own && a.target.isHero)
                    {
                        Helpfunctions.Instance.ErrorLog("and target to the enemy hero");
                    }
                }

            }
            if (a.actionType == actionEnum.attackWithMinion)
            {
                string name = "" + a.own.name;
                if (a.target.isHero)
                {
                    Helpfunctions.Instance.ErrorLog("attack with: " + name + " the enemy hero");
                }
                else
                {
                    string ename = "" + a.target.name;
                    Helpfunctions.Instance.ErrorLog("attack with: " + name + " the enemy: " + ename);
                }

            }

            if (a.actionType == actionEnum.attackWithHero)
            {
                if (a.target.isHero)
                {
                    Helpfunctions.Instance.ErrorLog("attack with your hero the enemy hero!");
                }
                else
                {
                    string ename = "" + a.target.name;
                    Helpfunctions.Instance.ErrorLog("attack with the hero, and choose the enemy: " + ename);
                }
            }
            if (a.actionType == actionEnum.useHeroPower)
            {
                Helpfunctions.Instance.ErrorLog("use your Heropower ");
                if (a.target != null)
                {
                    if (!a.target.own && !a.target.isHero)
                    {
                        string ename = "" + a.target.name;
                        Helpfunctions.Instance.ErrorLog("on enemy: " + ename);
                    }

                    if (a.target.own && !a.target.isHero)
                    {
                        string ename = "" + a.target.name;
                        Helpfunctions.Instance.ErrorLog("on your own: " + ename);
                    }

                    if (a.target.own && a.target.isHero)
                    {
                        Helpfunctions.Instance.ErrorLog("on your own hero");
                    }

                    if (!a.target.own && a.target.isHero)
                    {
                        Helpfunctions.Instance.ErrorLog("on your the enemy hero");
                    }

                }
            }
            Helpfunctions.Instance.ErrorLog("");

        }


    }

       public class Helpfunctions
    {

        HRSim.MainWindow window = null;

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

        private static Helpfunctions instance;

        public static Helpfunctions Instance
        {
            get
            {
                return instance ?? (instance = new Helpfunctions());
            }
        }

        private Helpfunctions()
        {
            //foreach (Window window in Application.Current.Windows)
            //{
            //    if (window.GetType() == typeof(HRSim.MainWindow))
            //    {
            //        this.window = (HRSim.MainWindow) window;
            //    }
            //}
            System.IO.File.WriteAllText(Settings.Instance.logpath + Settings.Instance.logfile, "");
        }

        private bool writelogg = true;
        public void loggonoff(bool onoff)
        {
            writelogg = onoff;
        }

        public void createNewLoggfile()
        {
            System.IO.File.WriteAllText(Settings.Instance.logpath + Settings.Instance.logfile, "");
        }

        public void logg(string s)
        {

            if (!writelogg) return;
            try
            {
                using (StreamWriter sw = File.AppendText(Settings.Instance.logpath + Settings.Instance.logfile))
                {
                    sw.WriteLine(s);
                }
            }
            catch { }
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
            this.sendbuffer += "<EoF>";
            while (writed)
            {
                try
                {
                    System.IO.File.WriteAllText(Settings.Instance.path + "crrntbrd.txt", this.sendbuffer);
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
            this.ErrorLog("write to action file: "+ sendbuffer);
            while (writed)
            {
                try
                {
                    System.IO.File.WriteAllText(Settings.Instance.path + "actionstodo.txt", this.sendbuffer);
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

    //implementation

    public class MyHandManager
        {

            public List<Handmanager.Handcard> handCards = new List<Handmanager.Handcard>();

            public int anzcards = 0;

            public int enemyAnzCards = 0;

            private int ownPlayerController = 0;

            Helpfunctions help;
            CardDB cdb = CardDB.Instance;

            public MyHandManager()
            {
                this.help = Helpfunctions.Instance;
            }

            public void clearAll()
            {
                this.handCards.Clear();
                this.anzcards = 0;
                this.enemyAnzCards = 0;
                this.ownPlayerController = 0;
            }

            public void setOwnPlayer(int player)
            {
                this.ownPlayerController = player;
            }

            public void setHandcards(List<Handmanager.Handcard> hc, int anzown, int anzenemy)
            {
                this.handCards.Clear();
                foreach (Handmanager.Handcard h in hc)
                {
                    this.handCards.Add(new Handmanager.Handcard(h));
                }
                //this.handCards.AddRange(hc);
                this.handCards.Sort((a, b) => a.position.CompareTo(b.position));
                this.anzcards = anzown;
                this.enemyAnzCards = anzenemy;
            }


            public void printcards(bool writeTobuffer = false)
            {
                help.logg("Own Handcards: ");
                foreach (Handmanager.Handcard c in this.handCards)
                {
                    help.logg("pos " + c.position + " " + c.card.name + " " + c.manacost + " entity " + c.entity + " " + c.card.cardIDenum + " " + c.addattack);
                }
                help.logg("Enemy cards: " + this.enemyAnzCards);

                //todo print died minions this turn!

                /*if(Ai.Instance.playaround)
                {
                    if(Hrtprozis.Instance.enemyHeroname == HeroEnum.mage)
                    {
                        help.logg("probs: "  + Probabilitymaker.Instance.anzCardsInDeck(CardDB.cardIDEnum.CS2_032) + " " + Probabilitymaker.Instance.anzCardsInDeck(CardDB.cardIDEnum.CS2_028));
                    }

                    if (Hrtprozis.Instance.enemyHeroname == HeroEnum.warrior)
                    {
                        help.logg("probs: "  + Probabilitymaker.Instance.anzCardsInDeck(CardDB.cardIDEnum.EX1_400));
                    }

                    if (Hrtprozis.Instance.enemyHeroname == HeroEnum.hunter)
                    {
                        help.logg("probs: "  + Probabilitymaker.Instance.anzCardsInDeck(CardDB.cardIDEnum.EX1_538));
                    }

                    if (Hrtprozis.Instance.enemyHeroname == HeroEnum.priest)
                    {
                        help.logg("probs: "  + Probabilitymaker.Instance.anzCardsInDeck(CardDB.cardIDEnum.CS1_112));
                    }

                    if (Hrtprozis.Instance.enemyHeroname == HeroEnum.shaman)
                    {
                        help.logg("probs: "  + Probabilitymaker.Instance.anzCardsInDeck(CardDB.cardIDEnum.EX1_259));
                    }

                    if (Hrtprozis.Instance.enemyHeroname == HeroEnum.pala)
                    {
                        help.logg("probs: "  + Probabilitymaker.Instance.anzCardsInDeck(CardDB.cardIDEnum.CS2_093));
                    }

                    if (Hrtprozis.Instance.enemyHeroname == HeroEnum.druid)
                    {
                        help.logg("probs: "  + Probabilitymaker.Instance.anzCardsInDeck(CardDB.cardIDEnum.CS2_012));
                    }
                }*/

                if (writeTobuffer)
                {
                    help.writeToBuffer("Own Handcards: ");
                    foreach (Handmanager.Handcard c in this.handCards)
                    {
                        help.writeToBuffer("pos " + c.position + " " + c.card.name + " " + c.manacost + " entity " + c.entity + " " + c.card.cardIDenum + " " + c.addattack);
                    }
                    help.writeToBuffer("Enemy cards: " + this.enemyAnzCards);

                    //todo print died minions this turn!

                }
            }


        }


    //end of implementation

}


namespace HREngine.Bots
{
    public class Bot
    {
        private int concedeLvl = 5; // the rank, till you want to concede
        PenalityManager penman = PenalityManager.Instance;
        DateTime starttime = DateTime.Now;
        Silverfish sf;
        Behavior behave = new BehaviorControl();

        public Bot(Silverfish sf)
        {
            starttime = DateTime.Now;
            Settings set = Settings.Instance;
            this.sf = sf;
            set.setSettings();
            sf.setnewLoggFile();

            bool teststuff = true;
            bool printstuff = false;

            //if (teststuff)
            //{
            //    Helpfunctions.Instance.logg("teststuff");
            //    Playfield p = new Playfield();
            //    Ai.Instance.autoTester( printstuff);
            //}
            //Helpfunctions.Instance.ErrorLog("wait for board...");
        }

        public void doData(string data)
        {
            Ai.Instance.updateTwoTurnSim();
            Ai.Instance.autoTester(false, data);
            HRSim.Helpfunctions.Instance.resetBuffer();
            HRSim.Helpfunctions.Instance.writeToBuffer("board " + Ai.Instance.currentCalculatedBoard);
            HRSim.Helpfunctions.Instance.writeToBuffer("value " + Ai.Instance.bestmoveValue);
            if (Ai.Instance.bestmove != null)
            {
                Ai.Instance.bestmove.print(true);
                foreach (Action a in Ai.Instance.bestActions)
                {
                    a.print(true);
                }
            }
            HRSim.Helpfunctions.Instance.writeBufferToActionFile();
            Ai.Instance.currentCalculatedBoard = "1";
            HRSim.Helpfunctions.Instance.ErrorLog("wait for next board...");

            //sf.readActionFile();
        }

        public void testing(int start)
        {
            //for (int i = start; i < 900; i++)
            //{
            //    homeHandManager.anzcards = 1;
            //    Handmanager.Instance.handCards.Clear();
            //    Handmanager.Handcard hc = new Handmanager.Handcard();
            //    hc.manacost = 1;
            //    hc.position = 1;
            //    hc.entity = 122;
            //    hc.card = CardDB.Instance.getCardDataFromID((CardDB.cardIDEnum)i);
            //    Handmanager.Instance.handCards.Add(hc);
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

    public class Silverfish
    {
        public string versionnumber = "115.0";
        private bool singleLog = false;
        private string botbehave = "rush";

        Settings sttngs = Settings.Instance;

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

        //public static Silverfish Instance
        //{
        //    get
        //    {
        //        return instance ?? (instance = new Silverfish());
        //    }
        //}

        public Silverfish()
        {
            this.singleLog = Settings.Instance.writeToSingleFile;
            HRSim.Helpfunctions.Instance.ErrorLog("init Silverfish");
            string path = "";
            //System.IO.Directory.CreateDirectory(path);
            sttngs.setFilePath("C:\\Code\\ConsoleApplication1\\ConsoleApplication1\\");

            if (!singleLog)
            {
                sttngs.setLoggPath(path);
            }
            else
            {
                sttngs.setLoggPath("");
                sttngs.setLoggFile("UILogg.txt");
                try
                {
                    HRSim.Helpfunctions.Instance.createNewLoggfile();
                }
                catch
                {
                }
            }
            PenalityManager.Instance.setCombos();
            Mulligan m = Mulligan.Instance; // read the mulligan list
        }

        public void updateHeroStuff(HRSim.Playfield p)
        {

            HRSim.Helpfunctions.Instance.logg("" + p.ownHero.name);

            int ownheroentity = p.ownHero.entitiyID;
            int enemyheroentity = p.enemyHero.entitiyID;
            this.ownHero = new Minion(p.ownHero);
            this.enemyHero = new Minion(p.enemyHero);

            this.currentMana = p.mana;
            this.ownMaxMana = p.ownMaxMana;
            this.enemyMaxMana = p.enemyMaxMana;

            //secret
            this.ownSecretList = new List<string>();
            foreach (CardDB.cardIDEnum cardIDenum in p.ownSecretsIDList)
            {
                this.ownSecretList.Add(cardIDenum.ToString());
            }
            enemySecretCount = p.enemySecretCount;

            //minions
            this.numMinionsPlayedThisTurn = p.mobsplayedThisTurn;
            this.cardsPlayedThisTurn = p.cardsPlayedThisTurn;
            //ueberladung = TritonHs.RecallOwed;

            //get weapon stuff
            this.ownHeroWeapon = p.ownWeaponName.ToString();
            this.heroWeaponAttack = p.ownWeaponAttack;
            this.heroWeaponDurability = p.ownWeaponDurability;

            this.ownHeroFatigue = p.ownHeroFatigue;
            this.enemyHeroFatigue = p.enemyHeroFatigue;

            this.ownDecksize = p.ownDeckSize;
            this.enemyDecksize = p.enemyDeckSize;

            //own hero stuff###########################
            int heroAtk = p.ownHero.Angr;
            int heroHp = p.ownHero.Hp;
            int heroDefence = p.ownHero.armor;
            this.heroname = "mage"; //hard coded

            //enmey weapon hardcoded for mage
            this.enemyHeroname = "mage";
            this.enemyHeroWeapon = p.enemyWeaponName.ToString();
            this.enemyWeaponAttack = p.enemyWeaponAttack;
            this.enemyWeaponDurability = p.enemyWeaponDurability;

            //own hero ablity stuff###########################################################
            //hard coded for mage

            this.heroAbility =
                CardDB.Instance.getCardData(CardDB.Instance.cardNamestringToEnum("fireblast"));
            this.ownAbilityisReady = p.ownAbilityReady;
            this.enemyAbility =
                CardDB.Instance.getCardData(CardDB.Instance.cardNamestringToEnum("fireblast"));

            //generate Heros
            this.ownHero = new Minion(p.ownHero);
            this.enemyHero = new Minion(p.enemyHero);
            this.enemyHero.Ready = false;

            this.ownHero.updateReadyness();

            //load enchantments of the heros
            //How to do this?

            //fastmode weapon correction:
            if (this.ownHero.Angr < this.heroWeaponAttack) this.ownHero.Angr = this.heroWeaponAttack;
            if (this.enemyHero.Angr < this.enemyWeaponAttack) this.enemyHero.Angr = this.enemyWeaponAttack;

            HRSim.Helpfunctions.Instance.logg("hero: " + ownHero.cardClass + " mana:" + this.currentMana + "/" + this.ownMaxMana + " weapon:" + ownHeroWeapon);

        }

        public void updateMinions(HRSim.Playfield p)
        {
            this.ownMinions.Clear();
            this.enemyMinions.Clear();
            foreach(HRSim.Minion m in p.ownMinions){
                Minion mMinion = new Minion(m);
                this.ownMinions.Add(mMinion);
            }
            foreach (HRSim.Minion m in p.enemyMinions)
            {
                Minion mMinion = new Minion(m);
                this.enemyMinions.Add(mMinion);
            }
        }

        private void updateHandcards(HRSim.Playfield p)
        {
            this.handCards.Clear();
            this.anzcards = 0;
            this.anzcards = p.owncards.Count;
            foreach (HRSim.Handmanager.Handcard hc in p.owncards)
            {
                Handmanager.Handcard mHandcard = new Handmanager.Handcard(hc);
                this.handCards.Add(mHandcard);
            }
            enemyAnzCards = p.enemyAnzCards;
        }

        private void updateDecks(HRSim.Playfield p)
        {


            List<CardDB.cardIDEnum> ownCards = new List<CardDB.cardIDEnum>();
            List<CardDB.cardIDEnum> enemyCards = new List<CardDB.cardIDEnum>();
            List<GraveYardItem> graveYard = new List<GraveYardItem>();

            //own hands
            foreach (HRSim.Handmanager.Handcard c in p.owncards)
            {
                CardDB.cardIDEnum mCardID = (CardDB.cardIDEnum)Enum.Parse(typeof(CardDB.cardIDEnum), c.card.cardIDenum.ToString());
                ownCards.Add(mCardID);
            }
            //own minions
            foreach (HRSim.Minion m in p.ownMinions)
            {
                CardDB.cardIDEnum mCardID = (CardDB.cardIDEnum)Enum.Parse(typeof(CardDB.cardIDEnum), m.handcard.card.cardIDenum.ToString());
                ownCards.Add(mCardID);
            }
            //own secret
            foreach (HRSim.CardDB.cardIDEnum c in p.ownSecretsIDList) {
                CardDB.cardIDEnum mCardID = (CardDB.cardIDEnum)Enum.Parse(typeof(CardDB.cardIDEnum), c.ToString());
                ownCards.Add(mCardID);
            }
            //own weapon
            CardDB.cardName weaponName = (CardDB.cardName)Enum.Parse(typeof(CardDB.cardName), p.ownWeaponName.ToString());
            ownCards.Add(CardDB.Instance.getCardData(weaponName).cardIDenum);

            //enemy minions
            foreach (HRSim.Minion m in p.enemyMinions)
            {
                Minion mMinion = new Minion(m);
                ownCards.Add(mMinion.handcard.card.cardIDenum);
            }
            //enemy weapon
            weaponName = (CardDB.cardName)Enum.Parse(typeof(CardDB.cardName), p.enemyWeaponName.ToString());
            ownCards.Add(CardDB.Instance.getCardData(weaponName).cardIDenum);

            //grave yard
            foreach(HRSim.GraveYardItem g in p.graveYard){
                CardDB.cardIDEnum graveCardID = (CardDB.cardIDEnum)Enum.Parse(typeof(CardDB.cardIDEnum), g.cardid.ToString());
                GraveYardItem mGraveItem = new GraveYardItem(graveCardID, g.entity, g.own);
                graveYard.Add(mGraveItem);
            }
            
            Probabilitymaker.Instance.setOwnCards(ownCards);
            Probabilitymaker.Instance.setEnemyCards(enemyCards);
            bool isTurnStart = false;
            if (Ai.Instance.nextMoveGuess.mana == -100)
            {
                isTurnStart = true;
                Ai.Instance.updateTwoTurnSim();
            }
            Probabilitymaker.Instance.setGraveYard(graveYard, isTurnStart);

        }


        private void updateBehaveString(Behavior botbase)
        {
            this.botbehave = "rush";
            if (botbase is BehaviorControl) this.botbehave = "control";
            if (botbase is BehaviorMana) this.botbehave = "mana";
            this.botbehave += " " + Ai.Instance.maxwide;
            this.botbehave += " face " + ComboBreaker.Instance.attackFaceHP;
            if (Settings.Instance.secondTurnAmount > 0)
            {
                if (Ai.Instance.nextMoveGuess.mana == -100)
                {
                    Ai.Instance.updateTwoTurnSim();
                }
                this.botbehave += " twoturnsim " + Settings.Instance.secondTurnAmount + " ntss " +
                                  Settings.Instance.nextTurnDeep + " " + Settings.Instance.nextTurnMaxWide + " " +
                                  Settings.Instance.nextTurnTotalBoards;
            }

            if (Settings.Instance.playarround)
            {
                this.botbehave += " playaround";
                this.botbehave += " " + Settings.Instance.playaroundprob + " " + Settings.Instance.playaroundprob2;
            }

            this.botbehave += " ets " + Settings.Instance.enemyTurnMaxWide;

            if (Settings.Instance.simEnemySecondTurn)
            {
                this.botbehave += " ets2 " + Settings.Instance.enemyTurnMaxWideSecondTime;
                this.botbehave += " ents " + Settings.Instance.enemySecondTurnMaxWide;
            }

            if (Settings.Instance.useSecretsPlayArround)
            {
                this.botbehave += " secret";
            }

            if (Settings.Instance.secondweight != 0.5f)
            {
                this.botbehave += " weight " + (int)(Settings.Instance.secondweight * 100f);
            }

            if (Settings.Instance.simulatePlacement)
            {
                this.botbehave += " plcmnt";
            }


        }

        public bool updateEverything(Behavior botbase, HRSim.Playfield mp, bool runExtern = false, bool passiveWait = false)
        {
            this.updateBehaveString(botbase);

            updateHeroStuff(mp);
            updateMinions(mp);
            updateHandcards(mp);
            updateDecks(mp);

            // send ai the data:
            Hrtprozis.Instance.clearAll();
            Handmanager.Instance.clearAll();

            Hrtprozis.Instance.setOwnPlayer(mp.ownController);
            Handmanager.Instance.setOwnPlayer(mp.ownController);

            this.numOptionPlayedThisTurn = 0;
            this.numOptionPlayedThisTurn += this.cardsPlayedThisTurn + this.ownHero.numAttacksThisTurn;
            foreach (Minion m in this.ownMinions)
            {
                if (m.Hp >= 1) this.numOptionPlayedThisTurn += m.numAttacksThisTurn;
            }

            Hrtprozis.Instance.updatePlayer(this.ownMaxMana, this.currentMana, this.cardsPlayedThisTurn,
                this.numMinionsPlayedThisTurn, this.numOptionPlayedThisTurn, this.ueberladung, mp.ownHero.entitiyID,
                mp.enemyHero.entitiyID);
            Hrtprozis.Instance.updateSecretStuff(this.ownSecretList, this.enemySecretCount);

            Hrtprozis.Instance.updateOwnHero(this.ownHeroWeapon, this.heroWeaponAttack, this.heroWeaponDurability,
                this.heroname, this.heroAbility, this.ownAbilityisReady, this.ownHero);
            Hrtprozis.Instance.updateEnemyHero(this.enemyHeroWeapon, this.enemyWeaponAttack, this.enemyWeaponDurability,
                this.enemyHeroname, this.enemyMaxMana, this.enemyAbility, this.enemyHero);

            Hrtprozis.Instance.updateMinions(this.ownMinions, this.enemyMinions);
            Handmanager.Instance.setHandcards(this.handCards, this.anzcards, this.enemyAnzCards);

            Hrtprozis.Instance.updateFatigueStats(this.ownDecksize, this.ownHeroFatigue, this.enemyDecksize,
                this.enemyHeroFatigue);

            Probabilitymaker.Instance.getEnemySecretGuesses(this.enemySecretList,
                Hrtprozis.Instance.enemyHeroStartClass);
            //learnmode :D

            Playfield p = new Playfield();

            if (lastpf != null)
            {
                if (lastpf.isEqualf(p))
                {
                    return false;
                }

                //board changed we update secrets!
                //if(Ai.Instance.nextMoveGuess!=null) Probabilitymaker.Instance.updateSecretList(Ai.Instance.nextMoveGuess.enemySecretList);
                Probabilitymaker.Instance.updateSecretList(p, lastpf);
                lastpf = p;
            }
            else
            {
                lastpf = p;
            }

            p = new Playfield(); //secrets have updated :D

            p.printBoard();

            // calculate stuff
            HRSim.Helpfunctions.Instance.ErrorLog("calculating stuff... " + DateTime.Now.ToString("HH:mm:ss.ffff"));
            if (runExtern)
            {
                HRSim.Helpfunctions.Instance.logg("recalc-check###########");
                if (p.isEqual(Ai.Instance.nextMoveGuess, true))
                {
                    //printstuff(false);
                    Debug.WriteLine("equal");
                    Ai.Instance.doNextCalcedMove();
                }
                else
                {
                    //printstuff(true);
                    Debug.WriteLine("not equal");
                    //readActionFile(passiveWait);
                }
            }
            else
            {
                // Drew: This prevents the freeze during AI updates, but no API functions may be called
                // during this time!
                //using (TritonHs.Memory.ReleaseFrame(true))
                //{
                //    printstuff(false);
                Ai.Instance.dosomethingclever(botbase);
                //}
            }

            HRSim.Helpfunctions.Instance.ErrorLog("calculating ended! " + DateTime.Now.ToString("HH:mm:ss.ffff"));
            return true;
        }

        public void setnewLoggFile()
        {
            if (!singleLog)
            {
                sttngs.setLoggFile("UILogg" + DateTime.Now.ToString("_yyyy-MM-dd_HH-mm-ss") + ".txt");
                HRSim.Helpfunctions.Instance.createNewLoggfile();
                HRSim.Helpfunctions.Instance.ErrorLog("#######################################################");
                HRSim.Helpfunctions.Instance.ErrorLog("fight is logged in: " + sttngs.logpath + sttngs.logfile);
                HRSim.Helpfunctions.Instance.ErrorLog("#######################################################");
            }
            else
            {
                sttngs.setLoggFile("UILogg.txt");
            }
        }

        public void readActionFile(bool passiveWaiting = false)
        {
            Ai.Instance.nextMoveGuess = new Playfield();
            bool readed = true;
            List<string> alist = new List<string>();
            float value = 0f;
            string boardnumm = "-1";
            while (readed)
            {
                try
                {
                    string data = System.IO.File.ReadAllText(Settings.Instance.path + "actionstodo.txt");
                    if (data != "" && data != "<EoF>" && data.EndsWith("<EoF>"))
                    {
                        data = data.Replace("<EoF>", "");
                        //Helpfunctions.Instance.ErrorLog(data);
                        HRSim.Helpfunctions.Instance.resetBuffer();
                        HRSim.Helpfunctions.Instance.writeBufferToActionFile();
                        alist.AddRange(data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
                        string board = alist[0];
                        if (board.StartsWith("board "))
                        {
                            boardnumm = (board.Split(' ')[1].Split(' ')[0]);
                            alist.RemoveAt(0);
                            /*if (boardnumm != Ai.Instance.currentCalculatedBoard)
                            {
                                if (passiveWaiting)
                                {
                                    System.Threading.Thread.Sleep(10);
                                    return;
                                }
                                continue;
                            }*/
                        }
                        string first = alist[0];
                        if (first.StartsWith("value "))
                        {
                            value = float.Parse((first.Split(' ')[1].Split(' ')[0]));
                            alist.RemoveAt(0);
                        }
                        readed = false;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(10);
                        if (passiveWaiting)
                        {
                            return;
                        }
                    }

                }
                catch
                {
                    System.Threading.Thread.Sleep(10);
                }

            }
            HRSim.Helpfunctions.Instance.logg("received " + boardnumm + " actions to do:");
            Ai.Instance.currentCalculatedBoard = "0";
            Playfield p = new Playfield();
            List<Action> aclist = new List<Action>();

            foreach (string a in alist)
            {
                aclist.Add(new Action(a, p));
                HRSim.Helpfunctions.Instance.logg(a);
            }

            Ai.Instance.setBestMoves(aclist, value);

        }

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

        HRSim.MainWindow window = null;

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

        private static Helpfunctions instance;

        public static Helpfunctions Instance
        {
            get
            {
                return instance ?? (instance = new Helpfunctions());
            }
        }

        private Helpfunctions()
        {
            //foreach (Window window in Application.Current.Windows)
            //{
            //    if (window.GetType() == typeof(HRSim.MainWindow))
            //    {
            //        this.window = (HRSim.MainWindow)window;
            //    }
            //}
            System.IO.File.WriteAllText(Settings.Instance.logpath + Settings.Instance.logfile, "");
        }

        private bool writelogg = true;
        public void loggonoff(bool onoff)
        {
            writelogg = onoff;
        }

        public void createNewLoggfile()
        {
            System.IO.File.WriteAllText(Settings.Instance.logpath + Settings.Instance.logfile, "");
        }

        public void logg(string s)
        {


            if (!writelogg) return;
            try
            {
                using (StreamWriter sw = File.AppendText(Settings.Instance.logpath + Settings.Instance.logfile))
                {
                    sw.WriteLine(s);
                }
            }
            catch { }
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
            this.sendbuffer += "<EoF>";
            while (writed)
            {
                try
                {
                    System.IO.File.WriteAllText(Settings.Instance.path + "crrntbrd.txt", this.sendbuffer);
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
                    System.IO.File.WriteAllText(Settings.Instance.path + "actionstodo.txt", this.sendbuffer);
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
