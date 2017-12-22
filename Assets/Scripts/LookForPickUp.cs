using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class LookForPickUp : MonoBehaviour
{
    public Text PickUpText;
    public Text FoundedItemText;
    public Slider FireSlider;

    private Collider _interestingPlace;
    private bool _isVisited;

    // Use this for initialization
    private void Start()
    {
        _interestingPlace = null;
        _isVisited = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "PickUps":
                Debug.Log(other.name + " and " + other.tag);
                Debug.Log("Yes, I have done it!");

                PickUpText.gameObject.SetActive(true);
                _interestingPlace = other;
                break;

            case "FireStick":
                Debug.Log("You founded avaliable fire stick");
                GameStatsControl.OnFired = true;
                Debug.Log(GameStatsControl.OnFired.ToString());
                if (PickUpText != null)
                {
                    PickUpText.gameObject.SetActive(true);
                    _interestingPlace = other;

                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "PickUps" && other.tag != "FireStick") return;

        Debug.Log(other.name + " and " + other.tag);
        Debug.Log("Yes, I move out");
        _interestingPlace = null;
        GameStatsControl.OnFired = false;
        Debug.Log(GameStatsControl.OnFired.ToString());

        if (PickUpText == null) return;

        PickUpText.gameObject.SetActive(false);
        _isVisited = false;
    }

    public void OnPickUp()
    {
        if (!PickUpText.IsActive() || _isVisited) return;

        switch (_interestingPlace.gameObject.tag)
        {
            case "PickUps":
                FlagsController.NumOfFlags += 1;
                Debug.Log("You looked for something..");
                break;

            case "FireStick":
                FireSlider.value = 100f;
                break;
        }

        _interestingPlace.gameObject.tag = "Untagged";
        ParticleSystem wallFire = _interestingPlace.GetComponentInChildren<ParticleSystem>();
        Light wallFireLight = _interestingPlace.GetComponentInChildren<Light>();
        wallFire.Stop();
        GameObject.Destroy(wallFire);
        GameObject.Destroy(wallFireLight);
        PickUpText.gameObject.SetActive(false);
        FoundedItemText.gameObject.SetActive(true);
        Invoke("VoidText", 2.5f);
    }

    void VoidText()
    {
        FoundedItemText.gameObject.SetActive(false);
    }
}
