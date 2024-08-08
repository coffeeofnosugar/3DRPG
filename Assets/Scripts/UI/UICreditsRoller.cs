using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;

namespace UI
{
    public class UICreditsRoller : MonoBehaviour
    {
        [SerializeField, BoxGroup("滚动参数")] private float _rollTime = 1;
        [SerializeField, BoxGroup("滚动参数")] private bool loop;
        [SerializeField, ReadOnly, BoxGroup("滚动参数")] private int _currentIndex = -1;

        [SerializeField, BoxGroup("显示区域")] private RectTransform _mask;
        [SerializeField, BoxGroup("显示区域")] private RectTransform _creditsText;
        [SerializeField, BoxGroup("显示区域")] private LocalizeStringEvent _text;
        
        private float _finishPoint;
        private Sequence _sequence;
        
        public void StartRolling(int count)
        {
            InitialOffset(count);
            _sequence.Restart();
        }

        private void InitialOffset(int count)
        {
            // 如果已经初始化了就跳过
            if (_sequence != null)
                return;
            
            _finishPoint = (_mask.rect.height + _creditsText.rect.height) / 2;
            
            _text.StringReference.TableEntryReference = "Credits_0";
            
            Tweener tweenerTopHalf = _creditsText.DOAnchorPosY(_finishPoint, _rollTime)
                // .SetSpeedBased()                // 以速度参数，而不是到达的时间    // 在sequence中不允许使用以速度为参数
                .SetEase(Ease.Linear)               // 平滑移动，全程匀速
                .SetAutoKill(false)                 // 取消自动释放资源
                .OnComplete(() =>
                {
                    _creditsText.anchoredPosition = new Vector2(_creditsText.anchoredPosition.x, -_finishPoint);
                    SettingText();
                } );

            Tweener tweenerBottomHalf = _creditsText.DOAnchorPosY(0, _rollTime)
                .SetEase(Ease.Linear)
                .SetAutoKill(false);
            
            _sequence = DOTween.Sequence();
            _sequence.Append(tweenerTopHalf)
                .SetAutoKill(false)
                .Append(tweenerBottomHalf)
                .SetLoops(loop ? -1 : count - 1)
                .OnRewind(() =>
                {
                    _text.StringReference.TableEntryReference = "Credits_0";
                    _currentIndex = 0;
                    _creditsText.anchoredPosition = new Vector2(_creditsText.anchoredPosition.x, 0);
                })
                .Pause();
        }

        private void SettingText()
        {
            _currentIndex = ++_currentIndex >= 4 ? 0 : _currentIndex;
            _text.StringReference.TableEntryReference = "Credits_" + _currentIndex;
        }
        
        public void StopRoll()
        {
            _sequence.Pause();
        }

        public void ChangeRollSpeed(Vector2 vector2)
        {
            if (vector2.y > .3f)
            {
                _sequence.timeScale = 2;
            }
            else if (vector2.y < -.3f)
            {
                _sequence.timeScale = .3f;
            }
            else
            {
                _sequence.timeScale = 1f;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _sequence.Kill();
            _sequence = null;
        }

        void Dispose(bool disposing)
        {
            // 释放托管资源
            if (disposing)
            {
                
            }
            _sequence.Kill();
            _sequence = null;
        }

        ~UICreditsRoller()
        {
            Dispose(false);
        }
    }
}