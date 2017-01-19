using UnityEngine;


namespace HoloToolkit.Unity
{
    /// <summary>
    /// GazeManager determines the location of the user's gaze, hit position and normals.
    /// </summary>
    public class GazeManager : Singleton<GazeManager>
    {
        [Tooltip("Maximum gaze distance for calculating a hit.")]
        public float MaxGazeDistance = 105.0f;

        [Tooltip("Select the layers raycast should target.")]
        public LayerMask RaycastLayerMask = Physics.DefaultRaycastLayers;

        /// <summary>
        /// Physics.Raycast result is true if it hits a Hologram.
        /// </summary>
        public bool Hit { get; private set; }

        /// <summary>
        /// HitInfo property gives access
        /// to RaycastHit public members.
        /// </summary>
        public RaycastHit HitInfo { get; private set; }

        /// <summary>
        /// Position of the user's gaze.
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// RaycastHit Normal direction.
        /// </summary>
        public Vector3 Normal { get; private set; }

        private Vector3 gazeOrigin;
        private Vector3 gazeDirection;
        
        private void Update()
        {
            gazeOrigin = Camera.main.transform.position;
            gazeDirection = Camera.main.transform.forward;

            UpdateRaycast();
        }

        /// <summary>
        /// Calculates the Raycast hit position and normal.
        /// </summary>
        private void UpdateRaycast()
        {
            RaycastHit hitInfo;

            // Perform a Unity Physics Raycast.
            // Collect return value in public property Hit.
            // Pass in origin as gazeOrigin and direction as gazeDirection.
            // Collect the information in hitInfo.
            // Pass in MaxGazeDistance and RaycastLayerMask.
            Hit = Physics.Raycast(gazeOrigin, gazeDirection, out hitInfo, MaxGazeDistance, RaycastLayerMask);

            HitInfo = hitInfo;

            if (Hit)
            {
                Position = hitInfo.point;
                Normal = hitInfo.normal;
            }
            else
            {
                Position = gazeOrigin + (gazeDirection * MaxGazeDistance);
                Normal = gazeDirection;
            }
        }
    }
}
