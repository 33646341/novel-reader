using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace ReadTool
{
    class chapter_reader
    {
        public SpeechSynthesizer speak = new SpeechSynthesizer();

        public void speak_begin(string chapter)
        {
            speak.Volume = 100;

            speak.Rate = -1;

            if (chapter != "")
            {
                speak.SpeakAsync(chapter);
            }
        }

        public void speak_puase()
        {
            speak.Pause();
        }
    }
}
