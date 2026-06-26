using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace demo
{
    public class GameVoice
    {
  

            private readonly SpeechSynthesizer _synth;

            public GameVoice()
            {
                _synth = new SpeechSynthesizer();
                try
                {
                    var femaleVoices = _synth.GetInstalledVoices()
                                            .Where(v => v.VoiceInfo.Gender == VoiceGender.Female);
                    if (femaleVoices.Any()) _synth.SelectVoice(femaleVoices.First().VoiceInfo.Name);
                    _synth.Rate = 1;
                    _synth.Volume = 100;
                }
                catch { /* Fallback to default if no female voice */ }
            }

            public void Speak(string text) => _synth.SpeakAsync(text);
        }
    }


