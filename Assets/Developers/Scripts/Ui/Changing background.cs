using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Changingbackground : MonoBehaviour
{
    [SerializeField] private GameObject[] backGrounds;

    private void Start()
    {
        StartCoroutine(ChoosePicture());
    }

    private IEnumerator ChoosePicture()
    {
        int nextPicture = Random.Range(0, backGrounds.Length);
        for (int i = 0; i < backGrounds.Length; i++)
        {
            backGrounds[i].SetActive(false);
        }
        backGrounds[nextPicture].SetActive(true);
        yield return new WaitForSeconds(0.9f);
        StartCoroutine(ChoosePicture());
    }
}
