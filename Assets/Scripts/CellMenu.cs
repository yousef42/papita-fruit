using UnityEngine;
using Spine;
using Spine.Unity;
using UnityEngine.UI;
using System.Collections;


public enum CellAnim { idle, Movy}

public class CellMenu : MonoBehaviour
{
    public GameObject mContentRootObj;
    
    [Space]
    [SerializeField] SkeletonGraphic skelGraph;
    [SerializeField] Text text;
    [SerializeField] Button button;

    CellData cellData;

    public void SetCellData(CellData cellData, int index,bool isLock)
    {
        if (isLock)//locked
        {
            this.cellData = cellData;
            skelGraph.skeletonDataAsset = cellData.skelData;
            skelGraph.material = cellData.skelMaterial;
            skelGraph.Initialize(true);
            this.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
            SetCellNumber(index);
            InitializeButton(1);
        }
        else
        {
            this.cellData = cellData;
            skelGraph.skeletonDataAsset = cellData.skelData;
            skelGraph.material = cellData.skelMaterial;
            skelGraph.Initialize(true);
            this.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
            SetCellNumber(index);
            InitializeButton(2);
        }
 
    }

    void InitializeButton(int a)
    {
        if (a == 1)//locked
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => showLockBox());
        }
        else
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => LoadMyScene());
        }
    
    }

    float loadedTime = 0;
    void Update()
    {
        loadedTime += Time.deltaTime;
    }

    public void SetCellToPlayAnimForMenu()
    {
        if (loadedTime <= 0.5f) return;
        StartCoroutine(SetCellAnim_Co());
    }

    IEnumerator SetCellAnim_Co()
    {
        float delayTime = (cellData.audio != null) ? cellData.audio.length : 2;
        TrackEntry track = skelGraph.AnimationState.SetAnimation(0, "play3", true);
        AudioController.instance.PlayOneShot(cellData.audio);
        //skelGraph.AnimationState.AddAnimation(0, "idle3", false, 0);
        yield return new WaitForSeconds(delayTime);
        SetCellToIdleAnimForMenu();
    }

    public void SetCellToIdleAnimForMenu()
    {
        skelGraph.AnimationState.SetAnimation(0, "idle3", true);
    }

    void SetCellNumber(int index)
    {
#if UNITY_EDITOR
        text.text = index.ToString();
#else
        text.gameObject.SetActive(false);
#endif
    }

    void LoadMyScene()
    {
        GameController.Instance.GoToCurrentScene(cellData.sceneName);

        AudioController.instance.PlaySound(SoundType.Click);
    }
    void showLockBox()
    {
        print("locked");
        if (GameObject.Find("MainMenuController") !=null)
        {
           LockLevelsManager locks = GameObject.Find("MainMenuController").GetComponent<LockLevelsManager>();
           locks.OpenLockBox();
        }
        else
        {
            print("not find !");
        }
      
        
    }
}
