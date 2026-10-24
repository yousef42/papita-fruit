using UnityEngine;
using SuperScrollView;
using UnityEngine.UI;


public class MenuScroll : MonoBehaviour
{
    [SerializeField] LoopListView2 mLoopListView;
    [SerializeField] float snapFinn = 20f;
    [SerializeField] Button previousButton;
    [SerializeField] Button nextButton;

    MenuData menuData;
    bool initialized;

    public void InitializeMenu(MenuData menuData)
    {
        if (initialized) ResetMenu();
        this.menuData = menuData;
        LoopListViewInitParam initParam = LoopListViewInitParam.CopyDefaultInitParam();
        initParam.mSnapFinishThreshold = snapFinn;
        
        mLoopListView.InitListView(-1, OnGetItemByIndex, initParam);
        initialized = true;

        mLoopListView.mOnBeginDragAction += () =>
        {
            foreach (var item in mLoopListView.ItemList)
            {
                item.GetComponent<CellMenu>().SetCellToIdleAnimForMenu();
            }
        };

        mLoopListView.mOnSnapItemFinished += (x,d) =>
        {
            int itemCount = mLoopListView.ItemList.Count;

            mLoopListView.GetShownItemNearestItemIndex(mLoopListView.GetMCurSnapNearestItemIndex()).
                                            GetComponent<CellMenu>().SetCellToPlayAnimForMenu();
        };

        previousButton.onClick.RemoveAllListeners();
        previousButton.onClick.AddListener(() => JumpToCell(-1));

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(() => JumpToCell(1));
    }

    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        int itemCount = menuData.cellDataList.Count;
        int val = 0;
        if (index >= 0)
        {
            val = index % itemCount;
        }
        else
        {
            val = itemCount + ((index + 1) % itemCount) - 1;
        }
        
        CellData cellData = menuData.cellDataList[val];
        if (cellData == null)
        {
            return null;
        }
        LoopListViewItem2 item = listView.NewListViewItem("CellMenu_");
        CellMenu cellMenu = item.GetComponent<CellMenu>();
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }
        if ( UnityEngine.PlayerPrefs.GetInt("status") == 0)
        {
            if (val == 2)
            {
                cellMenu.SetCellData(cellData, val, false);
            }
            else
            {
                cellMenu.SetCellData(cellData, val, false);
            }
        }
        else
        {
            cellMenu.SetCellData(cellData, val, false);
        }
       
       

        return item;
    }

    public float yPosOffsetCoefficient = 6;
    public int scaleOffsetCoefficient = 420;
    void LateUpdate()
    {
        mLoopListView.UpdateAllShownItemSnapData();
        int count = mLoopListView.ShownItemCount;
        for (int i = 0; i < count; ++i)
        {
            LoopListViewItem2 item = mLoopListView.GetShownItemByIndex(i);
            CellMenu itemScript = item.GetComponent<CellMenu>();
            float scale = 1 - Mathf.Abs(item.DistanceWithViewPortSnapCenter) / scaleOffsetCoefficient;
            scale = Mathf.Clamp(scale, 0.24f, 0.5f);
            //itemScript.mContentRootObj.GetComponent<CanvasGroup>().alpha = scale;
            itemScript.mContentRootObj.transform.localScale = new Vector3(scale, scale, 1);

            Vector3 loPo = itemScript.mContentRootObj.transform.localPosition;
            float xDis = 1 - Mathf.Abs(item.DistanceWithViewPortSnapCenter) / (yPosOffsetCoefficient == 0 ? 0.01f : yPosOffsetCoefficient);

            if (xDis > itemScript.transform.localPosition.y) xDis = itemScript.transform.localPosition.y;
            itemScript.mContentRootObj.transform.localPosition = new Vector3(loPo.x, xDis, loPo.z);
        }
    }

    public void ResetMenu()
    {
        mLoopListView.SetListItemCount(0);
        mLoopListView.ResetDobare();
        mLoopListView.ResetListView();
        mLoopListView.ClearSnapData();
    }

    void JumpToCell(int i)
    {
        int cur = mLoopListView.GetMCurSnapNearestItemIndex() + i - 2;
        mLoopListView.MovePanelToItemIndex(cur, 0);
    }

}
