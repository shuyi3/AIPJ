using System;
using System.Collections.Generic;
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
using HREngine.Bots;
using System.Diagnostics;
using System.IO.Pipes;
using System.Windows.Threading;
using ZeroMQ;
using Newtonsoft.Json;
using Python.Runtime;


//TODO: update enemy hero hp
//      trap for both sides
//      something in the trigger
//      noble sacrifice rework
//      secret trigger for both sides
//      repentence, counterspell rework
//      GC.collect, captain's parrot
//      科学家拉出来的奥秘应该加入坟场
//      各种start都要改 魔法计算也要改(perm 加到calculatemanacost里面？)
//      todo: overload for bothside
//      penalty for 苦痛，科学家，亡语怪

namespace HRSim
{

    public partial class MainWindow : Window
    {

        private ZSocket responder, requester;
        private GameRecord gameRecord = null;
        private int recordTurn = 0;
        private static MainWindow instance;

        public static MainWindow Instance
        {
            get
            {
                return instance ?? (instance = new MainWindow());
            }
        }

        public class CardInfo
        {
            public String Name { get; set; }
            public BitmapImage Image { get; set; }
            public CardInfo(String name, CardDB.cardIDEnum cardID)
            {
                Name = name;
                try
                {
                    Image = new BitmapImage(new Uri("C:\\Code\\WpfApplication1\\WpfApplication1\\hs-images\\" + cardID + ".png", UriKind.RelativeOrAbsolute));
                }
                catch (FileNotFoundException e)
                {
                    Image = new BitmapImage(new Uri("C:\\Code\\WpfApplication1\\WpfApplication1\\hs-images\\GVG_093.png", UriKind.RelativeOrAbsolute));
                }
            }

        }

        ObservableCollection<CardInfo> ownHandCards = new ObservableCollection<CardInfo>();
        ObservableCollection<CardInfo> enemyHandCards = new ObservableCollection<CardInfo>();
        ObservableCollection<CardInfo> ownMinions = new ObservableCollection<CardInfo>();
        ObservableCollection<CardInfo> enemyMinions = new ObservableCollection<CardInfo>();


        public ObservableCollection<CardInfo> OwnHandCards
        {
            get
            {
                return ownHandCards;
            }
        }

        public ObservableCollection<CardInfo> EnemyHandCards
        {
            get
            {
                return enemyHandCards;
            }
        }

        public ObservableCollection<CardInfo> OwnMinions
        {
            get
            {
                return ownMinions;
            }
        }

        public ObservableCollection<CardInfo> EnemyMinions
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
            instance = this;
            DataContext = this;
            InitializeComponent();

            //InitPipe();
            //bw.WorkerReportsProgress = true;
            //bw.WorkerSupportsCancellation = true;
            //bw.DoWork += BackgroundWorkerOnDoWork;
            //bw.ProgressChanged += BackgroundWorkerOnProgressChanged;
            //bw.RunWorkerAsync();
        }

        public void displayHandCards(bool shwoWithImage)
        {
            GameManager gameManager = GameManager.Instance;
            this.Dispatcher.Invoke((System.Action)(() =>
            {
                ownHandCards.Clear();
                enemyHandCards.Clear();
                //HRSim.Helpfunctions.Instance.ErrorLog("Own Handcards: ");
                foreach (Handmanager.Handcard c in gameManager.mPlayfield.playerFirst.owncards)
                {
                    if (shwoWithImage)
                        ownHandCards.Add(new CardInfo(c.card.name + "(" + c.getManaCost(gameManager.mPlayfield, true) + ")", c.card.cardIDenum));
                    //HRSim.Helpfunctions.Instance.ErrorLog("pos " + c.position + " " + c.card.name + " " + c.manacost + " entity " + c.entity + " " + c.card.cardIDenum + " " + c.addattack);
                }
                //HRSim.Helpfunctions.Instance.ErrorLog("Enemy Handcards: ");
                foreach (Handmanager.Handcard c in gameManager.mPlayfield.playerSecond.owncards)
                {
                    if (shwoWithImage)
                        enemyHandCards.Add(new CardInfo(c.card.name + "(" + c.getManaCost(gameManager.mPlayfield, false) + ")", c.card.cardIDenum));
                    //HRSim.Helpfunctions.Instance.ErrorLog("pos " + c.position + " " + c.card.name + " " + c.manacost + " entity " + c.entity + " " + c.card.cardIDenum + " " + c.addattack);
                }
            }));
        }

        public void displayMinions()
        {
            GameManager gameManager = GameManager.Instance;
            this.Dispatcher.Invoke((System.Action)(() =>
            {
                ownMinions.Clear();
                enemyMinions.Clear();
                foreach (Minion m in gameManager.mPlayfield.playerFirst.ownMinions)
                {
                    ownMinions.Add(new CardInfo(m.handcard.card.name + "(" + m.Angr + ", " + m.Hp + ")", m.handcard.card.cardIDenum));
                }
                foreach (Minion m in gameManager.mPlayfield.playerSecond.ownMinions)
                {

                    enemyMinions.Add(new CardInfo(m.handcard.card.name + "(" + m.Angr + ", " + m.Hp + ")", m.handcard.card.cardIDenum));
                }
            }));

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Helpfunctions.Instance.LogUpdate += appendLog;
            Thread game = new Thread(() => Init(false, 0));
            game.Start();
        }

        public void appendLog(object sender, string log)
        {
            //Debug.WriteLine("called " + log);
            this.Dispatcher.Invoke((System.Action)(() => { LogTextBlock.Text += Environment.NewLine + log; }));
        }

        public void updateHeroState()
        {
            Playfield p = GameManager.Instance.mPlayfield;
            this.Dispatcher.Invoke((System.Action)(() =>
           {
               OwnHeroBlock.Text = "" + p.playerFirst.ownHeroName + " || ";
               OwnHeroBlock.Text += "ownherohp: " + p.playerFirst.ownHero.Hp + " + " + p.playerFirst.ownHero.armor + " || ";
               OwnHeroBlock.Text += "mana " + p.playerFirst.mana + "/" + p.playerFirst.ownMaxMana + " || ";
               OwnHeroBlock.Text += "ownheroattac: " + p.playerFirst.ownHero.Angr + " || ";
               OwnHeroBlock.Text += "ownheroweapon: " + p.playerFirst.ownWeaponAttack + " " + p.playerFirst.ownWeaponDurability + " " + p.playerFirst.ownWeaponName + " || ";
               OwnHeroBlock.Text += "ownherostatus: frozen" + p.playerFirst.ownHero.frozen + " || ";

               foreach (CardDB.cardIDEnum secretID in p.playerFirst.ownSecretsIDList)
               {
                   CardDB.Card secCard = CardDB.Instance.getCardDataFromID(secretID);
                   OwnHeroBlock.Text += "ownherosec: " + secCard.name + ", ";
               }

               //Helpfunctions.Instance.logg("enemyherohp: " + this.enemyHero.Hp + " + " + this.enemyHero.armor + ((this.enemyHero.immune) ? " immune" : ""));

               //if (this.enemySecretCount >= 1) Helpfunctions.Instance.logg("enemySecrets: " + Probabilitymaker.Instance.getEnemySecretData(this.enemySecretList));
               //foreach (Action a in this.playactions)
               //{
               //    a.print();
               //}
               EnemyHeroBlock.Text = "" + p.playerSecond.ownHeroName + " || ";
               EnemyHeroBlock.Text += "enemyherohp: " + p.playerSecond.ownHero.Hp + " + " + p.playerSecond.ownHero.armor + " || ";
               EnemyHeroBlock.Text += "mana " + p.playerSecond.mana + "/" + p.playerSecond.ownMaxMana + " || ";
               EnemyHeroBlock.Text += "enemyheroattac: " + p.playerSecond.ownHero.Angr + " || ";
               EnemyHeroBlock.Text += "enemyherowwapon: " + p.playerSecond.ownWeaponAttack + " " + p.playerSecond.ownWeaponDurability + " " + p.playerSecond.ownWeaponName + " || ";
               EnemyHeroBlock.Text += "enemyherostatus: frozen" + p.playerSecond.ownHero.frozen + " || ";

               foreach (CardDB.cardIDEnum secretID in p.playerSecond.ownSecretsIDList)
               {
                   CardDB.Card secCard = CardDB.Instance.getCardDataFromID(secretID);
                   OwnHeroBlock.Text += "enemyherosec: " + secCard.name + ", ";
               }

           }));
        }

        public void Init(bool isLearning, int turn, Playfield playfield = null)
        {

            if (GameManager.Instance == null)
            {
                GameManager.Init();
            }
            else
            {
                if (playfield != null)
                {
                    GameManager.Instance.mPlayfield = new Playfield(playfield);
                }
                else
                {
                    GameManager.Instance.initPlayField();
                }
            }

            //setplayer 1

            if (isLearning)
            {
                //GameManager.Instance.setPlayer(1, new Silverfish()); // we can try set to the same player
                GameManager.Instance.setPlayer(1, new GreedyPlayer(false, GameManager.Instance.mPlayfield, false));
            }
            else
            {
                //setplayer 2
                //GameManager.Instance.setPlayer(1, new Silverfish());
                if (turn % 2 == 0)
                {
                    Helpfunctions.Instance.logg("Player 1: MCTSPlayer, Player 2: Silverfish");
                    //GameManager.Instance.setPlayer(1, new GreedyPlayer(false, GameManager.Instance.mPlayfield, false));
                    //GameManager.Instance.setPlayer(0, new GreedyPlayer(true, GameManager.Instance.mPlayfield, true));
                    //GameManager.Instance.setPlayer(0, new Silverfish());
                    //GameManager.Instance.setPlayer(1, new Silverfish());
                    //GameManager.Instance.setPlayer(1, new MCTSPlayer(false, GameManager.Instance.mPlayfield, false));
                    GameManager.Instance.setPlayer(0, new MCTSPlayer(true, GameManager.Instance.mPlayfield, false, 1.0));
                    GameManager.Instance.setPlayer(1, new MCTSPlayer(false, GameManager.Instance.mPlayfield, false, 0.0));
                }
                else
                {
                    Helpfunctions.Instance.logg("Player 1: Silverfish, Player 2: MCTSPlayer");
                    //GameManager.Instance.setPlayer(0, new Silverfish());
                    //GameManager.Instance.setPlayer(1, new Silverfish());
                    //GameManager.Instance.setPlayer(0, new GreedyPlayer(true, GameManager.Instance.mPlayfield, false));
                    //GameManager.Instance.setPlayer(1, new GreedyPlayer(false, GameManager.Instance.mPlayfield, true));
                    //GameManager.Instance.setPlayer(0, new MCTSPlayer(true, GameManager.Instance.mPlayfield, false));
                    GameManager.Instance.setPlayer(0, new MCTSPlayer(true, GameManager.Instance.mPlayfield, false, 0.0));
                    GameManager.Instance.setPlayer(1, new MCTSPlayer(false, GameManager.Instance.mPlayfield, false, 1.0));
                }
               
            }
            //if (GameManager.Instance.playerFirst == null)
            //{
            //    GameManager.Instance.setPlayer(0, new QLearningAgent(true, GameManager.Instance.mPlayfield));
            //}
            //else
            //{
            //    GameManager.Instance.playerFirst.updateState(GameManager.Instance.mPlayfield);
            //}

            //testField = new Playfield();

            //testField.initDeckFromFile("C:\\Code\\WpfApplication1\\WpfApplication1\\test_mage_deck.txt", true);
            //testField.initDeckFromFile("C:\\Code\\WpfApplication1\\WpfApplication1\\test_mage_deck.txt", false);

            //testField.drawInitCards();

            ////InitAISettings();

            ////displayHandCards(testField, false);
            //testField.playerOne = new Silverfish();
            //testField.playerOne.setnewLoggFile(1);

            ////testField.playerTwo = new Silverfish();
            ////testField.playerTwo.setnewLoggFile(2);

            //testField.mcPlayer = new MCTSPlayer(0, testField);

            //playerOneBot = new HREngine.Bots.Bot(testField.playerOne);
            //playerOneBH = new BehaviorControl(testField.playerOne);//change this to new BehaviorRush() for rush mode
            ////HRSim.Action moveTodo = null;

            ////playerTwoBot = new HREngine.Bots.Bot(testField.playerTwo);
            ////playerTwoBH = new BehaviorControl(testField.playerTwo);

            //testField.isOwnTurn = true;

            //HRSim.Helpfunctions.Instance.logg("PLAYER 1 ##########MOVE##########");
            //for (; ; )
            //{
            //    bool templearn = playerOne.updateEverything(behave, testField, true);
            //    if (playerOne.Ai.bestmove == null) break;
            //    moveTodo = new HRSim.Action(playerOne.Ai.bestmove);
            //    moveTodo.print();
            //    testField.doAction(moveTodo);

            //}
            //HRSim.Helpfunctions.Instance.logg("PLAYER 1 ##########MOVE##########");

            //testField.endTurn(false, false);

            //displayMinions(testField);

            //displayHandCards(testField, true);

            //updateHeroState(testField);

            //playerTwo = new Silverfish();
            //playerTwo.setnewLoggFile();

            //HRSim.Helpfunctions.Instance.logg("PLAYER 2 ##########MOVE##########");
            //for (; ; )
            //{
            //    bool templearn = playerTwo.updateEverything(behave, testField, false);
            //    if (playerTwo.Ai.bestmove == null) break;
            //    moveTodo = new HRSim.Action(playerTwo.Ai.bestmove);
            //    moveTodo.print();
            //    testField.doAction(moveTodo);
            //}
            //HRSim.Helpfunctions.Instance.logg("PLAYER 2 ##########MOVE##########");

            //testField.endTurn(false, false);

            //displayMinions(testField);

            //displayHandCards(testField, true);

            //updateHeroState(testField);

            //displayMinions(testField);
            //displayHandCards(testField, true);
            //updateHeroState(testField);
            isInit = true;

        }

        private void updateAllUI() {
            displayMinions();
            displayHandCards(true);
            updateHeroState();
            //testField.printGraveyard();
        }

        public int expandDecision(Node p) //0: lethal 
        {
            if (expand(p, HeuristicType.LethalCheck) == 1) return 0;
            expand(p, HeuristicType.Boardvalue); // no moves
            return p.children.Count;
        }

        public int expand(Node p, HeuristicType ht)
        {
            Playfield afterState = new Playfield(p.state);

            ParetoMCTSPlayer m_player = new ParetoMCTSPlayer(new ParetoTreePolicy(0.7), GameManager.getRNG(), afterState, ht);
            m_player.run(afterState, 30000, false);
            //m_player.m_root.printStats();
            //Helpfunctions.Instance.logg("turn: " + p.state.isOwnTurn);
            int memberSize = m_player.m_root.pa.m_members.size(); // will it always > 0?

            for (int i = 0; i < memberSize; i++) // this is other's turn
            {
                Node afterNode;
                Playfield pf = m_player.m_root.pa.m_members.get(i).m_state;
                if (ht == HeuristicType.DrawCard)
                {
                    if (pf.moveTrigger.newHandcardList.Count == 0)
                    {
                        continue;
                    }

                    afterNode = new ChanceNode(p, pf, null, p.depth, 1); //last param is wrong
                    pf.printActions();
                }
                else if (ht == HeuristicType.LethalCheck)
                {
                    if (pf.getLethalScore() == 1.0)
                    {
                        afterNode = new Node(p, pf, null, p.depth + 1);
                    }
                    continue;
                }
                else
                {
                    afterNode = new Node(p, pf, null, p.depth + 1);
                }
                p.children.Add(afterNode);
            }

            return p.children.Count;
        }
      
        //private string generateMessage(int result, Node currentState) // for player 1
        //{
        //    Message message = new Message();
        //    message.result = result;

        //    if (result == 1)
        //    {
        //        message.result = 0;
        //        return JsonConvert.SerializeObject(message);
        //    }

        //    int count = expandDecision(currentState);

        //    if (count == 0) //lethal
        //    {
        //        GameManager.Instance.mPlayfield = currentState.children[0].state;
        //        message.result = 0;
        //        return JsonConvert.SerializeObject(message);
        //    }

        //    message.features = new double[currentState.children.Count][];

        //    for (int i = 0; i < currentState.children.Count; ++i)
        //    {
        //        Node child = currentState.children[i];
        //        double[] delta = new double[] { child.state.getBoardValue(), -child.state.getEnemyBoardValue(), child.state.getHandValue() };
        //        message.features[i] = delta;
        //    }

        //    return JsonConvert.SerializeObject(message);
        //}

        private void connect()
        {
            using (var requester = new ZSocket(ZSocketType.REQ))
            {
                // Connect
                requester.Connect("tcp://127.0.0.1:5556");

                for (int n = 0; n < 10; ++n)
                {
                    string requestText = "Hello";
                    Console.Write("Sending {0}...", requestText);

                    // Send
                    requester.Send(new ZFrame(requestText));

                    // Receive
                    using (ZFrame reply = requester.ReceiveFrame())
                    {
                        Console.WriteLine(" Received: {0} {1}!", requestText, reply.ReadString());
                    }
                }
            }

            //requester = new ZSocket(ZSocketType.REP);
            //try
            //{
            //    // Bind
            //    //responder.Bind("tcp://*:5556");
            //    requester.Connect("tcp://127.0.0.1:5556");
            //    requester.Send(new ZFrame("Hello")); 
            //    Helpfunctions.Instance.logg("connected to python server");
            //}
            //catch (Exception e)
            //{
            //    Helpfunctions.Instance.logg(e.ToString());
            //}
        }

        private string sendMessage(string message)
        {
            //if (requester == null) connect();

            //requester.Send(new ZFrame(message));
            //// Receive
            //string result = null;
            //using (ZFrame reply = requester.ReceiveFrame())
            //{
            //    result = reply.ReadString();
            //    Console.WriteLine(" Received: {0}!", reply.ReadString());
            //}
            //return result;
            string result = null;
            using (var requester = new ZSocket(ZSocketType.REQ))
            {
                // Connect
                requester.Connect("tcp://127.0.0.1:5556");

                for (int n = 0; n < 10; ++n)
                {
                    //string requestText = "Hello";
                    Console.Write("Sending {0}...", message);

                    // Send
                    requester.Send(new ZFrame(message));

                    // Receive
                    using (ZFrame reply = requester.ReceiveFrame())
                    {
                        result = reply.ReadString();
                        Console.WriteLine(" Received: {0}", reply.ReadString());
                    }
                }
            }
            return result;
        }
       
        //private void FVI(object sender, RoutedEventArgs e) //For player 1
        //{
        //    //connect
        //    using (responder = new ZSocket(ZSocketType.REP))
        //    {
        //        // Bind
        //        responder.Bind("tcp://*:5556");
        //        Node currentState = null;
        //        bool turn = true;

        //        while (true)
        //        {
        //            // Receive
        //            using (ZFrame request = responder.ReceiveFrame())
        //            {
        //                int action = Int32.Parse(request.ReadString());
        //                string message;

        //                if (action == -1)
        //                {
        //                    //new a game
        //                    Init(true, 0);
        //                    currentState = new Node(null, GameManager.Instance.mPlayfield, null, 0);
        //                    message = generateMessage(-1, currentState);
        //                    responder.Send(new ZFrame(message));
        //                }
        //                else
        //                {   //do a action, return result and successors
        //                    Helpfunctions.Instance.logg("PLAYER 1 ##########MOVE##########");
        //                    Playfield state = currentState.children[action].state;
        //                    GameManager.Instance.mPlayfield = state;
        //                    int result = GameManager.Instance.mPlayfield.getGameResult(); //action result
        //                    updateAllUI();


        //                    if (result == -1)
        //                    {
        //                        while (!GameManager.Instance.mPlayfield.isOwnTurn && result == -1)
        //                        {
        //                            result = GameManager.Instance.playMove();
        //                        }
        //                    }
        //                    updateAllUI();

        //                    currentState = new Node(null, GameManager.Instance.mPlayfield, null, 0);
        //                    message = generateMessage(result, currentState);
        //                    responder.Send(new ZFrame(message));
        //                }     
        //            }
        //        }
        //    }          
        //}

        private void loadRecord(GameRecord gr)
        {
            this.gameRecord = gr;
            this.recordTurn = 0;
        }

        private void nextSequence(object sender, RoutedEventArgs e)
        {        
            Playfield playfield = Helpfunctions.Instance.readJsonFile();
            float score = DNNEval.Instance.getNNEval(playfield, playfield.isOwnTurn);
            Console.WriteLine("score = " + score.ToString());

            //while (playfield != null)
            //{
            //    PlayMultipleGames(5, playfield);
            //    playfield = Helpfunctions.Instance.readJsonFile();
            //}
           
            updateAllUI();
        }
       
        private float simulateEnemyTurn(Playfield state, bool own, bool isSimulate, dynamic agent)
        {
            string playerName;
            PlayerAgent playerAgent;
            if (own)
            {
                playerName = "Player 1";
                playerAgent = GameManager.Instance.playerFirst;
            }
            else
            {
                playerName = "Player 2";
                playerAgent = GameManager.Instance.playerSecond;
            }

            int result = state.getGameResult();
            float totalReward = 0.0f;

            while (result == -1)
            {
                float reward = 0.0f;
                dynamic state_t = DNNEval.Instance.parsePlayfield(state, own);
                GameManager.Instance.playerSecond.updateState(state);
                Action moveTodo = GameManager.Instance.playerSecond.getMove();
                if (moveTodo != null)
                {
                    if (!isSimulate)
                    {
                        Helpfunctions.Instance.logg(playerName + " ##########Move##########");
                        moveTodo.print();
                    }
                    state.doAction(moveTodo);
                    result = state.getGameResult();
                    reward = state.observerMoveReward();
                    totalReward += reward;

                    dynamic state_t1 = DNNEval.Instance.parsePlayfield(state, own);
                    Helpfunctions.Instance.logg("Gd reward = " + reward);
                    agent.store_transition(state_t, state_t1, reward);
                }
                else
                {
                    state.endTurn(false, false);
                    state.drawTurnStartCard();
                    dynamic state_t1 = DNNEval.Instance.parsePlayfield(state, own);
                    agent.store_transition(state_t, state_t1, 0.0f);
                    break;
                }
                result = state.getGameResult();
            }

            //bool winner = (result == 0) ? true : false;
            //if (winner == own)
            //    totalReward -= 1;
            //else
            //    totalReward += 1;

            return -totalReward;
        }

        private void DQNTurn(object sender, RoutedEventArgs e)
        {
            bool side = true; //own side
            int numGames = 500;
            using (Py.GIL())
            {
                dynamic dqn = Py.Import("simple_dqn.dqn");
                dynamic Agent = dqn.Agent;

                Console.WriteLine("Done importing");
                PyInt width = new PyInt(458);
                dynamic state_size = new PyTuple(new PyObject[] { width });
                dynamic agent = Agent(Py.kw("state_size", state_size), Py.kw("discount", 0.95));
                Console.WriteLine(agent.discount);

                for (int i = 0; i < numGames; i++)
                {
                    if (!isInit) Init(true, i);
                    Playfield state = GameManager.Instance.mPlayfield;
                    int result = state.getGameResult();
                    float total_cost = 0.0f;
                    float total_reward = 0.0f;
                    float reward = 0.0f;
                    float cost = 0.0f;
                    int num_move = 0;
                    int end_turn_count = 0;

                    agent.new_episode();

                    while (result == -1)
                    {

                        Action moveTodo = null;
                        reward = 0.0f;
                        cost = 0.0f;

                        if (state.isOwnTurn)
                        {

                            List<Action> moveList = Movegenerator.Instance.getMoveList(state, false, true, true, 0.0);
                            moveList.Add(null);

                            dynamic cur_state_vector = DNNEval.Instance.parsePlayfield(state, side);
                            List<Playfield> stateList = new List<Playfield>();
                            PyList state_vector_list = new PyList();
                            foreach (Action action in moveList)
                            {
                                Playfield nextState = new Playfield(state);
                                if (action != null)
                                {
                                    nextState.doAction(action);
                                    stateList.Add(nextState);
                                    reward = nextState.observerMoveReward();
                                }
                                else
                                {
                                    nextState.endTurn(false, false);
                                    nextState.drawTurnStartCard();
                                    //simulateEnemyTurn(nextState, false, true, agent);
                                }
                                dynamic state_vector = DNNEval.Instance.parsePlayfield(nextState, side);
                                state_vector_list.Append(state_vector);
                            }
                            int action_index = agent.act(cur_state_vector, state_vector_list);
                            Helpfunctions.Instance.logg("Length:" + state_vector_list.Length());

                            moveTodo = moveList[action_index];

                            if (moveTodo != null)
                            {
                                Helpfunctions.Instance.logg("Player 1 ##########Move##########");
                                moveTodo.print();
                                state.doAction(moveTodo);
                                reward = state.observerMoveReward();
                            }
                            else
                            {
                                if (state.playerFirst.mana == state.playerFirst.ownMaxMana)
                                    end_turn_count++;
                                Helpfunctions.Instance.logg("Player 1 ##########Move##########");
                                Helpfunctions.Instance.logg("Player 1 end turn");
                                state.endTurn(false, false);
                                state.drawTurnStartCard();
                                reward = 0.0f;
                                total_reward += simulateEnemyTurn(state, false, false, agent);
                                Helpfunctions.Instance.logg("Player 2 end turn");
                            }

                            //state.printBoard();

                            num_move += 1;

                            cost = agent.observe(reward);
                            Helpfunctions.Instance.logg("reward = " + reward);
                            total_cost += cost;
                            total_reward += reward;
                            result = state.getGameResult();

                        }
                        else
                        {
                            int debug = 1;
                        }
                    }

                    if (result == 0)
                    {
                        int debug = 1;
                    }

                    isInit = false;
                    Helpfunctions.Instance.logg("last reward: " + reward);
                    Helpfunctions.Instance.logg("total reward: " + total_reward);
                    Helpfunctions.Instance.logg("avg cost: " + total_cost / num_move);
                    Helpfunctions.Instance.WriteResultToFile(@"\learning_log.txt", "No." + i + " total/avg cost: " + total_reward + ", " + total_cost / num_move + ", end turn: " + (float)end_turn_count / num_move);

                }
            }
        }


        private void DQN(object sender, RoutedEventArgs e)
        {
            bool side = true; //own side
            int numGames = 5000;
            using (Py.GIL())
            {
                dynamic dqn = Py.Import("simple_dqn.dqn");
                dynamic Agent = dqn.Agent;

                Console.WriteLine("Done importing");
                PyInt width = new PyInt(458);
                dynamic state_size = new PyTuple(new PyObject[] {width});
                dynamic agent = Agent(Py.kw("state_size", state_size), Py.kw("discount", 0.95));
                Console.WriteLine(agent.discount);

                for (int i = 0; i < numGames; i++)
                {
                    PyFloat epsilon = new PyFloat(Math.Max(0.1, 1 - (double)(i + 1) * 4 / numGames));
                    agent.epsilon = epsilon;
                    if (!isInit) Init(true, i);
                    Playfield state = GameManager.Instance.mPlayfield;
                    int result = state.getGameResult();
                    float total_cost = 0.0f;
                    float total_reward = 0.0f;
                    float reward = 0.0f;
                    float cost = 0.0f;
                    int num_move = 0;
                    int end_turn_count = 0;

                    agent.new_episode();
                    GreedyPlayer greedyPlayer = (GreedyPlayer) GameManager.Instance.playerFirst;

                    while (result == -1)
                    {

                        Macro moveTodo = null;
                        reward = 0.0f;
                        cost = 0.0f;

                        if (state.isOwnTurn)
                        {
                            List<Macro> macroMoveList = greedyPlayer.getMacros(state);

                            macroMoveList.Add(null);

                            dynamic cur_state_vector = DNNEval.Instance.parsePlayfield(state, side);
                            List<Playfield> stateList = new List<Playfield>();
                            PyList state_vector_list = new PyList();
                            foreach (Macro macro in macroMoveList)
                            {
                                Playfield nextState = new Playfield(state);
                                if (macro != null)
                                {
                                    nextState.doMacroAction(macro);
                                    stateList.Add(nextState);
                                    reward = nextState.observerMoveReward();
                                }
                                else
                                {
                                    nextState.endTurn(false, false);
                                    nextState.drawTurnStartCard();
                                    //simulateEnemyTurn(nextState, false, true);
                                }
                                dynamic state_vector = DNNEval.Instance.parsePlayfield(nextState, side);
                                state_vector_list.Append(state_vector);
                            }
                            int action_index = agent.act(cur_state_vector, state_vector_list);
                            Helpfunctions.Instance.logg("Length:" + state_vector_list.Length());

                            moveTodo = macroMoveList[action_index];

                            if (moveTodo != null)
                            {
                                Helpfunctions.Instance.logg("Player 1 ##########Move##########");
                                //moveTodo.print();
                                state.doMacroAction(moveTodo);
                                reward = state.observerMoveReward();

                                num_move += 1;
                                cost = agent.observe(reward);
                                Helpfunctions.Instance.logg("action_index = " + action_index + "/" + (macroMoveList.Count - 1) + "reward = " + reward);
                            }
                            else
                            {
                                if (state.playerFirst.mana == state.playerFirst.ownMaxMana)
                                    end_turn_count++;
                                Helpfunctions.Instance.logg("Player 1 ##########Move##########");
                                Helpfunctions.Instance.logg("Player 1 end turn");
                                state.endTurn(false, false);
                                state.drawTurnStartCard();
                                
                                dynamic end_state_vector = DNNEval.Instance.parsePlayfield(state, side);
                                agent.update_transition(end_state_vector);
                                reward = 0.0f;

                                num_move += 1;
                                cost = agent.observe(reward);
                                Helpfunctions.Instance.logg("action_index = " + action_index + "/" + (macroMoveList.Count - 1) + "reward = " + reward);

                                total_reward += simulateEnemyTurn(state, false, false, agent);
                                Helpfunctions.Instance.logg("Player 2 end turn");
                            }

                            //state.printBoard();

                            total_cost += cost;
                            total_reward += reward;
                            result = state.getGameResult();

                        }
                        else
                        {
                            int debug = 1;
                        }
                    }

                    if (result == 0)
                    {
                        int debug = 1;
                    }

                    isInit = false;
                    Helpfunctions.Instance.logg("last reward: " + reward);
                    Helpfunctions.Instance.logg("total reward: " + total_reward);
                    Helpfunctions.Instance.logg("avg cost: " + total_cost / num_move);
                    Helpfunctions.Instance.WriteResultToFile(@"\learning_log.txt", "No." + i + " total/avg cost: " + total_reward + ", " + total_cost / num_move + ", end turn: " + (float)end_turn_count/num_move);

                }
            }


            //for e in xrange(num_episodes):
            //    observation = env.reset()
            //    done = False
            //    agent.new_episode()
            //    total_cost = 0.0
            //    total_reward = 0.0
            //    frame = 0
            //    while not done:
            //        frame += 1
            //        #env.render()
            //        action, values = agent.act(observation)
            //        #action = env.action_space.sample()
            //        observation, reward, done, info = env.step(action)
            //        print reward
            //        total_cost += agent.observe(reward)
            //        total_reward += reward
            //    print "total reward", total_reward
            //    print "mean cost", total_cost/frame

        }

        private void TrainMultipleGames(int numGame, Playfield playfield)
        {
            int moveNum = 0;
            int firstWon = 0;
            int secondWon = 0;
            int P1Won = 0;
            int P2Won = 0;
            GameRecord gc = new GameRecord();
            int trainStep = 10;
            DateTime checkPoint = DateTime.Now;

            for (int i = 0; i < numGame; i++)
            {
                //if (!isInit) Init(false, i, playfield);
                if (!isInit) Init(false, i);
                moveNum++;
                Helpfunctions.Instance.logg("movenum = " + moveNum);

                int result = GameManager.Instance.playMove();
                while (result == -1)
                {
                    moveNum++;
                    Helpfunctions.Instance.logg("movenum = " + moveNum);
                    result = GameManager.Instance.playMove();

                    dynamic res = LogitEval.Instance.Train();
                    //int trainResult = (int)res[0];
                    //if (trainResult == 1)
                    //{
                    //    Helpfunctions.Instance.WriteResultToFile(@"\batch_sample.txt", res.ToString());
                    //}
                    res.Dispose();

                }

                if (result == 0)
                {
                    firstWon++;
                    if (i % 2 == 0)
                        P1Won++;
                    else
                        P2Won++;
                    Helpfunctions.Instance.logg("i = " + i + "result = " + result);
                    gc.result = 0;
                }
                else
                {
                    secondWon++;
                    if (i % 2 == 0)
                        P2Won++;
                    else
                        P1Won++;
                    Helpfunctions.Instance.logg("i = " + i + "result = " + result);
                    gc.result = 1;
                }
  
                gc.playSec = GameManager.Instance.featureList;
                Helpfunctions.Instance.WriteResultToFile(@"\lvl_result.txt", JsonConvert.SerializeObject(gc));

                LogitEval.Instance.SaveExp(gc);

                if (i >= 10)
                {
                    Helpfunctions.Instance.logg("Train at trainStep = " + trainStep);
                    Helpfunctions.Instance.logg("SF 1 WON " + P1Won + " GAMES");
                    Helpfunctions.Instance.logg("MCTS 2 WON " + P2Won + " GAMES");
                    
                    if (i % 10 == 0)
                    {
                        DateTime newCheckPoint = DateTime.Now;
                        Helpfunctions.Instance.WriteResultToFile(@"\train_result.txt", "Time: " + GameManager.Instance.P1Time + " VS " + GameManager.Instance.P2Time +", P1: " + P1Won + " VS P2: " + P2Won + "\n");
                        //Helpfunctions.Instance.WriteResultToFile(@"\train_result.txt", "Time: " + GameManager.Instance.P1Time + " VS " + GameManager.Instance.P2Time + ", P1: " + P1Won + " VS P2: " + P2Won + "\n");
                        P1Won = 0;
                        P2Won = 0;
                        GameManager.Instance.P1Time = 0.0;
                        GameManager.Instance.P2Time = 0.0;
                        checkPoint = newCheckPoint;
                    }
                }

                GameManager.Instance.featureList.Clear();
                isInit = false;               
                GC.Collect();
                GameManager.resetSeed();
            }
            Helpfunctions.Instance.logg("SF 1 WON " + P1Won + " GAMES");
            Helpfunctions.Instance.logg("MCTS 2 WON " + P2Won + " GAMES");

        }

        private void SampleStates(int numGame, Playfield playfield)
        {
            for (int i = 0; i < numGame; i++)
            //for (; ; )
            {
                //if (!isInit) Init(false, i, playfield);
                if (!isInit) Init(false, i);
                IOUtils.SaveState(GameManager.Instance.mPlayfield, @"\starting_states.txt");
                isInit = false;
                GC.Collect();
                GameManager.resetSeed();
            }
        }

        private void PlayMultipleGames(int numGame, Playfield playfield)
        {
            int moveNum = 0;
            int firstWon = 0;
            int secondWon = 0;
            int P1Won = 0;
            int P2Won = 0;
            GameRecord gc = new GameRecord();

            for (int i = 0; i < numGame; i++)
            //for (; ; )
            {
                //if (!isInit) Init(false, i, playfield);
                if (!isInit) Init(false, i);
                moveNum++;
                Helpfunctions.Instance.logg("movenum = " + moveNum);
                GameManager.Instance.turnStartEntity = GameManager.Instance.mPlayfield.nextEntity;

                int result = GameManager.Instance.playMove();
                while (result == -1)
                {
                    moveNum++;
                    Helpfunctions.Instance.logg("movenum = " + moveNum);
                    result = GameManager.Instance.playMove();
                }

                //if (result == 0 && i % 2 == 0 || result == 1 && i % 2 != 0)
                if (result == 0)
                {
                    firstWon++;
                    if (i % 2 == 0)
                        P1Won++;
                    else
                        P2Won++;
                    Helpfunctions.Instance.logg("i = " + i + "result = " + result);
                    gc.result = 0;
                }
                else
                {
                    secondWon++;
                    if (i % 2 == 0)
                        P2Won++;
                    else
                        P1Won++;
                    Helpfunctions.Instance.logg("i = " + i + "result = " + result);
                    gc.result = 1;
                }
                //else
                //{//for debuging
                //    break;
                //}
                //gc.playSec = GameManager.Instance.featureList;
                //Helpfunctions.Instance.WriteResultToFile(@"\ava_result_4.txt", JsonConvert.SerializeObject(gc));
                //Console.WriteLine(gc.playSec[0].attackPlayer.playedActionJsonList[0].cardEntitiy);
                //Helpfunctions.Instance.WriteResultToFile(@"\svs_result_5.txt", JsonConvert.SerializeObject(gc));
                //Console.WriteLine(gc.playSec[0].attackPlayer.playedActionJsonList[0].cardEntitiy);
                //PyDataEncoder.Instance.replay(gc);
                //string player1BCString = ((GreedyPlayer)GameManager.Instance.playerFirst).getAvgBCString();
                //string player2BCString = ((GreedyPlayer)GameManager.Instance.playerSecond).getAvgBCString();
                //Helpfunctions.Instance.WriteResultToFile(@"\avg_bc.txt", player1BCString + " vs " + player2BCString);
                //Helpfunctions.Instance.WriteResultToFile(JsonConvert.SerializeObject(gc));
                //gcList.Add(gc);

                

                //GameManager.Instance.featureList.Clear();
                isInit = false;
                Helpfunctions.Instance.logg("SF 1 WON " + P1Won + " GAMES");
                Helpfunctions.Instance.logg("MCTS 2 WON " + P2Won + " GAMES");
                //((QLearningAgent)GameManager.Instance.playerFirst).printWeights();
                GC.Collect();
                GameManager.resetSeed();
            }
            Helpfunctions.Instance.logg("SF 1 WON " + P1Won + " GAMES");
            Helpfunctions.Instance.logg("MCTS 2 WON " + P2Won + " GAMES");
            //string sampleResult = firstWon.ToString() + '/' + secondWon.ToString();
            //loadRecord(gc);

            //((QLearningAgent)GameManager.Instance.playerSecond).printWeights();
            //updateAllUI();
        }

        private void AutoPlay(object sender, RoutedEventArgs e)
        {
            PlayMultipleGames(20000, null);
            //TrainMultipleGames(20000, null);
            //SampleStates(20000, null);
            //string myDocPath =
            //    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //string fileName = @"\data_turn_end\svs_result_3.txt";
            //string filePath = myDocPath + fileName;
            //PyDataEncoder.Instance.Encode(filePath);
        }

        private void MakeMove(object sender, RoutedEventArgs e)
        {
            if (!isInit) Init(false, 0);
            //testField.printBoard();
            int result = GameManager.Instance.playMove();
            if (result != -1) isInit = false;
            updateAllUI();
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


