using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
     */
    [SerializeField] private Transform[] AmatureBone;
    
    [SerializeField] private Vector3[] defaultRotation;

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
    private List<Vector3> trakingCoordinate = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0;i < defaultRotation.Length; i++)
        {
            AmatureBone[i].localEulerAngles = defaultRotation[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (trakingCoordinate == null || trakingCoordinate.Count == 0) return;


        // ����
        calcAngleAndSetRotation(7, 5, 1, -90);
        // ������
        calcAngleAndSetRotation(8, 6, 3, 90);
        // �� �Ϲ�
        calcAngleAndSetRotation(9, 7, 2, -90);
        // ���� �Ϲ�
        calcAngleAndSetRotation(10, 8, 4, 90);

    }

    public void setTrakingCoordinate(List<Vector3> trakingCoordinate)
    {
        this.trakingCoordinate = trakingCoordinate;
    }

    void calcAngleAndSetRotation(int topTrakingIndex, int bottomTrakingIndex, int amatureIndex, float tmp)
    {
        if (trakingCoordinate[topTrakingIndex] == new Vector3(-1, -1, -1) || trakingCoordinate[bottomTrakingIndex] == new Vector3(-1, -1, -1))
        {
            AmatureBone[amatureIndex].localEulerAngles = defaultRotation[amatureIndex];
        }
        else
        {
            float rightUpperArmRotation = Quaternion.FromToRotation(Vector3.forward, trakingCoordinate[bottomTrakingIndex] - trakingCoordinate[topTrakingIndex]).eulerAngles.z;
            Vector3 currAngle = AmatureBone[amatureIndex].eulerAngles;
            AmatureBone[amatureIndex].eulerAngles = new Vector3(currAngle.x, currAngle.y, rightUpperArmRotation + tmp);
        }
    }
}
