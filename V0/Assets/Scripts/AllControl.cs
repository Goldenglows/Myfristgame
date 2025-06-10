using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllControl : MonoBehaviour
{
    public class GameManager
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new GameManager();
                return _instance;
            }
        }
        public int score = 0;
    }
}
