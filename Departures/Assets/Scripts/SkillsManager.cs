using UnityEngine;
using System.Collections;

public class SkillsManager : MonoBehaviour {
    [SerializeField]
    private cycleRequirements[] config;
    public cycleRequirements currentCircle;
    public int CircleID;
    public bool check() {
        bool skilled = false;
        GameBoss GB = MainMenu.Instance().GB;
        if (GB != null)
        {
            CircleID = MainMenu.Instance().policeLevel;
            if (CircleID < config.Length)
            {

                MainMenu.Instance().ScoreRiched += GB.CurrentPoints;
                currentCircle = config[CircleID];
                // check wallete
                if (CircleID == MainMenu.Instance().walletLevel)
                {
                    if (MainMenu.Instance().ScoreRiched >= currentCircle.ScoreRequired)
                    {
                        MainMenu.Instance().walletLevel++;
                        skilled = true;
                    }
                }
                // check stamina
                if (MainMenu.Instance().walletLevel > MainMenu.Instance().staminaLevel)
                {
                    if (MainMenu.Instance().DaysPlayed >= currentCircle.daysPlayed)
                    {
                        MainMenu.Instance().staminaLevel++;
                        skilled = true;
                    }
                }
                // check concentration
                if (MainMenu.Instance().staminaLevel > MainMenu.Instance().icelevel)
                {
                    if (GB.scoreinRaw >= currentCircle.scoreInARaw)
                    {
                        MainMenu.Instance().icelevel++;
                        skilled = true;
                    }
                }
                // check truck
                if (MainMenu.Instance().icelevel > MainMenu.Instance().trucklevel)
                {
                    if (GB.carsSend >= currentCircle.maxCarSent)
                    {
                        MainMenu.Instance().trucklevel++;
                        skilled = true;
                    }
                }
                // check restricted
                if (MainMenu.Instance().trucklevel > MainMenu.Instance().policeLevel)
                {
                    if (GB.restricted >= currentCircle.restricted)
                    {
                        MainMenu.Instance().trucklevel++;
                        skilled = true;
                    }
                }
            }
        }
       MainMenu.Instance().SaveData();
        return skilled;
    }
    [System.Serializable]
    public class cycleRequirements {
        public float ScoreRequired = 1000;
        public float daysPlayed = 10;
        public float scoreInARaw = 70;
        public float maxCarSent = 5;
        public float restricted = 5;
    }
}
