using PathCreation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KnifeFest
{
    public class WallSpawner : Singleton<WallSpawner>
    {
        [SerializeField] private PathCreator _path;
        [SerializeField] private Transform _road;
        [SerializeField] private Wall _template;
        [SerializeField] private float _leftX;
        [SerializeField] private float _rightX;

        private readonly List<Wall> _walls = new List<Wall>();

        protected override void Awake()
        {
            base.Awake();
        }

        public void SpawnWalls(Level level)
        {
            float greatestZ = 0;

            for (var i = 0; i < level.Walls.Count; i++)
            {
                var wall = level.Walls[i];
                _walls.Add(Instantiate(_template, new Vector3(wall.Side == Side.Left ? _leftX : _rightX, 1.5f, wall.ZPosition * 20f), Quaternion.Euler(Vector3.zero), transform));
                _walls[i].Init(wall.Type, wall.value);

                if (greatestZ < wall.ZPosition)
                    greatestZ = wall.ZPosition;
            }

            _road.localScale = new Vector3(1f, 1f, greatestZ + 1f);
            _path.bezierPath.AddSegmentToEnd(new Vector3(0f, 0f, (greatestZ + 0.5f) * 20f));

            FinalCutscene.OnCreatingCurscene?.Invoke();
        }

        public void TryClearWalls()
        {
            if (_walls.Count <= 0) return;

            foreach (var wall in _walls)
            {
                Destroy(wall);
            }

            _walls.Clear();
        }
    }

    [Serializable]
    public class Level
    {
        public List<WallParameters> Walls;
    }

    [Serializable]
    public class WallParameters
    {
        public Side Side;
        public float ZPosition;
        public WallType Type;
        public int value;
    }

    [Serializable]
    public enum Side
    {
        Left,
        Right
    }
}
