using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleStateMachine : MonoBehaviour {


    public GameObject player;
    public GameObject enemy;

    public enum BattleStates
    {
        WAIT,
        CHECK_ATTACKS,
        PERFORM_ACTION,
        END
    }

    private IEnumerator GameEnder()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);    
    }

    public BattleStates battleState;
    public Text EndGameText;

    //public List<HandleAttacks> PerformList = new List<HandleAttacks>();

    public string playerAttack;
    public string enemyAttack;

    public float DamageToDo;
    public string RoundWinner;
    public string WinningAttack;
    public string LosingAttack;

    public string OutputMessage;

	// Use this for initialization
	void Start () {
        battleState = BattleStates.WAIT;
	}
	
	// Update is called once per frame
	void Update () {

        switch (battleState)
        {
            case BattleStates.WAIT:
                Debug.Log("Hits wait");
                CheckPlayerChose();
                break;

            case BattleStates.CHECK_ATTACKS:
                Debug.Log("Hits Check attacks");
                CheckAttacks();
                break;

            case BattleStates.PERFORM_ACTION:
                Debug.Log("Hits Perform Action");
                PerformAttack();
                break;

            case BattleStates.END:
                player.GetComponent<PlayerStateMachine>().enabled = false;
                enemy.GetComponent<EnemyStateMachine>().enabled = false;
                CheckForWinner();
                StartCoroutine(GameEnder());
                break;
        }
	}


    /// <summary>
    /// Checks the players attack to get all possible outcomes. (runs the game rules)
    /// </summary>
    void CheckAttacks()
    {
        //playerAttack = player.GetComponent<PlayerStateMachine>().player.currentChoice;
        //enemyAttack = enemy.GetComponent<EnemyStateMachine>().enemy.currentChoice;

        //If player is rock and enemy is scissors or lizard
        if (playerAttack == "rock" && enemyAttack == "scissors" || enemyAttack == "lizard")
        {
            RoundWinner = "Player";
            WinningAttack = playerAttack;
            LosingAttack = enemyAttack;
        }
        //If player is paper and enemy is rock or spock
        else if(playerAttack == "paper" && enemyAttack == "rock" || enemyAttack == "spock")
        {
            RoundWinner = "Player";
            WinningAttack = playerAttack;
            LosingAttack = enemyAttack;
        }
        //If player is scissors and enemy is paper or lizard
        else if (playerAttack == "scissors" && enemyAttack == "paper" || enemyAttack == "lizard")
        {
            RoundWinner = "Player";
            WinningAttack = playerAttack;
            LosingAttack = enemyAttack;
        }
        //If player is lizard and enemy is 
        else if (playerAttack == "lizard" && enemyAttack == "paper" || enemyAttack == "spock")
        {
            RoundWinner = "Player";
            WinningAttack = playerAttack;
            LosingAttack = enemyAttack;
        }
        //If player is scissors and enemy is
        else if (playerAttack == "spock" && enemyAttack == "scissors" || enemyAttack == "rock")
        {
            RoundWinner = "Player";
            WinningAttack = playerAttack;
            LosingAttack = enemyAttack;
        }
        //Otherwise, the enemy wins
        else
        {
            RoundWinner = "Enemy";
            WinningAttack = enemyAttack;
            LosingAttack = playerAttack;
        }
        
        if(RoundWinner != "" && WinningAttack != "" && LosingAttack != "")
        {
            battleState = BattleStates.PERFORM_ACTION;
        }
        else
        {
            battleState = BattleStates.WAIT;
        }

    }


    void PerformAttack()
    {
        OutputMessage = WinningAttack + GetAttackActionMessage(WinningAttack, LosingAttack) + LosingAttack;
        float damageToDo = GetDamageToDo();
        
        if(RoundWinner == "Player")
        {
            AttackEnemy(damageToDo);
        }
        else
        {
            AttackPlayer(damageToDo);
        }

        battleState = BattleStates.WAIT;
    }

    //Attack the enemy with a given damage
    void AttackEnemy(float dmgToDeal)
    {
        //Deal damage to the enemy
        float currHp = enemy.GetComponent<EnemyStateMachine>().currentHealth;
        float newHp = currHp - dmgToDeal;
        currHp = enemy.GetComponent<EnemyStateMachine>().currentHealth = newHp;

        //Set player states
        player.GetComponent<PlayerStateMachine>().currentState = PlayerStateMachine.TurnState.PROCESSING;
        enemy.GetComponent<EnemyStateMachine>().currentState = EnemyStateMachine.TurnState.PROCESSING;
    }


    //Attack the player with a given damage
    void AttackPlayer(float dmgToDeal)
    {
        //Deal damage to the player
        float currHp = player.GetComponent<PlayerStateMachine>().currentHealth;
        float newHp = currHp - dmgToDeal;
        currHp = player.GetComponent<PlayerStateMachine>().currentHealth = newHp;

        //Set player states
        player.GetComponent<PlayerStateMachine>().currentState = PlayerStateMachine.TurnState.PROCESSING;
        enemy.GetComponent<EnemyStateMachine>().currentState = EnemyStateMachine.TurnState.PROCESSING;
    }


    /// <summary>
    /// Sets the damage that will be done based on the winning attack
    /// </summary>
    public float GetDamageToDo()
    {
        float dmg = 0f;
        switch (WinningAttack)
        {
            case "rock":
                dmg = 25f;
                break;

            case "paper":
                dmg = 5f;
                break;

            case "scissors":
                dmg = 10f;
                break;

            case "lizard":
                dmg = 15f;
                break;

            case "spock":
                dmg = 20f;
                break;
        }
        DamageToDo = dmg;
        return dmg;
    }

    void CheckPlayerChose()
    {
        if(playerAttack != "")
        {
            enemy.GetComponent<EnemyStateMachine>().currentState = EnemyStateMachine.TurnState.CHOOSING;
            battleState = BattleStates.CHECK_ATTACKS;
        }
        else
        {
            battleState = BattleStates.WAIT;
        }
    }

    void CheckForWinner()
    {
        if(player.GetComponent<PlayerStateMachine>().currentState == PlayerStateMachine.TurnState.DEAD)
        {
            battleState = BattleStates.END;
            EndGameText.text = "You Lose!";
        }
        else if(enemy.GetComponent<EnemyStateMachine>().currentState == EnemyStateMachine.TurnState.DEAD)
        {
            battleState = BattleStates.END;
            EndGameText.text = "You Win!";
        }   

    }


    public string GetAttackActionMessage(string WinAttack, string LoseAttack)
    {
        string message = " beats ";


        return message;
    }

}
