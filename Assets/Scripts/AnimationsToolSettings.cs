using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParkingGame
{
    namespace AnimationTool
    {
        [ExecuteInEditMode]
        public class AnimationsToolSettings : MonoBehaviour
        {
            public bool positionIsLerping = false;
            public bool positionAnimationsEnabled = false;
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
            public enum PosMode { StartEndStart, Continuous, StartEnd };
            public PosMode positionMode;
            public float positionSpeed;

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
            public bool calculateRotation;
            private Vector3[] rotationVectors;

            private void Start()
            {
                positionMode = PosMode.StartEndStart;
                Debug.Log("Start");
                StartCoroutine("PositionAnimationsCor");
            }

            IEnumerator PositionAnimationsCor()
            {
                int step;
                float xTemp, yTemp, zTemp;
                if (positionIsLerping)
                {
                    switch (positionMode)
                    {
                        case PosMode.StartEndStart:
                            for (int i = 0; i < positionVectors.Length-1; i++)
                            {
                                xTemp = Mathf.SmoothStep(positionVectors[i].x, positionVectors[i + 1].x, Time.deltaTime * positionSpeed);
                                yTemp = Mathf.SmoothStep(positionVectors[i].y, positionVectors[i + 1].y, Time.deltaTime * positionSpeed);
                                zTemp = Mathf.SmoothStep(positionVectors[i].z, positionVectors[i + 1].z, Time.deltaTime * positionSpeed);
                                gameObject.transform.position = new Vector3(xTemp, yTemp, zTemp);
                            }
                            break;

                        case PosMode.Continuous:

                            break;

                        case PosMode.StartEnd:

                            break;
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
