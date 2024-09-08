using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AirtableManager : MonoBehaviour
{

    public static AirtableManager Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void PublishMetrics(float totalDist, float avgLegAngle, float maxLegAngle, int raises, float runtime, int painValue, int comfortValue, int helpfulValue)
    {
        StartCoroutine(PublishData(totalDist, avgLegAngle, maxLegAngle, raises, runtime, painValue, comfortValue, helpfulValue));
    }

    private IEnumerator PublishData(float totalDist, float avgLegAngle, float maxLegAngle, int raises, float runtime, int painValue, int comfortValue, int helpfulValue)
    {

        Root packet = new Root();
        Record record = new Record();
        Fields field = new Fields();
        field.TotalDistanceTravelled = totalDist;
        field.PatientID = 0;
        field.AverageLegRaiseAngle = avgLegAngle;
        field.MaximumLegRaiseAngle = maxLegAngle;
        field.LegRaises = raises;
        field.Runtime = runtime;
        field.PainValue = painValue;
        field.ComfortValue = comfortValue;
        field.HelpfulValue = helpfulValue;
        record.fields = field;
        packet.records = new List<Record>() { record };

        string json = JsonUtility.ToJson(packet);
        
        UnityWebRequest request = new UnityWebRequest("https://api.airtable.com/v0/appCyAeJv69B8w8Dq/Data", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer patPgCcqIy2koTFH6.533ba6bbff439dc944cc22c5c371dd3554104fcc2bafd60e7c9ded6c0babe6ed");
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("Upload complete!");
        }

    }

}

[System.Serializable]
public class Root
{
    public List<Record> records;
}

[System.Serializable]
public class Record
{
    public Fields fields;
}

[System.Serializable]
public class Fields
{
    public long PatientID;

    public long LegRaises;

    public double MaximumLegRaiseAngle;

    public double AverageLegRaiseAngle;

    public double TotalDistanceTravelled;

    public double Runtime;
    public long PainValue;
    public long ComfortValue;
    public long HelpfulValue;

}
