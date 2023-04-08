using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : PlaceName
{
    public Enemy[] enemies;
    public Pot[] pots;
    public Door[] doors;
    public GameObject[] lights;
    public GameObject virtualCamera;
    public GameObject target;
    public Collider2D boundary;
    public bool camTrigger;

    public virtual void Start()
    {
        camTrigger = false;
        if (boundary.bounds.Contains(target.transform.position))
        {
            virtualCamera.SetActive(true);
            showText();
        }

        else
        {
            virtualCamera.SetActive(false);
            DeSpawnGameObjects();
        }

    }

    private void Update()
    {
        if (boundary.bounds.Contains(target.transform.position) && !camTrigger)
        {
            virtualCamera.SetActive(true);
        }
        else
            virtualCamera.SetActive(false);
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            for(int i = 0; i < enemies.Length; i++)
            {
                ChangeActivation(enemies[i], true);
            }
           for(int i = 0; i < pots.Length; i++)
            {
                ChangeActivation(pots[i], true);
            }
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].SetActive(true);
            }
            showText();
        }
    }
    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                ChangeActivation(enemies[i], false);
            }
            for (int i = 0; i < pots.Length; i++)
            {
                ChangeActivation(pots[i], false);
            }
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].SetActive(false);
            }
        }
    }

    public void ChangeActivation(Component component, bool activation)
    {
        component.gameObject.SetActive(activation);
    }

    protected void SpawnGameObjects()
    {
            for (int i = 0; i < enemies.Length; i++)
            {
                ChangeActivation(enemies[i], true);
            }
            for (int i = 0; i < pots.Length; i++)
            {
                ChangeActivation(pots[i], true);
            }
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].SetActive(true);
            }
    }

    protected void DeSpawnGameObjects()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            ChangeActivation(enemies[i], false);
        }
        for (int i = 0; i < pots.Length; i++)
        {
            ChangeActivation(pots[i], false);
        }
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }
    }
}
