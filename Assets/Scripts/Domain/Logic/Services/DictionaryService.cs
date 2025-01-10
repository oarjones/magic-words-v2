using System.Collections.Generic;
using System.IO;
using System.Linq;
using MagicWords.Domain.Data.Models;
using UnityEngine;

namespace MagicWords.Domain.Logic.Services
{
    public class DictionaryService
    {
        private Trie trie;

        public DictionaryService(TextAsset dictionaryAsset)
        {
            LoadDictionary(dictionaryAsset);
        }

        private void LoadDictionary(TextAsset dictionaryAsset)
        {
            trie = new Trie();
            string[] words = dictionaryAsset.text.Split('\n');
            foreach (string word in words)
            {
                trie.Insert(word.Trim());
            }
        }

        public bool IsValidWord(string word)
        {
            return trie.Search(word);
        }

        public bool IsValidPrefix(string prefix)
        {
            return trie.StartsWith(prefix);
        }
    }
}