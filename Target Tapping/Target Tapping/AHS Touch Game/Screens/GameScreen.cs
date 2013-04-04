﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.UI;
using TargetTapping.Back_end;
using Microsoft.Xna.Framework.Media;

namespace TargetTapping.Screens
{
    class GameScreen : AbstractRichScreen
    {
        #region Variables

        //Stuff to draw objects on screen in order.
        int countFramesForEntity = 0;
        int countFramesForTime = 0;
        int time = 0;
        int totalTimeAllowed = 0;
        int currentListNumber = 0;

        // Stuff 'em in here, boss!
        Button btnPause, btnTouchToStart, youFinished;

        //Here were going to get the current level that we built in the leveleditor
        Level playingLevel = GameManager.GlobalInstance.activeLevel;

        //Song to play on button press
        Song song;

        //keep track of the score
        private int score;
        private int finalScore;
        //the font to prit the score too
        private SpriteFont font;
        //position to put the score at
        private Vector2 scorePosition;
        private Vector2 timePosition;

        //This boolean say whether the game has been started
        private bool hasTouchedToStart = false;
        private bool gameFinished = false;

        #endregion Variables

        public override void LoadContent()
        {
            base.LoadContent();
            btnTouchToStart = MakeButton(((ScreenWidth / 2) - 100), ((ScreenHeight / 2) - 100), "GameScreenContent/touchToStart");
            youFinished = MakeButton(((ScreenWidth / 2) - 150), ((ScreenHeight / 2) - 100), "GameScreenContent/gameFinished");

            btnPause = MakeButton(0, 0, "GUI/pauseButton");
            //initialize the score
            score = 0;
            //load the spritefount to print the scroe to
            this.font = Content.Load<SpriteFont>("Font");

            float scoreLength = (font.MeasureString("999/999")).X;
            //Load audio for button press
            song = Content.Load<Song>("ButtonPress");
            
            //double check this position
            scorePosition = new Vector2(ScreenWidth - 240, 30);
            timePosition = new Vector2(ScreenWidth - 120, 30);
        }

        public override void Update(GameTime gameTime)
        {
            if (hasTouchedToStart)
            {
                if (countFramesForTime == 60)
                {
                    time = time + 1;
                    countFramesForTime = 0;
                }
                if (countFramesForEntity == (playingLevel.upTime * 60))
                {
                    totalTimeAllowed = totalTimeAllowed + playingLevel.upTime;
                    currentListNumber = currentListNumber + 1;
                    countFramesForEntity = 0;
                }
                else
                {
                    bool check = true;
                    foreach (var myObject in playingLevel.objectList[currentListNumber])
                    {
                        if (myObject.shouldIbeDrawn)
                        {
                            check = false;
                        }
                    }
                    if (check)
                    {
                        currentListNumber = currentListNumber + 1;
                        countFramesForEntity = 0;
                    }
                }
                if (currentListNumber == playingLevel.objectList.Count)
                {
                    finalScore = score - (time - totalTimeAllowed);
                    //TO DO: Add Score Screen
                    //Until then hack to ignore stepping over list boundry and running for infinity
                    currentListNumber = currentListNumber - 1;
                }
                countFramesForTime = countFramesForTime + 1;
                countFramesForEntity = countFramesForEntity + 1;
            }

            //Touch to Start functionality.
            if (btnTouchToStart.IsClicked())
            {
                hasTouchedToStart = true;
                //Start Timer here and all game events
            }
            if (hasTouchedToStart == false)
            {
                btnTouchToStart.Update(MouseState);
            }

            //This all happens after touch to start!
            if (hasTouchedToStart)
            {
                if (btnPause.IsClicked())
                {
                    AddScreenAndChill(new PauseScreen());
                }
                btnPause.Update(MouseState);


                foreach (var myObject in playingLevel.objectList[currentListNumber])
                {
                    // Update the state of all objects that have been brought over from the leveleditor screen
                    myObject.Update(MouseState);
                }
                // This foreach loop will check if a button in the list of buttonlists
                // is clicked and if it is then we are going to move its position.
                foreach (var myObject in playingLevel.objectList[currentListNumber])
                {
                    if (myObject.IsClicked())
                    {
                        score++;

                        //Play a sound
                        MediaPlayer.Play(song);

                        //now were going to set the current clicked object on the gamescree 
                        //to have its property shouldIbeDrawn = false. Because its been clicked
                        //Lets also update the score
                        //playingLevel.objectList[currentListNumber][position].shouldIbeDrawn = false;
                        myObject.shouldIbeDrawn = false;
                    }
                    if (myObject.bMouseDownInside)
                    {
                        myObject.holdCount = myObject.holdCount + 1;
                        if (myObject.holdCount == playingLevel.holdTime * 40)
                        {
                            score = score + (playingLevel.holdTime * 2);

                            //Play a sound
                            MediaPlayer.Play(song);

                            myObject.shouldIbeDrawn = false;
                        }
                    }
                    else
                    {
                        myObject.holdCount = 0;
                    }
                }
            }

            base.Update(gameTime);
        }


        public override void PreparedDraw(SpriteBatch spriteBatch)
        {
            if (!hasTouchedToStart)
            {
                btnTouchToStart.Draw(spriteBatch);
            }
            else if (gameFinished)
            {
                // Send to end screen?
                youFinished.Draw(spriteBatch);
            }
            else
            {

                btnPause.Draw(spriteBatch);

                // draw all objects that were created 
                foreach (var myObject in playingLevel.objectList[currentListNumber])
                {
                    myObject.Draw(spriteBatch);
                }

                spriteBatch.DrawString(font, "Score: " + score.ToString(), scorePosition, Color.White);
                spriteBatch.DrawString(font, "Time: " + time.ToString(), timePosition, Color.White);

            }
        }
    }
}