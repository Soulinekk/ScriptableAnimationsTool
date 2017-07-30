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
            public bool positionIsLerping;
            public bool positionAnimationsEnabled;
            public int PositionSteps
            {
                get
                {
                    return PositionSteps;
                }
                set
                {
                    Debug.Log("!");
                    PositionSteps = value;
                    positionVectors = new Vector3[value];
                }
            }
            public Vector3[] positionVectors;
			public List<Vector3> positionVectorsList;
            public enum PosMode { StartEndStart, Continuous, StartEnd };
            public PosMode positionMode;
            public float positionSpeed;
			private float posLerpStartTime;
			public float posLerpProgress;

      /*    private int rotationSteps;
            public int RotationSteps
            {
                get
                {
                    return rotationSteps;
                }
                set
                {
                    rotationSteps = value;
                    rotationVectors = new Vector3[value];
                }
            }*/
            //public bool calculateRotation;
            //private Vector3[] rotationVectors;

            private void Start()
            {
				//positionMode = PosMode.Continuous;
                StartCoroutine("PositionAnimationsCor");
            }

            IEnumerator PositionAnimationsCor()
            {
                float xTemp, yTemp, zTemp;

                if (positionIsLerping)
                {
                    switch (positionMode)
                    {
                        #region
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

                        #region
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

                        #region
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
