﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using GameLibrary.UI;
using TargetTapping.Back_end;

namespace TargetTapping.Screens
{
    class GameScreen : AbstractRichScreen
    {
        #region Variables

        // Stuff 'em in here, boss!
        Button btnPause;

        //Here were going to get the current level that we built in the leveleditor
        Level playingLevel = GameManager.GlobalInstance.activeLevel;

        //keep track of the score
        private int score;
        //the font to prit the score too
        private SpriteFont font;
        //position to put the score at
        private Vector2 scorePosition;

        #endregion Variables

        public override void LoadContent()
        {
            base.LoadContent();
            btnPause = MakeButton(0, 0, "GUI/pauseButton");
            //initialize the score
            this.score = 0;
            //load the spritefount to print the scroe to
            this.font = content.Load<SpriteFont>("Font");

            float scoreLength = (font.MeasureString("999/999")).X;
            //double check this position
            scorePosition = new Vector2(this.ScreenManager.ScaleXPosition((this.ScreenManager.GraphicsDevice.PresentationParameters.BackBufferWidth / 2.0f) - (scoreLength / 2.0f)), this.ScreenManager.ScaleYPosition(20.0f));


        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (btnPause.IsClicked())
            {
                AddScreenAndChill(new PauseScreen());
            }
            btnPause.Update(mouseState);


            foreach (var myListofObjects in playingLevel.objectList)
            {
                foreach (var myObject in myListofObjects)
                {
                    // Update the state of all objects that have been brought over from the leveleditor screen
                    myObject.Update(mouseState);
                }
            }
            // This foreach loop will check if a button in the list of buttonlists
            // is clicked and if it is then we are going to move its position.
            foreach (var myListofObjects in playingLevel.objectList)
            {
                foreach (var myObject in myListofObjects)
                {
                    if (myObject.IsClicked())
                    {
                        this.score++;
                        //now were going to set the current clicked object on the gamescree 
                        //to have its property shouldIbeDrawn = false. Because its been clicked
                        //Lets also update the score 
                        myObject.shouldIbeDrawn = false;
                        

                    }                
                }
            }
           


            base.Update(gameTime);
        }


        public override void PreparedDraw(SpriteBatch spriteBatch)
        {
            btnPause.Draw(spriteBatch);

            // draw all objects that were created 
            foreach (var myListofObjects in playingLevel.objectList)
            {
                foreach (var myObject in myListofObjects)
                {
                    myObject.Draw(spriteBatch);
                }
            }
            //SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, transform);
            spriteBatch.DrawString(font, "Score: "+score.ToString(), scorePosition, Color.White);
            //SpriteBatch.End();


        }
    }
}