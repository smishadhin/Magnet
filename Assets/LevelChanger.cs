using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public GameObject fade;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = fade.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("playerbody"));
        {
            onFinishGame();
        }
    }

    void onFinishGame()
    {
        animator.SetTrigger("FadeOut");
    }
    
    
}