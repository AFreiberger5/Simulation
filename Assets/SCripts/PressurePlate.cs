using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PressurePlate : MonoBehaviour
{
    AudioSource audioClip;
    void Awake()
    {
        audioClip = GetComponent<AudioSource>();       
    }
    private void OnTriggerEnter(Collider other)
    {
        audioClip.Play();
    }
}
