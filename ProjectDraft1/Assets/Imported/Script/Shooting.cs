using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject Camera;
    public GameObject[] Guns = new GameObject[4];
    private int UpgradeLevel = 1;

    //Max ammo reserves
    private static int MAX_PISTOL = 2,
                       MAX_RIFLE = 1,
                       MAX_HMG = 1,
                       MAX_SNIPER = 2;

    //Start ammo
    private static int START_PISTOL = 7,
                       START_RIFLE = 30,
                       START_HMG = 100,
                       START_SNIPER = 5;
    //Track if the player has the gun already
    private bool hasPistol, hasRifle, hasHMG, hasSniper;

    //Track Reserves
    private int PistolMag, ARMag, HMGMag, SniperMag;

    //Track Loaded ammo
    private int PistolRounds, ARRounds, HMGRounds, SniperRounds;

    //Define different gun types
    enum GunSelect {NoGun = 0, Pistol = 1, AR = 2, HMG = 3, Sniper = 4};
    GunSelect CurrentGun;

    //Define gun ranges
    float range_P = 10.0f,
          range_AR = 50.0f,
          range_HMG = 80.0f,
          range_SR = 100.0f;

    //Track current range depending on gun type
    float range_Current;

    //Inialize player ammo and selected gun
    void Start()
    {
        //Player starts with no guns...
        hasPistol = false;
        hasRifle = false;
        hasHMG = false;
        hasSniper = false;

        //...thus no guns will be shown...
        foreach(GameObject gun in Guns) { 
            gun.GetComponent<MeshRenderer>().enabled = false;

            MeshRenderer[] parts = gun.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer part in parts) { part.enabled = false; }
        }

        //...and player has no range
        range_Current = 0;

        //Set all reserves to 0
        PistolMag = 0;
        ARMag = 0;
        HMGMag = 0;
        SniperMag = 0;

        //Set all ammo to full
        PistolRounds = START_PISTOL;
        ARRounds = START_RIFLE;
        HMGRounds = START_HMG;
        SniperRounds = START_SNIPER;

        //Player starts unarmed
        CurrentGun = GunSelect.NoGun;
    }

    // Check for Player Input and ammo pickup
    void Update(){
        //Fire currently selected gun
        if (Input.GetMouseButtonDown(0)){
            switch (CurrentGun)
            {
                case GunSelect.NoGun:
                    //Play sound to say no gun
                    Debug.Log("No Gun Equipped");
                    break;

                case GunSelect.Pistol:
                    //Reduce ammo
                    Shoot();
                    if(PistolRounds > 0) PistolRounds--;
                    else {
                        //TODO: Play Out of Ammo sound
                        //Check for mags
                        if (PistolMag > 0)
                        {
                            //TODO: Play pistol reload anim

                            //Convert mag to ammo
                            PistolMag--;
                            PistolRounds = START_PISTOL;
                        }
                    }

                    //Present ammo remaining
                    PresentAmmo();
                    break;

                case GunSelect.AR:
                    //Reduce ammo
                    if (ARRounds > 0) ARRounds--;
                    else
                    {
                        //TODO: Play Out of Ammo sound
                        //Check for mags
                        if (ARMag > 0)
                        {
                            //TODO: Play Rifle reload anim

                            //Convert mag to ammo
                            ARMag--;
                            ARRounds = START_RIFLE;
                        }
                    }

                    //Present ammo remaining
                    PresentAmmo();
                    break;

                case GunSelect.HMG:
                    //Reduce ammo
                    if (HMGRounds > 0) HMGRounds--;
                    else
                    {
                        //TODO: Play Out of Ammo sound
                        //Check for mags
                        if (HMGMag > 0)
                        {
                            //TODO: Play HMG reload anim

                            //Convert mag to ammo
                            HMGMag--;
                            HMGRounds = START_HMG;
                        }
                    }

                    //Present ammo remaining
                    PresentAmmo();
                    break;

                case GunSelect.Sniper:
                    //Reduce ammo
                    if (SniperRounds > 0) SniperRounds--;
                    else
                    {
                        //TODO: Play Out of Ammo sound
                        //Check for mags
                        if (SniperMag > 0)
                        {
                            //TODO: Play Sniper reload anim

                            //Convert mag to ammo
                            SniperMag--;
                            SniperRounds = START_SNIPER;
                        }
                    }

                    //Present ammo remaining
                    PresentAmmo();
                    break;

                default:
                    //Present ammo remaining
                    PresentAmmo();
                    break;
            }

            //Fire current gun (if any)
            if(CurrentGun != GunSelect.NoGun){ Shoot(); }
        }

        //Num pad gun select
        if (Input.GetKeyDown(KeyCode.Alpha1) ) { CurrentGun = GunSelect.NoGun; setActiveGun(); }
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasPistol) { CurrentGun = GunSelect.Pistol; setActiveGun(); }
        if (Input.GetKeyDown(KeyCode.Alpha3) && hasRifle) { CurrentGun = GunSelect.AR; setActiveGun(); }
        if (Input.GetKeyDown(KeyCode.Alpha4) && hasHMG) { CurrentGun = GunSelect.HMG; setActiveGun(); }
        if (Input.GetKeyDown(KeyCode.Alpha5) && hasSniper) { CurrentGun = GunSelect.Sniper; setActiveGun(); }

        //Check for reload
        if (Input.GetKeyDown(KeyCode.R)){ Reload(); }
    }

    //Reload current gun
    private void Reload()
    {
        //Check which gun is active
        switch (CurrentGun)
        {
            case GunSelect.Pistol:
                //Check if player has spare magazines
                if(PistolMag > 0){
                    PistolRounds = START_PISTOL;
                    PistolMag--;
                }
                break;

            case GunSelect.AR:
                //Check if player has spare magazines
                if (ARMag > 0){
                    ARRounds = START_RIFLE;
                    ARMag--;
                }
                break;

            case GunSelect.HMG:
                //Check if player has spare magazines
                if (HMGMag > 0)
                {
                    HMGRounds = START_HMG;
                    HMGMag--;
                }
                break;

            case GunSelect.Sniper:
                //Check if player has spare magazines
                if (SniperMag > 0)
                {
                    SniperRounds = START_SNIPER;
                    SniperMag--;
                }
                break;
        }

        PresentAmmo();
    }

    //Gun Shooting (Raycast Logic)
    private void Shoot(){
        //Set raycast variable to give details on object/s hit
        RaycastHit Impact;

        int PistolDMG = 10;

        int Damage = UpgradeLevel * PistolDMG;

        //Launch ray from camera to impact site (data stored will only matter if bullet actually impacts something)
        if(Physics.Raycast(Camera.transform.position, Camera.transform.forward, out Impact, range_Current))
        {
            Debug.Log(Impact.transform.name);
        }
    }

    //Giveth the Player the ammo
    public void OnTriggerEnter(Collider other)
    {
        //Check if object is a magazine
        if (other.gameObject.tag == "PMag")
        {
            if (PistolMag < MAX_PISTOL)
            {
                //Add ammo
                PistolMag++;

                //Delete Magazine object
                Destroy(other.gameObject);
            }
            PresentAmmo();
        }
        else if (other.gameObject.tag == "ARMag")
        {
            if (ARMag < MAX_RIFLE)
            {
                //Add ammo
                ARMag++;

                //Delete Magazine object
                Destroy(other.gameObject);
            }
            PresentAmmo();
        }
        else if (other.gameObject.tag == "HMGMag")
        {
            if (HMGMag < MAX_HMG)
            {
                //Add ammo
                HMGMag++;

                //Delete Magazine object
                Destroy(other.gameObject);
            }
            PresentAmmo();
        }
        else if (other.gameObject.tag == "SRMag")
        {
            if (SniperMag < MAX_SNIPER)
            {
                //Add ammo
                SniperMag++;

                //Delete Magazine object
                Destroy(other.gameObject);
            }
            PresentAmmo();
        }

        //Check if object is a gun
        else if (other.gameObject.tag == "Pistol")
        {
            //Give gun
            GivePistol();

            //YEET gun from world
            Destroy(other.gameObject);

            //Update ammo feedback
            PresentAmmo();
        }
        else if (other.gameObject.tag == "AR") {
            //Give gun
            GiveAR();

            //YEET gun from world
            Destroy(other.gameObject);

            PresentAmmo();
        }
        else if (other.gameObject.tag == "HMG") { 
            //Give gun
            GiveHMG();

            //YEET gun from world
            Destroy(other.gameObject);

            PresentAmmo();
        }
        else if (other.gameObject.tag == "Sniper") { 
            //Give gun
            GiveSniper();

            //YEET gun from world
            Destroy(other.gameObject);

            PresentAmmo();
        }
    }

    //Trigger functions (For reference by other objects)
    private void GivePistol()
    {
        CurrentGun = GunSelect.Pistol;
        if (!hasPistol) hasPistol = true;
        else if (PistolMag < MAX_PISTOL) PistolMag++;

        setActiveGun();
    }

    private void GiveAR()
    {
        CurrentGun = GunSelect.AR;
        if (!hasRifle) hasRifle = true;
        else if (ARMag < MAX_RIFLE) ARMag++;

        setActiveGun();
    }

    private void GiveHMG()
    {
        CurrentGun = GunSelect.HMG;
        if (!hasHMG) hasHMG = true;
        else if (HMGMag < MAX_HMG) HMGMag++;

        setActiveGun();
    }

    private void GiveSniper()
    {
        CurrentGun = GunSelect.Sniper;
        if (!hasSniper) hasSniper = true;
        if (SniperMag < MAX_SNIPER) SniperMag++;

        setActiveGun();
    }

    //Debug functions (delete when UI is implemented) (Or Repurpose)
    void PresentAmmo()
    {
        /*
        //Present Mag stock
        Debug.Log("M1911 Mags: " + PistolMag + "\n"
            + "M4 Mags: " + ARMag + "\n"
            + "M249 Mags: " + HMGMag + "\n"
            + "M107 Mags: " + SniperMag + "\n");

        //Present Bullet stock
        Debug.Log("M1911 Ammo: " + PistolRounds + "\n" 
            + "M4 Ammo: " + ARRounds + "\n" 
            + "M249 Ammo: " + HMGRounds + "\n" 
            + "M107 Ammo: " + SniperRounds);
        */
    }

    void setActiveGun()
    {
        //Set all guns to inactive
        foreach (GameObject gun in Guns)
        {
            gun.GetComponent<MeshRenderer>().enabled = false;

            MeshRenderer[] parts = gun.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer part in parts) { part.enabled = false; }
        }

        //Check which gun should be active and set it to active
        switch (CurrentGun)
        {
            case GunSelect.Pistol:
                //Set meshes to active to display gun
                Guns[0].SetActive(true);
                foreach (MeshRenderer part in Guns[0].GetComponentsInChildren<MeshRenderer>()) { part.enabled = true; }

                //Set appropriate range
                range_Current = range_P;
                break;

            case GunSelect.AR:
                //Set meshes to active to display gun
                Guns[1].SetActive(true);
                foreach (MeshRenderer part in Guns[1].GetComponentsInChildren<MeshRenderer>()) { part.enabled = true; }

                //Set appropriate range
                range_Current = range_AR;
                break;

            case GunSelect.HMG:
                //Set meshes to active to display gun
                Guns[2].SetActive(true);
                foreach (MeshRenderer part in Guns[2].GetComponentsInChildren<MeshRenderer>()) { part.enabled = true; }

                //Set appropriate range
                range_Current = range_HMG;
                break;

            case GunSelect.Sniper:
                //Set meshes to active to display gun
                Guns[3].SetActive(true);
                foreach (MeshRenderer part in Guns[3].GetComponentsInChildren<MeshRenderer>()) { part.enabled = true; }

                //Set appropriate range
                range_Current = range_SR;
                break;
        }
    }
}
