using System.Collections;
using UnityEngine;

public class LightningSmite : MonoBehaviour
{
    private bool isPlayerFriendly = true;
    [SerializeField] private float damage;
    [SerializeField] GameObject[] lightning;
    int index = 0;
    [SerializeField] float timeBetweenStrikes = 0.5f;
    float timer = 0;
    public void SetAllegiance(bool playerFriendly)
    {
        isPlayerFriendly = playerFriendly;

        foreach (var lightning in lightning)
        {
            lightning.GetComponent<Lightning>().GetLightningVariables(isPlayerFriendly, damage);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenStrikes)
        {
            index++;
            if (index >= lightning.Length)
                index = 0;

            lightning[index].SetActive(true);
            StartCoroutine(DespawnLightning(index));
            timer = 0;
        }
    }

    IEnumerator DespawnLightning(int index)
    {
        yield return new WaitForSeconds(0.5f);
        lightning[index].SetActive(false);
    }
}
