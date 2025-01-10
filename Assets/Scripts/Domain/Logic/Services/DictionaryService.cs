using System.Collections.Generic;
using System.IO;
using System.Linq;
using MagicWords.Domain.Data.Models;
using UnityEngine;

namespace MagicWords.Domain.Logic.Services
{
    [CreateAssetMenu(menuName = "Services/Dictionary Service")]
    public class DictionaryService : ScriptableObject
    {
        [SerializeField] private TextAsset dictionaryAsset;
        private Trie trie;

        public void LoadDictionary()
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