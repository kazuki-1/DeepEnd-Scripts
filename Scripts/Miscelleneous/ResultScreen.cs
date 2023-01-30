using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {



        resultScreen.SetActive(false);



    }

    public void Initialize()
    {
        Stats stats = MainController.Get().GetStats();


        int hit = 0;
        int total = 0;
        float ratio = 0.0f;

        // Battleship sunk
        Stats.EnemyStats enemyStats = stats.enemyStats[EnemySpawner.EnemyType.Destroyer];
        total = enemyStats.count;
        hit = enemyStats.sunkCount;
        ratio = (float)hit / (float)total;
        battleships.ratio = ratio;

        battleships.result = hit.ToString() + " / " + total.ToString();
        battleships.countText.text = battleships.result;


        // Submarine sunk
        enemyStats = stats.enemyStats[EnemySpawner.EnemyType.Submarine];
        total = enemyStats.count;
        hit = enemyStats.sunkCount;
        ratio = (float)hit / (float)total;
        battleships.ratio = ratio;

        battleships.result = hit.ToString() + " / " + total.ToString();
        battleships.countText.text = battleships.result;


        // Cannons fired
        Stats.ArmamentStats armamentStats = stats.stats[ArmamentController.Armaments.Cannon];
        total = armamentStats.fired;
        hit = armamentStats.hit;
        ratio = (float)hit / (float)total;
        if (ratio == float.NaN)
            ratio = 1.0f;

        cannon.ratio = ratio;

        cannon.result = hit.ToString() + " / " + total.ToString();
        cannon.countText.text = cannon.result;


        // Torpedoes fired
        armamentStats = stats.stats[ArmamentController.Armaments.AimedTorpedo];
        total = armamentStats.fired;
        hit = armamentStats.hit;
        ratio = (float)hit / (float)total;
        if (ratio == float.NaN)
            ratio = 1.0f;
        torpedo.ratio = ratio;

        torpedo.result = hit.ToString() + " / " + total.ToString();
        torpedo.countText.text = cannon.result;

        // CannonDamage taken
        total = stats.cannonHitsTaken;
        cannonDamage.countText.text = total.ToString();

        total = stats.torpedoHitsTaken;
        torpedoDamage.countText.text = total.ToString();


        ratio = 0.0f;
        // To prevent it dividing 0
        if (battleships.ratio > 0.0f)
            ratio += battleships.ratio;
        if(submarines.ratio > 0.0f)
            ratio += submarines.ratio;

        ratio = (battleships.ratio + submarines.ratio) / 2.0f;
        enemyEfficiencyTextMesh.text = (ratio * 100).ToString() + "%";

        ratio = (cannon.ratio + submarines.ratio) / 2.0f;
        accuracyTextMesh.text = (ratio * 100).ToString() + "%";

        ratio = (float)stats.damageTaken / 100.0f;
        ratio = Mathf.Min(ratio, 99.9f);
        damageTakenTextMesh.text = (100.0f - 99.9f).ToString() + "%";

    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(Input.GetKeyDown(KeyCode.T))
            {
                Initialize();
                resultScreen.SetActive(true);
            }
        }
    }
}
