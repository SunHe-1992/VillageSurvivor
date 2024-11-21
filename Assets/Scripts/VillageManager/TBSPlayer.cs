using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Event;
using SunHeTBS;
using System.IO;
public static class TBSPlayer
{
    public static bool fromGD = false;
    public static UserDetail UserDetail; //玩家的所有信息
    public static void SetUserDetail()
    {
        //init user detail info
        GenerateTestUserDetail();
        pointDic = new Dictionary<PointEnum, long>();
        foreach (var pt in UserDetail.points)
        {
            pointDic[(PointEnum)pt.PointType] = pt.PointValue;
        }

    }

    /// <summary>
    /// generate user detail info(should be delivered from server, or read from player's save file)
    /// </summary>
    static void GenerateTestUserDetail()
    {
        if (UserDetail != null)
            return;
        UserDetail = new UserDetail();
        UserDetail.userId = 301410195;
        UserDetail.userName = "He Sun";
        UserDetail.IndexOnMap = 39;
        UserDetail.currentChapter = 1001;
        UserDetail.diceCount = 12;
        UserDetail.points.Add(new UserPoint(PointEnum.Gold, 0));
        UserDetail.points.Add(new UserPoint(PointEnum.Gem, 0));

        int amount = 10;
        foreach (var data in ConfigManager.table.Item.DataList)
        {
            InsertItem(data.ID, amount);
        }

    }
    #region Point management

    static Dictionary<PointEnum, long> pointDic;
    public static long GetPointAmount(PointEnum ptType)
    {
        if (pointDic.ContainsKey(ptType))
            return pointDic[ptType];
        else
            return 0;
    }
    public static long GetGoldAmount()
    {
        return GetPointAmount(PointEnum.Gold);
    }
    public static bool IsAfford(PointEnum ptType, long needValue)
    {
        return GetPointAmount(ptType) >= needValue;
    }
    public static bool IsAffordGold(long needValue)
    {
        return GetPointAmount(PointEnum.Gold) >= needValue;
    }

    public static void UpdatePointAmount(PointEnum ptType, long changeValue)
    {
        if (pointDic.ContainsKey(ptType))
        {
            pointDic[ptType] += changeValue;
        }
        return;
    }
    public static void UpdateGoldAmount(long changeValue)
    {
        UpdatePointAmount(PointEnum.Gold, changeValue);
    }
    public static void SpendGold(long changeValue)
    {
        UpdateGoldAmount(GetGoldAmount() - changeValue);
    }
    #endregion

    #region item management

    public static void InsertItem(int itemId, int itemCount)
    {
        var findItem = UserDetail.items.Find(p => p.itemId == itemId);
        if (findItem != null)
        {
            findItem.itemCount += itemCount;
        }
        else
        {
            UserDetail.items.Add(new UserItem(itemId, itemCount));
        }
        Debugger.Log($"item inserted itemId={itemId} count={itemCount}");
    }
    public static void RemoveItem(int itemId, int itemCount)
    {
        var findItem = UserDetail.items.Find(p => p.itemId == itemId);
        if (findItem != null)
        {
            if (findItem.itemCount >= itemCount)
            {
                findItem.itemCount -= itemCount;
                UserDetail.items.RemoveAll(p => p.itemCount <= 0);
            }
            else
            {
                Debugger.LogWarning("item remove: insufficient, item id: " + itemId);
            }
        }
        else
        {
            Debugger.LogWarning("failed to find item to remove, item id: " + itemId);
        }
    }
    #endregion

    #region quests management
    public static void GenerateBoardQuests()
    {
        foreach (var data in ConfigManager.table.TbQuest.DataList)
        {
            if (data.Platform == true)
            {
                //data.DialogueID
                QuestEntry qe = new QuestEntry(data.ID);
                UserDetail.boardQuests.Add(data.ID, qe);
            }
        }
    }

    public static void AcceptQuest(int questId)
    {
        var data = UserDetail.boardQuests[questId];
        UserDetail.boardQuests.Remove(questId);

        data.status = QuestEntry.QuestStatus.Accepted;
        UserDetail.myQuests.Add(questId, data);
    }
    public static void AddQuestProgress(int questType, int addProgress)
    {
        var dic = TBSPlayer.UserDetail.myQuests;
        foreach (var data in dic.Values)
        {
            var cfg = ConfigManager.table.TbQuest.Get(data.questId);
            if (cfg.QuestType == questType)
            {
                data.currentProgress += addProgress;
            }
        }
        CheckQuestFinish();
    }
    public static void CheckQuestFinish()
    {
        var dic = TBSPlayer.UserDetail.myQuests;

        foreach (var data in dic.Values)
        {
            if (data.status == QuestEntry.QuestStatus.Accepted && data.currentProgress >= data.maxProgress)
            {
                data.status = QuestEntry.QuestStatus.Finished;
                var cfg = ConfigManager.table.TbQuest.Get(data.questId);
                int addGold = cfg.RewardGold;
                TBSPlayer.UpdateGoldAmount(addGold);
                //UIService.Inst.ShowMoneyAnim(addGold);
                if (cfg.AchievementID != 0)
                {
                    TBSPlayer.UserDetail.achievementList.Add(cfg.AchievementID);
                    Debugger.Log("insert achi id = " + cfg.AchievementID);
                }
            }

        }

    }
    #endregion

    #region load and save

    public static void SavePlayer()
    {
        string filePath = Application.persistentDataPath + "/player.json";
        string jsonString = JsonUtility.ToJson(TBSPlayer.UserDetail);
        // Write the JSON string to a file
        File.WriteAllText(filePath, jsonString);
        Debug.Log("File saved to: " + filePath);
    }
    public static void LoadPlayer()
    {
        ;
    }
    #endregion

    #region building data
    public static void CreateBuilding(int buildingId)
    {
        var bd = new UserBuilding(buildingId);

        UserDetail.buildingData.Add(bd);
    }
    #endregion
}
public class UserDetail
{
    public int userId;
    public string userName;
    public int currentChapter = 0;
    /// <summary>
    /// current index on map
    /// </summary>
    public int IndexOnMap = 0;
    /// <summary>
    /// how many dices I have
    /// </summary>
    public int diceCount = 0;
    public List<UserPoint> points = new List<UserPoint>();
    public List<UserItem> items = new List<UserItem>();
    public Dictionary<int, QuestEntry> boardQuests = new Dictionary<int, QuestEntry>();
    public Dictionary<int, QuestEntry> myQuests = new Dictionary<int, QuestEntry>();
    public List<int> achievementList = new List<int>();
    public List<UserBuilding> buildingData = new List<UserBuilding>();
    public float score = 0;
    public void AddScore(float _score)
    {
        this.score += _score;
    }
    public int difficulty = 0;
    public bool enableMusic = true;
    public bool enableSound = true;
}
public enum PointEnum
{
    Gold = 1,
    Gem = 2,
}
public class UserPoint
{
    public int PointType;
    public long PointValue;
    public UserPoint(PointEnum type, long value)
    {
        this.PointType = (int)type;
        this.PointValue = value;
    }
}
public class UserItem
{
    public int itemId;
    public int itemCount;

    public UserItem(int itemId, int itemCount)
    {
        this.itemId = itemId;
        this.itemCount = itemCount;
    }
}
public class QuestEntry
{
    public int questId;
    public int currentProgress;
    public int maxProgress;
    public QuestStatus status = QuestStatus.Onboard;

    public QuestEntry(int questId)
    {
        this.questId = questId;
        var cfg = ConfigManager.table.TbQuest.Get(this.questId);
        this.maxProgress = cfg.Param1;
        this.status = QuestStatus.Onboard;
    }

    public enum QuestStatus
    {
        Onboard = 0,
        Accepted = 1,
        Finished = 2,
    }
    public void UpdateProgress(int progress)
    {
        currentProgress = progress;
        //if (currentProgress >= maxProgress)
        //{
        //    status = QuestStatus.Finished;
        //}
    }

}
public class GameEventData : IEventMessage
{
    public int param1;
    public int questId;

    public GameEventData(int questId, int param1)
    {
        this.param1 = param1;
        this.questId = questId;
    }
}

/// <summary>
/// class for save user's building info
/// </summary>
public class UserBuilding
{

    public int buildingId;
    private int buildingSeqId;

    public UserBuilding(int buildingId)
    {
        this.buildingId = buildingId;

    }
}