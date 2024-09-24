using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] float delayInSequence = 1.0f;

    MeshRenderer[] meshRenderers;

    bool isTransitioning = false;

    //private void OnCollisionEnter(Collision collision)    //----- Not needed for the current project,  see table in Unity's collider doc
    //{
    //    Debug.Log(this.name + "--Collided with--" + collision.gameObject.name);
    //}

    private void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();       // To access the MeshRenderer of the children objects
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (isTransitioning)
        {
            return;
        }
        StartCrashSequence();
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        GameObject.FindWithTag("Laser").SetActive(false);
        GameObject.FindWithTag("Laser").SetActive(false);
        explosionParticles.Play();
        foreach (MeshRenderer meshRenderer in meshRenderers)         // To disable the meshRenderer of the child objects
        {
            meshRenderer.enabled = false;
        }
        GetComponent<PlayerControls>().enabled = false;
        Invoke("ReloadLevel", delayInSequence);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
