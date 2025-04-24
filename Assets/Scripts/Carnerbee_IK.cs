using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnerbee_IK : MonoBehaviour
{
    [SerializeField] private bool ikActive = false;
    [SerializeField] private float[] defaultRoation;

    protected Animator animator;

    private Vector3[] trakingCoordinate = new Vector3[17];
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTrakingCoordinate(Vector3[] trakingCoordinate)
    {
        this.trakingCoordinate = trakingCoordinate;
    }

    private void OnAnimatorIK()
    {
        if (animator)
        {
            if (ikActive) { 
            
            }
            else
            {
            }
        }
    }
}
