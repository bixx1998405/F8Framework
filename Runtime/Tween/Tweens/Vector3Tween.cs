using UnityEngine;

namespace F8Framework.Core
{
    public class Vector3Tween : BaseTween
    {
        private Vector3 from = Vector3.zero;
        private Vector3 to = Vector3.zero;
        private Vector3 tempValue = Vector3.zero;
        private Vector3 originalTo = Vector3.zero;
        private Vector3 originalFrom = Vector3.zero;
        
        public Vector3Tween(Vector3 from, Vector3 to, float t, int id)
        {
            this.id = id;
            Init(from, to, t);
        }

        internal void Init(Vector3 from, Vector3 to, float t)
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
                if (onUpdateVector3 != null)
                    onUpdateVector3(loopType == LoopType.Yoyo ? from : to);

                if (onUpdateVector2 != null)
                    onUpdateVector2(loopType == LoopType.Yoyo ? from : to);
            }
            else
            {
                float normalizedProgress = currentTime >= duration ? 1.0f : currentTime / duration;
                float curveProgress = GetCurveProgress(normalizedProgress);

                EasingFunctions.ChangeVector(from, to, curveProgress, ease, ref tempValue);

                if (onUpdateVector3 != null)
                    onUpdateVector3(tempValue);

                if (onUpdateVector2 != null)
                    onUpdateVector2(tempValue);
            }
        }
        
        internal override void Reset()
        {
            base.Reset();
            to = Vector3.zero;
            from = Vector3.zero;
            originalTo = Vector3.zero;
            originalFrom = Vector3.zero;
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
