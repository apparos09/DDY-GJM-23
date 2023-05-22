using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace util
{
    // A struct for mouse buttons.
    public struct MouseButton
    {
        // The keycode for the mouse button.
        public KeyCode keyCode;

        // The object being held on by the mouse button.
        public GameObject held;

        // The object that was last clicked by the mouse button.
        public GameObject lastClicked;

    }

    // used for tracking inputs with either the mouse or the touch.
    public class MouseTouchInput : MonoBehaviour
    {
        // If 'true', Mouse operations are tracked.
        public bool trackMouse = true;

        // if 'true', the Touch operations are tracked.
        public bool trackTouch = true;

        // The mouse interaction.
        [Header("Mouse")]

        // If set to 'true', the UI is ignored for raycasting.
        // If set to 'false', the UI can block a raycast.
        [Tooltip("if true, the UI is ignored for raycast collisions. If false, UI elements can block a raycast.")]
        public bool ignoreUI = true;

        // // The mouse key for mouse operations. The default is Keycode.Mouse0, which is the left mouse button.
        // // TODO: add functions for all 6 mouse buttons (0 - 6)
        // public KeyCode mouseKey = KeyCode.Mouse0;

        // The world position of the mouse.
        // NOTE: it appears that the touch input is detected as a mouse input as well. The latest input overrides this variable.
        public Vector3 mouseWorldPosition;

        // // The check rate for the mouse hovering over entities. 
        // public float hoverCheckRate = 0.0F;
        // 
        // // The hover timer for shooting a ray to check for hovering objects.
        // private float hoverTimer = 0.0F;

        // The mouse buttons that are being checked.
        private MouseButton[] mouseButtons;

        [Header("Mouse/Interactions")]

        // The object the mouse is hovering over.
        public GameObject mouseHoveredObject = null;

        // // The object that has been clicked and held on.
        // // When the mouse button is released, this is set to null. This variable gets set to null when the mouse button is released.
        // public GameObject mouseHeldObject = null;
        // 
        // // The last object that was clicked on. The next time someone clicks on something, this will be set to null.
        // public GameObject mouseLastClickedObject = null;

        // The callback for the mouse hover and mouse button.
        public delegate void MouseHoverCallback(GameObject hitObject);
        public delegate void MouseButtonCallback(KeyCode mousekey, GameObject hitObject);

        // TODO: maybe add the rest of the mouse functions.

        // The callback for the mouse hovering over an object.
        private MouseHoverCallback mouseHoveredCallback;

        // The callback for the mouse being pressed over an object.
        private MouseButtonCallback mousePressedCallback;

        // The callback for the mouse being held.
        private MouseButtonCallback mouseHeldCallback;

        // The callback for the mouse being released.
        private MouseButtonCallback mouseUpCallback;

        [Header("Touch")]

        // These could be put into one struct, but then it wouldn't show up in the inspector.
        // TODO: maybe change this for later.

        // NOTE: the header won't show up if the first object is not something that can be shown to the inspector.
        // The touch list cannot be shown to the inspector, so the header does not appear.
        // The order has been changed to allow the header to show up.

        // The list of currently touched objects.
        // If no object has been touched then the shared index is set to null.
        // This will always be the same size as currentTouches. If no object was touched then the element will be empty.
        public List<GameObject> touchObjects = new List<GameObject>();

        // The current touches. This does NOT show up in the inspector.
        // The touched object will be removed from the list when it is let go, but the amount of touches will be retained...
        // for the saved touch.
        public List<Touch> currentTouches = new List<Touch>();

        // The callback for the touch.
        public delegate void TouchCallback(Touch touch, GameObject hitObject);

        // The callback for touch began interaction.
        private TouchCallback touchBeganCallback;

        // The callback for touch held interaction.
        private TouchCallback touchHeldCallback;

        // The callback for touch ended interaction.
        private TouchCallback touchEndedCallback;

        // The callback for touch cancelled interaction.
        private TouchCallback touchCanceledCallback;

        // Awake is called when the script instance is being loaded.
        void Awake()
        {
            // Has an index for each mouse button.
            // Unity supports 6 mouse buttons (Mouse0 - Mouse6, 323 - 329).
            mouseButtons = new MouseButton[7];

            // The mouse key.
            KeyCode mouseKey = KeyCode.Mouse0;

            // Goes through each mouse button.
            for (int i = 0; i < mouseButtons.Length; i++)
            {
                // Creates a new mouse button object.
                mouseButtons[i] = new MouseButton();

                // Current mouse key.
                mouseButtons[i].keyCode = mouseKey;

                // Sets these values to null.
                mouseButtons[i].held = null;
                mouseButtons[i].lastClicked = null;

                // Moves onto the next mouse key.
                if (i + 1 < mouseButtons.Length)
                    mouseKey += 1;
            }
        }

        // // Start is called just before any of the Update methods is called the first time.
        // private void Start()
        // {
        //     AddOnMousePressedCallback(OnMousePressedTest);
        // }

        // Gets the screen to world point using the main camera.
        public static Vector3 GetScreenToWorldPoint(Vector3 position)
        {
            return GetScreenToWorldPoint(Camera.main, position);
        }

        // Gets the screen to world point.
        public static Vector3 GetScreenToWorldPoint(Camera cam, Vector2 position)
        {
            if (cam.orthographic) // orthographic (2d camera) - uses near clip plane so that it's guaranteed to be positive.
                return cam.ScreenToWorldPoint(new Vector3(position.x, position.y, cam.nearClipPlane));
            else // perspective (3d camera)
                return cam.ScreenToWorldPoint(new Vector3(position.x, position.y, cam.focalLength));
        }



        // MOUSE INPUT //

        // Checks to see if the cursor is in the window.
        public static bool MouseInWindow()
        {
            return MouseInWindow(Camera.main);
        }

        // Checks to see if the cursor is in the window.
        public static bool MouseInWindow(Camera cam)
        {
            // Checks the x-axis and the y-axis.
            bool inX, inY;

            // Gets the viewport position.
            Vector3 viewPos = cam.ScreenToViewportPoint(Input.mousePosition);

            // Checks the horizontal and vertical.
            inX = (viewPos.x >= 0 && viewPos.x <= 1.0);
            inY = (viewPos.y >= 0 && viewPos.y <= 1.0);

            return (inX && inY);
        }

        // Gets the mouse position in screen space.
        public static Vector3 GetMousePositionInScreenSpace()
        {
            return Input.mousePosition;
        }

        // Gets the mouse position in viewport space.
        public static Vector3 GetMousePositionInViewport()
        {
            return Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        // Gets the mouse position in world space using the main camera.
        public static Vector3 GetMousePositionInWorldSpace()
        {
            // Gets the screen to world point of the mouse.
            return GetScreenToWorldPoint(Input.mousePosition);
        }

        // Gets the mouse position in world space.
        public static Vector3 GetMousePositionInWorldSpace(Camera cam)
        {
            // gets the screen to world point.
            return GetScreenToWorldPoint(cam, Input.mousePosition);
        }

        // Gets the mosut target position in world space using the main camera.
        public static Vector3 GetMouseTargetPositionInWorldSpace(GameObject refObject)
        {
            return GetMouseTargetPositionInWorldSpace(refObject.transform.position);
        }

        // Gets the mouse target position in world space with a reference vector.
        public static Vector3 GetMouseTargetPositionInWorldSpace(Vector3 refPos)
        {
            Vector3 camWPos = GetMousePositionInWorldSpace(Camera.main);
            Vector3 target = camWPos - refPos;
            return target;
        }

        // MOUSE BUTTONS
        // Mouse 0 (Left)
        public MouseButton MouseButton0
        {
            get { return mouseButtons[0]; }
        }

        // Mouse 1 (Right)
        public MouseButton MouseButton1
        {
            get { return mouseButtons[1]; }
        }

        // Mouse 2 (Middle)
        public MouseButton MouseButton2
        {
            get { return mouseButtons[2]; }
        }

        // Mouse 3 (Additional Button)
        public MouseButton MouseButton3
        {
            get { return mouseButtons[3]; }
        }

        // Mouse 4 (Additional Button)
        public MouseButton MouseButton4
        {
            get { return mouseButtons[4]; }
        }

        // Mouse 5 (Additional Button)
        public MouseButton MouseButton5
        {
            get { return mouseButtons[5]; }
        }

        // Mouse 6 (Additional Button)
        public MouseButton MouseButton6
        {
            get { return mouseButtons[6]; }
        }

        // Returns the mouse button.
        private MouseButton GetMouseButton(int index)
        {
            // Checks if the mouse button is valid.
            if (index < 0 || index >= mouseButtons.Length)
                return new MouseButton();
            else
                return mouseButtons[index];
        }



        // TOUCH INPUT
        // Gets the list of touches.
        public static Touch[] GetTouches()
        {
            // checks the touch count.
            if (Input.touchCount != 0)
            {
                // returns the inputs as an array.
                return Input.touches;
            }
            else
            {
                // returns an empty array.
                return new Touch[0];

            }
        }

        // Gets the touch position in world space using the main camera.
        public static Vector3 GetTouchPositionInWorldSpace(Touch touch)
        {
            return GetTouchPositionInWorldSpace(Camera.main, touch);
        }

        // Gets the touch position in world space.
        public static Vector3 GetTouchPositionInWorldSpace(Camera cam, Touch touch)
        {
            Vector3 wPos = GetScreenToWorldPoint(cam, touch.position);
            return wPos;
        }

        // Gets the touch target position in world space using the main camera.
        public static Vector3 GetTouchTargetPositionInWorldSpace(Touch touch, GameObject refObject)
        {
            return GetTouchTargetPositionInWorldSpace(Camera.main, touch, refObject.transform.position);
        }

        // Gets the touch target position in world space using the provided camera.
        public static Vector3 GetTouchTargetPositionInWorldSpace(Camera cam, Touch touch, GameObject refObject)
        {
            return GetTouchTargetPositionInWorldSpace(cam, touch, refObject.transform.position);
        }

        // Gets the touch target position in world space using the main camera.
        public static Vector3 GetTouchTargetPositionInWorldSpace(Touch touch, Vector3 refPos)
        {
            return GetTouchTargetPositionInWorldSpace(Camera.main, touch, refPos);
        }

        // Gets the touch target position in world space with a reference vector.
        public static Vector3 GetTouchTargetPositionInWorldSpace(Camera cam, Touch touch, Vector3 refPos)
        {
            Vector3 camWPos = GetTouchPositionInWorldSpace(cam, touch);
            Vector3 target = camWPos - refPos;
            return target;
        }

        // Gets the touched object. Returns null if object is not in list, or if no object was touched.
        public GameObject GetTouchedObject(Touch touch)
        {
            // Checks if it's in the list.
            if (currentTouches.Contains(touch)) // In list.
            {
                int index = currentTouches.IndexOf(touch);
                return touchObjects[index];
            }
            else // Not in list.
            {
                return null;
            }
        }



        // UPDATES //
        // Check the mouse input.
        private void MouseUpdate()
        {
            Vector3 target; // ray's target
            Ray ray; // ray object
            RaycastHit hitInfo; // info on hit.
            bool rayHit; // true if the ray hit.

            // if 'ignoreUI' is true, this is always false (the UI will never block the ray).
            // if 'ignoreUI' is false, then a check is done to see if a UI element is blocking the ray.
            bool rayBlocked = (ignoreUI) ? false : EventSystem.current.IsPointerOverGameObject();

            // gets the mouse position.
            mouseWorldPosition = GetMousePositionInWorldSpace();

            // Tracks what pressed functions should be called.
            bool[] pressedCalls = new bool[mouseButtons.Length];

            // if the ray is not blocked.
            if (!rayBlocked)
            {
                // checks if the camera is perspective or orthographic.
                if (Camera.main.orthographic) // orthographic
                {
                    // tries to get a hit. Since it's orthographic, the ray goes straight forward.
                    target = Camera.main.transform.forward; // target is into the screen (z-direction), so camera.forward is used.

                    // ray position is mouse position in world space.
                    ray = new Ray(mouseWorldPosition, target.normalized);

                    // the max distance is the far clip plane minus the near clip plane.
                    float maxDist = Camera.main.farClipPlane - Camera.main.nearClipPlane;

                    // cast the ray about as far as the camera can see.
                    rayHit = Physics.Raycast(ray, out hitInfo, maxDist);
                }
                else // perspective
                {
                    // the target of the ray.
                    target = GetMouseTargetPositionInWorldSpace(Camera.main.gameObject);

                    // ray object. It offsets so that objects not in the camera's clipping plane will be ignored.
                    ray = new Ray(Camera.main.transform.position + Camera.main.transform.forward * Camera.main.nearClipPlane,
                        target.normalized);

                    // the max distance is the far clip plane minus the near clip plane.
                    float maxDist = Camera.main.farClipPlane - Camera.main.nearClipPlane;

                    // the max distance
                    rayHit = Physics.Raycast(ray, out hitInfo, maxDist);
                }


                // checks if the ray got a hit. If it did, save the object the mouse is hovering over.
                // also checks if object has been clicked on.
                if (rayHit)
                {
                    mouseHoveredObject = hitInfo.collider.gameObject;

                    // Checks what mouse buttons have been pressed.
                    for (int i = 0; i < mouseButtons.Length; i++)
                    {
                        // If the current mouse button is being pressed down.
                        // TODO: maybe make this happen on a button press, regardless of if the ray hits anything.
                        if (Input.GetKeyDown(mouseButtons[i].keyCode))
                        {
                            // Marks the array for whether or not a pressed call for this button should happen.
                            pressedCalls[i] = mouseButtons[i].held != hitInfo.collider.gameObject;

                            // Held and Last Clicked
                            mouseButtons[i].held = hitInfo.collider.gameObject;
                            mouseButtons[i].lastClicked = hitInfo.collider.gameObject;
                        }
                    }
                }
                else
                {
                    // if the camera is orthographic, attempt a 2D raycast as well.
                    if (Camera.main.orthographic)
                    {
                        // setting up the 2D raycast for the orthographic camera.
                        RaycastHit2D rayHit2D = Physics2D.Raycast(
                            new Vector2(mouseWorldPosition.x, mouseWorldPosition.y),
                            new Vector2(target.normalized.x, target.normalized.y),
                            Camera.main.farClipPlane - Camera.main.nearClipPlane
                            );

                        // if a collider was hit, then the rayhit was successful.
                        rayHit = rayHit2D.collider != null;

                        // checks rayHit value.
                        if (rayHit)
                        {
                            // the ray hit was successful.
                            rayHit = true;

                            // saves the hovered over object.
                            mouseHoveredObject = rayHit2D.collider.gameObject;


                            // Checks what mouse buttons have been pressed.
                            for (int i = 0; i < mouseButtons.Length; i++)
                            {
                                // If the current mouse button is being pressed down.
                                if (Input.GetKeyDown(mouseButtons[i].keyCode))
                                {
                                    // Marks the array for whether or not a pressed call for this button should happen.
                                    pressedCalls[i] = mouseButtons[i].held != hitInfo.collider.gameObject;

                                    // Held and Last Clicked
                                    mouseButtons[i].held = rayHit2D.collider.gameObject;
                                    mouseButtons[i].lastClicked = rayHit2D.collider.gameObject;
                                }
                            }
                        }
                    }

                    // if ray hit was not successful.
                    // this means the 3D raycast failed, and the 2D raycast (orthographic only).
                    if (!rayHit)
                    {
                        // no object beng hovered over.
                        mouseHoveredObject = null;

                        // Checks all the moues buttons.
                        for (int i = 0; i < mouseButtons.Length; i++)
                        {
                            // If the current mouse button is being pressed down, clear the last clicked.
                            if (Input.GetKeyDown(mouseButtons[i].keyCode))
                            {
                                mouseButtons[i].lastClicked = null;
                            }
                        }
                    }
                }

                // Calls the hover callback. This gets set if the rayHit was successful (from either 3D or 2D check).
                if (mouseHoveredObject != null)
                    OnMouseHoveredCallback(mouseHoveredObject);
            }

            // Clear out held objects.
            for (int i = 0; i < mouseButtons.Length; i++)
            {
                // Calls the pressed callback.
                if (pressedCalls[i] == true)
                    OnMousePressedCallback(mouseButtons[i].keyCode, mouseButtons[i].held);

                // If the provided mouse key was released.
                if (Input.GetKeyUp(mouseButtons[i].keyCode))
                {
                    // Clears out the held object.
                    if (mouseButtons[i].held != null)
                    {
                        // Triggers the callback.
                        OnMouseUpCallback(mouseButtons[i].keyCode, mouseButtons[i].held);

                        // Sets the object to null.
                        mouseButtons[i].held = null;
                    }
                }
                else // The key is being held down.
                {
                    // Calls the held down callback.
                    if (mouseButtons[i].held != null)
                        OnMouseHeldCallback(mouseButtons[i].keyCode, mouseButtons[i].held);
                }
            }
        }

        // Clears out all of the last mouse clicks.
        public void ClearAllMouseLastClicks()
        {
            // Checks every mouse button.
            for (int i = 0; i < mouseButtons.Length; i++)
            {
                // Clears out the last click.
                mouseButtons[i].lastClicked = null;
            }
        }

        // Updates the touch tracking.
        public void TouchUpdate()
        {
            // clears out the list of current touches and touched objects.
            currentTouches.Clear();
            touchObjects.Clear();

            // If no touches are saved.
            if (Input.touchCount == 0)
                return;

            // Gets all of the touches.
            Touch[] touches = Input.touches;

            // Puts in the values from the new array.
            currentTouches = new List<Touch>(touches);

            // Goes through every touch.
            foreach (Touch touch in touches)
            {
                // The ray's target.
                Vector3 target;

                // The ray object to be casted, the hit info, and if it hit anything.
                Ray ray;
                RaycastHit hitInfo;
                bool rayHit;

                // Saves the touch position in world space.
                Vector2 touchWorldPos = GetTouchPositionInWorldSpace(touch);

                // The touched object.
                GameObject touchedObject = null;

                // Checks if the camera is perspective or orthographic.
                if (Camera.main.orthographic) // orthographic
                {
                    // Tries to get a hit. Since it's orthographic, the ray goes straight forward.
                    target = Camera.main.transform.forward; // target is into the screen (z-direction), so camera.forward is used.

                    // Ray position is touch position in world space.
                    ray = new Ray(touchWorldPos, target.normalized);

                    // the max distance is the far clip plane minus the near clip plane.
                    float maxDist = Camera.main.farClipPlane - Camera.main.nearClipPlane;

                    // cast the ray about as far as the camera can see.
                    rayHit = Physics.Raycast(ray, out hitInfo, maxDist);
                }
                else // perspective
                {
                    // the target of the ray.
                    target = GetTouchTargetPositionInWorldSpace(touch, Camera.main.gameObject);

                    // ray object. It offsets the positioning so that objects not in the camera's clipping plane will be ignored.
                    ray = new Ray(Camera.main.transform.position + Camera.main.transform.forward * Camera.main.nearClipPlane,
                        target.normalized);

                    // The max distance is the far clip plane minus the near clip plane.
                    float maxDist = Camera.main.farClipPlane - Camera.main.nearClipPlane;

                    // the max distance
                    rayHit = Physics.Raycast(ray, out hitInfo, maxDist);
                }

                // RAY HIT CHECK

                // Checks if the ray got a hit. If it did, save the object the touch hit.
                if (rayHit)
                {
                    // Saves the touched object.
                    touchedObject = hitInfo.collider.gameObject;
                }
                else // No object touched.
                {
                    // if the camera is orthographic, attempt a 2D raycast as well.
                    if (Camera.main.orthographic)
                    {
                        // setting up the 2D raycast for the orthographic camera.
                        RaycastHit2D rayHit2D = Physics2D.Raycast(
                            new Vector2(touchWorldPos.x, touchWorldPos.y),
                            new Vector2(target.normalized.x, target.normalized.y),
                            Camera.main.farClipPlane - Camera.main.nearClipPlane
                            );

                        // If a collider was hit, then the rayhit was successful.
                        rayHit = rayHit2D.collider != null;

                        // Checks rayHit value.
                        if (rayHit)
                        {
                            // Saves the touched object.
                            touchedObject = rayHit2D.collider.gameObject;
                        }
                    }
                }

                // Adds the touched object.
                // If this is set to null, then the space will be empty.
                touchObjects.Add(touchedObject);
            }

            // GOes through all touches.
            for (int i = 0; i < touches.Length; i++)
            {
                switch (touches[i].phase)
                {
                    case TouchPhase.Began: // Touch started.
                        OnTouchBeganCallback(touches[i], touchObjects[i]);
                        break;

                    case TouchPhase.Ended: // Touch ended.
                        OnTouchEndedCallback(touches[i], touchObjects[i]);
                        break;


                    case TouchPhase.Stationary: // Touch held.
                    case TouchPhase.Moved:
                        OnTouchHeldCallback(touches[i], touchObjects[i]);
                        break;

                    case TouchPhase.Canceled: // Touch cancelled.
                        OnTouchCanceledCallback(touches[i], touchObjects[i]);
                        break;
                }

            }
        }


        // CALLBACKS //
        // MOUSE CALLBACKS
        // MOUSE HOVER CALLBACK
        // On mouse hover add callback.
        public void AddOnMouseHoveredCallback(MouseHoverCallback callback)
        {
            mouseHoveredCallback += callback;
        }

        // On mouse hover remove callback.
        public void RemoveOnMouseHoveredCallback(MouseHoverCallback callback)
        {
            mouseHoveredCallback -= callback;
        }

        // Trigger mouse hover callback.
        private void OnMouseHoveredCallback(GameObject hitObject)
        {
            if (mouseHoveredCallback != null)
                mouseHoveredCallback(hitObject);
        }


        // MOUSE PRESSED CALLBACK
        // On mouse pressed add callback. This is only called on the first frame of the mouse being pressed.
        public void AddOnMousePressedCallback(MouseButtonCallback callback)
        {
            mousePressedCallback += callback;
        }

        // On mouse pressed remove callback. This is only called on the first frame of the mouse being pressed.
        public void RemoveOnMousePressedCallback(MouseButtonCallback callback)
        {
            mousePressedCallback -= callback;
        }

        // On mouse pressed callback. This is only called on the first frame of the mouse being pressed.
        private void OnMousePressedCallback(KeyCode mouseKey, GameObject hitObject)
        {
            if (mousePressedCallback != null)
                mousePressedCallback(mouseKey, hitObject);
        }

        // MOUSE HELD CALLBACK
        // On mouse held add callback.
        public void AddOnMouseHeldCallback(MouseButtonCallback callback)
        {
            mouseHeldCallback += callback;
        }

        // On mouse held remove callback.
        public void RemoveOnMouseHeldCallback(MouseButtonCallback callback)
        {
            mouseHeldCallback -= callback;
        }

        // Trigger mouse held callback.
        private void OnMouseHeldCallback(KeyCode mouseKey, GameObject hitObject)
        {
            if (mouseHeldCallback != null)
                mouseHeldCallback(mouseKey, hitObject);
        }

        // MOUSE RELEASED CALLBACK
        // On mouse up add callback.
        public void AddOnMouseUpCallback(MouseButtonCallback callback)
        {
            mouseUpCallback += callback;
        }

        // On mouse up remove callback.
        public void RemoveOnMouseUpCallback(MouseButtonCallback callback)
        {
            mouseUpCallback -= callback;
        }

        // Trigger on mouse released callback.
        private void OnMouseUpCallback(KeyCode mouseKey, GameObject hitObject)
        {
            if (mouseUpCallback != null)
                mouseUpCallback(mouseKey, hitObject);
        }



        // TOUCH CALLBACKS //
        // TOUCH BEGAN
        // On touch began add callback.
        public void AddOnTouchBeganCallback(TouchCallback callback)
        {
            touchBeganCallback += callback;
        }

        // On touch began remove callback.
        public void RemoveOnTouchBeganCallback(TouchCallback callback)
        {
            touchBeganCallback -= callback;
        }

        // On touch began callback.
        private void OnTouchBeganCallback(Touch touch, GameObject hitObject)
        {
            if (touchBeganCallback != null)
                touchBeganCallback(touch, hitObject);
        }

        // TOUCH HELD
        // On touch held add callback.
        public void AddOnTouchHeldCallback(TouchCallback callback)
        {
            touchHeldCallback += callback;
        }

        // On touch held remove callback.
        public void RemoveOnTouchHeldCallback(TouchCallback callback)
        {
            touchHeldCallback -= callback;
        }

        // On touch held callback.
        private void OnTouchHeldCallback(Touch touch, GameObject hitObject)
        {
            if (touchHeldCallback != null)
                touchHeldCallback(touch, hitObject);
        }

        // TOUCH ENDED
        // On touch ended add callback.
        public void AddOnTouchEndedCallback(TouchCallback callback)
        {
            touchEndedCallback += callback;
        }

        // On touch ended remove callback.
        public void RemoveOnTouchEndedCallback(TouchCallback callback)
        {
            touchEndedCallback -= callback;
        }

        // On touch ended callback.
        private void OnTouchEndedCallback(Touch touch, GameObject hitObject)
        {
            if (touchEndedCallback != null)
                touchEndedCallback(touch, hitObject);
        }

        // TOUCH CANCELLED
        // On touch cancelled add callback.
        public void AddOnTouchCanceledCallback(TouchCallback callback)
        {
            touchCanceledCallback += callback;
        }

        // On touch cancelled remove callback.
        public void RemoveOnTouchCanceledCallback(TouchCallback callback)
        {
            touchCanceledCallback -= callback;
        }

        // On touch cancelled callback.
        private void OnTouchCanceledCallback(Touch touch, GameObject hitObject)
        {
            if (touchCanceledCallback != null)
                touchCanceledCallback(touch, hitObject);
        }


        // Test function.
        private void OnMousePressedTest(KeyCode mouseKey, GameObject hitObject)
        {
            Debug.Log("This is a test. This is only a test.");
        }


        // Update is called once per frame
        void Update()
        {
            // If the mouse should be tracked.
            if (trackMouse)
                MouseUpdate();

            // If the touch should be tracked.
            if (trackTouch)
                TouchUpdate();

            // // TODO: comment out.
            // // Prints Out Left Mouse Button Information.
            // {
            //     MouseButton mb = mouseButtons[1];
            //     Debug.Log("KeyCode: " + mb.keyCode.ToString());
            //     Debug.Log(mb.keyCode.ToString() + " - Mouse Held: " + ((mb.held != null) ? mb.held.name : ""));
            //     Debug.Log(mb.keyCode.ToString() + " - Mouse Last Click: " + ((mb.lastClicked != null) ? mb.lastClicked.name : ""));
            //     Debug.Log("");
            // }
        }
    }
}