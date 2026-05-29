using System;
using System.IO;
using System.Media;

namespace demo
{
    //start of namespace
    public class voice_greeting
    {//start of class



        //void method to play the sound named greet
        public void greet()
        { //star of greet method

            //replace the \bin\Debug\ from the path with greeting.wav
            try
            {
                string auto_path = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\", @"\myrecord.wav");

                if (!File.Exists(auto_path))
                    return; // Skip if file missing
                            //create an instance for the soundPlayer class
                SoundPlayer greetMe = new SoundPlayer(auto_path);
                //then greet
                greetMe.Play();
            }
            catch (Exception)
            {
                // Fail silently so app doesn't crash without sound
            }
        }//end of greet method



    }//end of class
}//end of namespace