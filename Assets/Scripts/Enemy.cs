using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    private NavMeshAgent agent;
    private Player player;
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private AudioSource audioSpeak;


    [Header("Detection")]
    [SerializeField] private BoxCollider detectionArea;
    private bool playerInRange = false;

    [SerializeField] private GameObject loseScreen;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float timeMax; // seg
    private float currentTime;
    private bool detected;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindFirstObjectByType<Player>();
        anim = GetComponent<Animator>();
        audioSpeak = GetComponent<AudioSource>();
        BoxCollider collider = detectionArea.GetComponent<BoxCollider>();
        detected = false;
        currentTime = timeMax;
    }

    // Update is called once per frame
    void Update()
    {
        anim.enabled = false;

        if (playerInRange)
        {
            detected = true;
            countdownText.gameObject.SetActive(true);
            //temp
            currentTime -= Time.deltaTime;
            UpdateCountdownText();

            if (currentTime <= 0)
            {
                audioSpeak.Stop();
                loseScreen.SetActive(true);
            }




             //Pers
            anim.enabled = true;
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            bool pathReady = !agent.pathPending && agent.pathStatus == NavMeshPathStatus.PathComplete; // error al principio frame y verificar si ya tiene la ruta calculada
                                                                                                        
            if (agent.remainingDistance <= agent.stoppingDistance && pathReady) // si la distancia es menor/igual  al rango de ataque -> ataca 
            {
                RotateEnemy();

                //anim attack
                agent.isStopped = true;
                audioSpeak.Stop();
                anim.SetBool("attacking", true);
                loseScreen.SetActive(true);
            }
        }
        else
        {
            agent.isStopped = true;
        }
        if (detected) {
            //temp
            currentTime -= Time.deltaTime;
            UpdateCountdownText();

            if (currentTime <= 0)
            {
                audioSpeak.Stop();
                countdownText.gameObject.SetActive(false);
                loseScreen.SetActive(true);
            }
        }

    }

    private void RotateEnemy() // enfocar obj
    {
        Vector3 dirObjet = (player.transform.position - transform.position).normalized;
        dirObjet.y = 0;
        Quaternion rotObj = Quaternion.LookRotation(dirObjet);
        transform.rotation = rotObj;
    }

    private void Grab()
    {
        audioSpeak.Stop();
        agent.isStopped = false;
        anim.SetBool("attacking", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(player.tag))
        {
            playerInRange = true;
            audioSpeak.Play(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(player.tag))
        {
            playerInRange = false;
            audioSpeak.Stop();
        }
    }

    private void UpdateCountdownText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
