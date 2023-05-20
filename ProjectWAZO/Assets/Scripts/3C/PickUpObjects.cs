using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sound;
using UnityEngine;
using Utilitaire;

namespace _3C
{
    public class PickUpObjects : MonoBehaviour
    {
    
        #region Trucs Chelou pour les Input
        public PlayerControls inputAction;
        private void OnEnable()
        {
       
            inputAction.Player.Enable();
        }

        private void OnDisable()
        {
      
            inputAction.Player.Disable();
        }

        #endregion

        [Header("Valeurs")] 
        public bool isThingTaken;
        public float pickUpSpeed;
        public Transform pickUpPosition;
        [SerializeField] private float objectEjectionSpeed;
        [SerializeField] private float birdMass = 1;
    
        [Header("Data")]
        private float velocity;
        public GameObject pickedObject;
        public Animator anim;
        public List<GameObject> objectsInRange;
        public int pickedObjectMass;
        public bool isEchelle;
        public GameObject currentEchelle;
    
        [Header("Respawn Spirits")]
        public SpiritRespawn spiritRespawn;
        public bool isPressing;
        public bool hasTriggerIn;

        public static PickUpObjects instance;

        //CleanerRbReference
        private Rigidbody _rbObject;
        [SerializeField] private Rigidbody birdRb;
    
        //BUG fix : pickUp button state
        private bool _beakPinch;

        private void Awake()
        {
            if (instance != default && instance!=this)
            {
                DestroyImmediate(this);
            }
            instance = this;
        
            inputAction = new PlayerControls();
            inputAction.Player.PickUp.performed += ctx => Prendre();
            inputAction.Player.PickUp.canceled += ctx => Lacher();
            inputAction.Player.PickUp.performed += ctx => isPressing = true;
            inputAction.Player.PickUp.canceled += ctx => isPressing = false;

            birdRb = transform.parent.gameObject.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (isPressing && spiritRespawn is not null)
            {
                DoRespawn();
            }
            else if (spiritRespawn is not null && spiritRespawn.holdingDuration > 0)
            {
                spiritRespawn.holdingDuration -= Time.deltaTime;
            }
        }

        public void Prendre()
        {
            _beakPinch = true;
            if (!isThingTaken && !isEchelle)
            {
                pickedObject = GetClosestObject();
                if (pickedObject != null)
                {
                    StartCoroutine(Animation());
                
                    _rbObject = pickedObject.GetComponent<Rigidbody>();
                    pickedObject.transform.parent = pickUpPosition.transform;
                
                    MoveToBeak();
                    SwitchSpiritSound();
                
                    if (pickedObject.gameObject.layer == 7)
                    {
                        pickedObjectMass = Mathf.RoundToInt(_rbObject.mass);
                        birdRb.mass += pickedObjectMass;
                        _rbObject.mass = 0;
                        _rbObject.angularDrag = 1;
                    
                        pickedObject.GetComponent<Spirit>().isTaken = true;
                        Controller.instance.isHoldingASpirit = true;
                    }
                    else
                    {
                        pickedObject.GetComponent<carotteManager>().IsTaken();
                    }

                    Controller.instance.ResetWeightOnDetector(pickedObject.transform);
                }
            }

            if (isEchelle && !isThingTaken)
            {
                EnterEchelle();
            }
        }

        IEnumerator Animation()
        {
            anim.SetBool("isTaking",true);
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("isTaking",false);
        }
    
        //Bug fix : variable to manage the Tween
        private Tweener _currentTween;
        private void MoveToBeak()
        {
            pickedObject.transform.localRotation = Quaternion.Euler(0,0,0);
            _currentTween = pickedObject.transform.DOLocalMove(Vector3.zero, pickUpSpeed).OnComplete(CheckBeak);
        }

        private void CheckBeak()
        {
            isThingTaken = true;
            if (!_beakPinch) Lacher();
            anim.SetBool("isTaking",false);
        }

        private void Lacher()
        {
            anim.SetBool("isTaking",false);
            _beakPinch = false;
            if (pickedObject != null && isThingTaken && !_moveOnLadder)
            {
                if(_currentTween.IsActive()) _currentTween.Kill();
                isThingTaken = false;
            
                if (pickedObject.gameObject.layer == 7)
                {
                    _rbObject.angularDrag = 0;
                    birdRb.mass = birdMass;
                
                    var spirit = pickedObject.GetComponent<Spirit>();
                    spirit.isTaken = false;
                    _rbObject.mass = spirit.spiritMass;
                    Controller.instance.isHoldingASpirit = false;
                }
                else
                {
                    pickedObject.GetComponent<carotteManager>().IsLeft();
                    _rbObject.AddForce(transform.forward*objectEjectionSpeed,ForceMode.Impulse);
                }
                pickedObject.transform.parent = null;
                pickedObject = null;
                pickedObjectMass = 0;

                Controller.instance.ResetWeightOnDetector();
            }

            if (_moveOnLadder)
            {
                QuitEchelle();
            }
        }

        public void DoRespawn() //sert à faire respawn les esprits dans un puzzle
        {
            if (hasTriggerIn) //bool false par défaut qui devient true une fois qu'on a passé un puzzle
            {
                if (spiritRespawn.isInTrigger && !isThingTaken) //si on est dans la zone et qu'on a pas d'esprit dans le bec
                {
                    if (isPressing) 
                    {
                        if (spiritRespawn.holdingDuration > spiritRespawn.durationUntilReset)
                        {
                            foreach (var spirit in spiritRespawn.spiritsToRespawn) 
                            {
                                spirit.Respawn();
                                spiritRespawn.holdingDuration = 0;
                            }
                        }
                        else
                        {
                            spiritRespawn.holdingDuration += Time.deltaTime;
                        }
                    }
                }
            }
        }
    

        // BUG fix
        private bool _moveOnLadder;
    
        public void EnterEchelle()
        {
            if (!Controller.instance.isEchelle)
            {
                _moveOnLadder = true;
                anim.SetBool("isFlying",false);
                anim.SetBool("isClimbing",true);
                anim.SetTrigger("startClimb");
                Controller.instance.gravityScale = -4;
                Controller.instance.canJump = true;
                Controller.instance.canPlaner = false;
                Controller.instance.canMove = false;
                Controller.instance.rb.constraints = RigidbodyConstraints.FreezeRotation;
                Controller.instance.rb.useGravity = false;
                switch (currentEchelle.GetComponent<echelleData>().orientation)
                {
                    case echelleData.Orientation.nord:
                        Controller.instance.transform.DOMove(new Vector3(currentEchelle.transform.position.x, Controller.instance.transform.position.y +0.7f, currentEchelle.transform.position.z-0.4f),0.5f).OnComplete(SetIsEchelle);
                        break;
                    case echelleData.Orientation.sud:
                        Controller.instance.transform.DOMove(new Vector3(currentEchelle.transform.position.x, Controller.instance.transform.position.y +0.7f, currentEchelle.transform.position.z+0.4f),0.5f).OnComplete(SetIsEchelle);
                        break;
                    case echelleData.Orientation.est:
                        Controller.instance.transform.DOMove(new Vector3(currentEchelle.transform.position.x-0.4f, Controller.instance.transform.position.y +0.7f, currentEchelle.transform.position.z),0.5f).OnComplete(SetIsEchelle);
                        break;
                    case echelleData.Orientation.ouest:
                        Controller.instance.transform.DOMove(new Vector3(currentEchelle.transform.position.x+0.4f, Controller.instance.transform.position.y +0.7f, currentEchelle.transform.position.z),0.5f).OnComplete(SetIsEchelle);
                        break;
                } 
            }
        }
        private void SetIsEchelle()
        {
            Controller.instance.isEchelle = true;
        }
    
        public void QuitEchelle()
        {
            Controller.instance.isEchelle = false;
            Controller.instance.canPlaner = true;
            Controller.instance.canMove = true;
            Controller.instance.rb.useGravity = true;
            _moveOnLadder = false;
            anim.SetBool("isClimbing",false);
            anim.SetBool("isFlying",true);
            anim.ResetTrigger("startClimb");
       
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7) //Spirit
            {
                objectsInRange.Add(other.gameObject);
                GetClosestObject();
            }

            if (other.gameObject.layer == 14) //Object
            {
                objectsInRange.Add(other.gameObject);
                GetClosestObject();
            }
        
            if (other.gameObject.layer == 9) // Si l'objet est une echelle
            {
                isEchelle = true;
                currentEchelle = other.gameObject;
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 7)  //Spirit
            {
                objectsInRange.Remove(other.gameObject);
                GetClosestObject();
            }
        
            if (other.gameObject.layer == 14) //Object
            {
                objectsInRange.Remove(other.gameObject);
                GetClosestObject();
            }
        
            if (other.gameObject.layer == 9) // Si l'objet est une echelle
            {
                QuitEchelle();
                isEchelle = false;
                currentEchelle = null;
            }
        }

        GameObject GetClosestObject()
        {
            GameObject closestObject = null;
            float closestDistance = Mathf.Infinity;

            if (objectsInRange != null)
            {
                for (int i = 0; i < objectsInRange.Count; i++)
                {
                    float newDistance = Vector3.Distance(pickUpPosition.position, objectsInRange[i].transform.position);
                    if (newDistance < closestDistance)
                    {
                        closestDistance = newDistance;
                        closestObject = objectsInRange[i];
                    }
                    else
                    {
                        //objectsInRange[i].GetComponent<Spirit>().isClosest = false;
                    }
                }
            
                if (objectsInRange.Count == 1)
                {
                    closestObject = objectsInRange[0];
                    // closestObject.GetComponent<Spirit>().isClosest = true;
                }
            }
      

            if (closestObject != null)
            {
                //closestObject.GetComponent<Spirit>().isClosest = true;
            }
            return closestObject;
        }

        private void SwitchSpiritSound()
        {
            var audioList = AudioList.Instance;
            switch (_rbObject.mass)
            {
                case 1:
                    audioList.PlayOneShot(audioList.grabPetitAir,audioList.grabPetitAirVolume);
                    Debug.Log("Petit esprit air");
                    break;
                case 2:
                    audioList.PlayOneShot(audioList.grabGrosAir,audioList.grabGrosAirVolume);
                    Debug.Log("Gros esprit air");
                    break;
                case 4:
                    audioList.PlayOneShot(audioList.grabGrosTerre,audioList.grabGrosTerreVolume);
                    Debug.Log("Gros esprit terre");
                    break;
                default:
                    audioList.PlayOneShot(audioList.buyInShop,audioList.buyInShopVolume);
                    break;
            }
        }
    }
}
