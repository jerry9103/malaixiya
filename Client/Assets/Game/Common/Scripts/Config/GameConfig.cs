using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig  : ConfigDada
{
    public int gameCode;
    public string name;
    public bool isOpen;
    public List<GameConfigData> config;
}

public class GameConfigData {
    public string levelName;
    public int level;
    public int baseScore;
    /// <summary>
    /// 最低进入
    /// </summary>
    public int minScore;
}
