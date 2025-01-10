using System.Collections.Generic;
using System.IO;
using System.Linq;
using MagicWords.Domain.Data.Models;
using UnityEngine;

namespace MagicWords.Domain.Logic.Services
{
    public class DictionaryService
    {
        private HashSet<string> words;
        private HashSet<string> prefixes;

        public DictionaryService(TextAsset dictionaryAsset)
        {
            LoadDictionary(dictionaryAsset);
        }

        private void LoadDictionary(TextAsset dictionaryAsset)
        {
            words = new HashSet<string>(dictionaryAsset.text.Split('\n').Select(word => word.Trim().ToUpper()));
            prefixes = new HashSet<string>();

            // Generar todos los prefijos posibles
            foreach (string word in words)
            {
                for (int i = 1; i <= word.Length; i++)
                {
                    prefixes.Add(word.Substring(0, i));
                }
            }
        }

        public bool IsValidWord(string word)
        {
            return words.Contains(word.ToUpper());
        }

        public bool IsValidPrefix(string prefix)
        {
            return prefixes.Contains(prefix.ToUpper());
        }
    }
}