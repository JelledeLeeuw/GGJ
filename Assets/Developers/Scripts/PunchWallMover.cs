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
            yield return StartCoroutine(MoveObjectToPosition(randomObject, Vector3.left * 5f));

            // Beweeg het object weer naar 0
            yield return StartCoroutine(MoveObjectToPosition(randomObject, Vector3.right * 5f));

            // Nadat het object is bewogen, slaan we dit object op als het laatst bewogen object
            lastMovedObject = randomObject;

            // Wacht voor de opgegeven interval voordat de volgende beweging plaatsvindt
            yield return new WaitForSeconds(moveInterval);
        }
    }

    // Numerator om de objects naar de -5 op de x as te brengen
    private IEnumerator MoveObjectToPosition(GameObject obj, Vector3 localOffset)
    {
        Vector3 targetPosition = obj.transform.position + obj.transform.TransformDirection(localOffset);

        while (Vector3.Distance(obj.transform.position, targetPosition) > 0.1f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, moveSpeed * Time.deltaTime);
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
