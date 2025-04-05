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

    // 0. 6, 7, 8 => ÄÚ
    // 1. 9, 10, 11 => ¿Þ ´«
    // 2. 12, 13, 14 => ¿À ´«
    // 3. 15, 16, 17 => ¿Þ ±Í
    // 4. 18, 19, 18 => ¿À ±Í
    // 5. 21. 22, 23 => ¿Þ ¾î
    // 6. 24. 25, 26 => ¿À ¾î
    // 7. 27, 28, 29 => ¿Þ ÆÈ²Þ
    // 8. 30, 31, 32 => ¿À ÆÈ²Þ
    // 9. 33, 34, 35 => ¿Þ ¼Õ
    // 10. 36, 37, 38 => ¿À ¼Õ
    // 11. 39, 40, 41 => ¿Þ °ñ
    // 12. 42, 43, 44 => ¿À °ñ
    // 13. 45, 46, 47 => ¿Þ ¹«
    // 14. 48, 49, 50 => ¿À ¹«
    // 15. 51, 52, 53 => ¿Þ ¹ß
    // 16. 54, 55, 56 => ¿À ¹ß
    private Vector3[] trakingCoordinate = new Vector3[17];
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


        // ¿Þ À§ÆÈ
        calcAngleAndSetRotation(7, 5, 1, (angle, currAngle, direction) => { 
            Vector3 result = new Vector3(currAngle.x, currAngle.y, 0);
            result.z = angle * -1 ;
            return result;
        });
        // ¿À¸¥ À§ÆÈ
        calcAngleAndSetRotation(8, 6, 3, (angle, currAngle, direction) => {
            
            Vector3 result = new Vector3(currAngle.x, currAngle.y, 0);
            result.z = angle * -1 ;
            return result;
        });
        // ¿Þ ÆÈ¶Ò
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

        // ¿À¸¥ ÆÈ¶Ò
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

       /* // ¿Þ Çã¹÷Áö
        float leftUppefLeg = calcAngleSetRotationForLeg(13, 11, 5, (angle) => {
            if (angle >= 250) return -90f;
            else return 90f;
        });
        // ¿À¸¥ Çã¹÷Áö
        float rightUppefLeg = calcAngleSetRotationForLeg(14, 12, 7, (angle) => {
            if (angle >= 250) return -90f;
            else return 90f;
        });*/

        //Debug.Log(leftUppefLeg);
        //Debug.Log(rightUppefLeg + "," + AmatureBone[7].localEulerAngles.x);

        // ¿Þ Á¾¾Æ¸®
        //calcAngleAndSetRotation(15, 13, 6, (angle) => 0f);
        // ¿À¸¥ Á¾¾Æ¸®
        //calcAngleAndSetRotation(16, 14, 8, (angle) => 0f);


        //Debug.Log("¾î±ú : " + trakingCoordinate[5] + ","+ trakingCoordinate[6]+" °ñ¹Ý : " + trakingCoordinate[11]+", "+ trakingCoordinate[12]);
        // Ã´Ãß
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

        // ¸Ó¸®
        calcAngleAndSetRotation(shoulder, trakingCoordinate[0], 0, (boneAngle, currAngle, direction) => {
            Debug.Log("angle : " + Mathf.Ceil(boneAngle) + ", direction : " + direction);
            Vector3 result = new Vector3(currAngle.x, currAngle.y, 0);
            //if (boneAngle == 0) { return prevRotation[0]; }
            if (direction.x > 0)
            {
                if(boneAngle > 90) result.z = 360 - boneAngle;
                else result.z = boneAngle - 90f;
            }
            else
            {
                if (boneAngle < 90) result.z = 360 - boneAngle;
                else result.z = boneAngle + 90f;
            }

            result.z *= -1;
            return result;
        });
    }

    public void setTrakingCoordinate(Vector3[] trakingCoordinate)
    {
        this.trakingCoordinate = trakingCoordinate;
    }

    float calcAngleAndSetRotation(int topTrakingIndex, int bottomTrakingIndex, int amatureIndex, Func<float, Vector3, Vector3, Vector3> tmp)
    {
        return this.calcAngleAndSetRotation(trakingCoordinate[topTrakingIndex], trakingCoordinate[bottomTrakingIndex], amatureIndex, tmp);
    }

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

    float calcAngleSetRotationForLeg(int topTrakingIndex, int bottomTrakingIndex, int amatureIndex, Func<float, int, Vector3> tmp)
    {
        if (trakingCoordinate[topTrakingIndex] == new Vector3(-1, -1, -1) || trakingCoordinate[bottomTrakingIndex] == new Vector3(-1, -1, -1))
        {
            AmatureBone[amatureIndex].localEulerAngles = defaultRotation[amatureIndex];
            return -1;
        }
        else
        {
            float boneAngle = Quaternion.FromToRotation(Vector3.forward, trakingCoordinate[topTrakingIndex] - trakingCoordinate[bottomTrakingIndex]).eulerAngles.z;
            Vector3 currAngle = AmatureBone[amatureIndex].eulerAngles;
            AmatureBone[amatureIndex].eulerAngles = new Vector3(boneAngle,currAngle.y,currAngle.z);
            return boneAngle;
        }
    }

    /// <summary>
    /// Ã´Ãß È¸Àü°ªÀ» °è»ê ÇÏ±â À§ÇØ¼­ ¾î±ú¿Í °ñ¹ÝÀÇ Áß°£ ÁöÁ¡ÀÇ º¤ÅÍ¸¦ ±¸ÇÏ±â À§ÇÑ ¿¬»ê ÇÔ¼ö
    /// </summary>
    /// <param name="p1">ÁöÁ¡ 1 º¤ÅÍ</param>
    /// <param name="p2">ÁöÁ¡ 2 º¤ÅÍ</param>
    /// <returns></returns>
    Vector3 calcCenterVector(Vector3 p1, Vector3 p2)
    {
        if (p1 == new Vector3(-1, -1, -1) || p2 == new Vector3(-1, -1, -1))
            return new Vector3(-1, -1, -1);
        Vector3 middleVector = (p2 - p1)/2;

        return p1 + middleVector;
    }
}
