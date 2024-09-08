using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public bool GameStarted { get; private set; }

    public int enemiesKilled;

    public bool Work;

    [Header("Connection")]
    [SerializeField] private Transform connectionMenu;
    [SerializeField] private TextAffector connectionTitle;
    [SerializeField] private TextAffector connectedDevices;
    [SerializeField] private TextAffector pressStart;
    [Header("Calibration")]
    [SerializeField] private Transform calibrationMenu;
    [SerializeField] private TextAffector textAffectorTitle;
    [SerializeField] private TextAffector calibrationMessage;
    [SerializeField] private TextAffector pressStart2;
    [Header("Metrics")]
    [SerializeField] private Transform metricsMenu;
    [SerializeField] private TMP_Text avgExtension, maxExtension, extensions, runtimeText, enemies, distanceTrav;
    [Header("SurveyMenu")]
    [SerializeField] private SurveyMenu surveyMenu;
    [Header("Garage")]
    [SerializeField] private GarageMenu garageMenu;

    private float runtime;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(GameLoop());
    }

    void Update()
    {
        if(GameStarted)
        runtime += Time.deltaTime;
    }

    public IEnumerator GameLoop()
    {
        yield return new WaitForSeconds(1f);
        connectionTitle.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.25f);
        connectedDevices.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.25f);
        pressStart.gameObject.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) == true);


        calibrationMenu.gameObject.SetActive(true);
        textAffectorTitle.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.25f);
        calibrationMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.25f);
        pressStart2.gameObject.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) == true);
        calibrationMenu.gameObject.SetActive(false);
        connectionMenu.gameObject.SetActive(false);
        garageMenu.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        yield return new WaitUntil(() => Work == true);
        garageMenu.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        // Game Start
        GameStarted = true;
        Player.Instance.gameObject.SetActive(true);
        EnemyManager.Instance.StartNewWave();

        yield return new WaitUntil(() => EnemyManager.Instance.GameComplete);
        distanceTrav.SetText(MetricManager.Instance.GetDistanceTravelled().ToString("0.0") + " meters");
        avgExtension.SetText(MetricManager.Instance.GetAvgLegAngle().ToString("0.0") + " degrees");
        maxExtension.SetText(MetricManager.Instance.GetMaxLegAngle().ToString("0.0") + " degrees");
        runtimeText.SetText(runtime.ToString("0.0") + " seconds");
        enemies.SetText(enemiesKilled + " enemies");
        extensions.SetText(MetricManager.Instance.GetRaises().ToString());

        foreach(Enemy enemy in FindObjectsOfType<Enemy>())
        {
            Destroy(enemy.gameObject);
        }

        Player.Instance.gameObject.SetActive(false);

        metricsMenu.gameObject.SetActive(true);
        MetricManager.Instance.PopulateLine();
        GameStarted = false;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) == true);

        yield return new WaitForSeconds(0.5f);

        metricsMenu.gameObject.SetActive(false);
        surveyMenu.gameObject.SetActive(true);


        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) == true);

        yield return new WaitForSeconds(0.5f);

        AirtableManager.Instance.PublishMetrics(MetricManager.Instance.GetDistanceTravelled(), MetricManager.Instance.GetAvgLegAngle(), MetricManager.Instance.GetMaxLegAngle(), MetricManager.Instance.GetRaises(), runtime, surveyMenu.GetPainValue(), surveyMenu.GetComfortValue(), surveyMenu.GetHelpfulValue());


    }

}
