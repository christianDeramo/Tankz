using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    static class Game
    {
        
        public static Scene CurrentScene { get; private set; }
        public static int NumJoysticks;

        public static Window window;
        //static float totalTime;
        static float gravity;

        public static float DeltaTime { get { return window.deltaTime; } }
        public static float Gravity { get { return gravity; } }

        static Game()
        {
            window = new Window(1280, 600, "Run!", false);
            gravity = 400.0f;
            window.SetVSync(false);

            //scenes creation
            PlayScene playScene = new PlayScene();

            //scenes config

            CurrentScene = playScene;
            CurrentScene.Start();


            string[] joysticks = Game.window.Joysticks;

            for(int i=0;i<joysticks.Length;i++)
            {
                if (joysticks[i]!=null && joysticks[i]!="Unmapped Controller")
                    NumJoysticks++;
            }
        }

        public static void Play()
        {
            while (window.opened)
            {
                if (!CurrentScene.IsPlaying)
                {
                    //next scene
                    if (CurrentScene.NextScene != null)
                    {
                        CurrentScene.OnExit();
                        CurrentScene = CurrentScene.NextScene;
                        CurrentScene.Start();
                    }
                    else
                    {
                        return;
                    }
                }

                //totalTime += GfxTools.Win.deltaTime;
                Console.SetCursorPosition(0, 0);
                //float fps = 1 / window.deltaTime;
                //if(fps<59)
                //    Console.Write((1 / window.deltaTime) + "                   ");

                //Input
                if (window.GetKey(KeyCode.Esc))
                    break;

                CurrentScene.Input();

                //Update
                CurrentScene.Update();

                //Draw
                CurrentScene.Draw();

                window.Update();
            }
        }
    }
}
