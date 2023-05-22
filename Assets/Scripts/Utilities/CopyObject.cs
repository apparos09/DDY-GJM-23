/*
 * References:
 * https://docs.unity3d.com/ScriptReference/ExecuteInEditMode.html
 * https://docs.unity3d.com/ScriptReference/ExecuteAlways.html
 * https://www.youtube.com/watch?v=EFSXZO32h_0
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace util
{
    // copies the object.
    // this makes the script execute in edit mode.
    [ExecuteInEditMode]
    public class CopyObject : MonoBehaviour
    {
        // the direction of the copying.
        public enum repDirec { PositiveX, NegativeX, PositiveY, NegativeY, PositiveZ, NegativeZ };

        // the original object.
        [Tooltip("The original object the copy came from. Set automatically.")]
        public GameObject original;

        // if 'true', the original is used as the object's parent.
        [Tooltip("If true, the copies have the original as their parent transform. " +
            "If false, the copies keep the originals parent.")]
        public bool originalAsParent = false;

        // if 'true', the copies are inactive when generated. 
        [Tooltip("Deactivates copies if true. If false, the activeSelf value is kept from the parent.")]
        public bool deactivateCopies = false;

        // initiates in the start function.
        [Tooltip("Generates copies when the program starts. This is set to 'false' when the Start() function is finished.")]
        public bool copyOnStart = true;

        // the editor variables.
        [Header("Editor")]

        // if 'true', the copies are made in edit mode. This acts a button of sorts, and forces 'copyOnStart' to be true until it's done.
        [Tooltip("A button that generates copies in edit mode. This forces copyOnStart to be active until the copies are made."
            + "The copies must also be manually deleted by the user.")]
        public bool copyInEditMode = false;

        [Header("Iterations")]

        [Tooltip("The number of the generated copy. The original has a value of 0.")]
        public int iteration = 0;

        [Tooltip("The total amount of iterations for the copier.")]
        public int totalIterations = 0;

        [Header("Spacing")]

        // the replicator direction.
        [Tooltip("The translation direction of the copies.")]
        public repDirec direction;

        // if 'true', the direction is relative to the original object.
        // if 'false', the direction is relative to the standard Vector.
        [Tooltip("If true, shift objects relative to the original's orientation.")]
        public bool relativeToOriginal = true;

        // spacing of the object.
        [Tooltip("The spacing between the copies along the set direction.")]
        public float spacing = 1.0F;

        // if 'true' apply the object scale for spacing.
        [Tooltip("Apply the scale factor when spacing out iterations.")]
        public bool applyScaleForSpacing = true;

        // replication offsets
        [Tooltip("Offsets the position of the copy after 'spacing' is applied.")]
        public Vector3 offset = new Vector3(0.0F, 0.0F, 0.0F);

        // Start is called before the first frame update
        void Start()
        {
            // copy the object in edit mode, or if the application is playing.
            if (copyInEditMode || Application.isPlaying)
            {
                // should not make more copies once this runs once in edit mode.
                copyInEditMode = false;

                // this is the original game object.
                if (original == null)
                    original = gameObject;

                // generates the copies.
                if (copyOnStart)
                {
                    // sets this as false for generating copies on start.
                    // this is to prevent an infinte loop if the object has multiple instances of this script attached to it.
                    copyOnStart = false;

                    GenerateCopies();
                }
            }

        }



        // returns the iteration number of the variable.
        public int Iteration
        {
            get
            {
                return iteration;
            }
        }

        // gets the direction the copy will be translated in.
        public Vector3 GetDirection()
        {
            // the direction object.
            Vector3 direc;

            // chooses the direction.
            // 'relativeToOriginal' determines if it's relative to the object's transform, or if it's a standard vector.
            switch (direction)
            {
                case repDirec.PositiveX: // +x
                    direc = (relativeToOriginal) ? transform.right : Vector3.right;
                    break;

                case repDirec.NegativeX: // -x
                    direc = (relativeToOriginal) ? -transform.right : Vector3.left;
                    break;

                case repDirec.PositiveY: // +y
                    direc = (relativeToOriginal) ? transform.up : Vector3.up;
                    break;

                case repDirec.NegativeY: // -y
                    direc = (relativeToOriginal) ? -transform.up : Vector3.down;
                    break;

                default:
                case repDirec.PositiveZ: // +z
                    direc = (relativeToOriginal) ? transform.forward : Vector3.forward;
                    break;

                case repDirec.NegativeZ: // -z
                    direc = (relativeToOriginal) ? -transform.forward : Vector3.back;
                    break;
            }

            return direc;
        }

        // generates a copy, providing the iteration of said copy.
        public CopyObject GenerateCopy(int iter, bool deactivateCopy = false)
        {
            // makss a copy.
            CopyObject copy = Instantiate(this, transform.position, transform.rotation);

            // if 'true', it deactivates the copy.
            if (deactivateCopy)
                copy.gameObject.SetActive(false);

            // this is the original.
            copy.original = original;

            // saves iteration.
            copy.iteration = iter;

            // the direction
            Vector3 direc = GetDirection();

            // spaces out the copy.
            if (applyScaleForSpacing) // account for object's scale
            {
                // calculates the direction by the object's scale.
                Vector3 direcScaled = direc.normalized;
                direcScaled.Scale(transform.localScale);

                copy.transform.position += direcScaled * spacing * iter;
            }
            else // ignore object's scale
            {
                copy.transform.position += direc.normalized * spacing * iter;
            }


            // translates the copy by the offset.
            copy.transform.Translate(offset * iter);

            // if 'original' should be the parent of the copy.
            if (originalAsParent)
            {
                // checks if original is set. If it isn't, it uses this object.
                if (original != null)
                    copy.transform.parent = original.transform;
                else
                    copy.transform.parent = transform;
            }

            return copy;
        }

        // generates a series of copies in accordance with totalIterations.
        public void GenerateCopies()
        {
            GenerateCopies(totalIterations, deactivateCopies);
        }

        // generates a series of copies in accordance with totalIterations.
        public void GenerateCopies(int totalIters, bool deactivateAll = false)
        {
            // no iterations.
            if (totalIters <= 0)
                return;

            // goes through all original objects.
            for (int i = 1; i <= totalIters; i++)
            {
                // generates a copy.
                CopyObject copy = GenerateCopy(i, deactivateAll);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            // recalls the start function if this should run in edit mode.
            if (Application.isEditor && copyInEditMode)
            {
                // saves the original start value.
                bool startOrig = copyOnStart;

                // since this is running in the editor, this should be set to true.
                copyOnStart = true;

                // calls Start() to set things up.
                Start();

                // restores it so that it's set for next time.
                copyOnStart = startOrig;
            }
        }
    }
}