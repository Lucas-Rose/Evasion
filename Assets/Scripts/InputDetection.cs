using Liminal.SDK.Core;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class InputDetection : MonoBehaviour
{
    private Animator anim;
    private GameManager gManager;
    public GameObject pHitbox;

    private void Start()
    {
        anim = GetComponent<Animator>();
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pHitbox.GetComponent<Collider>().enabled = false;

    }

    private void Update()
    {
        /*var device = VRDevice.Device;
        if (device != null)
        {
            if (device.GetButtonDown(VRButton.Primary))
            {
                anim.SetTrigger("fade");
                gManager.StartSeated();
            }
            if (device.GetButtonDown(VRButton.Back))
            {
                anim.SetTrigger("fade");
                gManager.StartStanding();
            }
        }*/
    }

    //List of Available Controls
    public void AppendDeviceInput(StringBuilder builder, IVRInputDevice inputDevice, string deviceName)
    {
        if (inputDevice == null)
            return;

        builder.AppendLine($"{deviceName} Back: {inputDevice.GetButton(VRButton.Back)}");
        builder.AppendLine($"{deviceName} Touch Pad Touching: {inputDevice.IsTouching}");
        builder.AppendLine($"{deviceName} Trigger: {inputDevice.GetButton(VRButton.Trigger)}");
        builder.AppendLine($"{deviceName} Primary: {inputDevice.GetButton(VRButton.Primary)}");
        builder.AppendLine($"{deviceName} Seconday: {inputDevice.GetButton(VRButton.Seconday)}");
        builder.AppendLine($"{deviceName} Three: {inputDevice.GetButton(VRButton.Three)}");
        builder.AppendLine($"{deviceName} Four: {inputDevice.GetButton(VRButton.Four)}");

        builder.AppendLine($"{deviceName} Axis One: {inputDevice.GetAxis2D(VRAxis.One)}");
        builder.AppendLine($"{deviceName} Axis One Raw: {inputDevice.GetAxis2D(VRAxis.OneRaw)}");

        builder.AppendLine($"{deviceName} Axis Two: {inputDevice.GetAxis1D(VRAxis.Two)}");
        builder.AppendLine($"{deviceName} Axis Two Raw: {inputDevice.GetAxis1D(VRAxis.TwoRaw):0.00}");

        builder.AppendLine($"{deviceName} Axis Three: {inputDevice.GetAxis1D(VRAxis.Three)}");
        builder.AppendLine($"{deviceName} Axis Three Raw: {inputDevice.GetAxis1D(VRAxis.ThreeRaw):0.00}");

        if (inputDevice.GetButtonUp(VRButton.Trigger))
        {
            Debug.Log("Button up");
        }
    }

    public void End()
    {
        ExperienceApp.End();
    }
    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }
    public void PlayStanding()
    {
        gManager.StartStanding();
        anim.SetTrigger("fade");

        pHitbox.GetComponent<Collider>().enabled = true;
        
    }
    public void PlaySeated()
    {
        gManager.StartSeated();
        anim.SetTrigger("fade");

        pHitbox.GetComponent<Collider>().enabled = true;
    }
}
