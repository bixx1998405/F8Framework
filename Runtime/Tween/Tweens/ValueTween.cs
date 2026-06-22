using UnityEngine;

namespace F8Framework.Core
{
    public class ValueTween : BaseTween
    {
        private float from = 0.0f;
        private float to = 0.0f;
        private float originalTo = 0.0f;
        private float originalFrom = 0.0f;
        
        public ValueTween(float from, float to, float t, int id)
        {
            this.id = id;
            Init(from, to, t);
        }

        internal void Init(float from, float to, float t)
        {
            this.from = from;
            this.to = to;
            this.originalTo = to;
            this.originalFrom = from;
            this.duration = t;
            this.PauseReset = () => this.Init(from, to, t);
        }

        internal override void UpdateValue(bool isEnd = false)
        {
            base.UpdateValue(isEnd);
            if (isEnd)
            {
                if (onUpdateFloat != null)
                    onUpdateFloat(loopType == LoopType.Yoyo ? from : to);
            }
            else
            {
                float normalizedProgress = currentTime >= duration ? 1.0f : currentTime / duration;
                float curveProgress = GetCurveProgress(normalizedProgress);

                float value = EasingFunctions.ChangeFloat(from, to, curveProgress, ease);

                if (onUpdateFloat != null)
                    onUpdateFloat(value);
            }
        }
        
        internal override void Reset()
        {
            base.Reset();
            from = 0.0f;
            to = 0.0f;
            originalTo = 0.0f;
            originalFrom = 0.0f;
        }

        public override BaseTween ReplayReset()
        {
            base.ReplayReset();
            to = originalTo;
            from = originalFrom;
            return this;
        }
        
        public override BaseTween LoopReset()
        {
            base.LoopReset();
            return this;
        }

        protected override void OnLoopFlip()
        {
            (from, to) = (to, from);
        }

        protected override void OnLoopIncrement()
        {
            var delta = to - from;
            from = to;
            to += delta;
        }
    }
}
