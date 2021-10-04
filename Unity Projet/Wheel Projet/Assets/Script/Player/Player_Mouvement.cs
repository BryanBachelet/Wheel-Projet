using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Mouvement : MonoBehaviour
{
    public Controls playerInput;
    public float speed = 10.0f;
    public float maxSpeed = 10.0f;
    public LayerMask layerMask;

    private Vector2 p_InputDeplacement;
    private Rigidbody rigidbodyPlayer;


    // Start is called before the first frame update
    void Start()
    {
        // Récupération des inputs 
        playerInput = new Controls();
        rigidbodyPlayer = GetComponent<Rigidbody>();
        playerInput.Player.Mouvements.performed += ctx => p_InputDeplacement = ctx.ReadValue<Vector2>();
        playerInput.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        float playerOrientation = -OrientationPlayer();
        float currentRot = transform.eulerAngles.x ;
        float angleX = playerOrientation + currentRot;
     
        Debug.Log("Player X rot = " + currentRot + " Add angle = " + playerOrientation);



        transform.rotation *= Quaternion.AngleAxis(playerOrientation, transform.right) ; 
        DeplacementPlayer(p_InputDeplacement, speed);
     
    }

    private float OrientationPlayer()
    {
        Ray ray = new Ray(transform.position - transform.up*0.5f, transform.forward);
        RaycastHit hit = new RaycastHit();
        float angle = 0;
        Debug.DrawRay(ray.origin,ray.direction *1f,Color.red);
        if (Physics.Raycast(ray, out hit, 1.5f, layerMask))
        {
            Vector3 hitPos = hit.normal;
             angle = Vector3.Angle(transform.up, hitPos);
        }
        Debug.Log(angle);
        return  angle;
    }

    private void DeplacementPlayer(Vector2 input, float speed)
    {
        Vector3 dirInput = new Vector3(input.x, 0, input.y);
        rigidbodyPlayer.AddForce(transform.forward * input.y * speed, ForceMode.Acceleration);
        rigidbodyPlayer.velocity = Vector3.ClampMagnitude(rigidbodyPlayer.velocity, maxSpeed);
       
    }
}
