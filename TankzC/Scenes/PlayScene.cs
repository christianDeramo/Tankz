using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    class PlayScene : Scene
    {
        protected List<Player> players;
        protected int currentPlayerIndex;

        protected WeaponsGUI weaponsGUI;

        protected Background bg_far;
        protected Background bg_medium;
        protected Background bg_near;
        protected List<TextObject> playersName;

        public TextObject timer;
        public float PlayerTimer { get; protected set; }
        public const int TIMER_START_VALUE = 16;

        public float MinY { get; protected set; }
        public Player CurrentPlayer { get; protected set; }

        public override void Start()
        {
            base.Start();

            Vector2 screenCenter = new Vector2(Game.window.Width / 2, Game.window.Height / 2);

            GfxManager.Init();

            GfxManager.AddTexture("bg0", "Assets/cyberpunk-street3.png");
            GfxManager.AddTexture("bg1", "Assets/cyberpunk-street2.png");
            GfxManager.AddTexture("bg2", "Assets/cyberpunk-street1.png");

            GfxManager.AddTexture("greenTank", "Assets/tanks_tankGreen_body1.png");
            GfxManager.AddTexture("tracks", "Assets/tanks_tankTracks1.png");
            GfxManager.AddTexture("turret", "Assets/tanks_turret2.png");

            GfxManager.AddTexture("bullet", "Assets/tank_bullet1.png");
            GfxManager.AddTexture("crate", "Assets/crate.png");

            GfxManager.AddTexture("playerBar", "Assets/loadingBar_bar.png");
            GfxManager.AddTexture("barFrame", "Assets/loadingBar_frame.png");

            GfxManager.AddTexture("bulletIcon", "Assets/bullet_ico.png");
            GfxManager.AddTexture("rocketIcon", "Assets/missile_ico.png");
            GfxManager.AddTexture("weaponsGUIframe", "Assets/weapons_GUI_frame.png");
            GfxManager.AddTexture("weaponsGUIselection", "Assets/weapon_GUI_selection.png");


            UpdateManager.Init();
            DrawManager.Init();
            PhysicsManager.Init();
            BulletManager.Init();

            FontManager.Init();
            FontManager.AddFont("stdFont", "Assets/textSheet.png", 15, 32, 20, 20);
            FontManager.AddFont("comics", "Assets/comics.png", 10, 32, 61, 65);

            CameraManager.Init(screenCenter, screenCenter);
            CameraManager.AddCamera("bg0", 0.2f);
            CameraManager.AddCamera("bg1", 0.4f);
            CameraManager.AddCamera("bg2", 0.8f);
            CameraManager.AddCamera("GUI", 0);

            playersName = new List<TextObject>();

            MinY = Game.window.Height - 20;

            /*Tile crate4 = new Tile(new Vector2(200, 400));
            Tile crate3 = new Tile(new Vector2(200, 300));
            Tile crate2 = new Tile(new Vector2(200, 200));
            Tile crate = new Tile(new Vector2(200, 100));

            Tile crate1000 = new Tile(new Vector2(900, 200));
            Tile crate1001 = new Tile(new Vector2(830, 200));
            Tile crate1002 = new Tile(new Vector2(760, 200));
            Tile crate1003 = new Tile(new Vector2(690, 200));
            Tile crate1004 = new Tile(new Vector2(620, 200));*/ 

            players = new List<Player>();

            timer = new TextObject(new Vector2(Game.window.Width / 2, 100), "", FontManager.GetFont("comics"), 0.7f);

            CreatePlayers(4);
            CurrentPlayer.Play();

            bg_far = new Background("bg0", new Vector2(-640, -60), 0);
            bg_far.SetCamera(CameraManager.GetCamera("bg0"));

            bg_medium = new Background("bg1", new Vector2(-640,0), 0);
            bg_medium.SetCamera(CameraManager.GetCamera("bg1"));

            bg_near = new Background("bg2", new Vector2(-640,0), 0);
            bg_near.SetCamera(CameraManager.GetCamera("bg2"));

            weaponsGUI = new WeaponsGUI(new Vector2(100, 500), "weaponsGUIframe", DrawManager.Layer.GUI);

            CameraManager.SetTarget(CurrentPlayer);
        }

        public override void Draw()
        {
            DrawManager.Draw();
        }

        public override void Input()
        {
            foreach (var player in players)
            {
                player.UpdateFSM();
            }
        }

        public void CreatePlayers(int numPlayers = 2)
        {
            for (int i = 0; i < numPlayers; i++)
            {
                players.Add(new Player("greenTank", new Vector2(-200 + (200 * i), 20), i));
                playersName.Add(new TextObject(new Vector2(50 + i * 250, 20), "player " + (i+1).ToString()));
            }

            CurrentPlayer = players[0];
            currentPlayerIndex = 0;
        }

        public void OnPlayerDie(Player player)
        {
            if (players.Contains(player))
            {
                players.Remove(player);
                player.IsActive = false;
                if (players.Count == 1)
                    IsPlaying = false;
            }
        }

        public virtual void NextPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            CurrentPlayer = players[currentPlayerIndex];

            CameraManager.MoveCameraTo(CurrentPlayer.Position);
            CameraManager.SetTarget(CurrentPlayer);
            CurrentPlayer.Play();
        }

        public virtual void ResetTimer()
        {
            PlayerTimer = TIMER_START_VALUE;
            timer.Text = PlayerTimer.ToString();
            timer.IsActive = true;
        }

        public virtual void StopTimer()
        {
            timer.IsActive = false;
        }

        public override void Update()
        {
            if (timer.IsActive)
            {
                PlayerTimer -= Game.DeltaTime;
                timer.Text = ((int)PlayerTimer).ToString();
            }

            PhysicsManager.Update();
            UpdateManager.Update();
            PhysicsManager.CheckCollisions();
            CameraManager.Update();
        }

        public override void OnExit()
        {
            UpdateManager.RemoveAll();
            DrawManager.RemoveAll();
            PhysicsManager.RemoveAll();
            GfxManager.RemoveAll();
            BulletManager.RemoveAll();
        }
    }
}
