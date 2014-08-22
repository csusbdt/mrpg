using System;
using System.Collections.Generic;
using System.Text;
using Tao.Sdl;

namespace Client
{
    static class GuiSystem
    {
        #region Fields

        static List<Button> buttons = new List<Button>();
        static List<Image> images = new List<Image>();
        static List<Text> texts = new List<Text>();

        #endregion

        #region Life Cycle

        public static void Init()
        {
            Program.MouseButtonUpEvent += MouseButtonUpHandler;
            Program.KeyUpEvent += KeyUpEventHandler;
        }

        public static void Update(TimeSpan dt)
        {
            foreach (Button button in buttons)
            {
                button.Update(dt);
            }

            foreach (Image image in images)
            {
                image.Update(dt);
            }

            foreach (Text text in texts)
            {
                text.Update(dt);
            }
        }

        #endregion

        #region Gui Event Handlers

        public static void MouseButtonUpHandler(ScreenCoordinate clickPoint)
        {
            ClientState currentState = Program.ClientState;
            foreach (Button button in buttons)
            {
                if (button.IsInside(clickPoint) && button.Enabled)
                {
                    if (button is AvatarSelectButton)
                    {
                        AvatarSelectButton avatarSelectButton = (AvatarSelectButton)button;
                        Program.Avatar = avatarSelectButton.Avatar;
                    }
                    
                    button.Clicked = true;
                    button.ClickEventHandler();                    
                    if (currentState != Program.ClientState)
                    {
                        return;
                    }
                }
                else
                {
                    button.Clicked = false;
                }
            }

            foreach (Text text in texts)
            {
                TextInput textInput = text as TextInput;
                bool found = false;
                if (textInput != null)
                {
                    if (!found && textInput.IsInside(clickPoint))
                    {
                        textInput.focusHandler();
                        found = true;
                    }
                    else
                    {
                        textInput.blurHandler();
                    }
                }
            }
        }

        public static void KeyUpEventHandler(Sdl.SDL_keysym keysym)
        {
            foreach (Text text in texts)
            {
                TextInput textInput = text as TextInput;
                if (textInput != null)
                {
                    if (textInput.Focused)
                    {
                        textInput.modifyTextHandler(keysym);
                        Log.Write("Hit key " + keysym.sym);
                        return;
                    }
                }
            }
        }


        #endregion

        #region Operations

        public static void Clear()
        {
            foreach (Button button in buttons)
            {
                GraphicsSystem.GuiComponents.Remove(button);
            }
            foreach (Image image in images)
            {
                GraphicsSystem.GuiComponents.Remove(image);
            }
            foreach (Text text in texts)
            {
                GraphicsSystem.GuiComponents.Remove(text);
            }
            buttons.Clear();
            images.Clear();
            texts.Clear();
        }


        public static void AddButton(Button button)
        {
            buttons.Add(button);
            GraphicsSystem.GuiComponents.Add(button);
        }

        public static void RemoveButton(Button button)
        {
            buttons.Remove(button);
            GraphicsSystem.GuiComponents.Remove(button);
        }

        public static void AddImage(Image image)
        {
            images.Add(image);
            GraphicsSystem.GuiComponents.Add(image);
        }

        public static void RemoveImage(Image image)
        {
            images.Remove(image);
            GraphicsSystem.GuiComponents.Remove(image);
        }

        public static void AddText(Text text)
        {
            texts.Add(text);
            GraphicsSystem.GuiComponents.Add(text);
        }

        public static void RemoveText(Text text)
        {
            texts.Remove(text);
            GraphicsSystem.GuiComponents.Remove(text);
        }

        #endregion

    }
}
