using System;
using System.Linq;
using OpenUtau.Api;
using OpenUtau.Plugin.Builtin;
using System.Collections.Generic;

namespace OpenUtau.Api {

    [Phonemizer("X-Sampa Phonemizer", "X-SAMPA", "Hilmi Yafi A")]
    public class XSampaPhonemizer : SyllableBasedPhonemizer {

        string[] vowels = "i,e,E,a,A,O,o,u,y,2,9,&,Q,V,7,M,1,},I,Y,U,@,8,6,{,3,@`,3\\,@\\".Split(",");

        protected override string[] GetVowels() => vowels;

        protected override List<string> ProcessEnding(Ending ending) {
            List<string> phonemes = new List<string>();
            string[] cc = ending.cc;
            if (cc.Length > 0) {
                if (!TryAddPhoneme(phonemes, ending.tone, ending.prevV + " " + cc[0], ending.prevV + cc[0]))
                    phonemes.Add(cc[0]);
            } else {
                TryAddPhoneme(phonemes, ending.tone, ending.prevV + " -", ending.prevV + "-");
            }
            for (int i = 1; i < cc.Length; i++)
                phonemes.Add(cc[i]);
            return phonemes;
        }

        protected override List<string> ProcessSyllable(Syllable syllable) {
            List<string> phonemes = new List<string>();
            string[] cc;
            if (syllable.prevV != "" && syllable.PreviousWordCc.Length > 0) {
                cc = syllable.PreviousWordCc;
                if (cc.Length > 0) {
                    if (!TryAddPhoneme(phonemes, syllable.tone, syllable.prevV + " " + cc[0], syllable.prevV + cc[0]))
                        phonemes.Add(cc[0]);
                } else {
                    TryAddPhoneme(phonemes, syllable.tone, syllable.prevV + " -", syllable.prevV + "-");
                }
                for (int i = 1; i < cc.Length; i++)
                    phonemes.Add(cc[i]);
            }
            cc = syllable.CurrentWordCc;
            for (int i = 0; i < cc.Length - 1; i++) {
                if (cc[i + 1] == "w")
                    phonemes.Add(cc[i] + "u");
                else if (cc[i + 1] == "j")
                    phonemes.Add(cc[i] + "i");
                else
                    phonemes.Add(cc[i] + "@");
            }
            phonemes.Add((cc.Length > 0 ? cc.Last() : "") + syllable.v);
            return phonemes;
        }
    }
}
