using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField] private List<GameObject> effectList = new List<GameObject>();
    private Queue<GameObject> effectPool = new Queue<GameObject>();

    public void InitializeEffect()
    {
        effectPool.Clear();
    }
    public GameObject GetFromPool()
    {
        if (effectPool.Count > 0)
        {
            GameObject obj = effectPool.Dequeue();
            obj.SetActive(true);
            Animator anim = obj.GetComponent<Animator>();
            if (anim != null)
            {
                anim.enabled = true;
                anim.Rebind(); // 关键重置方法
                anim.Update(0f);
            }
            return obj;
        }
        else
            return CreateEffect(0);
    }

    public GameObject CreateEffect(int P_index)
    {
        GameObject go = Instantiate(effectList[P_index], this.transform);

        return go;
    }

    public void SetEffect(int Eindex, Vector2 dir, Vector2 pos, float time, float scale)
    {
        GameObject e = GetFromPool();
        e.GetComponent<Animator>().runtimeAnimatorController = effectList[Eindex].GetComponent<Animator>().runtimeAnimatorController;
        e.transform.position = pos;
        e.transform.localScale = Vector3.one * scale;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        e.transform.rotation = rotation;
        e.GetComponent<SpriteRenderer>().sortingOrder = 100;
        StartCoroutine(ReturnToPoolAfter(e, time));
    }
    public void ReturnToPool(GameObject p)
    {
        p.SetActive(false);
        effectPool.Enqueue(p);
    }
    // 延时回收协程
    private IEnumerator ReturnToPoolAfter(GameObject effect, float delay)
    {
        Animator anim = effect.GetComponent<Animator>();
        if (anim == null)
            yield return new WaitForSeconds(delay);
        else
            yield return new WaitForSeconds(GetAnimationDuration(anim, anim.runtimeAnimatorController.name));
        // 停止动画

        if (anim != null)
        {
            anim.StopPlayback();
            anim.enabled = false;
        }

        effect.SetActive(false);
        effectPool.Enqueue(effect);
    }

    float GetAnimationDuration(Animator animator, string animationName)
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        foreach (var clip in ac.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length / animator.speed; // 计算时长
            }
        }
        Debug.LogError($"Animation {animationName} not found!");
        return 0f;
    }
}
