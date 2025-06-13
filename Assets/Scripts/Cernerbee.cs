using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Cernerbee : MonoBehaviour
{
    [Header("amarture")]
    /*
     * 0. head
     * 1. left upper arm
     * 2. left lower arm
     * 3. right upper arm
     * 4. right lower arm
     * 5. left upper leg
     * 6. left lower leg
     * 7. right upper leg
     * 8. right lower leg
     * 9. spine
     * 10. hip
     */
    [SerializeField] private Transform[] AmatureBone;

    [SerializeField] private Vector3[] defaultRotation;

    public GameObject PelvisDebug;
    public GameObject ShoulderDebug;

    // 0. 6, 7, 8 => 코
    // 1. 9, 10, 11 => 왼 눈
    // 2. 12, 13, 14 => 오 눈
    // 3. 15, 16, 17 => 왼 귀
    // 4. 18, 19, 18 => 오 귀
    // 5. 21. 22, 23 => 왼 어
    // 6. 24. 25, 26 => 오 어
    // 7. 27, 28, 29 => 왼 팔꿈
    // 8. 30, 31, 32 => 오 팔꿈
    // 9. 33, 34, 35 => 왼 손
    // 10. 36, 37, 38 => 오 손
    // 11. 39, 40, 41 => 왼 골
    // 12. 42, 43, 44 => 오 골
    // 13. 45, 46, 47 => 왼 무
    // 14. 48, 49, 50 => 오 무
    // 15. 51, 52, 53 => 왼 발
    // 16. 54, 55, 56 => 오 발
    private Vector3[] trakingCoordinate = new Vector3[17];
    private Vector3[] firstTrakingCoordinate = new Vector3[17];

    private Vector3[] prevRotation = new Vector3[10];

    private bool traking;

    private Vector3 defaultHipPos;
    private Vector3 defaultPelvis = new Vector3(-312, -380, 0);

    private Animator animator;
   
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0;i < defaultRotation.Length; i++)
        {
            AmatureBone[i].localEulerAngles = defaultRotation[i];
        }
        for (int i = 0; i < prevRotation.Length; i++)
        {
            prevRotation[i] = new Vector3(-1,-1,-1);
        }
        traking = false;
        
        defaultHipPos = AmatureBone[10].transform.position;
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (!animator.GetBool(Manager.IS_TRAKING)) return;
        if (!isTraking()) return;
        if (trakingCoordinate == null || trakingCoordinate.Length == 0) return;
        if(firstTrakingCoordinate == null)
        {
            for (int i = 0; i < trakingCoordinate.Length; i++)
            {
                if (trakingCoordinate[i] == new Vector3(-1, -1, -1)) break;
                if (i == trakingCoordinate.Length - 1)
                {
                    firstTrakingCoordinate = trakingCoordinate;
                }
            }
        }

        Vector3 shoulder = calcCenterVector(trakingCoordinate[5], trakingCoordinate[6]);
        Vector3 pelvis = calcCenterVector(trakingCoordinate[11], trakingCoordinate[12]);
        
        Vector3 movement = defaultHipPos +(pelvis - defaultPelvis);
        movement.y -= 70f;
        //Debug.Log(defaultHipPos + "," +movement);
        //movement.y = movement.y <  ?  : movement.y;
        movement.x *= -1;

        AmatureBone[10].transform.position = movement;


        // 왼 위팔
        calcAngleAndSetRotation(7, 5, 1, (angle, currAngle, direction) => { 
            Vector3 result = new Vector3(currAngle.x, currAngle.y, 0);
            
            if (direction.x < 0)
            {
                if (direction.y < 0)
                {
                    // 270 ~ 360
                    if (angle < 270) result.z = angle + 180;
                    else result.z = angle;

                    if (angle < 320) result.z = 320f;
                }
                else
                {
                    // 0 ~ 90 
                    if (angle > 90) result.z = angle - 180;
                    else result.z = angle;
                }
            }
            else
            {
                if (direction.y < 0)
                {
                    // 180 ~ 270
                    if (angle < 180) result.z = angle + 180;
                    else result.z = angle;
                }
                else
                {
                    // 90 ~ 180
                    if (angle > 180) result.z = angle - 180;
                    else result.z = angle;
                }
            }
            
            result.z *= -1 ;
            return result;
        });
        // 오른 위팔
        calcAngleAndSetRotation(8, 6, 3, (angle, currAngle, direction) => {
            
            Vector3 result = new Vector3(currAngle.x, currAngle.y, 0);
            //Debug.Log(angle + ", " + direction);
            if (direction.x < 0)
            {
                if (direction.y < 0)
                {
                    // 90 ~ 180
                    if (angle > 180) result.z = angle - 180;
                    else result.z = angle;
                }
                else
                {
                    // 180 ~ 270 
                    if (angle < 180) result.z = angle + 180;
                    else result.z = angle;
                }
            }
            else
            {
                if (direction.y < 0)
                {
                    // 0 ~ 90
                    if (angle > 90) result.z = angle - 180;
                    else result.z = angle;

                    if (angle > 40) result.z = 40f;
                }
                else
                {
                    // 270 ~ 360
                    if (angle < 270) result.z = angle + 180;
                    else result.z = angle;
                }
            }
            result.z *= -1 ;
            return result;
        });
        // 왼 팔뚝
        float leftarm = calcAngleAndSetRotation(9, 7, 2, (angle, currAngle, direction) => {
            //Debug.Log("angle : "+ Mathf.Ceil(angle) + ", direction : " + direction);
            Vector3 result = new Vector3(currAngle.x, currAngle.y, 0);
            if (direction.x < 0)
            {
                if(direction.y < 0)
                {
                    // 270 ~ 360
                    if (angle < 270) result.z = angle + 180;
                    else result.z = angle;

                    if (angle < 340) result.z = 340f;
                }
                else
                {
                    // 0 ~ 90 
                    if (angle > 90) result.z = angle - 180;
                    else result.z = angle;
                }
            }
            else
            {
                if (direction.y < 0)
                {
                    // 180 ~ 270
                    if (angle < 180) result.z = angle + 180;
                    else result.z = angle;

                    
                }
                else
                {
                    // 90 ~ 180
                    if (angle > 180) result.z = angle - 180;
                    else result.z = angle;
                }
            }


            result.z *= -1;
            return result;
        });

        // 오른 팔뚝
        float rightarm = calcAngleAndSetRotation(10, 8, 4, (angle, currAngle, direction) => {
            //Debug.Log("angle : " + Mathf.Ceil(angle) + ", direction : " + direction);
            Vector3 result = new Vector3(currAngle.x, currAngle.y, 0);
            if (direction.x < 0)
            {
                if (direction.y < 0)
                {
                    // 90 ~ 180
                    if (angle > 180) result.z = angle - 180;
                    else result.z = angle;
                }
                else
                {
                    // 180 ~ 270 
                    if (angle < 180) result.z = angle + 180;
                    else result.z = angle;
                }
            }
            else
            {
                if (direction.y < 0)
                {
                    // 0 ~ 90
                    if (angle > 90) result.z = angle - 180;
                    else result.z = angle;

                    if (angle > 20) result.z = 20f;
                }
                else
                {
                    // 270 ~ 360
                    if (angle < 270) result.z = angle + 180;
                    else result.z = angle;
                }
            }


            result.z *= -1;
            //result.z -= 45f;
            return result;
        });
        //Debug.Log(rightarm);

        // 왼 허벅지
        float leftUppefLeg = calcAngleAndSetRotation(13, 11, 5, (angle, currAngle, direction) =>
        {
            Vector3 result = new Vector3(currAngle.x, currAngle.y, currAngle.z);
            //Debug.Log("angle : " + angle + ", dircetion : " + direction);
            if (direction.x < 0)
            {
                if (direction.y < 0)
                {
                    // 90 ~ 180
                    if (angle > 180) result.x = angle - 180;
                    else result.x = angle;
                }
                else
                {
                    // 180 ~ 270 
                    if (angle < 180) result.x = angle + 180;
                    else result.x = angle;
                }
            }
            else
            {
                if (direction.y < 0)
                {
                    // 0 ~ 90
                    if (angle > 90) result.x = angle - 180;
                    else result.x = angle;
                }
                else
                {
                    // 270 ~ 360
                    if (angle < 270) result.x = angle + 180;
                    else result.x = angle;
                }
            }
            return (result.x-90f)*-1;
        });

        // 오른 허벅지
        float rightUppefLeg = calcAngleAndSetRotation(14, 12, 7, (angle, currAngle, direction) => {
            Vector3 result = new Vector3(currAngle.x, currAngle.y, currAngle.z);
            if (direction.x < 0)
            {
                if (direction.y < 0)
                {
                    // 90 ~ 180
                    if (angle > 180) result.x = angle - 180;
                    else result.x = angle;
                }
                else
                {
                    // 180 ~ 270 
                    if (angle < 180) result.x = angle + 180;
                    else result.x = angle;
                }
            }
            else
            {
                if (direction.y < 0)
                {
                    // 0 ~ 90
                    if (angle > 90) result.x = angle - 180;
                    else result.x = angle;
                }
                else
                {
                    // 270 ~ 360
                    if (angle < 270) result.x = angle + 180;
                    else result.x = angle;
                }
            }
            return (result.x - 90f);
        });

        //Debug.Log(leftUppefLeg);
        //Debug.Log(rightUppefLeg + "," + AmatureBone[7].localEulerAngles.x);

        // 왼 종아리
        calcAngleAndSetRotation(15, 13, 6, (angle, currAngle, direction) => {
            Vector3 result = new Vector3(currAngle.x, currAngle.y, currAngle.z);
            if (direction.x < 0)
            {
                if (direction.y < 0)
                {
                    // 90 ~ 180
                    if (angle > 180) result.x = angle - 180;
                    else result.x = angle;
                }
                else
                {
                    // 180 ~ 270 
                    if (angle < 180) result.x = angle + 180;
                    else result.x = angle;
                }
            }
            else
            {
                if (direction.y < 0)
                {
                    // 0 ~ 90
                    if (angle > 90) result.x = angle - 180;
                    else result.x = angle;
                }
                else
                {
                    // 270 ~ 360
                    if (angle < 270) result.x = angle + 180;
                    else result.x = angle;
                }
            }
            return -1*(result.x - 90f);
        });
        // 오른 종아리
        calcAngleAndSetRotation(16, 14, 8, (angle, currAngle, direction) => {
            Vector3 result = new Vector3(currAngle.x, currAngle.y, currAngle.z);
            if (direction.x < 0)
            {
                if (direction.y < 0)
                {
                    // 90 ~ 180
                    if (angle > 180) result.x = angle - 180;
                    else result.x = angle;
                }
                else
                {
                    // 180 ~ 270 
                    if (angle < 180) result.x = angle + 180;
                    else result.x = angle;
                }
            }
            else
            {
                if (direction.y < 0)
                {
                    // 0 ~ 90
                    if (angle > 90) result.x = angle - 180;
                    else result.x = angle;
                }
                else
                {
                    // 270 ~ 360
                    if (angle < 270) result.x = angle + 180;
                    else result.x = angle;
                }
            }
            return (result.x - 90f);
        });


        //Debug.Log("어깨 : " + trakingCoordinate[5] + ","+ trakingCoordinate[6]+" 골반 : " + trakingCoordinate[11]+", "+ trakingCoordinate[12]);
        // 척추
        
        //PelvisDebug.transform.position = pelvis;
        //ShoulderDebug.transform.position = shoulder;
        
        calcAngleAndSetRotation(shoulder, pelvis, 9, (boneAngle, currAngle, direction) => {
            //Debug.Log(boneAngle);
            if(boneAngle == 0){return prevRotation[9];}

            Vector3 result = new Vector3(currAngle.x, currAngle.y, 0);
            if (boneAngle >= 180) result.z = (boneAngle + 90f)*-1;
            else result.z = (boneAngle - 90f) * -1;
            prevRotation[9] = result;
            return result;
        });

        // 머리
        float headAngle = calcPerpendicularAngle(trakingCoordinate[3], trakingCoordinate[4]);
        Vector3 nextHeadAngle = AmatureBone[0].localEulerAngles;

        nextHeadAngle.z = headAngle;
        AmatureBone[0].localEulerAngles = nextHeadAngle;
    }

    /// <summary>
    /// 측정된 각 관절 좌표 저장
    /// </summary>
    /// <param name="trakingCoordinate"></param>
    public void setTrakingCoordinate(Vector3[] trakingCoordinate)
    {
        this.trakingCoordinate = trakingCoordinate;
    }

    /// <summary>
    /// 현재 트래킹 상태인지 확인하는 함수
    /// </summary>
    /// <returns>트래킹 상태 true : 트래킹 중, false : 트래킹 안하는 중</returns>
    public bool isTraking() { return traking; }
    /// <summary>
    /// 트래킹 상태 변환 함수
    /// </summary>
    /// <param name="traking"> 변환 할 트래킹 상태</param>
    public void setTraking(bool traking) { this.traking = traking; }
    /// <summary>
    /// 커너비 등장 완료 후 트래킹 상태를 true로 바꾸는 함수
    /// </summary>
    public void startTraking() { traking = true; }

    public void setDefaultPelvis(Vector3 leftPelvis, Vector3 rightPelvis)
    {
        defaultPelvis =  calcCenterVector(leftPelvis, rightPelvis);
    }

    /// <summary>
    /// 두 개의 traking 좌표를 통해 회전값 및 방향을 찾아 지정된 bone의 회전값을 반영하는 함수
    /// </summary>
    /// <param name="topTrakingIndex"> 첫번째 traking 좌표 index </param>
    /// <param name="bottomTrakingIndex"> 두번째 traking 좌표 index </param>
    /// <param name="amatureIndex"> bone 배열의 index </param>
    /// <param name="tmp">각도를 보정 하는 함수 </param>
    /// <returns> 측정된 각도 </returns>
    float calcAngleAndSetRotation(int topTrakingIndex, int bottomTrakingIndex, int amatureIndex, Func<float, Vector3, Vector3, Vector3> tmp)
    {
        return this.calcAngleAndSetRotation(trakingCoordinate[topTrakingIndex], trakingCoordinate[bottomTrakingIndex], amatureIndex, tmp);
    }

    /// <summary>
    /// 두 개의 traking 좌표를 통해 회전값 및 방향을 찾아 지정된 bone의 회전값을 반영하는 함수
    /// </summary>
    /// <param name="topTrakingIndex"> 첫번째 traking 좌표 Vector </param>
    /// <param name="bottomTrakingIndex"> 두번째 traking 좌표 Vector </param>
    /// <param name="amatureIndex"> bone 배열의 index </param>
    /// <param name="tmp">각도를 보정 하는 함수 </param>
    /// <returns> 측정된 각도 </returns>
    float calcAngleAndSetRotation(Vector3 topTrakingVector, Vector3 bottomTrakingVector, int amatureIndex, Func<float, Vector3, Vector3, Vector3> tmp)
    {
        if (topTrakingVector == new Vector3(-1, -1, -1) || bottomTrakingVector == new Vector3(-1, -1, -1))
        {
            AmatureBone[amatureIndex].localEulerAngles = defaultRotation[amatureIndex];
            return -1;
        }
        else
        {
            float boneAngle = Quaternion.FromToRotation(Vector3.forward, topTrakingVector - bottomTrakingVector).eulerAngles.z;
            Vector3 direction = topTrakingVector - bottomTrakingVector;

            Vector3 currAngle = AmatureBone[amatureIndex].localEulerAngles;
            AmatureBone[amatureIndex].localEulerAngles = tmp(boneAngle, currAngle, direction.normalized);
            return boneAngle;
        }
    }

    /// <summary>
    /// 다리의 움직임을 제어하기 위한
    /// </summary>
    /// <param name="topTrakingVector"></param>
    /// <param name="bottomTrakingVector"></param>
    /// <param name="amatureIndex"></param>
    /// <param name="tmp"></param>
    /// <returns></returns>
    float calcAngleAndSetRotation(int topTrakingVector, int  bottomTrakingVector, int amatureIndex, Func<float, Vector3, Vector3, float> tmp)
    {
        if (trakingCoordinate[topTrakingVector] == new Vector3(-1, -1, -1) || trakingCoordinate[topTrakingVector] == new Vector3(-1, -1, -1))
        {
            AmatureBone[amatureIndex].localEulerAngles = defaultRotation[amatureIndex];
            return -1;
        }
        else
        {
            float boneAngle = Quaternion.FromToRotation(Vector3.forward, trakingCoordinate[topTrakingVector] - trakingCoordinate[bottomTrakingVector]).eulerAngles.z;
            Vector3 direction = trakingCoordinate[topTrakingVector] - trakingCoordinate[bottomTrakingVector];

            Vector3 nextAngle = AmatureBone[amatureIndex].eulerAngles;
            nextAngle.x = tmp(boneAngle, nextAngle, direction.normalized);
            AmatureBone[amatureIndex].eulerAngles = nextAngle;
            return boneAngle;
        }
    }


    /// <summary>
    /// 수직 벡터 연산을 위한 함수 (머리 각도 측정 용)
    /// </summary>
    /// <param name="leftEar"> 왼쪽 귀 벡터</param>
    /// <param name="rightEar">오른쪽 귀 벡터</param>
    /// <returns></returns>
    float calcPerpendicularAngle(Vector3 leftEar, Vector3 rightEar)
    {
        if(leftEar == new Vector3(-1,-1,-1)) 
        {
            leftEar = trakingCoordinate[0]; 
        }
        else if(rightEar == new Vector3(-1, -1, -1))
        {
            rightEar = trakingCoordinate[0];
        }

        Vector3 direction = (leftEar - rightEar).normalized;
        Vector3 resultVector = Vector3.zero;
        Vector3.OrthoNormalize(ref direction,ref resultVector);
        float result = Quaternion.FromToRotation(Vector3.forward, resultVector).eulerAngles.z;

        if (resultVector.x > 0)
        {
            if (result > 90) result = 360 - result;
            else result = result - 90f;
        }
        else
        {
            if (result < 90) result = 360 - result;
            else result = result + 90f;
        }
        result *= -1;

        return result;
    }

    /// <summary>
    /// 척추 회전값을 계산 하기 위해서 어깨와 골반의 중간 지점의 벡터를 구하기 위한 연산 함수
    /// </summary>
    /// <param name="p1">지점 1 벡터</param>
    /// <param name="p2">지점 2 벡터</param>
    /// <returns></returns>
    Vector3 calcCenterVector(Vector3 p1, Vector3 p2)
    {
        if (p1 == new Vector3(-1, -1, -1) || p2 == new Vector3(-1, -1, -1))
            return new Vector3(-1, -1, -1);
        Vector3 middleVector = (p2 - p1)/2;

        return p1 + middleVector;
    }

}
