using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;
    private int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Text winText;
    public Text controlText;
    public Text hardmodeText;

    public bool win;
    public bool hardmode;
    public GameObject starfieldclose;
    public GameObject starfielddistant;
    public GameObject background;
    private Vector3 startPosition;
    private BGScroller scroller;
    private BGScroller bgscroller;
    private ParticleSystem psClose;
    private ParticleSystem psFar;

    public AudioSource musicSource;
    public AudioClip music1;
    public AudioClip music2;
    public AudioClip music3;
    public AudioSource soundSource;
    public AudioClip timestopSound;

    private bool timeNorm;
    private bool gameOver;
    private bool restart;
    private int score;

    void Start()
    {
        hardmode = false;
        hazardCount = 10;
        gameOver = false;
        restart = false;
        win = false;
        timeNorm = true;
        restartText.text = "";
        gameOverText.text = "";
        winText.text = "";
        hardmodeText.text = "Press H for a Challenge.";
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());

        startPosition = transform.position;
        scroller = background.GetComponent<BGScroller>();
        psClose = starfieldclose.GetComponent<ParticleSystem>();
        psFar = starfielddistant.GetComponent<ParticleSystem>();

        soundSource.clip = timestopSound;
        musicSource.clip = music1;
        musicSource.Play();

    }
    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                SceneManager.LoadScene("Martinez_Raymond_DIG3480_FinalProject");
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            timeNorm = !timeNorm;
            if (timeNorm == false)
            {
                musicSource.Stop();
                soundSource.Play();
                score = score - 10;
                UpdateScore();
            }
            if (timeNorm == true)
            {
                soundSource.Stop();
                musicSource.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (hardmode == false)
            {
                hardmode = true;
                hazardCount = 20;
                score = score - 50;
                UpdateScore();
                hardmodeText.text = "";
            }
        }
    }
    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
                {
                    GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(hazard, spawnPosition, spawnRotation);
                    yield return new WaitForSeconds(spawnWait);
                }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "Press 'X' to Restart";
                restart = true;
                break;
            }
            if (win)
            {
                restartText.text = "Press 'X' to Restart";
                restart = true;
                break;
            }
        }
    }
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }
    void UpdateScore()
    {
        scoreText.text = "Points: " + score;
        if (score >= 100 && win == false)
        {
            VGameOver();
        }
    }
    public void GameOver()
    {

        if (win == false)
        {
            hardmodeText.text = "";
            gameOverText.text = "Game Over!";
            gameOver = true;
            musicSource.Stop();
            musicSource.clip = music3;
            musicSource.Play();
        }
    }
    public void VGameOver()
    {
        win = true;
        hardmodeText.text = "";
        winText.text = "Game created by Raymond Martinez";
        gameOver = true;
        restart = true;
        DestroyAll();
        musicSource.Stop();
        musicSource.clip = music2;
        musicSource.Play();

        scroller.scrollSpeed = -20f;
        var far = psFar.main;
        var close = psClose.main;
        close.simulationSpeed = 100f;
        far.simulationSpeed = 100f;
    }
    public void DestroyAll()
    {
        Destroy(GameObject.FindWithTag("Enemy"));
    }
    void FixedUpdate()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}