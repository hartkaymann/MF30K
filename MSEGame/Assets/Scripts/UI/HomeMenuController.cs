using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeMenuController : MonoBehaviour
{
    [SerializeField] private Transform playerParent;
    [SerializeField] private Transform monsterParent;

    [SerializeField] private List<GameObject> playerPrefabs;
    [SerializeField] private List<GameObject> monsterPrefabs;

    private GameObject currentPlayer;
    private GameObject currentMonster;

    [SerializeField] private Transform[] grounds;
    [SerializeField] private float speed = .0f;

    [SerializeField] private GameObject gameScene;

    void Start()
    {
        StartCoroutine(SwapPlayer());
        StartCoroutine(SwapMonster());
    }

    private void Update()
    {
        foreach (Transform ground in grounds)
        {
            ground.Translate(speed * Time.deltaTime * Vector3.left);

            if (ground.position.x < -10)
                ground.position = new Vector3(20f, -.4f, 0f);
        }
    }

    private IEnumerator SwapPlayer()
    {
        if (currentPlayer != null)
            Destroy(currentPlayer);

        int idx = Random.Range(0, playerPrefabs.Count);
        currentPlayer = Instantiate(playerPrefabs[idx], playerParent);
        if (currentPlayer.TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.StartRunning();
        }
        yield return Timer(Random.Range(3, 8));

        yield return SwapPlayer();
    }

    private IEnumerator SwapMonster()
    {
        if (currentMonster != null)
        {
            if (currentMonster.TryGetComponent<NpcController>(out var npcCtrl))
            {
                npcCtrl.Die();
                yield return new WaitForSeconds(3);
            }
            Destroy(currentMonster);
        }

        int idx = Random.Range(0, monsterPrefabs.Count);
        currentMonster = Instantiate(monsterPrefabs[idx], monsterParent);

        yield return Timer(Random.Range(3, 8));
        yield return SwapMonster();
    }


    private IEnumerator Timer(float duration)
    {
        yield return new WaitForSeconds(duration);
    }

    private void OnEnable()
    {
        gameScene.SetActive(true);
    }

    private void OnDisable()
    {
        if (gameScene != null)
        {
            gameScene.SetActive(false);
        }
    }
}
