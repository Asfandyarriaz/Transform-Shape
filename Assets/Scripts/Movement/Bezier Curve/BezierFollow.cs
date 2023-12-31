using System.Collections;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
    [SerializeField] private Transform[] routes;

    private int routeToGo;

    private float tParam;

    private Vector3 objectPosition;

    [SerializeField] public float speedModifier;

    public bool startBezierCurve;
    public bool bezierRunning;
    public bool bezierCompleted;

    // Start is called before the first frame update
    void Start()
    {
        bezierRunning = false;
        routeToGo = 0;
        tParam = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (startBezierCurve)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    private IEnumerator GoByTheRoute(int routeNum)
    {
        startBezierCurve = false;
        bezierRunning = true;
        bezierCompleted = false;

        Vector3 p0 = routes[routeNum].GetChild(0).position;
        Vector3 p1 = routes[routeNum].GetChild(1).position;
        Vector3 p2 = routes[routeNum].GetChild(2).position;
        Vector3 p3 = routes[routeNum].GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;

            transform.position = objectPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0;
        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
        }
        bezierRunning = false;
        bezierCompleted = true;
    }
}
