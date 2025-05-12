using System.Linq;
using GameEnums;
using GameRecords;
using UnityEngine;

namespace GameStores
{
    [CreateAssetMenu(fileName = "AnimalStore",
                     menuName = "Stores/AnimalStore")]
    public class AnimalStore : ScriptableObject
    {
        [SerializeField] private AnimalRecord[] animalRecords;

        public AnimalRecord GetRecordByAnimal(Animal animal)
            => animalRecords.FirstOrDefault(ar => ar.animal == animal);
    }
}