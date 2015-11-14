using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSim
{
    public class QLearningAgent{

    private static long   serialVersionUID    = 0L;

    private static Random    rng = new Random();
    private static bool      debug;

    // q-learning constants
    private static float  DISCOUNT_FACTOR     = 0.9f;
    private static float  LEARNING_RATE       = 0.0001f;
    private static float  EPSILON             = 0.02f;
    private static int    NUM_FEATURES        = 98;
    private static int    TRAINING_EPS        = 10;
    private static int    EVALUATION_EPS      = 5;

    // agent parameters
    private int                 maxEpisodes;            // max number of episodes to play
    private int                 numEpisodes;            // number of episodes played
    private int                 evalEpisodes;           // number of episodes left for evaluation
    private float               evalTotalReward;        // cumulative reward during evaluation
    private bool                freezeQ;                // do not update Q when set to true
    private List<float>         weights;                // weights of features to learn
    private float reward = 0;

    // episode parameters
    private float               episodeReward;          // current total reward for this episode
    //private QLState             lastQls;                // cache the last state for comparing to current
    private Playfield           initState, lastState;
    private float               qLast;
    private int playerSide;

    public QLearningAgent(int playerSide, Playfield p) {
 
        initState = new Playfield(p);
        playerSide = playerSide;

        maxEpisodes = 100;
        // initialize agent parameters
        numEpisodes             = 0;
        evalEpisodes            = 0;
        evalTotalReward         = 0f;
        freezeQ                 = false;
        weights                 = new List<float>();
        for(int i = 0; i < NUM_FEATURES; i++)
            weights.Add((float)rng.NextDouble() * 2f - 1f);    // set initial weights to random value between -1 and 1
        //printWeights();
        normalizeWeights();

        Helpfunctions.Instance.logg("Q-Learning Agent initialized for " + maxEpisodes + " episodes");
    }

    public void QLearning(){
    
        for (int i = 0; i < maxEpisodes; i++){
            
            lastState = new Playfield(initState);
            lastState.drawInitCards();
            qLast = Q(lastState, lastState.isOwnTurn);

            int score = lastState.getGameResult();
            while (score == -1)
            {
                List<Action> moves = Movegenerator.Instance.getMoveList(lastState, false, false, true);

                if (moves.Count == 0)
                {
                    lastState.endTurn(false, false);
                }
                else {
                    float maxQValue = 0;
                    Action bestAction = null;
                    Playfield bestState = null;    
                    //psilon greedy             
                    if (rng.NextDouble() < EPSILON){
                        bestAction = moves[rng.Next(moves.Count)];
                    }else{
                        foreach (Action action in moves){
                            Playfield afterState = new Playfield(lastState);
                            afterState.doAction(action);                 
                            float QValue = Q(afterState, afterState.isOwnTurn);
                            if (QValue > maxQValue){
                                maxQValue = QValue;
                                bestAction = action;
                                bestState = afterState;
                            }
                        }
                    }

                    //update weights
                    float difference = reward + DISCOUNT_FACTOR * maxQValue - qLast;
                    //if(debug) System.out.printf("%.5f\n", difference);
                    List<float> features = getFeatures(lastState, lastState.isOwnTurn);
                    for(int j = 0; j < NUM_FEATURES; j++) {
                        //if(debug) System.out.printf("w%d = %.5f + %.5f * %.5f * %.1f = ", i, weights.get(i), LEARNING_RATE, difference, features.get(i));
                        weights[j] = weights[j] + LEARNING_RATE * difference * features[j];
                        //if(debug) System.out.printf("%.5f\n", weights.get(i));
                    }
                    normalizeWeights();

                    lastState = bestState;
                    qLast = maxQValue;
                }
               
                score = lastState.getGameResult();
            }
        }
    
    }


    //public Map<Integer, Action> initialStep(StateView state, HistoryView stateHistory) {
    //    // initialize episode parameters
    //    episodeReward = 0f;

    //    // initialize step parameters
    //    QLState qls = new QLState(state);

    //    if(debug) System.out.printf("Initial step:\n");

    //    // analyze the state and determine next action for each footman
    //    for(QLUnit f: qls.getFriendly()) {
    //        int fid = f.getID();
    //        // choose a random target with probability EPSILON
    //        if(rng.nextFloat() < EPSILON) qls.setRandomTargetFor(fid);
    //        // otherwise choose greedy option (target footman with best Q score for current policy)
    //        else qls.setTarget(fid, getQMaxTarget(qls, fid).b);
    //    }

    //    lastQls = qls;                                  // cache this state
    //    if(debug) qls.print();

    //    return translateActions(qls);
    //}

    //public Map<Integer, Action> middleStep(StateView state, HistoryView stateHistory) {
    //    // initialize step parameters
    //    Map<Integer, Action> actions = new HashMap<Integer, Action>();
    //    Map<Integer, Integer> bestTargets = new HashMap<Integer, Integer>();
    //    int step = state.getTurnNumber();
    //    QLState qls = new QLState(state);
    //    // get the combat logs for the last step
    //    List<DamageLog> damageLogs = stateHistory.getDamageLogs(step - 1);
    //    List<DeathLog> deathLogs = stateHistory.getDeathLogs(step - 1);
    //    // mark that event has occurred if any friendly units took damage or enemies died
    //    bool eventOccurred = false;
    //    for(DamageLog log: damageLogs) {
    //        if(log.getDefenderController() == 0) {
    //            eventOccurred = true;
    //            break;
    //        }
    //    }

    //    if(!eventOccurred) {
    //        for(DeathLog log: deathLogs) {
    //            if(log.getController() == 1) {
    //                eventOccurred = true;
    //                break;
    //            }
    //        }
    //    }

    //    if(debug) System.out.printf("\nStep %d:\n", step);

    //    // decompose reward for each footman and determine best target for next move
    //    for(QLUnit f: lastQls.getFriendly()) {
    //        bool died = false;                       // did the friendly footman die this turn?
    //        int fid = f.getID();                        // friendly footman id
    //        int tid = lastQls.getTargetFor(fid);        // enemy target id

    //        float reward = -0.1f;                       // each step costs -0.1

    //        if(debug) System.out.printf("Reward for ATTACK(%d,%d) = %.1f", fid, tid, reward);

    //        // check for damage dealt / taken between fid and tid
    //        for(DamageLog log: damageLogs) {
    //            if(log.getAttackerID() == fid) {
    //                reward += (float)log.getDamage();   // damage dealt rewards +damage
    //                if(debug) System.out.printf(" + %d", log.getDamage());
    //            }
    //            if(log.getDefenderID() == fid) {
    //                reward -= (float)log.getDamage();   // damage taken costs -damage
    //                if(debug) System.out.printf(" - %d", log.getDamage());
    //            }
    //        }

    //        // check for death of fid and tid
    //        for(DeathLog log: deathLogs) {
    //            if(log.getDeadUnitID() == tid) {
    //                reward += 100f;                     // killing enemy rewards +100
    //                if(debug) System.out.print(" + 100");
    //            }
    //            if(log.getDeadUnitID() == fid) {
    //                reward -= 100f;                     // dying costs -100
    //                died = true;
    //                if(debug) System.out.print(" - 100");
    //            }
    //        }

    //        if(debug) System.out.printf(" = %.1f\n", reward);

    //        episodeReward += reward;                    // accumulate reward

    //        float qLast = Q(lastQls, fid, tid);
    //        float qMax;
    //        int bestTarget;

    //        if(died) {
    //            qMax = qLast;
    //        } else {
    //            // identify best targets for next move
    //            Pair<Float, Integer> qMaxTarget = getQMaxTarget(qls, fid);
    //            qMax = qMaxTarget.a;
    //            bestTarget = qMaxTarget.b;
    //            bestTargets.put(fid, bestTarget);
    //        }

    //        // update the Q function with feedback
    //        if(!freezeQ) {
    //            if(debug) System.out.printf("Difference = %.1f + %.1f * %.3f - %.3f = ", reward, DISCOUNT_FACTOR, qMax, qLast);
    //            float difference = reward + DISCOUNT_FACTOR * qMax - qLast;
    //            if(debug) System.out.printf("%.5f\n", difference);
    //            List<Float> features = getFeatures(lastQls, fid, tid);
    //            // update weights for each feature
    //            for(int i = 0; i < NUM_FEATURES; i++) {
    //                if(debug) System.out.printf("w%d = %.5f + %.5f * %.5f * %.1f = ", i, weights.get(i), LEARNING_RATE, difference, features.get(i));
    //                weights.set(i, weights.get(i) + LEARNING_RATE * difference * features.get(i));
    //                if(debug) System.out.printf("%.5f\n", weights.get(i));
    //            }
    //            normalizeWeights();
    //        }
    //    }

    //    if(eventOccurred) {                             // reallocate targets at event point
    //        for(QLUnit f: qls.getFriendly()) {
    //            int fid = f.getID();
    //            // choose a random target with probability EPSILON
    //            if(rng.nextFloat() < EPSILON) qls.setRandomTargetFor(fid);
    //            // otherwise choose greedy option (target footman with best Q score for current policy)
    //            else qls.setTarget(fid, bestTargets.get(fid));
    //        }

    //        actions = translateActions(qls);            // generate actions
    //    } else {                                        // otherwise maintain same targets
    //        qls.copyTargetsFrom(lastQls);
    //    }

    //    lastQls = qls;                                  // update last state
    //    if(debug) qls.print();

    //    return actions;
    //}

    //@Override
    //public void terminalStep(StateView state, HistoryView stateHistory) {
    //    if(evalEpisodes == 0) {
    //        if(debug) {
    //            System.out.printf("Completed episode %d with reward %.1f\n", numEpisodes, episodeReward);
    //            printWeights();
    //        } else {
    //            System.out.print("|");
    //        }
    //        numEpisodes++;
    //        if(numEpisodes % TRAINING_EPS == 0) {       // finished training
    //            freezeQ = true;                         // stop Q from updating
    //            evalEpisodes = EVALUATION_EPS;          // evaluate for next EVALUATION_EPS episodes
    //            evalTotalReward = 0;                    // reset cumulative reward
    //        }
    //    } else {                                        // evaluation
    //        if(debug) System.out.printf("Completed evaluation episode with reward %.1f\n", episodeReward);
    //        else System.out.print("*");
    //        evalTotalReward += episodeReward;           // accumulate reward
    //        if(--evalEpisodes == 0) {                   // finished evaluating
    //            System.out.println();
    //            freezeQ = false;
    //            float avgReward = evalTotalReward / EVALUATION_EPS;
    //            System.out.printf("Episodes Played: %d, Average Reward: %.1f\n", numEpisodes, avgReward);
    //            if(debug) printWeights();
    //            if(numEpisodes == maxEpisodes) System.exit(0);
    //        }
    //    }
    //}

    // returns a list of feature values for a = ATTACK(fid, eid)
    private List<float> getFeatures(Playfield p, bool own) {
        Player mPlayer, ePlayer;
        if (own){
            mPlayer = p.playerFirst;
            ePlayer = p.playerSecond;
        }else{
            mPlayer = p.playerSecond;
            ePlayer = p.playerSecond;
        }
        List<float> features = new List<float>();
        //Minions
                //public float getPlayfieldValue(Playfield p, bool own)

        features.Add(mPlayer.ownHero.Hp + mPlayer.ownHero.armor);
        features.Add(mPlayer.ownMaxMana);
        features.Add(mPlayer.spellpower);
        features.Add((mPlayer.ownHero.frozen)? 0: mPlayer.ownWeaponAttack);

        int minionCount = 0;
        foreach (Minion m in mPlayer.ownMinions)
        {
            features.Add(m.Hp);
            features.Add(m.Angr);
            features.Add((m.silenced)? 0: Convert.ToSingle(m.handcard.card.isSpecialMinion));
            features.Add(Convert.ToSingle(m.taunt));
            features.Add(Convert.ToSingle(m.stealth));
            features.Add(Convert.ToSingle(m.divineshild));
            minionCount++;
        }

        int emptySlotCount = 7 - minionCount;
        for (int i = 0; i < emptySlotCount * 6; i++){
            features.Add(0);
        }

        int mobsInHand = 0;
        int bigMobsInHand = 0;
        foreach (Handmanager.Handcard hc in mPlayer.owncards)
        {
            if (hc.card.type == CardDB.cardtype.MOB)
            {
                 mobsInHand++;
                 if (hc.card.Attack >= 3) bigMobsInHand++;
            }
        }

        features.Add(mobsInHand);
        features.Add(bigMobsInHand);
        features.Add(mPlayer.owncards.Count);

        features.Add(ePlayer.ownHero.Hp + ePlayer.ownHero.armor);
        features.Add(ePlayer.ownMaxMana);
        features.Add(ePlayer.spellpower);
        features.Add((ePlayer.ownHero.frozen)? 0: ePlayer.ownWeaponAttack);

        minionCount = 0;
        foreach (Minion m in ePlayer.ownMinions)
        {
            features.Add(m.Hp);
            features.Add(m.Angr);
            features.Add((m.silenced)? 0: Convert.ToSingle(m.handcard.card.isSpecialMinion));
            features.Add(Convert.ToSingle(m.taunt));
            features.Add(Convert.ToSingle(m.stealth));
            features.Add(Convert.ToSingle(m.divineshild));
            minionCount++;
        }

        emptySlotCount = 7 - minionCount;
        for (int i = 0; i < emptySlotCount * 6; i++){
            features.Add(0);
        }

        mobsInHand = 0;
        bigMobsInHand = 0;
        foreach (Handmanager.Handcard hc in ePlayer.owncards)
        {
            if (hc.card.type == CardDB.cardtype.MOB)
            {
                 mobsInHand++;
                 if (hc.card.Attack >= 3) bigMobsInHand++;
            }
        }

        features.Add(mobsInHand);
        features.Add(bigMobsInHand);
        features.Add(ePlayer.owncards.Count);

        return features;

    }

    // normalize weights
    private void normalizeWeights() {
        float total = 0;
        foreach(float w in weights){
            total += w;
        }
        for(int i = 0; i < NUM_FEATURES; i++){
            weights[i] = weights[i] / total;
        }
    }

    // evaluate linear approximation Q function for s = qls and a = ATTACK(fid, eid)
    private float Q(Playfield afterState, bool own) {
        float q = 0;
        List<float> features = getFeatures(afterState, own);

        // w0 + w * f(s, a)
        for(int i = 0; i < NUM_FEATURES; i++)
            q += weights[i] * features[i];

        return q;
    }

    public static Random getRng() { return rng; }

    }
}
