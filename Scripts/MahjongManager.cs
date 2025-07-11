using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class MahjongManager : MonoBehaviour
{
    #region ����
    public static MahjongManager Instance;

    private void Awake()
    {
        if (Instance != null)        //����ģʽ
        {
            Destroy(gameObject);
        }
        else
            Instance = this;
    }
    #endregion
    [SerializeField] private Mahjong[] AllMahjongType = new Mahjong[36];
    [SerializeField] private Transform spawnMTransform;
    public List<Mahjong> M_OnBoard = new List<Mahjong>();
    public List<Mahjong> M_Taken = new List<Mahjong>();
    public List<Mahjong> M_Spawn = new List<Mahjong>();
    public Mahjong CurrentDraggingM = null;
    public Board CurrentPlacingBoard = null;
    [SerializeField] private GameObject Mpreview;
    [SerializeField] private float spawnSpacing = 0.8f;
    [SerializeField] private float spawnScale = 1.25f;
    public void Initialize()
    {
        if (M_OnBoard.Count > 0)
        {
            foreach (var m in M_OnBoard)
            {
                Destroy(m.gameObject);
            }
        }
        M_OnBoard.Clear();
        M_Taken.Clear();
        if (M_Spawn.Count > 0)
        {
            foreach (var m in M_Spawn)
            {
                Destroy(m.gameObject);
            }
        }
        M_Spawn.Clear();
        
        Mpreview.SetActive(false);
    }
    private void Start()
    {
        Mpreview = Instantiate(Mpreview);
        Initialize();
        GenerateRandomMahjong(3);
        CurrentDraggingM = null;
        
    }
    private void Update()
    {
        if (CurrentDraggingM != null)
        {
            UpatePreview();
        }
        else
            HidePreview();
    }
    public void GenerateRandomMahjong(int k)
    {
        List<Mahjong> tmp = new List<Mahjong>();
        for (int i = 0; i<k; i++)
        {
            int r = Random.Range(0,AllMahjongType.Length);
            M_Spawn.Add(AllMahjongType[r]);
        }
        float sizeX = BoardManager.Instance.blockSizeX;
        float totalWidth = (k * sizeX) + ((k - 1) * spawnSpacing);
        Vector2 startPos = new Vector2(
            -totalWidth / 2 + sizeX / 2,
            -4f);
        for (int i = 0;i<M_Spawn.Count;i++)
        {
            Vector2 cellPos = new Vector2(startPos.x + i* (sizeX +spawnSpacing), startPos.y);
            Mahjong m = Instantiate(M_Spawn[i],cellPos,Quaternion.identity,spawnMTransform);
            tmp.Add(m);
            m.transform.localScale = Vector3.one * spawnScale;
            m.SetMoveable(true);
        }
        M_Spawn.Clear();
        M_Spawn = tmp;
    }
    public void SetCurrentDraggingMahjong(Mahjong m)
    {
        CurrentDraggingM = m;
    }
    public void RefreshCurrentDraggingMahjong()
    {
        if (CurrentDraggingM != null)
            CurrentDraggingM = null;
        CurrentPlacingBoard = null;
    }
    public void UpatePreview()
    {
        if(CurrentDraggingM!= null)
        {
            Mpreview.SetActive(true);
            Mpreview.GetComponent<SpriteRenderer>().sprite = CurrentDraggingM.GetComponent<SpriteRenderer>().sprite;
            CurrentPlacingBoard = BoardManager.Instance.GetNearestBoard(CurrentDraggingM.transform.position);
            Mpreview.transform.position = CurrentPlacingBoard.transform.position;
        }
    }
    public void HidePreview()
    {
        Mpreview.SetActive(false);
        CurrentPlacingBoard = null;
    }

    public void TryToPlaceMahjong()
    {
        if(CurrentPlacingBoard != null && CurrentDraggingM != null)
        {
            if (CurrentPlacingBoard.IsPlacable)         //ִ�з���
            {
                CurrentDraggingM.transform.position = CurrentPlacingBoard.transform.position;
                CurrentDraggingM.transform.SetParent(CurrentPlacingBoard.transform);
                CurrentDraggingM.SetMoveable(false);
                CurrentDraggingM.GetComponent<SpriteRenderer>().sortingOrder = 1;
                CurrentDraggingM.X = CurrentPlacingBoard.X;
                CurrentDraggingM.Y = CurrentPlacingBoard.Y;
                M_Spawn.Remove(CurrentDraggingM);
                M_OnBoard.Add(CurrentDraggingM);
                FinishPlacing(CurrentDraggingM);
                RefreshCurrentDraggingMahjong();
            }
            else
            {
                CurrentDraggingM.CancelMoving();
            }
        }
    }
    public void FinishPlacing(Mahjong m)
    {
        //�ж��������齫
        if (spawnMTransform.transform.childCount == 0)
        {
            GenerateRandomMahjong(3);
        }
        //�ж�����
        CheckElimination(m);
        //�жϰ����ǲ�������
        if(M_OnBoard.Count >= BoardManager.Instance.mapX * BoardManager.Instance.mapY)
        {
            Debug.Log("���˰�");
            GameManager.Instance.GameOver();
        }
        
    }

    public void CheckElimination(Mahjong m)
    {
        List<Mahjong> pairMahjongs = FindConnectedMahjongsByNumber(m);
        if (pairMahjongs.Count >= 3)
        {
            GameManager.Instance.GetScore(true, pairMahjongs.Count,m.MahjongType);
            EliminateMatchedMahjongs(pairMahjongs);
        }
        List<Mahjong> straightMahjongs =FindConnectedMahjongsStraight(m);
        if (straightMahjongs.Count >= 2)
        {
            GameManager.Instance.GetScore(false, straightMahjongs.Count, m.MahjongType);
            EliminateMatchedMahjongs(straightMahjongs);
        }
    }

    private List<Mahjong> FindConnectedMahjongsByNumber(Mahjong startM)
    {
        List<Mahjong> result = new List<Mahjong>();
        HashSet<Mahjong> visited = new HashSet<Mahjong>();
        Stack<Mahjong> stack = new Stack<Mahjong>();

        if(startM == null || visited.Contains(startM))
            return result;
        stack.Push(startM);
        visited.Add(startM);
        while(stack.Count > 0)
        {
            Mahjong current = stack.Pop();
            result.Add(current);
            // ����ĸ���������ڷ���
            CheckAdjacentBlockByNumber(current.X + 1, current.Y, current.MahjongType, current.Value, visited, stack);
            CheckAdjacentBlockByNumber(current.X - 1, current.Y, current.MahjongType, current.Value, visited, stack);
            CheckAdjacentBlockByNumber(current.X, current.Y + 1, current.MahjongType, current.Value, visited, stack);
            CheckAdjacentBlockByNumber(current.X, current.Y - 1, current.MahjongType, current.Value, visited, stack);
        }
        return result;
    }
    // ����ض�λ�õ����ڷ���
    private void CheckAdjacentBlockByNumber(int x, int y, MahjongType mType,int mValue, HashSet<Mahjong> visited, Stack<Mahjong> stack)
    {
        Mahjong adjacent = FindBlockAtPosition(x, y);
        if (adjacent != null && adjacent.MahjongType == mType && adjacent.Value == mValue && !visited.Contains(adjacent))
        {
            visited.Add(adjacent);
            stack.Push(adjacent);
        }
    }

    private List<Mahjong> FindConnectedMahjongsStraight(Mahjong startM)
    {
        //if(startM.MahjongType == MahjongType.��||
        //   startM.MahjongType == MahjongType.Ͳ||
        //   startM.MahjongType == MahjongType.��)
        //{

        //}
        List<Mahjong> result = new List<Mahjong>();
        HashSet<Mahjong> visited = new HashSet<Mahjong>();
        Stack<Mahjong> stack = new Stack<Mahjong>();

        if (startM == null || visited.Contains(startM))
            return result;
        stack.Push(startM);
        visited.Add(startM);
        while (stack.Count > 0)
        {
            Mahjong current = stack.Pop();
            result.Add(current);
            CheckAdjacentBlockStraight(current.X + 1, current.Y, current.MahjongType, visited, stack);
            CheckAdjacentBlockStraight(current.X - 1, current.Y, current.MahjongType, visited, stack);
            CheckAdjacentBlockStraight(current.X, current.Y + 1, current.MahjongType, visited, stack);
            CheckAdjacentBlockStraight(current.X, current.Y - 1, current.MahjongType, visited, stack);
        }
        List<Mahjong> sortedResult = result.OrderBy(m => m.Value).ToList();
        List<Mahjong> longestSequence = new List<Mahjong>();
        List<Mahjong> currentSequence = new List<Mahjong>();
        List<Mahjong> nonRepeatSequence = new List<Mahjong>();
        List<Mahjong> connectedLongestSequence = new List<Mahjong>();
        // �ҳ����������������
        for (int i = 0; i < sortedResult.Count; i++)
        {
            // ����ǵ�һ��Ԫ�ػ���ǰһ�����ֲ�Ϊ1�����뵱ǰ����
            if (currentSequence.Count == 0 ||
                sortedResult[i].Value == currentSequence.Last().Value + 1||
                sortedResult[i].Value == currentSequence.Last().Value)
            { 
                currentSequence.Add(sortedResult[i]);
            }
            else
            {
                // ����Ƿ��ҵ�����������
                if (currentSequence.Count > longestSequence.Count)
                {
                    longestSequence = new List<Mahjong>(currentSequence);
                }
                currentSequence.Clear();
                currentSequence.Add(sortedResult[i]);
            }
        }
        // ����ټ��һ�ε�ǰ����
        if (currentSequence.Count > longestSequence.Count)
        {
            longestSequence = new List<Mahjong>(currentSequence);
        }

        //����Ƿ�����
        //for (int i = 0; i < longestSequence.Count; i++)
        //{
        //    for (int j = i+1; j < longestSequence.Count; j++)
        //    {
        //        if (connectedLongestSequence.Count == 0 ||
        //            Mathf.Abs(connectedLongestSequence.Last().X - longestSequence[j].X) ==1||
        //            Mathf.Abs(connectedLongestSequence.Last().Y - longestSequence[j].Y) ==1)
        //        {
        //            if (!connectedLongestSequence.Contains(longestSequence[i]))
        //            connectedLongestSequence.Add(longestSequence[i]);
        //        }
        //    }    
        //}

        //ȥ���ظ�Ԫ�ص��б�
        for (int i = 0; i < longestSequence.Count; i++)
        {
            if(nonRepeatSequence.Count == 0||
                longestSequence[i].Value != nonRepeatSequence.Last().Value)
            {
                nonRepeatSequence.Add(longestSequence[i]);
            }
        }


        if (startM.MahjongType == MahjongType.��)
            return nonRepeatSequence.Count >= 4 ? longestSequence : new List<Mahjong>();
        else if (startM.MahjongType == MahjongType.��)
            return nonRepeatSequence.Count >= 2 ? longestSequence : new List<Mahjong>();
        else
        return nonRepeatSequence.Count >= 3 ? longestSequence : new List<Mahjong>();
    }

    private void CheckAdjacentBlockStraight(int x, int y, MahjongType mType, HashSet<Mahjong> visited, Stack<Mahjong> stack)
    {
        Mahjong adjacent = FindBlockAtPosition(x, y);
        if (adjacent != null && adjacent.MahjongType == mType && !visited.Contains(adjacent))
        {
            visited.Add(adjacent);
            stack.Push(adjacent);
        }
    }
    // ����������ҷ���
    private Mahjong FindBlockAtPosition(int x, int y)
    {
        foreach (Mahjong m in M_OnBoard)
        {
            if (m.X == x && m.Y == y)
                return m;
        }
        return null;
    }

    private void EliminateMatchedMahjongs(List<Mahjong> MToEliminate)
    {
        foreach (var m in MToEliminate)
        {
            M_OnBoard.Remove(m);
            Destroy(m.gameObject);
        }
    }
}
