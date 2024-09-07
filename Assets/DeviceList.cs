using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceList : MonoBehaviour
{

    [SerializeField] private Transform deviceContainer;
    [SerializeField] private GameObject containerPrefab;

    private Dictionary<int, DeviceContainer> containers = new Dictionary<int, DeviceContainer>();

    // Start is called before the first frame update
    void Start()
    {
        HardwareManager.Instance.TrackerConnected.AddListener((hardware) => {
            DeviceContainer container = Instantiate(containerPrefab, deviceContainer).GetComponent<DeviceContainer>();
            container.SetHardware(hardware);
            containers[hardware.ID] = container;
        });

        HardwareManager.Instance.TrackerDisconnected.AddListener((hardware) => {
            if(containers.ContainsKey(hardware.ID))
            {
                Destroy(containers[hardware.ID].gameObject);
                containers.Remove(hardware.ID);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
