# OpenUtau X-Sampa Phonemizer
This repository contains a C# library code for OpenUtau X-Sampa Phonemizer. 

## Installation
1. Put the dll file into the Plugins folder inside OpenUtau root folder.
2. Put the yaml file into your voicebank folder.
3. Optional: you can open the yaml file using notepad and modify the list of vowels to suit your voicebank.

## Usage
- Write only one syllable per note. A group of vowels (not separated by any consonant) is considered one syllable.
One syllable: 
friend `[f r E n d]`
strikes `[s t r a I k s]`
Not one syllable: 
brother `[b r V D @ r]`
singing `[s I N I N]`
You have to split them into two notes:
brother `[b r V] [D @ r]`
singing `[s I] [N I N]`

- Add consonants at the end and/or beginning of a note to create CVVC.
I'm a pilot `[a i m] [@] [p a i] [l @ t]` becomes `[a i m] [m @ p] [p a i l] [l @ t]`

- If you have endings, you can treat it as a consonant.
