using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] AudioSource soundFront;
    [SerializeField] AudioSource soundRear;
    [SerializeField] AudioSource soundTop;
    [SerializeField] AudioSource soundBottom;
    [SerializeField] AudioSource soundLeft;
    [SerializeField] AudioSource soundRight;
    [SerializeField] Material mA, mB, mC, mD;
    [SerializeField] GameObject parent;

    public static int playbackState = 0;
    public int curState;

    Vector3[] hyperplanes = new Vector3[3];

    // Start is called before the first frame update
    void Start()
    {
        hyperplanes[0] = Vector3.up;
        hyperplanes[1] = Vector3.right;
        hyperplanes[2] = Vector3.forward;
        //Glockenspiel stuff:
        /*
         * top is up; bottom is -up
         * right is right; left is -right
         * rear is forward; front is -forward
         */
    }

    public int inOctant()
    {
        Vector3 camRelative = Camera.main.transform.position - this.transform.position;
        if (Vector3.Dot(hyperplanes[2], camRelative) < 0.0f)
        {
            //front plane
            if (Vector3.Dot(hyperplanes[0], camRelative) > 0.0f)
            {
                //Front top
                if (Vector3.Dot(hyperplanes[1], camRelative) > 0.0f)
                {
                    //Front Top Right
                    return 1;
                }
                else
                {
                    //Front Top Left
                    return 2;
                }
            }
            else
            {
                //Front bottom
                if (Vector3.Dot(hyperplanes[1], camRelative) > 0.0f)
                {
                    //Front Bottom Right
                    return 4;
                }
                else
                {
                    //Front Bottom Left
                    return 3;
                }
            }
        }
        else
        {
            //rear plane
            if (Vector3.Dot(hyperplanes[0], camRelative) > 0.0f)
            {
                //Rear top
                if (Vector3.Dot(hyperplanes[1], camRelative) > 0.0f)
                {
                    //Rear Top Right
                    return 5;
                }
                else
                {
                    //Rear Top Left
                    return 6;
                }
            }
            else
            {
                //Rear bottom
                if (Vector3.Dot(hyperplanes[1], camRelative) > 0.0f)
                {
                    //Rear Bottom Right
                    return 8;
                }
                else
                {
                    //Rear Bottom Left
                    return 7;
                }
            }
        }
    }

    public Vector3 baryCoordinates(Vector3 a, Vector3 b, Vector3 c, Vector3 s)
    {
        //Slightly modified code from StackExchange:
        //https://math.stackexchange.com/questions/1887215/fast-way-of-computing-barycentric-coordinates-explained
        //Modifications include translating to Unity's Vector types
        //Additionally, precomputing the divisor as divisions are expensive compared to multiplication
        Vector3 v0 = b - a, v1 = c - a, v2 = s - a;
        float d00 = Vector3.Dot(v0, v0);
        float d01 = Vector3.Dot(v0, v1);
        float d11 = Vector3.Dot(v1, v1);
        float d20 = Vector3.Dot(v2, v0);
        float d21 = Vector3.Dot(v2, v1);
        float denom = 1.0f / (d00 * d11 - d01 * d01);
        Vector3 temp;
        temp.y = (d11 * d20 - d01 * d21) * denom;
        temp.z = (d00 * d21 - d01 * d20) * denom;
        temp.x = 1.0f - temp.y - temp.z;
        return temp;
    }

    public void updateSound(int octant)
    {
        Vector3 cam = Camera.main.transform.position;
        Vector3 temp = Vector3.zero;
        Vector3 u = transform.position, r = transform.position, f = transform.position;
        switch (octant)
        {
            case 1:
                {
                    u.y += 0.125f;
                    r.x += 0.125f;
                    f.z -= 0.125f;
                    temp = baryCoordinates(u, r, f, cam);
                    soundBottom.volume = 0.0f;
                    soundRear.volume = 0.0f;
                    soundLeft.volume = 0.0f;
                    soundTop.volume = temp.x;
                    soundRight.volume = temp.y;
                    soundFront.volume = temp.z;
                    break;
                }
            case 2:
                {
                    u.y += 0.125f;
                    r.x -= 0.125f;
                    f.z -= 0.125f;
                    temp = baryCoordinates(u, r, f, cam);
                    soundBottom.volume = 0.0f;
                    soundRear.volume = 0.0f;
                    soundRight.volume = 0.0f;
                    soundTop.volume = temp.x;
                    soundLeft.volume = temp.y;
                    soundFront.volume = temp.z;
                    break;
                }
            case 3:
                {
                    u.y -= 0.125f;
                    r.x -= 0.125f;
                    f.z -= 0.125f;
                    temp = baryCoordinates(u, r, f, cam);
                    soundTop.volume = 0.0f;
                    soundRear.volume = 0.0f;
                    soundRight.volume = 0.0f;
                    soundBottom.volume = temp.x;
                    soundLeft.volume = temp.y;
                    soundFront.volume = temp.z;
                    break;
                }
            case 4:
                {
                    u.y -= 0.125f;
                    r.x += 0.125f;
                    f.z -= 0.125f;
                    temp = baryCoordinates(u, r, f, cam);
                    soundTop.volume = 0.0f;
                    soundRear.volume = 0.0f;
                    soundLeft.volume = 0.0f;
                    soundBottom.volume = temp.x;
                    soundRight.volume = temp.y;
                    soundFront.volume = temp.z;
                    break;
                }
            case 5:
                {
                    u.y += 0.125f;
                    r.x += 0.125f;
                    f.z += 0.125f;
                    temp = baryCoordinates(u, r, f, cam);
                    soundBottom.volume = 0.0f;
                    soundFront.volume = 0.0f;
                    soundLeft.volume = 0.0f;
                    soundTop.volume = temp.x;
                    soundRight.volume = temp.y;
                    soundRear.volume = temp.z;
                    //print("case 5 " + temp.ToString());
                    break;
                }
            case 6:
                {
                    u.y += 0.125f;
                    r.x -= 0.125f;
                    f.z += 0.125f;
                    temp = baryCoordinates(u, r, f, cam);
                    soundBottom.volume = 0.0f;
                    soundRight.volume = 0.0f;
                    soundLeft.volume = 0.0f;
                    soundTop.volume = temp.x;
                    soundLeft.volume = temp.y;
                    soundRear.volume = temp.z;
                    //print("case 6 " + temp.ToString());
                    break;
                }
            case 7:
                {
                    u.y -= 0.125f;
                    r.x -= 0.125f;
                    f.z += 0.125f;
                    temp = baryCoordinates(u, r, f, cam);
                    soundTop.volume = 0.0f;
                    soundFront.volume = 0.0f;
                    soundRight.volume = 0.0f;
                    soundBottom.volume = temp.x;
                    soundLeft.volume = temp.y;
                    soundRear.volume = temp.z;
                    //print("case 7 " + temp.ToString());
                    break;
                }
            case 8:
                {
                    u.y -= 0.125f;
                    r.x += 0.125f;
                    f.z += 0.125f;
                    temp = baryCoordinates(u, r, f, cam);
                    soundTop.volume = 0.0f;
                    soundFront.volume = 0.0f;
                    soundLeft.volume = 0.0f;
                    soundBottom.volume = temp.x;
                    soundRight.volume = temp.y;
                    soundRear.volume = temp.z;
                    //print("case 8 " + temp.ToString());
                    break;
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Get Camera position to alter stuff
        /*Vector3 translation;
        translation.x = 0.0f;
        translation.z = 1.0f * Time.deltaTime;
        translation.y = 0.0f;*/

        /*if (Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.G))
        {
            this.transform.position += translation;
        }
        if (Input.GetKey(KeyCode.JoystickButton1) || Input.GetKey(KeyCode.B))
        {
            this.transform.position -= translation;
        }*/
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            sound.volume = 1.0f;
            sound.Play();
        }
        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            sound.volume = 0.5f;
            soundToo.volume = 0.5f;
            sound.Play();
            soundToo.Play();
        }*/

        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            playbackState = (playbackState + 1) % 3;
        }

        float moveUp = Input.GetAxis("Vertical");
        Vector3 translate = Vector3.up * (moveUp) * Time.deltaTime;
        parent.transform.position += translate;

        switch (playbackState)
        {
            case 0:
                //Updating Sounds
                updateSound(inOctant());

                if (Input.GetKeyDown(KeyCode.JoystickButton0))//V key is Low pitch; B is the mid pitch; and N is the high pitch
                {
                    soundBottom.Play();
                    soundFront.Play();
                    soundLeft.Play();
                    soundRear.Play();
                    soundRight.Play();
                    soundTop.Play();
                }
                break;
            case 1:
                soundFront.volume = 1.0f;
                soundFront.spatialize = false;
                soundFront.spatialBlend = 0.0f;
                if (Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    soundFront.Play();
                }
                break;
            case 2:
                soundFront.spatialize = true;
                soundFront.volume = 1.0f;
                soundFront.spatialBlend = 0.0f;
                if (Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    soundFront.Play();
                }
                break;
        }

        if (soundFront.isPlaying)
        {
            switch (playbackState)
            {
                case 0:
                    this.GetComponent<MeshRenderer>().material = mB;
                    break;
                case 1:
                    this.GetComponent<MeshRenderer>().material = mC;
                    break;
                case 2:
                    this.GetComponent<MeshRenderer>().material = mD;
                    break;
            }
        }
        else
        {
            this.GetComponent<MeshRenderer>().material = mA;
        }
        curState = playbackState;
    }
}
