using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AttackManager : MonoBehaviour
{
    [SerializeField] private GameManager mGameManager;
   // public List<GameObject> _TargetPoints = new List<GameObject>();
    public List<GameObject> _spawnedTargetPoints = new List<GameObject>();
    public GameObject _TargetPrefab;
    public GameObject _multiplierPrefab;
    public GameObject _multiplierGameObject;
    public GameObject _Cannon;
    public float _MultiplierSwitchTime = 1.0f;
    public GameObject _ScorePanel;
    public Text _ScoreTextOne;
    public Text _ScoreTextTwo;
    public Text _ScoreTextThree;
    public GameObject _bulletPre;
    public Sprite _Sprite1, _Sprite2, _Sprite3, _Sprite4, _Sprite5;
    public Transform _TargetTransform;

    private Camera cam;
    private int cachedTargetPoint = -1;

    private void Awake()
    {
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        for (int i = 0; i< mGameManager._BuildingDetails.Count ; i++ )
        {
            Instantiate(mGameManager._BuildingDetails[i], mGameManager._PositionDetails[i], mGameManager._RotationList[i]);
        }    
        
    }

    private void Start()
    {
        cam = Camera.main;
      
        TargetInstantiation();
        MultiplierInstantiation();
        InvokeRepeating("DoMultiplierSwitching", 0f, _MultiplierSwitchTime);

      
    }

    // Update is called once per frame
    void Update()
    {   
           
    }

    /// <summary>
    /// This Function helps in moving the 2X Multiplier randomly Over the buildings
    /// </summary>
    void DoMultiplierSwitching()
    {
        if (_multiplierGameObject == null)
        {
            Vector3 newMultiplier = mGameManager._TargetMarkPost[0];
            _multiplierGameObject = Instantiate(_multiplierPrefab, newMultiplier, Quaternion.identity);

        }

        if (cachedTargetPoint != -1)
            _spawnedTargetPoints[cachedTargetPoint].SetActive(true); 


        int rand = Random.Range(0, mGameManager._TargetMarkPost.Count);
        cachedTargetPoint = rand;
        _multiplierGameObject.name = cachedTargetPoint.ToString();
        _spawnedTargetPoints[cachedTargetPoint].SetActive(false);
        _multiplierGameObject.transform.localPosition = _spawnedTargetPoints[cachedTargetPoint].transform.localPosition;
        _multiplierGameObject.transform.localRotation = _spawnedTargetPoints[cachedTargetPoint].transform.localRotation;
    }


    /// <summary>
    /// This helps in Instantiating the Target Mark on the Buildings.
    /// </summary>    
    void TargetInstantiation()
    {
        for (int i = 0; i < mGameManager._BuildingDetails.Count; i++)
        {
            GameObject go = Instantiate(_TargetPrefab, mGameManager._TargetMarkPost[i], Quaternion.identity);
            go.name = i.ToString();
            _spawnedTargetPoints.Add(go);
        }
    }

    /// <summary>
    /// This Helps in Instantiating the 2X Multiplier 
    /// </summary>    
    void MultiplierInstantiation()
    {
        Vector3 newMultiplier = mGameManager._TargetMarkPost[0];
        _multiplierGameObject = Instantiate(_multiplierPrefab, newMultiplier, Quaternion.identity);
        _multiplierGameObject.name = 0.ToString();
    }



    /*  void LaunchProjectile()
      {
          Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
          RaycastHit hit;

          if (Physics.Raycast(camRay, out hit, 100f, layer))
          {
              Cursor.SetActive(true);
              Cursor.transform.position = hit.point + Vector3.up * 0.1f;

              Vector3 Vo = CalculateVelocity(hit.point, _shotPoint.position, 1f);

              transform.rotation = Quaternion.LookRotation(Vo);

              if (Input.GetMouseButtonDown(0))
              {
                  Rigidbody obj = Instantiate(_bulletPrefab, _shotPoint.position, Quaternion.identity);
                  obj.velocity = Vo;
              }

          }
          else
          {
              Cursor.SetActive(false);
          }
      } */


    /// <summary>
    ///  Calcuate the Projectile  of the Bullet from from Origin to Target
    /// </summary>
    /// <param name="target"></param>
    /// <param name="origin"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    Vector3 CalculateVelocity(Vector3 target, Vector3 origin,float time)
    {
        //Define 
        Vector3 _distance = target - origin;
        Vector3 _distanceXZ = _distance;
        _distanceXZ.y = 0f;

        //Distance Value

        float sY = _distance.y;
        float sXZ = _distanceXZ.magnitude;

        float Vxz = sXZ / time;
        float Vy = sY / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = _distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;

    }


    /// <summary>
    /// This gets the Target mark Transform Details during on mouse Down click 
    /// </summary>
    /// <param name="trans"></param>
    public void AssignTarget(Transform trans)
    {
        _TargetTransform = trans;

        for (int i = 0; i < mGameManager._BuildingCost.Count; i++)
        {
            if (mGameManager._BuildingShield[i] == true)
            {
                _spawnedTargetPoints[i].transform.GetChild(0).gameObject.SetActive(true);
                Debug.Log("Target ");
                if (_multiplierGameObject.name == _TargetTransform.gameObject.name)
                {
                    Debug.Log("multiplier 2x");
                    _multiplierGameObject.transform.GetChild(0).gameObject.SetActive(true);
                }
            } 
            
            switch (mGameManager._BuildingCost[i])
            {
                case 1000:
                    _spawnedTargetPoints[i].GetComponent<SpriteRenderer>().sprite = _Sprite1;
                    break;
                case 2000:
                    _spawnedTargetPoints[i].GetComponent<SpriteRenderer>().sprite = _Sprite2;
                    break;
                case 3000:
                    _spawnedTargetPoints[i].GetComponent<SpriteRenderer>().sprite = _Sprite3;
                    break;
                case 4000:
                    _spawnedTargetPoints[i].GetComponent<SpriteRenderer>().sprite = _Sprite4;
                    break;
                case 5000:
                    _spawnedTargetPoints[i].GetComponent<SpriteRenderer>().sprite = _Sprite5;
                    break;
            }


        }
        switch (int.Parse(_multiplierGameObject.name))
        {
            case 0:
                _multiplierGameObject.GetComponent<SpriteRenderer>().sprite = _Sprite1;
                break;
            case 1:
                _multiplierGameObject.GetComponent<SpriteRenderer>().sprite = _Sprite2;
                break;
            case 2:
                _multiplierGameObject.GetComponent<SpriteRenderer>().sprite = _Sprite3;
                break;
            case 3:
                _multiplierGameObject.GetComponent<SpriteRenderer>().sprite = _Sprite4;
                break;
            case 4:
                _multiplierGameObject.GetComponent<SpriteRenderer>().sprite = _Sprite5;
                break;
        }
      
        Invoke("DisableBuildingCost", 2f);
        CancelInvoke("DoMultiplierSwitching");
        Invoke("PerformTarget", 2.1f);
    }

    public void DisableBuildingCost()
    {
        for (int i = 0; i < _spawnedTargetPoints.Count; i++)
        {
            _spawnedTargetPoints[i].SetActive(false);

        }
        _multiplierGameObject.SetActive(false);
    }

    public void PerformTarget()
    {
        int transIndex = int.Parse(_TargetTransform.gameObject.name);
        bool shield = mGameManager._BuildingShield[transIndex];

        for (int i = 0; i < _spawnedTargetPoints.Count; i++)
        {
            if (i != int.Parse(_TargetTransform.gameObject.name))
            {
                _spawnedTargetPoints[i].SetActive(false);
            }
        }
        if (_multiplierGameObject.name != _TargetTransform.gameObject.name)
        {
            _multiplierGameObject.SetActive(false);
        }
        Camera cam = Camera.main;
        _Cannon.SetActive(true);
        _Cannon.GetComponent<CannonShotController>().AssignPos(_TargetTransform);
       // _Cannon.SetActive(false);
        // ScoreCalculation(trans);
        StartCoroutine(ScoreCalculation(_TargetTransform));
        if (shield == true)
        {
            Debug.Log("shield Activated");
        }
        else
        {
            Debug.Log("shield Not Activated");
        }

        Debug.Log(_TargetTransform.transform.position);
        Debug.Log(_TargetTransform.transform.position + new Vector3(0f,-10f,0f));
        Debug.Log(_TargetTransform.transform.position);
        Debug.Log(_Cannon.transform.position);
        Debug.Log(_Cannon.transform.localPosition);
        Debug.Log(Quaternion.identity);

    }
    
     public IEnumerator ScoreCalculation(Transform trans)
     {

         int RewardValue = mGameManager._BuildingCost[int.Parse(trans.gameObject.name)];
         _ScoreTextOne.text = "Building Cost - " + RewardValue;
         if (trans.gameObject.name == _multiplierGameObject.name)
         {
            _ScoreTextTwo.text = "Multiplier (2x) - " + RewardValue + " * 2";
            RewardValue = RewardValue * 2;
              
         }
         _ScoreTextThree.text = "Your Score Are - " + RewardValue;
         yield return new WaitForSeconds(2);
         _ScoreTextThree.transform.parent.gameObject.SetActive(true);
         Debug.Log("I am Here");


    }


        /*if (_multiplierGameObject.name == _TargetTransform.gameObject.name)
                    _multiplierGameObject.transform.GetChild(0).gameObject.SetActive(true);*/
}
