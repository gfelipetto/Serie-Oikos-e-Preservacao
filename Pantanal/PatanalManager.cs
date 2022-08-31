using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PatanalManager : MonoBehaviour
{
    [SerializeField] private GameObject[] fishObj;
    [SerializeField] private GameObject[] snakeObj;
    [SerializeField] private GameObject[] alligatorObj;
    [SerializeField] private Image[] progressBarAnimals;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject finalScreen;
    [SerializeField] private ParticleSystem ps_pantanalClick;
    private Vector2 mousePosition;

    //0 - Peixe
    //1 - Cobra
    //2 - Jacaré
    private int[] _curentQuantityAnimalsInScene = new int[] { 1, 1, 1 };
    private int[] _timesToSpawnAnimals = new int[3];
    private int[] _limts = new int[] { 5, 3, 3 };

    private bool _gameOver = false;

    private bool _CoroutineFishLimit = false;
    private bool _CoroutineSnakeLimit = false;
    private bool _CoroutineAlligatorLimit = false;

    private void Start()
    {
        StartCoroutine(Timer());
        StartCoroutine(SpawnAnimalPerTime(0, 6.5f));
        StartCoroutine(SpawnAnimalPerTime(1, 12f));
        StartCoroutine(SpawnAnimalPerTime(2, 15f));
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.GetComponent<AnimalType>() != null)
            {
                if (hit.collider.GetComponent<SpriteRenderer>().enabled)
                {
                    DestroyAnimalByClick(hit.collider.gameObject);
                }
            }
        }
    }

    private void PutAnimalInScene(int index, GameObject[] animalObj)
    {
        int maxCount = 7;
        if (_curentQuantityAnimalsInScene[index] < maxCount)
        {
            bool go = false;
            while (!go)
            {
                int i = Random.Range(0, maxCount);
                if (!animalObj[i].GetComponent<SpriteRenderer>().enabled)
                {
                    animalObj[i].GetComponent<SpriteRenderer>().enabled = true;
                    _curentQuantityAnimalsInScene[index]++;
                    progressBarAnimals[index].fillAmount += 0.125f;
                    CheckQuantityAnimalToChanceTime();
                    go = true;
                }
            }
        }
    }
    private void TakeAnimalOffOfScene(int index, GameObject[] animalObj)
    {
        int maxCount = 7;
        if (_curentQuantityAnimalsInScene[index] < 0)
        {
            bool go = false;
            while (!go)
            {
                int i = Random.Range(0, maxCount);
                if (animalObj[i].GetComponent<SpriteRenderer>().enabled)
                {
                    animalObj[i].GetComponent<SpriteRenderer>().enabled = false;
                    _curentQuantityAnimalsInScene[index]--;
                    progressBarAnimals[index].fillAmount -= 0.125f;
                    CheckQuantityAnimalToChanceTime();
                    go = true;
                }
            }
        }
    }
    private void TakeAnimalOffOfSceneByClick(int index, GameObject animalObj)
    {
        animalObj.GetComponent<SpriteRenderer>().enabled = false;
        _curentQuantityAnimalsInScene[index]--;
        progressBarAnimals[index].fillAmount -= 0.125f;
        CheckQuantityAnimalToChanceTime();
    }
    private void CheckQuantityAnimalToChanceTime()
    {
        for (int i = 0; i < _curentQuantityAnimalsInScene.Length; i++)
        {
            if (_curentQuantityAnimalsInScene[i] == 0)
            {
                GameOver(false);
                return;
            }
        }
        CallCoroutinesSpawnAnimals();
    }
    private void DestroyAnimalByClick(GameObject animal)
    {
        
        switch (animal.GetComponent<AnimalType>().animalTypeVar)
        {
            case AnimalType.AnimalTypeEnum.Fish:
                TakeAnimalOffOfSceneByClick(0, animal);
                break;

            case AnimalType.AnimalTypeEnum.Snake:
                TakeAnimalOffOfSceneByClick(1, animal);
                break;

            case AnimalType.AnimalTypeEnum.Alligator:
                TakeAnimalOffOfSceneByClick(2, animal);
                break;

            default:
                break;
        }
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(ps_pantanalClick, new Vector3(mousePosition.x, mousePosition.y, 0f), Quaternion.identity);
    }
    private void GameOver(bool isWin)
    {
        StopAllCoroutines();
        if (isWin) finalScreen.transform.GetChild(1).gameObject.SetActive(true);
        else finalScreen.transform.GetChild(2).gameObject.SetActive(true);
        _gameOver = true;
        finalScreen.SetActive(true);
    }
    private void CallCoroutinesSpawnAnimals()
    {
        if (_curentQuantityAnimalsInScene[0] > _limts[0])
        {
            if (!_CoroutineFishLimit)
            {
                print("Iniciou o peixe");
                _CoroutineFishLimit = true;
                StartCoroutine(SpawnSnake(5));
            }
        }
        else
        {
            print("Terminou o peixe");
            _CoroutineFishLimit = false;
        }

        if (_curentQuantityAnimalsInScene[1] > _limts[1])
        {
            if (!_CoroutineSnakeLimit)
            {
                print("Iniciou a cobra");
                _CoroutineSnakeLimit = true;
                StartCoroutine(SpawnAlligator(10));
            }
        }
        else
        {
            print("Terminou o cobra");
            _CoroutineSnakeLimit = false;
        }


        if (_curentQuantityAnimalsInScene[2] > _limts[2])
        {
            if (!_CoroutineAlligatorLimit)
            {
                print("Iniciou a jacare");
                _CoroutineAlligatorLimit = true;
                StartCoroutine(SpawnFish(8));
            }
        }
        else
        {
            print("Terminou o jacare");
            _CoroutineAlligatorLimit = false;
        }
    }
    private IEnumerator SpawnAnimalPerTime(int indexAnimal, float time)
    {
        while (true)
        {
            switch (indexAnimal)
            {
                case 0:
                    PutAnimalInScene(indexAnimal, fishObj);
                    break;
                case 1:
                    PutAnimalInScene(indexAnimal, snakeObj);
                    break;
                case 2:
                    PutAnimalInScene(indexAnimal, alligatorObj);
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(time);
        }
    }
    private IEnumerator DestroyAnimalPerTime(int indexAnimal, float time)
    {
        while (true)
        {
            switch (indexAnimal)
            {
                case 0:
                    TakeAnimalOffOfScene(indexAnimal, fishObj);
                    break;
                case 1:
                    TakeAnimalOffOfScene(indexAnimal, snakeObj);
                    break;
                case 2:
                    TakeAnimalOffOfScene(indexAnimal, alligatorObj);
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(time);
        }
    }
    private IEnumerator Timer()
    {
        float time = 90f;
        while (time >= 0)
        {
            timerText.SetText($"Tempo: {time.ToString("F0")}");
            time -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (!_gameOver) GameOver(true);
    }


    private IEnumerator SpawnSnake(float time)
    {
        while (_CoroutineFishLimit)
        {
            PutAnimalInScene(1, snakeObj);
            yield return new WaitForSeconds(time);
        }
    }
    private IEnumerator SpawnAlligator(float time)
    {
        while (_CoroutineSnakeLimit)
        {
            PutAnimalInScene(2, alligatorObj);
            yield return new WaitForSeconds(time);
        }
    }
    private IEnumerator SpawnFish(float time)
    {
        while (_CoroutineAlligatorLimit)
        {
            TakeAnimalOffOfScene(0, fishObj);
            yield return new WaitForSeconds(time);
        }
    }
}
