using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Animatuin管理+ヘルパクラス
/// 
/// NOTE: Motion Blendを使用するものは、適宜派生させること
/// NOTE: 汎化させるよりは、仕様が同じものをまとめて特化させた方がオーバヘッドが少ないので、そうした方がよい(Player、NPC、Enemy、Bossなど)
/// </summary>
public class AnimationCtrl : MonoBehaviour
{
    public struct AnimStack
    {
        public string stateName;
        public float duration;
    }
    public delegate void Callback();

    [SerializeField] Animator _animator;
    [SerializeField] bool _isActiveStart = true;

    int _targetLayer = 0;
    Callback _callback = null;
    string _animState = null;


    Queue<AnimStack> _animQueue = new Queue<AnimStack>();

    private void Awake()
    {
        if (!_isActiveStart)
        {
            DisActive();
        }
    }

    private void Start()
    {
        if (_animator == null) Debug.LogError("Animator Not Found:" + gameObject.name);
    }

    public virtual void Active()
    {
        _animator.enabled = true;
    }

    public virtual void DisActive()
    {
        _animator.enabled = false;
    }

    /// <summary>
    /// アニメーションを切り替える(ループ)
    /// </summary>
    /// <param name="stateName">ステート名</param>
    /// <param name="dur">遅延時間</param>
    /// <returns></returns>
    public void Play(string stateName, float dur = 0.0f)
    {
        Active();
        _animator.CrossFade(stateName, dur);
        //_animator.Play(stateName/*, _targetLayer, dur*/);
        //Debug.Log("Play:" + stateName);
    }
    /// <summary>
    /// アニメーションを切り替える(非ループ)
    /// </summary>
    /// <param name="stateName">ステート名</param>
    /// <param name="dur">遅延時間</param>
    /// <returns></returns>
    public void Play2(string stateName, float dur = 0.0f)
    {
        Active();
        //_animator.CrossFade(stateName, dur);
        _animator.Play(stateName, _targetLayer, dur);
        //Debug.Log("Play:" + stateName);
    }



    /// <summary>
    /// 再生が終わったら次のアニメーションを流す
    /// </summary>
    /// <param name="stateName">ステート名</param>
    /// <param name="dur">遅延時間</param>
    /// <returns></returns>
    public void PlayQueue(string stateName, float dur = 0.0f)
    {
        _animQueue.Enqueue(new AnimStack() { stateName = stateName, duration = dur });
        Debug.Log("PlayQueue:" + stateName);
    }

    /// <summary>
    /// 対象レイヤーのアニメーションが再生中かどうかを返す
    /// </summary>
    /// <param name="layer">レイヤーID、基本は0で省略すると0が入る</param>
    /// <returns></returns>
    public bool IsPlayingAnimation(int layer = 0)
    {
        var state = _animator.GetCurrentAnimatorStateInfo(layer);
        if (state.loop) return true; //ループは永遠にtrue帰る
        return state.normalizedTime < 1.0f;
    }

    //public bool IsPlayingAnimation(int layer = 0)
    //{
    //    if (_duration > 0.0f) return true;
    //    var state = _animator.GetCurrentAnimatorStateInfo(layer);
    //    if (state.loop) return true; //ループは永遠にtrue帰る
    //    return state.normalizedTime < 1.0f;
    //}

    /// <summary>
    /// 再生終了時のコールバックを設定する
    /// </summary>
    /// <param name="cb">コールバック</param>
    /// <param name="target">監視対象レイヤー</param>
    public void SetPlaybackDelegate(Callback cb, int target = 0)
    {
        _callback = cb;
        _targetLayer = target;
    }



    //将来的にコルーチンにする
    private void Update()
    {
        if (_animator == null) return;

        if (_animQueue.Count > 0)
        {
            //0でいいのかはおいおい考える
            if (!IsPlayingAnimation(0))
            {
                var anim = _animQueue.Dequeue();
                Play(anim.stateName, anim.duration);
                Debug.Log("here");
            }
        }

        if (_callback != null)
        {
            if (!IsPlayingAnimation(_targetLayer))
            {
                _callback();
                _callback = null;
            }
        }

        if (_animState != null)
        {
            if (!IsPlayingAnimation(_targetLayer))
            {
                Play(_animState);
                _animState = null;
            }
        }
    }


}