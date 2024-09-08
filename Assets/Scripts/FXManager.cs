using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance { get; private set; }

    [SerializeField] private GameObject sfxPrefab, vfxPrefab;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public AudioSource PlaySFX(AudioClip clip, float volume, Vector3? pos = null)
    {
        AudioSource source = Instantiate(sfxPrefab, pos != null ? (Vector3)pos : Vector3.zero, Quaternion.identity).GetComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;

        if (pos != null)
            source.spatialBlend = 1;

        source.Play();

        Destroy(source.gameObject, clip.length);

        return source;
    }

    public AudioSource PlaySFX(string name, float volume, Vector3? pos = null)
    {
        AudioClip clip = Resources.Load<AudioClip>("SFX/" + name);
        AudioSource source = Instantiate(sfxPrefab, pos != null ? (Vector3)pos : Vector3.zero, Quaternion.identity).GetComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;

        if (pos != null)
            source.spatialBlend = 1;

        source.Play();

        Destroy(source.gameObject, clip.length);

        return source;
    }

    public void PlayVFX(string name, Vector3 position, float scale)
    {
        VisualEffectAsset vfxAsset = Resources.Load<VisualEffectAsset>("VFX/" + name);
        VisualEffect vfx = Instantiate(vfxPrefab, position, Quaternion.identity).GetComponent<VisualEffect>();

        vfx.visualEffectAsset = vfxAsset;
        vfx.transform.localScale = new Vector3(scale, scale, scale);

        if (vfx.HasFloat("_Duration"))
        {
            Destroy(vfx.gameObject, vfx.GetFloat("_Duration"));
        }
        else
        {
            Debug.LogWarning("VFX does not have a _Duration property, will not be destroyed!");
        }

    }
}