using System;
using UnityEngine;

public class MoneyPoint : MonoBehaviour
{
    public event Action<MoneyPoint> OnCollected;

    [Header("Visuals")]
    public GameObject collectionEffect;
    public float destroyDelay = 0.5f;

    [Header("Money Value")]
    public int moneyValue = 1;

    private MoneyCount _moneySystem;
    private bool _wasCollected = false;

    private void Start()
    {
        //_moneySystem = FindObjectOfType<MoneyCount>();
        //if (_moneySystem == null)
        //{
        //    Debug.LogError("MoneyCount не найден на сцене!");
        //}
    }

    public void SetSelected(bool state)
    {
        if (state && !_wasCollected)
        {
            _wasCollected = true;
            OnCollected?.Invoke(this);
            CollectMoney();
        }
    }

    private void CollectMoney()
    {
        _moneySystem = FindObjectOfType<MoneyCount>();

        if (_moneySystem != null)
        {
            _moneySystem.AddMoney(moneyValue);
        }

        if (collectionEffect != null)
        {
            Instantiate(collectionEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, destroyDelay);
    }

    private void OnMouseDown()
    {
        SetSelected(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetSelected(true);
        }
    }
}
