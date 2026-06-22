using UnityEngine;
using System.Collections.Generic;

namespace F8Framework.Core
{
    public class PathTween : BaseTween
    {
        private Transform targetTransform;
        private Vector3[] path;
        private PathType pathType = PathType.CatmullRom;
        private PathMode pathMode = PathMode.Ignore;
        private int resolution = 10;
        private bool closePath = false;
        
        private PathCalculator pathCalculator;
        private Vector3 tempValue = Vector3.zero;
        private Quaternion tempRotation = Quaternion.identity;
        private bool isLocalPath = false;
        
        public PathTween(Transform target, IList<Vector3> path, float duration, PathType pathType, PathMode pathMode, int resolution, bool closePath, bool isLocalPath, int id)
        {
            this.id = id;
            Init(target, path, duration, pathType, pathMode, resolution, closePath, isLocalPath);
        }

        internal void Init(Transform target, IList<Vector3> path, float duration, PathType pathType, PathMode pathMode, int resolution, bool closePath, bool isLocalPath = false)
        {
            this.targetTransform = target;
            if (path is Vector3[] array)
            {
                this.path = array;
            }
            else
            {
                this.path = new Vector3[path.Count];
                for (int i = 0; i < path.Count; i++)
                {
                    this.path[i] = path[i];
                }
            }
            this.duration = duration;
            this.pathType = pathType;
            this.pathMode = pathMode;
            this.resolution = Mathf.Max(2, resolution);
            this.closePath = closePath;
            this.isLocalPath = isLocalPath;
            
            // 初始化路径计算器
            InitializePathCalculator();
            
            this.PauseReset = () => this.Init(target, path, duration, pathType, pathMode, resolution, closePath, isLocalPath);
        }

        /// <summary>
        /// 初始化路径计算器
        /// </summary>
        private void InitializePathCalculator()
        {
            if (path == null || path.Length < 2)
                return;
                
            var options = new PathOptions
            {
                pathType = pathType,
                pathMode = pathMode,
                resolution = resolution,
                closePath = closePath
            };
            
            pathCalculator = new PathCalculator(path, options);
        }

        internal override void Update(float deltaTime)
        {
            if (isPause || IsComplete || IsRecycle || targetTransform == null || pathCalculator == null)
                return;

            if (tempDelay > 0.0f)
            {
                tempDelay -= deltaTime;
                return;
            }
            
            UpdateEvents(deltaTime);

            currentTime += deltaTime;
            
            if (currentTime >= duration)
            {
                this.UpdateValue(true);
                
                bool shouldComplete = !HandleLoop();
                if (shouldComplete)
                    onComplete();
            }
            else
            {
                this.UpdateValue(false);
            }
        }

        internal override void UpdateValue(bool isEnd = false)
        {
            base.UpdateValue(isEnd);
            if (isEnd)
            {
                UpdateTransform(1f);
            }
            else
            {
                float normalizedProgress = currentTime >= duration ? 1.0f : currentTime / duration;
                // 通过曲线函数计算缓动进度
                float curveProgress = GetCurveProgress(normalizedProgress);
                
                // 更新变换
                UpdateTransform(curveProgress);
            }
        }
        
        /// <summary>
        /// 更新位置和旋转
        /// </summary>
        private void UpdateTransform(float progress)
        {
            if (pathCalculator == null || targetTransform == null)
                return;
                
            // 获取路径上的位置
            Vector3 newPosition;
            if (loopType == LoopType.Yoyo && progress >= 1f)
            {
                newPosition = pathCalculator.GetPoint(0f);
            }
            else
            {
                newPosition = pathCalculator.GetPoint(progress);
            }
            tempValue = newPosition;
            if (isLocalPath)
            {
                targetTransform.localPosition = newPosition;
            }
            else
            {
                targetTransform.position = newPosition;
            }
            
            // 处理朝向
            UpdateRotation(progress, newPosition);
            
            // 触发值更新回调
            if (onUpdateVector3 != null)
                onUpdateVector3(tempValue);

            if (onUpdateVector2 != null)
                onUpdateVector2(tempValue);
        }

        /// <summary>
        /// 更新旋转
        /// </summary>
        private void UpdateRotation(float progress, Vector3 currentPosition)
        {
            if (pathMode == PathMode.Ignore)
                return;
                
            Vector3 direction = pathCalculator.GetDirection(progress);
            if (direction == Vector3.zero)
                return;
                
            switch (pathMode)
            {
                case PathMode.Full3D:
                    // 3D朝向，Z轴指向移动方向
                    tempRotation = Quaternion.LookRotation(direction);
                    if (isLocalPath)
                    {
                        targetTransform.localRotation = tempRotation;
                    }
                    else
                    {
                        targetTransform.rotation = tempRotation;
                    }
                    break;
                    
                case PathMode.TopDown2D:
                    // 俯视2D，在XY平面内旋转
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    tempRotation = Quaternion.Euler(0, 0, angle - 90);
                    if (isLocalPath)
                    {
                        targetTransform.localRotation = tempRotation;
                    }
                    else
                    {
                        targetTransform.rotation = tempRotation;
                    }
                    break;
                    
                case PathMode.Sidescroller2D:
                    // 横版2D，根据X方向翻转
                    UpdateSidescrollerRotation(direction);
                    break;
            }
        }

        /// <summary>
        /// 更新横版2D旋转
        /// </summary>
        private void UpdateSidescrollerRotation(Vector3 direction)
        {
            if (Mathf.Abs(direction.x) > 0.1f)
            {
                Vector3 scale = targetTransform.localScale;
                scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction.x);
                targetTransform.localScale = scale;
            }
        }

        /// <summary>
        /// 设置是否闭合路径
        /// </summary>
        internal PathTween SetClosePath(bool close)
        {
            this.closePath = close;
            InitializePathCalculator(); // 重新初始化计算器
            return this;
        }

        internal override void Reset()
        {
            base.Reset();
            targetTransform = null;
            path = null;
            pathCalculator = null;
            pathType = PathType.CatmullRom;
            pathMode = PathMode.Ignore;
            resolution = 10;
            closePath = false;
            isLocalPath = false;
        }

        public override BaseTween ReplayReset()
        {
            base.ReplayReset();
            return this;
        }
        
        public override BaseTween LoopReset()
        {
            base.LoopReset();
            return this;
        }
        

    }
}