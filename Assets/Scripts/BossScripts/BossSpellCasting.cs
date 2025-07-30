using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossSpellCasting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject spellPrefab;
    public int spellCount;
    public List<GameObject> spellList = new List<GameObject>();



    public void Shoot()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        firePoint.position = new Vector2(player.transform.position.x, player.transform.position.y + 2.4f);
        GameObject spellInstanciate;
        spellInstanciate = Instantiate(spellPrefab, firePoint.position, firePoint.rotation);
        spellList.Add(spellInstanciate);
        StartCoroutine(WaitForShoot(spellInstanciate));
    }
    public IEnumerator WaitForShoot(GameObject spellObject)
    {
        yield return new WaitForSeconds(1f);
        Destroy(spellObject);
        spellCount = 0;
    }
}
