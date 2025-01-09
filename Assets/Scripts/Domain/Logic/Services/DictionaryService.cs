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

        public DictionaryService(TextAsset dictionaryAsset)
        {
            LoadDictionary(dictionaryAsset);
        }

        private void LoadDictionary(TextAsset dictionaryAsset)
        {
            words = new HashSet<string>(dictionaryAsset.text.Split('\n').Select(word => word.Trim().ToUpper()));
        }

        public bool IsValidWord(string word)
        {
            return words.Contains(word.ToUpper());
        }
    }
}