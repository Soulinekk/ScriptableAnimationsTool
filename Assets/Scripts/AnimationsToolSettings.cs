using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParkingGame
{
    namespace AnimationTool
    {
        //[ExecuteInEditMode]
        public class AnimationsToolSettings : MonoBehaviour
        {
            private Transform objectToAnimate; // For now used same like this.transform, but in future im thinking about useing this for pivot point

            private float posLerpStartTime;
            public bool positionIsLerping;
            public bool positionAnimationsEnabled;
            public Vector3[] positionVectors;
			public List<Vector3> positionVectorsList;
            public enum PosMode { StartEndStart, Continuous, StartEnd };
            public PosMode positionMode;
            public float positionSpeed;
			public float posLerpProgress;

            //public bool calculateRotation;
            private float rotLerpStartTime;
            public bool rotationAnimationsEnabled;
            public List<Vector3> rotationVectors;
            public bool rotationIsLerping;
            public enum RotMode { Continuous, StartEndStart, StartEnd };
            public RotMode rotationMode;
            public float rotationSpeed;
            public float rotLerpProgress;
            public bool smoothLerping;
            public enum continuousCalculation { Additive, TowardDirection };
            public continuousCalculation continuousRotationHandling;
            public Vector3 additiveValueForContinuousRotation;
            //public bool rotateByCustomPivot; //* Not sure if i want to implement it
            //public Transform pivotTransform; //* Not sure if i want to implement it

            private void Awake()
            {
                objectToAnimate = this.gameObject.transform;
            }

            private void Start()
            {
                StartCoroutine("PositionAnimationsCoroutine");
                StartCoroutine("RotationAnimationsCoroutine");
            }

            IEnumerator PositionAnimationsCoroutine()
            {
                float xTemp, yTemp, zTemp;

                if (positionIsLerping)
                {
                    switch (positionMode)
                    {
#region Mode: Start-End-Start lerp
                        case PosMode.StartEndStart:
                            for (int i = 0; i < positionVectors.Length-1; i++)
                            {
								posLerpStartTime = Time.time;
								posLerpProgress = 0;
								while (posLerpProgress < 1)
								{
									yield return new WaitForEndOfFrame();
									posLerpProgress = (Time.time - posLerpStartTime) / positionSpeed;
									xTemp = Mathf.SmoothStep(positionVectors[i].x, positionVectors[i + 1].x, posLerpProgress);
									yTemp = Mathf.SmoothStep(positionVectors[i].y, positionVectors[i + 1].y, posLerpProgress);
									zTemp = Mathf.SmoothStep(positionVectors[i].z, positionVectors[i + 1].z, posLerpProgress);
                                	gameObject.transform.position = new Vector3(xTemp, yTemp, zTemp);
                                    if (positionIsLerping != true) break;
								}
                            }
                            for (int i = positionVectors.Length - 1; i > 0; i--)
                            {
                                posLerpStartTime = Time.time;
                                posLerpProgress = 0;
                                while (posLerpProgress < 1)
                                {
                                    yield return new WaitForEndOfFrame();
                                    posLerpProgress = (Time.time - posLerpStartTime) / positionSpeed;
                                    xTemp = Mathf.SmoothStep(positionVectors[i].x, positionVectors[i - 1].x, posLerpProgress);
                                    yTemp = Mathf.SmoothStep(positionVectors[i].y, positionVectors[i - 1].y, posLerpProgress);
                                    zTemp = Mathf.SmoothStep(positionVectors[i].z, positionVectors[i - 1].z, posLerpProgress);
                                    gameObject.transform.position = new Vector3(xTemp, yTemp, zTemp);
                                    if (positionIsLerping != true) break;
                                }
                            }
                            break;
#endregion

#region Continuous lerp
                        case PosMode.Continuous:
                            Vector3 normalizedDirection = (positionVectors[1] - positionVectors[0]).normalized;
                            transform.position = positionVectors[0];
							while (positionIsLerping)
							{
								yield return new WaitForEndOfFrame();
                                transform.position += normalizedDirection * positionSpeed * Time.deltaTime;
							}
                            break;
#endregion

#region Start-End lerp
                        case PosMode.StartEnd:
                            for (int i = 0; i < positionVectors.Length - 1; i++)
                            {
                                posLerpStartTime = Time.time;
                                posLerpProgress = 0;
                                while (posLerpProgress < 1)
                                {
                                    yield return new WaitForEndOfFrame();
                                    posLerpProgress = (Time.time - posLerpStartTime) / positionSpeed;
                                    xTemp = Mathf.SmoothStep(positionVectors[i].x, positionVectors[i + 1].x, posLerpProgress);
                                    yTemp = Mathf.SmoothStep(positionVectors[i].y, positionVectors[i + 1].y, posLerpProgress);
                                    zTemp = Mathf.SmoothStep(positionVectors[i].z, positionVectors[i + 1].z, posLerpProgress);
                                    gameObject.transform.position = new Vector3(xTemp, yTemp, zTemp);
                                    if (positionIsLerping != true) break;
                                }
                            }
                            break;
#endregion
                    }
                }
                yield return null;
            }

            IEnumerator RotationAnimationsCoroutine()
            {
                float xTemp, yTemp, zTemp;

                if (rotationIsLerping)
                {
                    switch (rotationMode)
                    {
#region Continuous lerp
                        case RotMode.Continuous:
                            if(continuousRotationHandling == continuousCalculation.Additive)
                            {
                                while (rotationIsLerping)
                                {
                                    yield return new WaitForEndOfFrame();
                                    xTemp = additiveValueForContinuousRotation.x * rotationSpeed * Time.deltaTime;
                                    yTemp = additiveValueForContinuousRotation.y * rotationSpeed * Time.deltaTime;
                                    zTemp = additiveValueForContinuousRotation.z * rotationSpeed * Time.deltaTime;

                                    transform.Rotate(xTemp,yTemp,zTemp);
                                }
                                /*Vector3 normalizedDirection = (rotationVectors[1] - rotationVectors[0]).normalized;
                                objectToAnimate.rotation = Quaternion.Euler(rotationVectors[0]);
                                while (rotationIsLerping)
                                {
                                    yield return new WaitForEndOfFrame();
                                    xTemp = objectToAnimate.localEulerAngles.x + (normalizedDirection.x * rotationSpeed * Time.deltaTime);
                                    yTemp = objectToAnimate.localEulerAngles.y + (normalizedDirection.y * rotationSpeed * Time.deltaTime);
                                    zTemp = objectToAnimate.localEulerAngles.z + (normalizedDirection.z * rotationSpeed * Time.deltaTime);

                                    objectToAnimate.localEulerAngles = new Vector3(xTemp, yTemp, zTemp);
                                }*/
                            }
                            else
                            {
                                //TODO Continuous lerp - Doesnt work for now!!
                                Vector3 normalizedDirection = (rotationVectors[1] - rotationVectors[0]).normalized;
                                objectToAnimate.rotation = Quaternion.Euler(rotationVectors[0]);
                                while (rotationIsLerping)
                                {
                                    yield return new WaitForEndOfFrame();
                                    xTemp = objectToAnimate.localEulerAngles.x + (normalizedDirection.x * rotationSpeed * Time.deltaTime);
                                    yTemp = objectToAnimate.localEulerAngles.y + (normalizedDirection.y * rotationSpeed * Time.deltaTime);
                                    zTemp = objectToAnimate.localEulerAngles.z + (normalizedDirection.z * rotationSpeed * Time.deltaTime);

                                    objectToAnimate.localEulerAngles = new Vector3(xTemp, yTemp, zTemp);
                                }
                            }
                            
                            break;
#endregion

#region Start-End-Start lerp
                        case RotMode.StartEndStart:

                            for (int i = 0; i < rotationVectors.Count - 1; i++)
                            {
                                Quaternion fromRotation, toRotation;

                                rotLerpStartTime = Time.time;
                                rotLerpProgress = 0;
                                if (smoothLerping)
                                {
                                    while (rotLerpProgress < 1)
                                    {
                                        yield return new WaitForEndOfFrame();
                                        rotLerpProgress = (Time.time - rotLerpStartTime) / rotationSpeed;
                                        fromRotation = Quaternion.Euler(rotationVectors[i].x, rotationVectors[i].y, rotationVectors[i].z);
                                        toRotation = Quaternion.Euler(rotationVectors[i + 1].x, rotationVectors[i + 1].y, rotationVectors[i + 1].z);
                                        objectToAnimate.rotation = Quaternion.Slerp(fromRotation, toRotation, rotLerpProgress);
                                        if (rotationIsLerping != true) break;
                                    }
                                }
                                else
                                {
                                    while (rotLerpProgress < 1)
                                    {
                                        yield return new WaitForEndOfFrame();
                                        rotLerpProgress = (Time.time - rotLerpStartTime) / rotationSpeed;
                                        fromRotation = Quaternion.Euler(rotationVectors[i].x, rotationVectors[i].y, rotationVectors[i].z);
                                        toRotation = Quaternion.Euler(rotationVectors[i + 1].x, rotationVectors[i + 1].y, rotationVectors[i + 1].z);
                                        objectToAnimate.rotation = Quaternion.Lerp(fromRotation, toRotation, rotLerpProgress);
                                        if (rotationIsLerping != true) break;
                                    }
                                }
                            }
                            for (int i = rotationVectors.Count - 1; i > 0; i--)
                            {
                                Quaternion fromRotation, toRotation;

                                rotLerpStartTime = Time.time;
                                rotLerpProgress = 0;
                                if (smoothLerping)
                                {
                                    while (rotLerpProgress < 1)
                                    {
                                        yield return new WaitForEndOfFrame();
                                        rotLerpProgress = (Time.time - rotLerpStartTime) / rotationSpeed;
                                        fromRotation = Quaternion.Euler(rotationVectors[i].x, rotationVectors[i].y, rotationVectors[i].z);
                                        toRotation = Quaternion.Euler(rotationVectors[i - 1].x, rotationVectors[i - 1].y, rotationVectors[i - 1].z);
                                        objectToAnimate.rotation = Quaternion.Slerp(fromRotation, toRotation, rotLerpProgress);
                                        if (rotationIsLerping != true) break;
                                    }
                                }
                                else
                                {
                                    while (rotLerpProgress < 1)
                                    {
                                        yield return new WaitForEndOfFrame();
                                        rotLerpProgress = (Time.time - rotLerpStartTime) / rotationSpeed;
                                        fromRotation = Quaternion.Euler(rotationVectors[i].x, rotationVectors[i].y, rotationVectors[i].z);
                                        toRotation = Quaternion.Euler(rotationVectors[i - 1].x, rotationVectors[i - 1].y, rotationVectors[i - 1].z);
                                        objectToAnimate.rotation = Quaternion.Lerp(fromRotation, toRotation, rotLerpProgress);
                                        if (rotationIsLerping != true) break;
                                    }
                                }
                            }
                            break;
#endregion

#region Start-End lerp
                        case RotMode.StartEnd:
                            for (int i = 0; i < rotationVectors.Count - 1; i++)
                            {
                                Quaternion fromRotation, toRotation;

                                rotLerpStartTime = Time.time;
                                rotLerpProgress = 0;
                                if (smoothLerping)
                                {
                                    while(rotLerpProgress < 1)
                                    {
                                        yield return new WaitForEndOfFrame();
                                        rotLerpProgress = (Time.time - rotLerpStartTime) / rotationSpeed;
                                        fromRotation = Quaternion.Euler(rotationVectors[i].x, rotationVectors[i].y, rotationVectors[i].z);
                                        toRotation = Quaternion.Euler(rotationVectors[i + 1].x, rotationVectors[i + 1].y, rotationVectors[i + 1].z);
                                        objectToAnimate.rotation = Quaternion.Slerp(fromRotation, toRotation, rotLerpProgress);
                                        if (rotationIsLerping != true) break;
                                    }
                                }
                                else
                                {
                                    while (rotLerpProgress < 1)
                                    {
                                        yield return new WaitForEndOfFrame();
                                        rotLerpProgress = (Time.time - rotLerpStartTime) / rotationSpeed;
                                        fromRotation = Quaternion.Euler(rotationVectors[i].x, rotationVectors[i].y, rotationVectors[i].z);
                                        toRotation = Quaternion.Euler(rotationVectors[i + 1].x, rotationVectors[i + 1].y, rotationVectors[i + 1].z);
                                        objectToAnimate.rotation = Quaternion.Lerp(fromRotation, toRotation, rotLerpProgress);
                                        if (rotationIsLerping != true) break;
                                    }
                                }
                            }
                            break;
#endregion
                    }
                }

                yield return null;
            }

            void FinishPositionAnimationCor()
            {
                if (positionIsLerping)
                {
                    positionIsLerping = false;
                    StopCoroutine("PositionAnimationsCor");
                }
            }
        }

        
    }
}
