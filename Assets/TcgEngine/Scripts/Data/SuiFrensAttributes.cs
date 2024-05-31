using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace TcgEngine
{
    [CreateAssetMenu(fileName = "sui_frens_attributes", menuName = "TcgEngine/SuiFrensAttributes", order = 5)]
    public class SuiFrensAttributes : ScriptableObject
    {
        public string skin;
        public string primary_color;
        public string secondary_color;
        public string mood;
        public string characteristic;

        public bool IsBullShark()
        {
            return characteristic == "classic";
        }

        public string SpeciesType()
        {
            return IsBullShark() ? "BullShark" : "Capy";
        }

        public string FrenName()
        {
            return $"{mood} {skin} {SpeciesType()}";
        }
    }
}
