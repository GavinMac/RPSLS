using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateMachine : MonoBehaviour {

    public Player player;
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
    public float currentHealth;
    private float DamageDone;

    public Image HealthBar;
    public Text HpValueText;
    public GameObject ActionSelector;
    private SpriteRenderer spriteRenderer;

    public bool HasLost = false;

	// Use this for initialization
	void Start () {
        currentState = TurnState.PROCESSING;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        ActionSelector.SetActive(false);
        currentHealth = 100f;
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        switch (currentState)
        {
            case TurnState.PROCESSING:
                ActionSelector.SetActive(false);
                player.currentChoice = "";
                BSM.playerAttack = "";
                UpdateHealthBar();
                currentState = TurnState.CHOOSING;
                break;

            case TurnState.CHOOSING:
                ActionSelector.SetActive(true);
                CheckChoiceMade();
                break;

            case TurnState.WAITING:
                ActionSelector.SetActive(false);
                break;

            case TurnState.ACTION:
                ActionSelector.SetActive(false);
                break;

            case TurnState.DEAD:
                ActionSelector.SetActive(false);
                Debug.Log("Player is Dead");
                break;
        }

	}

    public void UpdateHealthBar()
    {
        HpValueText.text = currentHealth.ToString();
        HealthBar.transform.localScale = new Vector3(Mathf.Clamp(currentHealth, 0, 1), HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);

        if (currentHealth <= 0)
        {
            spriteRenderer.sprite = Sprites[5];
            currentState = TurnState.DEAD;
        }
    }

    void CheckChoiceMade()
    {
        if(player.currentChoice != "")
        {
            currentState = TurnState.WAITING;
        }
    }

    public void SetChoiceRock()
    {
        player.currentChoice = "rock";
        BSM.playerAttack = "rock";
        spriteRenderer.sprite = Sprites[0];
    }

    public void SetChoicePaper()
    {
        player.currentChoice = "paper";
        BSM.playerAttack = "paper";
        spriteRenderer.sprite = Sprites[1];
    }

    public void SetChoiceScissors()
    {
        player.currentChoice = "scissors";
        BSM.playerAttack = "scissors";
        spriteRenderer.sprite = Sprites[2];
    }

    public void SetChoiceLizard()
    {
        player.currentChoice = "lizard";
        BSM.playerAttack = "lizard";
        spriteRenderer.sprite = Sprites[3];
    }

    public void SetChoiceSpock()
    {
        player.currentChoice = "spock";
        BSM.playerAttack = "spock";
        spriteRenderer.sprite = Sprites[4];
    }




}
