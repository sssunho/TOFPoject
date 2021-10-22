using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class CameraHandler : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerManager playerManager;
        Camera camera;
        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        private Transform myTransform;
        private Vector3 cameraTransformPosition;
        public LayerMask ignoreLayer;
        public LayerMask environmentLayer;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;

        public float lookSpeed = 0.001f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        private float targetPosition;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        private bool normal = true;
        //private float zoom = 1f;
        private float smooth = 0.3f;
        private float minSize = 25f;
        private float maxSize = 40f;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;
        public float lockedPivotPosition = 2.25f;
        public float unlockedPivotPosition = 1.65f;

        public CharacterManager currentLockOnTarget;

        List<CharacterManager> availableTargets = new List<CharacterManager>();
        public CharacterManager nearestLockOnTarget;
        public CharacterManager leftLockTarget;
        public CharacterManager rightLockTarget;
        public float maximumLockOnDistance = 30;

        private void Awake()
        {
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayer = ~(1 << 8 | 1 << 9 | 1 << 10);

            FindTarget();
            //targetTransform = FindObjectOfType<PlayerManager>().transform;
            //inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
            camera = GetComponentInChildren<Camera>();
        }

        private void Start()
        {
            environmentLayer = LayerMask.NameToLayer("Environment");
        }

        private void FixedUpdate()
        {
            if (normal)
            {
            }
            else 
            {
                camera.fieldOfView -= smooth;
                camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minSize, maxSize);
            }
        }

        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp
                (myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
            myTransform.position = targetPosition;

            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if (playerManager.isPassing)
            {
            }
            else if (inputHandler.lockOnFlag == false && currentLockOnTarget == null)
            {
                lookAngle += (mouseXInput * lookSpeed) / delta;
                pivotAngle -= (mouseYInput * pivotSpeed) / delta;
                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

                Vector3 rotation = Vector3.zero;
                rotation.y = lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                myTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            else if (currentLockOnTarget != null)
            {
                Vector3 direction = currentLockOnTarget.transform.position - transform.position;
                Vector3 right = Vector3.Cross(direction, Vector3.up).normalized;
                Quaternion look = Quaternion.LookRotation(direction);

                cameraPivotTransform.rotation = look;
                cameraPivotTransform.RotateAround(cameraPivotTransform.position, right, -20.0f);
            }
        }

        private void HandleCameraCollisions(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast
                (cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayer))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffset);
            }

            if(Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }

        public void HandleLokOn()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                if (character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    float disatanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                    RaycastHit hit;

                    if (character.transform.root != targetTransform.root && viewableAngle > -50 &&
                        viewableAngle < 50 && disatanceFromTarget <= maximumLockOnDistance)
                    {
                        if(Physics.Linecast(playerManager.lockOnTransform.position,character.lockOnTransform.position, out hit))
                        {

                            if(hit.transform.gameObject.layer == environmentLayer)
                            {

                            }
                            else
                            {
                                availableTargets.Add(character);
                            }
                        }
                    }
                }
            }

            for (int k = 0; k < availableTargets.Count; k++)
            {
                float disatanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[k].transform.position);

                if(disatanceFromTarget<shortestDistance)
                {
                    shortestDistance = disatanceFromTarget;
                    nearestLockOnTarget = availableTargets[k];
                }

                if (inputHandler.lockOnFlag)
                {
                    Vector3 relativeEnemyPositon = inputHandler.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    var distanceFromLeftTarget = relativeEnemyPositon.x;
                    var distanceFromRightTarget = relativeEnemyPositon.x;

                    if (relativeEnemyPositon.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget
                        && availableTargets[k] != currentLockOnTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = availableTargets[k];
                    }
                    else if (relativeEnemyPositon.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget
                        && availableTargets[k] != currentLockOnTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = availableTargets[k];
                    }
                }
            }
        }

        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPositon = new Vector3(0, lockedPivotPosition);
            Vector3 newUnLockedPositon = new Vector3(0, unlockedPivotPosition);

            if(currentLockOnTarget!=null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, newLockedPositon, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, newUnLockedPositon, ref velocity, Time.deltaTime);
            }
        }

        public void PassingShot()
        {
            playerManager.isPassing = true;
            normal = false;
        }

        public void NormalShot()
        {
            playerManager.isPassing = false;
            camera.fieldOfView = 40;
            normal = true;
        }

        public void FindTarget()
        {
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
        }
    }
}

