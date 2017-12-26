
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class LookForPickUp : MonoBehaviour
{
    public Text PickUpText;
    public Text FoundedItemText;

    public Slider FireSlider;

    public AudioSource Screamer;
    public AudioClip scream;

    Collider InterestingPlace;

    bool IsVisited;
    bool IsUsed = false;

    // Use this for initialization
    private void Start()
    {
        InterestingPlace = null;
        IsVisited = false;
        Screamer = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!IsUsed)
        {
            OnPickUp();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "FireStick":
                Debug.Log("You found avaliable fire stick");
                GameStatsControl.OnFired = true;
                Debug.Log(GameStatsControl.OnFired.ToString());
                if (PickUpText != null)
                {
                    PickUpText.gameObject.SetActive(true);
                    InterestingPlace = other;
                }

                break;

            case "Screamer":
                Debug.Log("Screamer triggered");
                Screamer.Play();

                int scene = SceneManager.GetActiveScene().buildIndex;
                switch (scene)
                {
                    case 2:
                        Screamer.PlayOneShot(scream, 5F);
                        break;
                    case 3:
                        Screamer.PlayOneShot(scream, 10F);
                        break;
                    case 5:
                        Screamer.PlayOneShot(scream, 20F);
                        break;
                }
                break;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "FireStick") return;

        Debug.Log(other.name + " and " + other.tag);
        Debug.Log("Yes, I move out");

        InterestingPlace = null;
        GameStatsControl.OnFired = false;
        Debug.Log(GameStatsControl.OnFired.ToString());

        if (PickUpText == null) return;

        PickUpText.gameObject.SetActive(false);
        IsVisited = false;
        IsUsed = false;
    }


    private void OnPickUp()
    {
        if (!PickUpText.IsActive() || IsVisited || InterestingPlace.gameObject.tag != "FireStick" ||
            !Input.GetKeyDown("e"))
        {
            return;
        }

        FireSlider.value = 100f;
        InterestingPlace.gameObject.tag = "Untagged";

        ParticleSystem WallFire = InterestingPlace.GetComponentInChildren<ParticleSystem>();
        Light WallFireLight = InterestingPlace.GetComponentInChildren<Light>();

        WallFire.Stop();
        GameObject.Destroy(WallFireLight);
        PickUpText.gameObject.SetActive(false);
        FoundedItemText.gameObject.SetActive(true);

        Invoke("VoidText", 2.5f);
        IsUsed = true;
    }

    void VoidText()
    {
        FoundedItemText.gameObject.SetActive(false);
    }
}