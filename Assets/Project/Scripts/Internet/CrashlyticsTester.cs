using UnityEngine;

/* Source of code below:
 * https://firebase.google.com/codelabs/understand-unity-games-crashes-using-advanced-crashlytics#3 */
public class CrashlyticsTester : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        // Tests your Crashlytics implementation by
        // throwing an exception every 60 frames.
        // You should see reports in the Firebase console
        // a few minutes after running your app with this method.
        if (Time.frameCount > 0 && (Time.frameCount % 60) == 0)
        {
            throw new System.Exception("Test exception; please ignore");
        }
    }
}