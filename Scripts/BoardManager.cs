using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Board shadowBlock;

    public int mapX;
    public int mapY;
    public float blockSizeX = 0.9f;
    public float blockSizeY = 1f;
    public float spacing = 0.1f;
    public List<Board> AllBoards = new List<Board>();
    #region 单例
    public static BoardManager Instance;

    private void Awake()
    {
        if (Instance != null)        //单例模式
        {
            Destroy(gameObject);
        }
        else
            Instance = this;
    }
    #endregion

    private void Start()
    {
        CreateBoard(mapX, mapY);
    }

    private void CreateBoard(int x,int y)
    {
        // 计算总宽度和高度（包括间距）
        float totalWidth = (x * blockSizeX) +((x-1)*spacing);
        float totalHeight = (y * blockSizeY)+((y-1)*spacing);

        // 计算起始位置（左下角）
        Vector2 startPos = new Vector3(
            -totalWidth / 2 + blockSizeX / 2,
            -totalHeight / 2 + blockSizeY / 2);

        for (int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                Vector2 cellPos = new Vector2(startPos.x + i* (blockSizeX +spacing), startPos.y + j* (blockSizeY+spacing));
                Board b = Instantiate(shadowBlock, cellPos, Quaternion.identity,transform);
                b.X = i;
                b.Y = j;
                AllBoards.Add(b);
            }
        }
    }
    public Board GetNearestBoard(Vector2 pos)
    {
        Board nstb = null;
        float minDistance = float.MaxValue;
        foreach (Board b in AllBoards)
        {
            if (b == null) continue;
            float distance = Vector2.Distance(b.transform.position, pos);
            if (distance < minDistance)
            {
                minDistance = distance;
                nstb = b;
            }
        }
        return nstb;
    }

    public bool IsMouseInsideTheBoard(Vector2 mousepos)
    {
        return mousepos.x > AllBoards[0].transform.position.x - blockSizeX / 2 - spacing
            && mousepos.x < AllBoards[AllBoards.Count-1].transform.position.x + blockSizeX / 2 + spacing
            && mousepos.y > AllBoards[0].transform.position.y - blockSizeY / 2 - spacing
            && mousepos.y < AllBoards[AllBoards.Count-1].transform.position.y + blockSizeY / 2 + spacing;
    }
}
