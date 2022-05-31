using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitCamera : MonoBehaviour
{
    public int Height;
    public int Width;

    private void Update()
    {
        MatchCamera();
    }

    /// <summary>
    /// Change the scaling of the camera so the board stays centred and scaled to fit.
    /// </summary>
    private void MatchCamera()
    {
        // https://stackoverflow.com/questions/58341038/how-to-adjust-the-size-of-sprites-according-to-different-screen-resolutions-in-u
        var cam = GetComponent<Camera>();
        if (cam == null) return;

        var position = cam.ViewportToWorldPoint(Vector3.zero);
        var up = cam.ViewportToWorldPoint(Vector3.up) - position;
        var right = cam.ViewportToWorldPoint(Vector3.right) - position;

        var matchSize = Mathf.Max(Height, Width * up.magnitude / right.magnitude);

        cam.orthographicSize = matchSize;
    }
}
