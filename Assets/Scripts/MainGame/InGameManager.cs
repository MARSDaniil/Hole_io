using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InGame {
    public class InGameManager :MonoBehaviour {
        public PlayerControler pc;
        public CameraController cam;
        public MenuManager ui;

        public bool started;
        public bool gameOver;

        public float gravity = 10f;
        public float time = 120;
    }
}