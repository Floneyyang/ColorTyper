using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BallSpawner : MonoBehaviour
{
    public GameObject deathAnimation;
    private ParticleSystem deathParticle;
    private GameObject ball;
    private GameObject BallPrefab;

    [HideInInspector]
    public Ball ballData;

    private Transform player;
    private Rigidbody ballRd;
    private MeshRenderer ballMesh;
    private bool hasBeenTriggered = false;
    private Material[] ballMats;

    private bool shoot = true;
    //Subscription
    Subscription<CorrectColorSelectedEvent> CorrectColorSelectedEventSubscription;

    private void Awake()
    {
        CorrectColorSelectedEventSubscription = _EventBus.Subscribe<CorrectColorSelectedEvent>(_ChangeBallMovement);
    }

    void OnDestroy()
    {
        _EventBus.Unsubscribe<CorrectColorSelectedEvent>(CorrectColorSelectedEventSubscription);
    }

    private void Start()
    {
        Ball ballData = new Ball();
        ballData.scale = 1;
        ballData.colors = new int[] { 0, 1, 2, 3, 4 };
        ballData.initialVelocity = 10f;

        string json = JsonUtility.ToJson(ballData);
        File.WriteAllText(Application.persistentDataPath + "/BallData.json", json);
        

        player = GameObject.FindWithTag("Player").transform;
        deathParticle = deathAnimation.GetComponentInChildren<ParticleSystem>();
        LoadBallDataFromJson();

        InstantiateBalls();
        LoadBallMats();

    }

    private void Update()
    {
        if (shoot)
        {
            hasBeenTriggered = false;
            ShootBall();
            shoot = false;
        }
    }

    void LoadBallDataFromJson()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/BallData.json");
        ballData = JsonUtility.FromJson<Ball>(json);
    }

    void InstantiateBalls()
    {
        ball = new GameObject();
        BallPrefab = LoadGameobject("Sphere");
        ball = Instantiate(BallPrefab, transform.position, Quaternion.identity);
        ball.transform.localScale = new Vector3(ballData.scale, ballData.scale, ballData.scale);
        ballRd = ball.GetComponent<Rigidbody>();
        ballMesh = ball.GetComponent<MeshRenderer>();
        ball.SetActive(false);

    }

    void LoadBallMats()
    {
        ballMats = new Material[ballData.colors.Length];
        ballMats = LoadMaterials("BallMats");

        Color[] colors = new Color[ballMats.Length];
        for(int i = 0; i < ballMats.Length; i++)
        {
            colors[i] = ballMats[i].color;
        }

        AssignUIColorEvent e = new AssignUIColorEvent(colors);
        _EventBus.Publish<AssignUIColorEvent>(e);
    }

    Material[] LoadMaterials(string dir)
    {
        return Resources.LoadAll<Material>(dir);
    }

    GameObject LoadGameobject(string dir)
    {
        return Resources.Load<GameObject>(dir);
    }

    void ShootBall()
    {
        if (!hasBeenTriggered)
        {
            SelectColor((int)Random.Range(0f, ballData.colors.Length-0.1f));
            BallTrajectory(player.position);
            hasBeenTriggered = true;
        }
    }

    private void SelectColor(int color)
    {
        ballMesh.material = ballMats[color];
        UpdateButtonColorEvent e = new UpdateButtonColorEvent(color);
        _EventBus.Publish<UpdateButtonColorEvent>(e);
    }

    //Reference: https://en.wikipedia.org/wiki/Trajectory
    private void BallTrajectory(Vector3 targetLocation)
    {
        float range = CalculateRange(Mathf.PI / 4);
        Vector3 startPos = CalculateStartPosition(range, Mathf.PI/4); //45 degree
        ball.transform.position = transform.position;
        ball.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        ball.transform.position = startPos;
        ball.SetActive(true);
        Vector3 direction = (targetLocation - startPos).normalized;
        Vector3 elevation = CalculateAngleOfElevation(range, ballData.initialVelocity);
        float directionAngle = AngleBetweenAboutAxis(ball.transform.forward, direction, ball.transform.up);
        Vector3 velocity = Quaternion.AngleAxis(directionAngle, ball.transform.up) * elevation * ballData.initialVelocity;
        ballRd.velocity = Vector3.zero;
        ballRd.angularVelocity = Vector3.zero;
        ballRd.AddForce(velocity, ForceMode.VelocityChange);
        //Debug
        Debug.Log("range: " + range);
        Debug.Log("starPos: "+ startPos);
        Debug.Log("direction: " + direction);
        Debug.Log("elevation: " + elevation);
        Debug.Log("directionAngle: " + directionAngle);
        Debug.Log("velocity: " + velocity);

    }

    private float CalculateRange(float angle)
    {
        return ballData.initialVelocity * ballData.initialVelocity * Mathf.Sin(2 * angle) / Physics.gravity.magnitude;
    }

    //given an angle calculate the greatest distance the object travels
    private Vector3 CalculateStartPosition(float range, float angle)
    {
        float height = ballData.initialVelocity * ballData.initialVelocity * Mathf.Sin(angle) * Mathf.Sin(angle) / Physics.gravity.magnitude / 2;
        return new Vector3(Random.Range(-range, range), 0f, Random.Range(0, range));
    }

    //Reference: https://stackoverflow.com/questions/30290262/how-to-throw-a-ball-to-a-specific-point-on-plane
    //Changed the distance calculation to a range calculation
    private Vector3 CalculateAngleOfElevation(float range, float initialVelocity)
    {
        float angle = 0.5f * Mathf.Asin((Physics.gravity.magnitude * range) / (initialVelocity * initialVelocity)) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, ball.transform.right) * ball.transform.up;
    }

    public static float AngleBetweenAboutAxis(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    void _ChangeBallMovement(CorrectColorSelectedEvent e)
    {
        ParticleSystem.MainModule settings = deathParticle.main;
        settings.startColor = new ParticleSystem.MinMaxGradient(ballMesh.material.color);
        deathAnimation.transform.position = ball.transform.position;
        deathAnimation.SetActive(true);
        deathAnimation.GetComponent<MeshRenderer>().material = ballMesh.material;
        deathAnimation.GetComponent<Rigidbody>().velocity = ballRd.velocity*2f;
        deathParticle.Play();
        shoot = true;
    }

}
