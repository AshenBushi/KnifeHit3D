using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnifeFest
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private List<Level> _levels;

        public Level CurrentLevel { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            CurrentLevel = _levels[0];
        }
    }
}