using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultScreen : MonoBehaviour
{

    [System.Serializable]
    public class Result
    {
        public TextMeshProUGUI countText;

        [HideInInspector]
        public string result;

        [HideInInspector]
        public float ratio;
    }

    public Sprite cRankEmblem;
    public Sprite bRankEmblem;
    public Sprite aRankEmblem;
    public Sprite sRankEmblem;

    public Image rankImage;
    public Button quitToMenuButton;
    public GameObject resultScreen;
    public Result battleships   = new Result(); 
    public Result submarines    = new Result();
    public Result cannon        = new Result();
    public Result torpedo       = new Result();
    public Result cannonDamage  = new Result();
    public Result torpedoDamage = new Result();

    public TextMeshProUGUI enemyEfficiencyTextMesh;
    public TextMeshProUGUI accuracyTextMesh;
    public TextMeshProUGUI damageTakenTextMesh;

    bool isOpen = false;
    CanvasGroup cg;


    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Button>().onClick.AddListener(GetComponentInParent<SceneController>().ToMainMenu);
        cg = resultScreen.GetComponent<CanvasGroup>();
        cg.alpha = 0.0f;

        resultScreen.SetActive(false);
        quitToMenuButton.onClick.AddListener(SceneController.Get().ToMainMenu);


    }

    public void Initialize()
    {
        Stats stats = MainController.Get().GetStats();
        resultScreen.SetActive(true);

        int hit = 0;
        int total = 0;
        float ratio = 0.0f;

        // Battleship sunk
        Stats.EnemyStats enemyStats = stats.enemyStats[EnemySpawner.EnemyType.Destroyer];
        total = enemyStats.count;
        hit = enemyStats.sunkCount;
        if (total > 0)
        {
            ratio = (float)hit / (float)total;
            battleships.ratio = ratio;
        }
        battleships.result = hit.ToString() + " / " + total.ToString();
        battleships.countText.text = battleships.result;


        // Submarine sunk
        enemyStats = stats.enemyStats[EnemySpawner.EnemyType.Submarine];
        total = enemyStats.count;
        hit = enemyStats.sunkCount;

        if (total > 0)
        {
            ratio = (float)hit / (float)total;
            submarines.ratio = ratio;
        }
        submarines.result = hit.ToString() + " / " + total.ToString();
        submarines.countText.text = submarines.result;


        // Cannons fired
        Stats.ArmamentStats armamentStats = stats.stats[ArmamentController.Armaments.Cannon];
        total = armamentStats.fired;
        hit = armamentStats.hit;

        if (total > 0)
        {
            ratio = (float)hit / (float)total;

            cannon.ratio = ratio;
        }
        cannon.result = hit.ToString() + " / " + total.ToString();
        cannon.countText.text = cannon.result;


        // Torpedoes fired
        armamentStats = stats.stats[ArmamentController.Armaments.AimedTorpedo];
        total = armamentStats.fired;
        hit = armamentStats.hit;

        if (total > 0)
        {
            ratio = (float)hit / (float)total;
            torpedo.ratio = ratio;
        }
        torpedo.result = hit.ToString() + " / " + total.ToString();
        torpedo.countText.text = torpedo.result;

        // CannonDamage taken
        total = stats.cannonHitsTaken;
        cannonDamage.countText.text = total.ToString();

        total = stats.torpedoHitsTaken;
        torpedoDamage.countText.text = total.ToString();


        ratio = 0.0f;
        float sinkRatio = 0.0f;
        // To prevent it dividing 0

        sinkRatio = (battleships.ratio + submarines.ratio) / 2.0f;
        enemyEfficiencyTextMesh.text = (sinkRatio * 100).ToString() + "%";

        float hitRatio = (cannon.ratio + submarines.ratio) / 2.0f;
        accuracyTextMesh.text = (hitRatio * 100).ToString() + "%";



        float damageRatio = (float)stats.damageTaken / 100.0f;
        damageRatio = Mathf.Min(ratio, 99.9f);
        damageTakenTextMesh.text = damageRatio.ToString() + "%";

        float finalRank = sinkRatio + damageRatio + hitRatio / 3.0f;

        Sprite tex = null;
        if (finalRank > .9f)
            tex = sRankEmblem;
        else if (finalRank > .8f)
            tex = aRankEmblem;
        else if (finalRank > .6f)
            tex = bRankEmblem;
        else
            tex = cRankEmblem;


        rankImage.sprite = tex;





        Time.timeScale = 0.0f;
        Pause.Get().isPaused = true;
        isOpen = true;

        SceneController.Get().ShowCursor();
    }
    // Update is called once per frame
    void Update()
    {
        // Only lerps the alpha of the canvas and skip initialization conditions if is laready opened
        if (isOpen)
        {
            cg.alpha = Mathf.Lerp(cg.alpha, 1.0f, 0.1f);
            return;
        }



        // Conditions to show result screen
        int maxEnemy = EnemySpawner.Get().enemyCount;
        int sunkEnemies = 0;

        foreach (var entity in Stats.Get().enemyStats)
        {
            sunkEnemies += entity.Value.sunkCount;
        }
        
        // If stage timer done
        if (GetComponentInChildren<StageTimer>().IsDone() || sunkEnemies >= maxEnemy)
            Initialize();



    }
}
