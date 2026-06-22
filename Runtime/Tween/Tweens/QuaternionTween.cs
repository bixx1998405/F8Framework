using UnityEngine;

namespace F8Framework.Core
{
	public class QuaternionTween : BaseTween
	{
        private Quaternion from = Quaternion.identity;
        private Quaternion to = Quaternion.identity;
        private Quaternion originalTo = Quaternion.identity;
        private Quaternion originalFrom = Quaternion.identity;
        
        public QuaternionTween(Quaternion from, Quaternion to, float t, int id)
        {
            this.id = id;
            Init(from, to, t);
        }

        internal void Init(Quaternion from, Quaternion to, float t)
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
                if (onUpdateQuaternion != null)
                    onUpdateQuaternion(loopType == LoopType.Yoyo ? from : to);
            }
            else
            {
                float normalizedProgress = currentTime >= duration ? 1.0f : currentTime / duration;
                float curveProgress = GetCurveProgress(normalizedProgress);

                float v = EasingFunctions.ChangeFloat(0.0f, 1.0f, curveProgress, ease);
                Quaternion value = Quaternion.SlerpUnclamped(from , to , v);

                if (onUpdateQuaternion != null)
                    onUpdateQuaternion(value);
            }
        }
        
        internal override void Reset()
        {
            base.Reset();
            from = Quaternion.identity;
            to = Quaternion.identity;
            originalTo = Quaternion.identity;
            originalFrom = Quaternion.identity;
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
            var delta = to * Quaternion.Inverse(from);
            from = to;
            to = delta * to;
        }
    }
}
