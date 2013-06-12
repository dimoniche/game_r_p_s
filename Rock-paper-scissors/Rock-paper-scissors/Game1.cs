using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace Rock_paper_scissors
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D[] rock            = new Texture2D[2];
        Vector2[] position_rock     = new Vector2[2];
        Texture2D[] paper           = new Texture2D[2];
        Vector2[] position_paper    = new Vector2[2];
        Texture2D[] scissors        = new Texture2D[2];
        Vector2[] position_scissors = new Vector2[2];

        Texture2D[] border = new Texture2D[2];
        Vector2[] position_border = new Vector2[2];

        static int max_size_playground = 64;
        static int size_playground     = 0;

        //
        Texture2D[] play_ground = new Texture2D[max_size_playground];
        Vector2[] position_play_figure = new Vector2[max_size_playground];

        // 
        enum type_figure { none = 0, rock, paper, scissors };
        enum type_user   { computer = 0, numan };
        enum check_battle { none = 0, can_move, death_both, we_win, we_lose };

        type_figure[] playground = new type_figure[max_size_playground];

        type_user user1 = type_user.numan;
        type_user user2 = type_user.computer;

        // позиция центра игры
        int line_win = max_size_playground / 2;

        int position_user1 = 0;
        int position_user2 = max_size_playground / 2 - 1;

        enum step_game { start = 0, game_user1, game_user2 , win1, win2 };
        step_game stepgame = step_game.start;

        Random rand = new Random(((int)DateTime.Now.Ticks & 0x0000FFFF));

        int switchtest = 0;
        int delay = 0;

        int shift1 = 0, shift2 = 0;

        void VectorAdd(int index, int X, int Y)
        {
            position_play_figure[index] = new Vector2(20 + 128 + 64 * Y, 20 + 64 * X);
            size_playground++;
            line_win = size_playground / 2;
            position_user2 = size_playground / 2 - 1;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

            for (int i = 0; i < size_playground; i++)
            {   // обновим поле
                playground[i] = type_figure.none;
            }

            VectorAdd(0, 2, 0);
            VectorAdd(1, 2, 1);
            VectorAdd(2, 2, 2);
            VectorAdd(3, 2, 3);

            VectorAdd(4, 3, 3);
            VectorAdd(5, 4, 3);

            VectorAdd(6, 4, 4);
            VectorAdd(7, 4, 5);
            VectorAdd(8, 4, 6);
            VectorAdd(9, 4, 7);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            border[0] = this.Content.Load<Texture2D>("border");
            position_border[0] = new Vector2(128, 0);

            rock[0] = this.Content.Load<Texture2D>("k_red");
            position_rock[0] = new Vector2(10, 60);

            paper[0] = this.Content.Load<Texture2D>("b_red");
            position_paper[0] = new Vector2(10, 180);

            scissors[0] = this.Content.Load<Texture2D>("n_red");
            position_scissors[0] = new Vector2(10, 300);


            rock[1] = this.Content.Load<Texture2D>("k_blue");
            position_rock[1] = new Vector2(10 + 660, 60);

            paper[1] = this.Content.Load<Texture2D>("b_blue");
            position_paper[1] = new Vector2(10 + 660, 180);

            scissors[1] = this.Content.Load<Texture2D>("n_blue");
            position_scissors[1] = new Vector2(10 + 660, 300);

            stepgame = step_game.game_user2;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        void Move_pole_user1()
        {
            if (playground[0] != type_figure.none)
            {
                for (int j = line_win-1; j > 0; j--)
                {
                    playground[j] = playground[j - 1];
                }
            }

            playground[0] = type_figure.none;
        }

        void Move_pole_user1_()
        {
            if (playground[0] != type_figure.none)
            {
                for (int j = line_win - 1; j >= 0; j--)
                {
                    if (j == line_win - 1)
                    {
                        switch (Check(j, j + 1))
                        {
                            case check_battle.can_move:
                                playground[j + 1] = playground[j];
                                playground[j] = type_figure.none;
                                break;
                            case check_battle.we_win:
                                playground[j + 1] = playground[j];
                                playground[j] = type_figure.none;
                                line_win++;
                                break;
                            case check_battle.death_both:
                                playground[j + 1] = type_figure.none;
                                playground[j] = type_figure.none;
                                return;
                                break;
                            case check_battle.we_lose:
                                playground[j] = type_figure.none;
                                break;
                        }
                    }
                    else
                    {
                        playground[j + 1] = playground[j];
                        playground[j] = type_figure.none;
                    }
                }
            }

        }

        void Move_pole_user2_()
        {
            if (playground[0] != type_figure.none)
            {
                for (int j = line_win; j <= size_playground; j++)
                {
                    if (j == line_win)
                    {
                        switch (Check(j, j - 1))
                        {
                            case check_battle.can_move:
                                playground[j - 1] = playground[j];
                                playground[j] = type_figure.none;
                                break;
                            case check_battle.we_win:
                                playground[j - 1] = playground[j];
                                playground[j] = type_figure.none;
                                line_win--;
                                break;
                            case check_battle.death_both:
                                playground[j - 1] = type_figure.none;
                                playground[j] = type_figure.none;
                                return;
                                break;
                            case check_battle.we_lose:
                                playground[j] = type_figure.none;
                                break;
                        }
                    }
                    else
                    {
                        playground[j - 1] = playground[j];
                        playground[j] = type_figure.none;
                    }
                }
            }

        }

        void Move_pole_user2()
        {
            if (playground[size_playground - 1] != type_figure.none)
            {
                for (int j = line_win; j < size_playground - 1; j++)
                {
                    playground[j] = playground[j + 1];
                }
            }

            playground[size_playground - 1] = type_figure.none;
        }

        type_figure Getnewfigureuser1()
        {
            TouchCollection touchLocations = TouchPanel.GetState();
            foreach (TouchLocation touchLocation in touchLocations)
            {
                if (touchLocation.State == TouchLocationState.Pressed)
                {
                    if (touchLocation.Position.X >= position_rock[0].X + 30 && touchLocation.Position.X <= position_rock[0].X + 90)
                        if (touchLocation.Position.Y >= position_rock[0].Y + 30 && touchLocation.Position.Y <= position_rock[0].Y + 90)
                        {
                            return type_figure.rock;
                        }

                    if (touchLocation.Position.X >= position_paper[0].X + 30 && touchLocation.Position.X <= position_paper[0].X + 90)
                        if (touchLocation.Position.Y >= position_paper[0].Y + 30 && touchLocation.Position.Y <= position_paper[0].Y + 90)
                        {
                            return type_figure.paper;
                        }

                    if (touchLocation.Position.X >= position_scissors[0].X + 30 && touchLocation.Position.X <= position_scissors[0].X + 90)
                        if (touchLocation.Position.Y >= position_scissors[0].Y + 30 && touchLocation.Position.Y <= position_scissors[0].Y + 90)
                        {
                            return type_figure.scissors;
                        }
                }
            }

            return type_figure.none;
        }

        type_figure Getnewfigureuser2()
        {
            if (user2 == type_user.numan)
            {
                TouchCollection touchLocations = TouchPanel.GetState();
                foreach (TouchLocation touchLocation in touchLocations)
                {
                    if (touchLocation.State == TouchLocationState.Pressed)
                    {
                        if (touchLocation.Position.X >= position_rock[0].X + 30 && touchLocation.Position.X <= position_rock[0].X + 90)
                            if (touchLocation.Position.Y >= position_rock[0].Y + 30 && touchLocation.Position.Y <= position_rock[0].Y + 90)
                            {
                                return type_figure.rock;
                            }

                        if (touchLocation.Position.X >= position_paper[0].X + 30 && touchLocation.Position.X <= position_paper[0].X + 90)
                            if (touchLocation.Position.Y >= position_paper[0].Y + 30 && touchLocation.Position.Y <= position_paper[0].Y + 90)
                            {
                                return type_figure.paper;
                            }

                        if (touchLocation.Position.X >= position_scissors[0].X + 30 && touchLocation.Position.X <= position_scissors[0].X + 90)
                            if (touchLocation.Position.Y >= position_scissors[0].Y + 30 && touchLocation.Position.Y <= position_scissors[0].Y + 90)
                            {
                                return type_figure.scissors;
                            }
                    }
                }
            }
            else if (user2 == type_user.computer)
            {
                switch (/*(switchtest++)%3*/rand.Next(3))
                {
                    case 0:
                        return type_figure.paper;
                    case 1:
                        return type_figure.rock;
                    case 2:
                        return type_figure.scissors;
                }
            }

            return type_figure.none;
        }

        void Update_play_ground()
        {
            for (int i = 0; i < line_win; i++)
            {
                if (playground[i] == type_figure.rock)
                {
                    play_ground[i] = this.Content.Load<Texture2D>("k_red_64");
                }
                else if (playground[i] == type_figure.paper)
                {
                    play_ground[i] = this.Content.Load<Texture2D>("b_red_64");
                }
                else if (playground[i] == type_figure.scissors)
                {
                    play_ground[i] = this.Content.Load<Texture2D>("n_red_64");
                }
                else
                {
                    play_ground[i] = null;
                }
            }

            for (int i = line_win; i < size_playground; i++)
            {
                if (playground[i] == type_figure.rock)
                {
                    play_ground[i] = this.Content.Load<Texture2D>("k_blue_64");
                }
                else if (playground[i] == type_figure.paper)
                {
                    play_ground[i] = this.Content.Load<Texture2D>("b_blue_64");
                }
                else if (playground[i] == type_figure.scissors)
                {
                    play_ground[i] = this.Content.Load<Texture2D>("n_blue_64");
                }
                else
                {
                    play_ground[i] = null;
                }
            }
        }

        check_battle Check(int indOur, int indMove)
        {   // анализ выигрыша
            if (indMove > size_playground - 1)
            {   // user 1 game win
                stepgame = step_game.win1;
            }
            else if (indMove < 1)
            {   // user 2 game win
                stepgame = step_game.win2;
            }
            else if (playground[indMove] == type_figure.none) return check_battle.can_move;
            else 
            {   // потоки встретились
                if (playground[indOur] == playground[indMove])
                    return check_battle.death_both;
                else if (((playground[indOur] == type_figure.rock) && (playground[indMove] == type_figure.scissors))
                        || ((playground[indOur] == type_figure.scissors) && (playground[indMove] == type_figure.paper))
                        || ((playground[indOur] == type_figure.paper) && (playground[indMove] == type_figure.rock))
                    )
                {   // user1 win
                    return check_battle.we_win;

                }
                else
                {   // user2 win
                    return check_battle.we_lose;

                }
            }
            return check_battle.none;
        }

        void Check_win()
        {   // анализ выигрыша
            if (line_win >= size_playground-1)
            {   // user 1 game win
                stepgame = step_game.win1;
            }
            else if (line_win <= 1)
            {   // user 2 game win
                stepgame = step_game.win2;
            }
            else if ((playground[size_playground / 2 - 1] != type_figure.none) && (playground[size_playground/2] != type_figure.none))
            {   // потоки встретились
                if (((playground[line_win - 1] == type_figure.rock) && (playground[line_win] == type_figure.rock))
                    || ((playground[line_win - 1] == type_figure.paper) && (playground[line_win] == type_figure.paper))
                    || ((playground[line_win - 1] == type_figure.scissors) && (playground[line_win] == type_figure.scissors)))
                {   // ничья - сдвигаем поле
                    Move_pole_user1();
                    Move_pole_user2();
                }
                else if (((playground[line_win - 1] == type_figure.rock) && (playground[line_win] == type_figure.scissors))
                        || ((playground[line_win - 1] == type_figure.scissors) && (playground[line_win] == type_figure.paper))
                        || ((playground[line_win - 1] == type_figure.paper) && (playground[line_win] == type_figure.rock))
                    )
                {   // user1 win
                    line_win++;

                    Move_pole_user1();
                }
                else
                {   // user2 win
                    line_win--;

                    Move_pole_user2();
                }
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (stepgame == step_game.game_user1)
            {
                if (delay % 30 == 0)
                {
                    type_figure new_figure = type_figure.none;

                    // пользователь1  добавил новую фигуру
                    new_figure = Getnewfigureuser1();

                    if (new_figure != type_figure.none)
                    {
                        // сначала все поле сдвинем
                        Move_pole_user1_();

                        if (playground[0] == type_figure.none)
                        {
                            playground[0] = new_figure;
                            position_user1++;

                            shift1 = 64;
                            shift2 = 0;
                            delay++;
                        }

                        if (stepgame == step_game.game_user1)
                        {
                            stepgame = step_game.game_user2;
                        }
                    }

                    Update_play_ground();
                }
                else
                {
                    delay++;
                    shift2 -= 2;
                }
            }
            else if (stepgame == step_game.game_user2)
            {
                if (delay % 30 == 0)
                {
                    type_figure new_figure = type_figure.none;
                    // ход компьютера - если первый походил
                    new_figure = Getnewfigureuser2();

                    if (new_figure != type_figure.none)
                    {
                        Move_pole_user2_();

                        if (playground[size_playground - 1] == type_figure.none)
                        {
                            playground[size_playground - 1] = new_figure;
                            position_user2--;

                            shift1 = 0;
                            shift2 = 64;
                            delay++;
                        }

                        if (stepgame == step_game.game_user2)
                        {
                            stepgame = step_game.game_user1;
                        }
                    }

                    Update_play_ground();
                }
                else
                {
                    delay++;
                    shift1 -= 2;
                }
            }
            else if (stepgame == step_game.win1 || stepgame == step_game.win2)
            {
                TouchCollection touchLocations = TouchPanel.GetState();
                foreach (TouchLocation touchLocation in touchLocations)
                {
                    if (touchLocation.State == TouchLocationState.Pressed)
                    {
                        stepgame = step_game.start;
                        LoadContent();

                        for (int i = 0; i < size_playground; i++)
                        {   // обновим поле
                            playground[i] = type_figure.none;
                        }

                        line_win = size_playground / 2;
                        position_user2 = size_playground / 2 - 1;
                    }
                }

                Update_play_ground();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (stepgame == step_game.game_user1 || stepgame == step_game.game_user2)
            {
                GraphicsDevice.Clear(Color.White);

                spriteBatch.Begin();

                spriteBatch.Draw(rock[0], position_rock[0], Color.White);
                spriteBatch.Draw(rock[1], position_rock[1], Color.White);

                spriteBatch.Draw(paper[0], position_paper[0], Color.White);
                spriteBatch.Draw(paper[1], position_paper[1], Color.White);

                spriteBatch.Draw(scissors[0], position_scissors[0], Color.White);
                spriteBatch.Draw(scissors[1], position_scissors[1], Color.White);

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        position_border[0].X = 20 + 128 + 64 * i;
                        position_border[0].Y = 20 + 64 * j;

                        spriteBatch.Draw(border[0], position_border[0], Color.White);
                    }
                }

                for (int i = line_win-1; i >= 0; i--)
                {
                    if (play_ground[i] != null)
                    {
                        Vector2 vector = position_play_figure[i];

                        //if (i != 0)
                        {
                            vector.X -= shift1;
                        }

                        spriteBatch.Draw(play_ground[i], vector, Color.White);
                    }
                }

                for (int i = line_win; i < size_playground; i++)
                {
                    if (play_ground[i] != null)
                    {
                        Vector2 vector = position_play_figure[i];

                        //if (i != size_playground-1)
                        {
                            vector.X += shift2;
                        }

                        spriteBatch.Draw(play_ground[i],vector, Color.White);
                    }
                }

                spriteBatch.End();
            }
            else if (stepgame == step_game.win1)
            {
                GraphicsDevice.Clear(Color.Red);
            }
            else if (stepgame == step_game.win2)
            {
                GraphicsDevice.Clear(Color.Blue);
            }

            base.Draw(gameTime);
        }
    }
}
