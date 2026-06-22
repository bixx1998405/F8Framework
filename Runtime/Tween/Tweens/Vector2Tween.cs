using UnityEngine;

namespace F8Framework.Core
{
    public class Vector2Tween : BaseTween
    {
        private Vector2 from = Vector2.zero;
        private Vector2 to = Vector2.zero;
        private Vector2 tempValue = Vector2.zero;
        private Vector2 originalTo = Vector2.zero;
        private Vector2 originalFrom = Vector2.zero;
        
        public Vector2Tween(Vector2 from, Vector2 to, float t, int id)
        {
            this.id = id;
            Init(from, to, t);
        }
        
        internal void Init(Vector2 from, Vector2 to, float t)
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
                if(onUpdateVector2 != null)
                    onUpdateVector2(loopType == LoopType.Yoyo ? from : to);
            }
            else
            {
                float normalizedProgress = currentTime >= duration ? 1.0f : currentTime / duration;
                float curveProgress = GetCurveProgress(normalizedProgress);

                EasingFunctions.ChangeVector(from, to, curveProgress, ease, ref tempValue);

                if(onUpdateVector2 != null)
                    onUpdateVector2(tempValue);
            }
        }
        
        internal override void Reset()
        {
            base.Reset();
            from = Vector2.zero;
            to = Vector2.zero;
            originalTo = Vector2.zero;
            originalFrom = Vector2.zero;
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
