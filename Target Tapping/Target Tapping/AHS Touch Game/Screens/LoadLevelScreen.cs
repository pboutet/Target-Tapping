﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.UI;
using TargetTapping.FrontEnd;

namespace TargetTapping.Screens
{
    class LoadLevelScreen : AbstractRichScreen
    {
        Texture2D myLoadLevelTitle, textBackgorund, magnifyGlass, listBackground;
        Vector2 magnifyGlassPosition = (new Vector2(175, 85));
        Vector2 lisBackgroundPosition = (new Vector2(350, 150));
        Vector2 searchBackground = (new Vector2(200, 85));
        Vector2 myLoadLevelTitlePosition = (new Vector2(330, 0));
        Vector2 myCancelButtonPosition = (new Vector2(0, 0));

        SpriteFont font;
        String searchQuery;
        Vector2 searchQueryPosition = (new Vector2(200, 90));
        bool loadKeyBoard = false;
        Button btnCancel, btnOpen, delSearch, goSearch;
        Button clearSearchButton;
        private Keyboard keyboard;

        public override void LoadContent()
        {
            base.LoadContent();
            font = content.Load<SpriteFont>("font");
            searchQuery = "Search...";
            textBackgorund = content.Load<Texture2D>("GUI/textBackground");
            magnifyGlass = content.Load<Texture2D>("GUI/magnifyGlass");
            listBackground = content.Load<Texture2D>("GUI/listBackground");
            myLoadLevelTitle = content.Load<Texture2D>("GUI/loadGameTitle");
            btnCancel = MakeButton(0, 0, "GUI/cancel");
            btnOpen = MakeButton(1160, 0, "GUI/openButton");
            delSearch = MakeButton(400, 85, "Gui/miniX");
            goSearch = MakeButton(425, 85, "Gui/go");
            clearSearchButton = MakeButton(175, 85, "GUI/nothing2");
            keyboard = new Keyboard(401, 520, content);
            keyboard.LoadContent();
	    // Load buttons 'n' stuff, yo!
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
	    // Lagic!
	    //AddScreenAndChill(new LoadLevelScreen(levelName));

            if (keyboard.CurrentKey != "")
            {
                if (keyboard.CurrentKey == "0")
                {
                   if (searchQuery.Length > 0)
                        searchQuery = searchQuery.Remove((searchQuery.Length) - 1);
                }
                else
                {
                    searchQuery = searchQuery + keyboard.CurrentKey;
                }
            }
            if (goSearch.IsClicked())
            {

            }
            if (btnCancel.IsClicked())
            {
                AddScreenAndChill(new MenuScreen());
            }
            if (btnOpen.IsClicked())
            {
                AddScreenAndChill(new LevelEditScreen());
            }
            if (delSearch.IsClicked())
            {
                searchQuery = "";
            }
            if (clearSearchButton.IsClicked())
            {
                if (loadKeyBoard == false) { loadKeyBoard = true; } //else { loadKeyBoard = false; }
            }
            // update load screen
            btnCancel.Update(mouseState);
            btnOpen.Update(mouseState);
            delSearch.Update(mouseState);
            clearSearchButton.Update(mouseState);
            goSearch.Update(mouseState);
            if (loadKeyBoard == true)
            {
                keyboard.Update(mouseState);
            }
            base.Update(gameTime);
        }

        public override void PreparedDraw(SpriteBatch SpriteBatch)
        {
            
            btnCancel.Draw(SpriteBatch);
            btnOpen.Draw(SpriteBatch);
            SpriteBatch.Draw(myLoadLevelTitle, myLoadLevelTitlePosition, Color.White);
            goSearch.Draw(SpriteBatch);
            clearSearchButton.Draw(SpriteBatch);
            if (loadKeyBoard == false)
            {
                SpriteBatch.DrawString(font, searchQuery, searchQueryPosition, Color.White);
            }
            else
            {
                SpriteBatch.DrawString(font, searchQuery, searchQueryPosition, Color.Black);
            }
            SpriteBatch.Draw(magnifyGlass, magnifyGlassPosition, Color.White);
            SpriteBatch.Draw(listBackground, lisBackgroundPosition, Color.White);
            delSearch.Draw(SpriteBatch);

            if (loadKeyBoard == true)
            {
                keyboard.Draw(SpriteBatch);
            }


        }
    }
}
