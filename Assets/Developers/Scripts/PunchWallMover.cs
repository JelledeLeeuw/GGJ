using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchWallMover : MonoBehaviour
{
    // Een lijst van GameObjects die je wilt laten bewegen, ofterwijl van de punching dingen
    public List<GameObject> objectsToMove;

    // Snelheid voor het bewegen
    public float moveSpeed = 5f;

    // Tijd tussen nieuwe beweging
    public float moveInterval = 1f;

    // Houdt bij welk object als laatste is bewogen
    private GameObject lastMovedObject = null;

    private void Start()
    {
        StartCoroutine(MoveObjectsBackAndForth());
    }

    private IEnumerator MoveObjectsBackAndForth()
    {
        while (true)
        {
            // Kies een willekeurig object, maar niet het laatst bewogen object
            GameObject randomObject = GetRandomObject();

            // Beweeg het object naar -5
            yield return StartCoroutine(MoveObjectToPosition(randomObject, new Vector3(randomObject.transform.position.x - 5f, randomObject.transform.position.y, randomObject.transform.position.z)));

            // Beweeg het object weer naar 0
            yield return StartCoroutine(MoveObjectToPosition(randomObject, new Vector3(randomObject.transform.position.x + 5f, randomObject.transform.position.y, randomObject.transform.position.z)));

            // Nadat het object is bewogen, slaan we dit object op als het laatst bewogen object
            lastMovedObject = randomObject;

            // Wacht voor de opgegeven interval voordat de volgende beweging plaatsvindt
            yield return new WaitForSeconds(moveInterval);
        }
    }

    // Numerator om de objects naar de -5 op de x as te brengen
    private IEnumerator MoveObjectToPosition(GameObject obj, Vector3 targetX)
    {
        while (Mathf.Abs(obj.transform.position.x - targetX.x) > 0.1f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position,
                                                        targetX,
                                                         moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Methode om een willekeurig object te kiezen, maar niet het laatst bewogen object
    private GameObject GetRandomObject()
    {
        GameObject randomObject;

        do
        {
            randomObject = objectsToMove[Random.Range(0, objectsToMove.Count)];
        } while (randomObject == lastMovedObject);

        return randomObject;
    }


}
