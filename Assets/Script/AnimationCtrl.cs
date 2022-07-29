using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Animatuin�Ǘ�+�w���p�N���X
/// 
/// NOTE: Motion Blend���g�p������̂́A�K�X�h�������邱��
/// NOTE: �ĉ���������́A�d�l���������̂��܂Ƃ߂ē��������������I�[�o�w�b�h�����Ȃ��̂ŁA�������������悢(Player�ANPC�AEnemy�ABoss�Ȃ�)
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
    /// �A�j���[�V������؂�ւ���
    /// </summary>
    /// <param name="stateName">�X�e�[�g��</param>
    /// <param name="dur">�x������</param>
    /// <returns></returns>
    public void Play(string stateName, float dur = 0.0f)
    {
        Active();
        _animator.CrossFade(stateName, dur);
        //Debug.Log("Play:" + stateName);
    }

    /// <summary>
    /// �Đ����I������玟�̃A�j���[�V�����𗬂�
    /// </summary>
    /// <param name="stateName">�X�e�[�g��</param>
    /// <param name="dur">�x������</param>
    /// <returns></returns>
    public void PlayQueue(string stateName, float dur = 0.0f)
    {
        _animQueue.Enqueue(new AnimStack() { stateName = stateName, duration = dur });
        Debug.Log("PlayQueue:" + stateName);
    }

    /// <summary>
    /// �Ώۃ��C���[�̃A�j���[�V�������Đ������ǂ�����Ԃ�
    /// </summary>
    /// <param name="layer">���C���[ID�A��{��0�ŏȗ������0������</param>
    /// <returns></returns>
    public bool IsPlayingAnimation(int layer = 0)
    {
        var state = _animator.GetCurrentAnimatorStateInfo(layer);
        if (state.loop) return true; //���[�v�͉i����true�A��
        return state.normalizedTime < 1.0f;
    }

    //public bool IsPlayingAnimation(int layer = 0)
    //{
    //    if (_duration > 0.0f) return true;
    //    var state = _animator.GetCurrentAnimatorStateInfo(layer);
    //    if (state.loop) return true; //���[�v�͉i����true�A��
    //    return state.normalizedTime < 1.0f;
    //}

    /// <summary>
    /// �Đ��I�����̃R�[���o�b�N��ݒ肷��
    /// </summary>
    /// <param name="cb">�R�[���o�b�N</param>
    /// <param name="target">�Ď��Ώۃ��C���[</param>
    public void SetPlaybackDelegate(Callback cb, int target = 0)
    {
        _callback = cb;
        _targetLayer = target;
    }



    //�����I�ɃR���[�`���ɂ���
    private void Update()
    {
        if (_animator == null) return;

        if (_animQueue.Count > 0)
        {
            //0�ł����̂��͂��������l����
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