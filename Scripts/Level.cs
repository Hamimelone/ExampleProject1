using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Game/Level")]
public class Level : ScriptableObject
{
    [Header("�ؿ���ͼ��Ϣ")]                   //MapManager��ͼ���
    public Tilemap LevelMap;                   //�ؿ���ͼ
    [Header("�ؿ���Ϸ��Ϣ")]                   //GameManager��Ϸ���
    public int initialGold;                    //�ؿ���ʼ���
}
