using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{

    public static NotificationManager Instance { get; private set; }

    [SerializeField] private Transform notificationParent;
    [SerializeField] private GameObject notificationPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void CreateNotification(string title, string message, float duration)
    {
        Notification not = Instantiate(notificationPrefab, notificationParent).GetComponent<Notification>();
        not.SetData(title, message, duration);
    }

}
