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
     */
    [SerializeField] private Transform[] AmatureBone;
    
    [SerializeField] private Vector3[] defaultRotation;

    public GameObject PelvisDebug;
    public GameObject ShoulderDebug;

    // 0. 6, 7, 8 => ��
    // 1. 9, 10, 11 => �� ��
    // 2. 12, 13, 14 => �� ��
    // 3. 15, 16, 17 => �� ��
    // 4. 18, 19, 18 => �� ��
    // 5. 21. 22, 23 => �� ��
    // 6. 24. 25, 26 => �� ��
    // 7. 27, 28, 29 => �� �Ȳ�
    // 8. 30, 31, 32 => �� �Ȳ�
    // 9. 33, 34, 35 => �� ��
    // 10. 36, 37, 38 => �� ��
    // 11. 39, 40, 41 => �� ��
    // 12. 42, 43, 44 => �� ��
    // 13. 45, 46, 47 => �� ��
    // 14. 48, 49, 50 => �� ��
    // 15. 51, 52, 53 => �� ��
    // 16. 54, 55, 56 => �� ��
    private Vector3[] trakingCoordinate = new Vector3[17];
    private Vector3[] firstTrakingCoordinate = new Vector3[17];

    private Vector3[] prevRotation = new Vector3[10];

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
    }

    // Update is called once per frame
    void Update()
    {
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
        

        // �� ����
        calcAngleAndSetRotation(7, 5, 1, (angle, currAngle, direction) => { 
            Vector3 result = new Vector3(currAngle.x, currAngle.y, 0);
            result.z = angle * -1 ;
            return result;
        });
        // ���� ����
        calcAngleAndSetRotation(8, 6, 3, (angle, currAngle, direction) => {
            
            Vector3 result = new Vector3(currAngle.x, currAngle.y, 0);
            result.z = angle * -1 ;
            return result;
        });
        // �� �ȶ�
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

        // ���� �ȶ�
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

        // �� �����
        float leftUppefLeg = calcAngleAndSetRotation(13, 11, 5, (angle, currAngle, direction) =>
        {
            Vector3 result = new Vector3(currAngle.x, currAngle.y, currAngle.z);
            


            Debug.Log("angle : " + angle + ", dircetion : " + direction);
            result.x = angle;
            return result.x;
        });

        // ���� �����
        float rightUppefLeg = calcAngleAndSetRotation(14, 12, 7, (angle, currAngle, direction) => {
            Vector3 result = new Vector3(currAngle.x, currAngle.y, currAngle.z);
            //Debug.Log("angle : "+angle + ", dircetion : "+direction);
            if(direction.x < 0)
            {
                if (direction.y < 0)
                {
                    // 90 ~ 180
                    if (angle > 180) result.x = angle-180;
                    else result.x = angle;
                }
                else
                {
                    // 180 ~ 270 
                    if (angle < 180) result.x = angle+180;
                    else result.x = angle;
                }
            }
            else
            {
                if (direction.y < 0)
                {
                    // 0 ~ 90
                    if (angle > 90) result.x = angle-180;
                    else result.x = angle;
                }
                else
                {
                    // 270 ~ 360
                    if (angle < 270) result.x = angle+180;
                    else result.x = angle;
                }
            }

            result.x *= -1;
            return result.x;
        });

        //Debug.Log(leftUppefLeg);
        //Debug.Log(rightUppefLeg + "," + AmatureBone[7].localEulerAngles.x);

        // �� ���Ƹ�
        //calcAngleAndSetRotation(15, 13, 6, (angle) => 0f);
        // ���� ���Ƹ�
        //calcAngleAndSetRotation(16, 14, 8, (angle) => 0f);


        //Debug.Log("��� : " + trakingCoordinate[5] + ","+ trakingCoordinate[6]+" ��� : " + trakingCoordinate[11]+", "+ trakingCoordinate[12]);
        // ô��
        Vector3 shoulder = calcCenterVector(trakingCoordinate[5], trakingCoordinate[6]);
        Vector3 pelvis = calcCenterVector(trakingCoordinate[11], trakingCoordinate[12]);
        PelvisDebug.transform.position = pelvis;
        ShoulderDebug.transform.position = shoulder;
        
        calcAngleAndSetRotation(shoulder, pelvis, 9, (boneAngle, currAngle, direction) => {
            //Debug.Log(boneAngle);
            if(boneAngle == 0){return prevRotation[9];}

            Vector3 result = new Vector3(currAngle.x, currAngle.y, 0);
            if (boneAngle >= 180) result.z = (boneAngle + 90f)*-1;
            else result.z = (boneAngle - 90f) * -1;
            prevRotation[9] = result;
            return result;
        });

        // �Ӹ�
        float headAngle = calcPerpendicularAngle(trakingCoordinate[3], trakingCoordinate[4]);
        Vector3 nextHeadAngle = AmatureBone[0].localEulerAngles;

        nextHeadAngle.z = headAngle;
        AmatureBone[0].localEulerAngles = nextHeadAngle;
    }

    public void setTrakingCoordinate(Vector3[] trakingCoordinate)
    {
        this.trakingCoordinate = trakingCoordinate;
    }

    /// <summary>
    /// �� ���� traking ��ǥ�� ���� ȸ���� �� ������ ã�� ������ bone�� ȸ������ �ݿ��ϴ� �Լ�
    /// </summary>
    /// <param name="topTrakingIndex"> ù��° traking ��ǥ index </param>
    /// <param name="bottomTrakingIndex"> �ι�° traking ��ǥ index </param>
    /// <param name="amatureIndex"> bone �迭�� index </param>
    /// <param name="tmp">������ ���� �ϴ� �Լ� </param>
    /// <returns> ������ ���� </returns>
    float calcAngleAndSetRotation(int topTrakingIndex, int bottomTrakingIndex, int amatureIndex, Func<float, Vector3, Vector3, Vector3> tmp)
    {
        return this.calcAngleAndSetRotation(trakingCoordinate[topTrakingIndex], trakingCoordinate[bottomTrakingIndex], amatureIndex, tmp);
    }

    /// <summary>
    /// �� ���� traking ��ǥ�� ���� ȸ���� �� ������ ã�� ������ bone�� ȸ������ �ݿ��ϴ� �Լ�
    /// </summary>
    /// <param name="topTrakingIndex"> ù��° traking ��ǥ Vector </param>
    /// <param name="bottomTrakingIndex"> �ι�° traking ��ǥ Vector </param>
    /// <param name="amatureIndex"> bone �迭�� index </param>
    /// <param name="tmp">������ ���� �ϴ� �Լ� </param>
    /// <returns> ������ ���� </returns>
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
    /// ���� ���� ������ ���� �Լ� (�Ӹ� ���� ���� ��)
    /// </summary>
    /// <param name="leftEar"> ���� �� ����</param>
    /// <param name="rightEar">������ �� ����</param>
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
    /// ô�� ȸ������ ��� �ϱ� ���ؼ� ����� ����� �߰� ������ ���͸� ���ϱ� ���� ���� �Լ�
    /// </summary>
    /// <param name="p1">���� 1 ����</param>
    /// <param name="p2">���� 2 ����</param>
    /// <returns></returns>
    Vector3 calcCenterVector(Vector3 p1, Vector3 p2)
    {
        if (p1 == new Vector3(-1, -1, -1) || p2 == new Vector3(-1, -1, -1))
            return new Vector3(-1, -1, -1);
        Vector3 middleVector = (p2 - p1)/2;

        return p1 + middleVector;
    }
}
