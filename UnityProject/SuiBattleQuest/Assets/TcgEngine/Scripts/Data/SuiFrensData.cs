using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using System.Collections;
using System;
using Unity.VectorGraphics;
using System.IO;
using System.Security.Cryptography;

namespace TcgEngine
{
    public class CoroutineManager : MonoBehaviour
    {
        private static CoroutineManager _instance;

        public static CoroutineManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("CoroutineManager");
                    _instance = obj.AddComponent<CoroutineManager>();
                    DontDestroyOnLoad(obj);
                }
                return _instance;
            }
        }

        public void StartManagedCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }
    }

    [CreateAssetMenu(fileName = "suifren", menuName = "TcgEngine/SuiFrenData", order = 5)]
    public class SuiFrensData : ScriptableObject
    {
        public string id;

        public string image;
        public SuiFrensAttributes attribute;
        public int[] genes;

        public static List<SuiFrensData> frens_list = new();
        public static Dictionary<string, SuiFrensData> frens_dict = new();

        public static void Load(string folder = "")
        {
            if (frens_list.Count == 0)
            {
                frens_list.AddRange(Resources.LoadAll<SuiFrensData>(folder));
                
                foreach (SuiFrensData fren in frens_list)
                    frens_dict.Add(fren.id, fren);
            }
        }

        public static byte[] GenerateRandomByteArray(int length)
        {
            byte[] randomBytes = new byte[length];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        public static IEnumerator GetImage(string url, Action<Sprite> callback)
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            while (request.isDone == false)
                yield return null;

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                callback(null);
            }
            else
            {
                
                string b64Value = request.downloadHandler.text;
                Sprite sprite = B642PNG(b64Value);
                if (sprite != null)
                    callback(sprite);
            }
        }

        private static Sprite B642PNG(string b64)
        {
            byte[] image_bytes = Convert.FromBase64String(b64);

            var tex = new Texture2D(1, 1);
            tex.LoadImage(image_bytes);
            var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));

            return sprite;
        }

        public CardData ConvertToCard()
        {
            CardData returnCard = CreateInstance<CardData>();
            Sprite image_full = B642PNG(image);
            Rect image_board_rect = new(
                image_full.rect.x,
                image_full.rect.y + image_full.rect.height / 2,
                image_full.rect.width,
                image_full.rect.height / 2
            );
            Sprite image_board = Sprite.Create(
                image_full.texture,
                image_board_rect,
                new Vector2(0.5f, 1.0f),
                image_full.pixelsPerUnit
            );
            TeamData[] teams = Resources.LoadAll<TeamData>("Teams");
            RarityData[] rarities = Resources.LoadAll<RarityData>("Rarities");
            AbilityData[] all_abilities = Resources.LoadAll<AbilityData>("Abilities");

            returnCard.id = id;
            returnCard.art_full = image_full;
            returnCard.art_board = image_board;
            returnCard.type = CardType.Character;

            // The various traits below will be generated using the Gene array:
            // - Team is first to be generated (0th index)
            // - Rarity is second to be generated (1st index)
            // - Trait data will be generated with such:
            //   - Bull Shark: "classic" value for characteristic
            //   - Capy: Anything other value for characteristic
            // - Abilities
            //   - Amount will be determined by the third value (2nd index)
            //   - Next n amount of bytes will choose the Abilities (113 abilities)
            // - HP, Mana, and Attack are assigned the three bits after above step

            byte[] random_gene = GenerateRandomByteArray(32);
            //byte[] random_gene = genes;

            int max_hp = 20;
            int max_mana = 5;
            int max_attack = max_hp / 2;

            int team_idx = random_gene[0] % teams.Length;
            int rarity_idx = random_gene[1] % rarities.Length;
            int abilities_length = (random_gene[2] + 2) % 26;
            int hp_value = random_gene[random_gene.Length - 1] % max_hp;
            int mana_value = random_gene[random_gene.Length - 2] % max_mana;
            int attack_value = random_gene[random_gene.Length - 3] % max_attack;

            List<AbilityData> abilities = new();
            for (int i = 0; i < abilities_length; ++i)
            {
                int abilities_idx = random_gene[2 + i] % all_abilities.Length;
                abilities.Add(all_abilities[abilities_idx]);
            }

            returnCard.team = teams[team_idx];
            returnCard.rarity = rarities[rarity_idx];
            returnCard.abilities = abilities.ToArray();

            var capy = Resources.Load<TraitData>("Traits/capy");
            var bull_shark = Resources.Load<TraitData>("Traits/bull_shark");
            returnCard.traits = new List<TraitData>() { attribute.IsBullShark() ? bull_shark : capy }.ToArray();

            returnCard.hp = hp_value;
            returnCard.mana = mana_value;
            returnCard.attack = attack_value;

            returnCard.title = attribute.FrenName();

            returnCard.deckbuilding = true;

            return returnCard;
        }
    }
}
