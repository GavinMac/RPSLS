using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStateMachine : MonoBehaviour {

    public Enemy enemy;
    private BattleStateMachine BSM;
    public List<Sprite> Sprites = new List<Sprite>();

    public enum TurnState
    {
        PROCESSING,
        CHOOSING,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    private float maxHealth = 100f;
    public float currentHealth = 100f;
    private float DamageDone;
    private SpriteRenderer spriteRenderer;
    public bool IsDead = false;

    public Image HealthBar;
    public Text HpValueText;

    // Use this for initialization
    void Start()
    {
        currentState = TurnState.WAITING;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        currentHealth = 100f;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case TurnState.PROCESSING:
                Debug.Log("enemy processing");          
                enemy.currentChoice = "";
                BSM.enemyAttack = "";
                UpdateHealthBar();
                spriteRenderer.sprite = Sprites[0];
                currentState = TurnState.CHOOSING;
                break;

            case TurnState.CHOOSING:
                Debug.Log("enemy choosing");
                SetRandomChoice();
                currentState = TurnState.WAITING;
                break;

            case TurnState.WAITING:
                Debug.Log("enemy waiting");
                break;

            case TurnState.ACTION:
                break;

            case TurnState.DEAD:            
                Debug.Log("Enemy is Dead");
                break;
        }

    }

    public void UpdateHealthBar()
    {
        HpValueText.text = currentHealth.ToString();
        HealthBar.transform.localScale = new Vector2(currentHealth / maxHealth, HealthBar.transform.localScale.y);

        if (currentHealth <= 0f)
        {
            HealthBar.transform.localScale = new Vector2(0, 0);
            spriteRenderer.sprite = Sprites[5];
            currentState = TurnState.DEAD;
            IsDead = true;
        }
    }

    void SetRandomChoice()
    {
        string[] Choices = new string[] { "rock", "paper", "scissors", "lizard", "spock" };
        int index = Random.Range(0, 4);
        enemy.currentChoice = Choices[index];
        BSM.enemyAttack = Choices[index];
        spriteRenderer.sprite = Sprites[index];
    }

}
