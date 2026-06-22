using UnityEngine;

namespace F8Framework.Core
{
    public class ColorTween : BaseTween
    {
        private Color from = Color.white;
        private Color to = Color.white;
        private Color originalTo = Color.white;
        private Color originalFrom = Color.white;

        public ColorTween(Color from, Color to, float t, int id)
        {
            this.id = id;
            Init(from, to, t);
        }

        internal void Init(Color from, Color to, float t)
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
                if (onUpdateColor != null)
                    onUpdateColor(loopType == LoopType.Yoyo ? from : to);
            }
            else
            {
                float normalizedProgress = currentTime >= duration ? 1.0f : currentTime / duration;
                float curveProgress = GetCurveProgress(normalizedProgress);

                Color color = Color.LerpUnclamped(from, to, EasingFunctions.ChangeFloat(0f, 1f, curveProgress, ease));

                if (onUpdateColor != null)
                    onUpdateColor(color);
            }
        }
        
        internal override void Reset()
        {
            base.Reset();
            from = Color.white;
            to = Color.white;
            originalTo = Color.white;
            originalFrom = Color.white;
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
