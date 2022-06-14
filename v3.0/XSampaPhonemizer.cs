using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenUtau.Api;
using OpenUtau.Core.Ustx;
using OpenUtau.Plugin.Builtin;

namespace OpenUtau.Api {

    [Phonemizer("X-Sampa Phonemizer", "X-SAMPA", "Hilmi Yafi A")]
    public class XSampaPhonemizer : SyllableBasedPhonemizer {

        public XSampaPhonemizerConfig config;

        protected override string[] GetVowels() => config.vowels;

        protected override List<string> ProcessEnding(Ending ending) {
            var phonemes = new List<string>();
            var prev = ending.prevV;
            var tone = ending.tone;
            var cc = ending.cc;
            for (int i = 0; i < cc.Length; prev = cc[i++])
                if (!TryAddPhoneme(phonemes, tone, prev + " " + cc[i], prev + cc[i], cc[i], cc[i] + config.fallbackVowel, cc[i] + " " + config.fallbackVowel))
                    phonemes.Add(cc[i]);
            return phonemes;
        }

        protected override List<string> ProcessSyllable(Syllable syllable) {
            var phonemes = new List<string>();
            var tone = syllable.tone;
            var prev = syllable.prevV;
            var cc = syllable.PreviousWordCc;
            if (prev != "" && cc.Length > 0)
                for (int i = 0; i < cc.Length; prev = cc[i++])
                    if (!TryAddPhoneme(phonemes, tone, prev + " " + cc[i], prev + cc[i], cc[i], cc[i] + config.fallbackVowel, cc[0] + " " + config.fallbackVowel))
                        phonemes.Add(cc[i]);
            cc = syllable.CurrentWordCc;
            if (cc.Length > 0)
                prev = cc.Last();
            else if (syllable.prevV != "" && syllable.prevWordConsonantsCount == 0)
                prev = syllable.prevV;
            else
                prev = "-";
            for (int i = 0; i < cc.Length - 1; i++)
                if (!TryAddPhoneme(phonemes, tone, cc[i] + cc[i + 1], cc[i] + " " + cc[i + 1], cc[i] + config.fallbackVowel, cc[i] + " " + config.fallbackVowel))
                    phonemes.Add(cc[i]);
            if (!TryAddPhoneme(phonemes, tone, prev + " " + syllable.v, prev + syllable.v))
                phonemes.AddRange((!config.vowels.Contains(prev) && prev != "-") ? new string[] { prev, syllable.v } : new string[] {  syllable.v });
            return phonemes;
        }

        public override void SetSinger(USinger singer) {
            base.SetSinger(singer);
            try {
                string file = Path.Combine(singer.Location, "xsampa.yaml");
                if (!File.Exists(file))
                    WriteConfig(new StreamWriter(file, false, System.Text.Encoding.UTF8));
                else if (!ReadConfig(new StreamReader(file)))
                    config = new XSampaPhonemizerConfig();
            } catch {
                config = new XSampaPhonemizerConfig();
            }
        }

        void WriteConfig(StreamWriter writer) {
            config = new XSampaPhonemizerConfig();
            writer.WriteLine("fallback_vowel: \"@\"");
            writer.WriteLine("vowels: [\"a\", \"{\", \"E\", \"e\", \"I\", \"i\", \"y\", \"Y\", \"2\", \"9\", \"&\", \"6\", \"3\", \"@\", \"@\\\", \"1\", \"}\", \"8\", \"3\\\", \"A\", \"V\", \"7\", \"M\", \"u\", \"U\", \"o\", \"O\", \"Q\", \"@`\"]");
            writer.Flush();
            writer.Close();
        }

        bool ReadConfig(StreamReader reader) {
            try {
                config = Core.Yaml.DefaultDeserializer.Deserialize<XSampaPhonemizerConfig>(reader);
            } catch {
                return false;
            }
            return true;
        }
    }

    public class XSampaPhonemizerConfig {
        public string fallbackVowel = "@";
        public string[] vowels = "i,e,E,a,A,O,o,u,y,2,9,&,Q,V,7,M,1,},I,Y,U,@,8,6,{,3,@`,3\\,@\\".Split(",");
    }
}
