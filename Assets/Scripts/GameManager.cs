using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public bool GameStarted { get; private set; }

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

        // Game Start
        GameStarted = true;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R) == true);
        metricsMenu.gameObject.SetActive(true);
        MetricManager.Instance.PopulateLine();
        GameStarted = false;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) == true);



    }

}
