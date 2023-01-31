using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialController : MonoBehaviour
{
    [System.Serializable]
    public class TutorialEntity
    {
        public List<string> textList;
        public float timeToPlay;

        string cur_text;
        int cur_ind = 0;

        [HideInInspector]
        public bool ended = false;

        public bool timed = true;

        [HideInInspector]
        public TextMeshProUGUI text;

        public void Initialize(TextMeshProUGUI txt)
        {
            text = txt;
            cur_text = textList[cur_ind];
            text.text = cur_text;
            //txt.gameObject.SetActive(true);
        }
        public void Execute()
        {
            if (Input.GetMouseButtonDown(0))
                NextLine();
        }
        public void NextLine()
        {
            ++cur_ind;
            if(cur_ind >= textList.Count)
            {
                ended = true;
                return;
            }

            cur_text = textList[cur_ind];
            text.text = cur_text;
        }


    }

    [SerializeField]
    GameObject panel;

    [SerializeField]
    List<TutorialEntity>entities = new List<TutorialEntity>();

    GameObject tutorialDestroyer;
    GameObject tutorialSubmarine;

    bool defeatedDestroyer = false;

    bool beginSubmarineSequence = false;
    bool defeatedSubmarine = false;


    Timer timer = new Timer();
    int cur_ind = 0;
    bool isPlaying = true;

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {


        timer.Execute();
        cur_ind = Mathf.Clamp(cur_ind, 0, entities.Count - 1);

        if (timer.OnPassOnce(entities[cur_ind].timeToPlay) && isPlaying && entities[cur_ind].timed)
        {
            if (!entities[cur_ind].ended)
            {
                Pause();
                isPlaying = false;
                panel.SetActive(!isPlaying);
                entities[cur_ind].Initialize(panel.GetComponentInChildren<TextMeshProUGUI>());
            }
        }

        if (!isPlaying)
        {
            if (!entities[cur_ind].ended)
            {
                entities[cur_ind].Execute();

                if (entities[cur_ind].ended)
                {
                    Resume();
                    isPlaying = true;
                    panel.SetActive(!isPlaying);
                    ++cur_ind;
                }
            }
        }



        // Timer enemy spawning
        if (timer.OnPassOnce(9.0f) && !defeatedDestroyer)
            tutorialDestroyer = EnemySpawner.Get().Spawn(EnemySpawner.EnemyType.Destroyer, new Vector3(500, 0, 0));

        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        // Prompt for defeating destroyer
        if(tutorialDestroyer != null && (player.transform.position - tutorialDestroyer.transform.position).magnitude < 400.0f && !defeatedDestroyer)
        {
            DestroyerStateMachine dsm = (tutorialDestroyer.GetComponent<EnemyDestroyerController>().GetStateMachine() as DestroyerStateMachine);
            if(dsm.GetStateEnum() == (int)DestroyerStateMachine.StateEnum.Sink)
            {
                // Not enough time to make this interactive 
                // so force it to play third sequence when you sink the tutorial enemy
                BeginSequence(2);
                defeatedDestroyer = true;
                tutorialSubmarine = EnemySpawner.Get().Spawn(EnemySpawner.EnemyType.Submarine, new Vector3(-500, 0, 500));
                timer.Reset();
            }
        }

        // Prompt for starting submarine sequence
        {
            if (!beginSubmarineSequence)
            {
                GameObject[] torps = GameObject.FindGameObjectsWithTag("Torpedo");
                foreach (GameObject torp in torps)
                {
                    AimedTorpedoScript tp = torp.GetComponent<AimedTorpedoScript>();
                    if (tp.source != player.gameObject && Vector3.Distance(tp.transform.position, player.position) < 400.0f)
                    {
                        beginSubmarineSequence = true;
                        BeginSequence(3);
                    }
                }
            }
        }
        

    }

    void Pause()
    {
        Time.timeScale = 0.0f;
    }
    void Resume()
    {
        Time.timeScale = 1.0f;
    }

    void BeginSequence(int ind)
    {
        Pause();
        isPlaying = false;
        panel.SetActive(!isPlaying);
        entities[ind].Initialize(panel.GetComponentInChildren<TextMeshProUGUI>());

    }
}
