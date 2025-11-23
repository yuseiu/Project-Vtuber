using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataHandle : MonoBehaviour
{
    public static CharacterDataHandle Instance { get; private set; }

    public CharacterDatabase characterDatabase;
    public CharacterDataInGame characterDataInGame;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void InitializeCharacterData()
    {
        if (characterDataInGame.LastSyncedVersion < characterDatabase.DataVersion)
        {
            int oldVersion = characterDataInGame.LastSyncedVersion;

            characterDataInGame.Content.Clear();

            foreach (var dbContent in characterDatabase.Content)
            {
                if (!dbContent.CreateData) continue;

                characterDataInGame.Content.Add(CreateNewContentFromDatabase(dbContent));
            }

            characterDataInGame.LastSyncedVersion = characterDatabase.DataVersion;
            Debug.Log($"Database version changed → Reloaded all characters. Update version from {oldVersion} to {characterDataInGame.LastSyncedVersion}.");
        }
        else
        {
            foreach (var dbContent in characterDatabase.Content)
            {
                if (!dbContent.CreateData) continue;

                var existing = characterDataInGame.Content.Find(
                    c => c.CharacterInformation.ID == dbContent.CharacterInformation.ID
                );

                if (existing == null)
                {
                    var newContent = CreateNewContentFromDatabase(dbContent);
                    characterDataInGame.Content.Add(newContent);
                    Debug.Log($"Created new character: {dbContent.CharacterInformation.ID}");
                }
                else if (dbContent.UpdateStat)
                {
                    existing.BaseStat = new CharacterStat
                    {
                        MaxHP = dbContent.Stat.MaxHP,
                        Damage = dbContent.Stat.Damage,
                        Speed = dbContent.Stat.Speed,
                        Luck = dbContent.Stat.Luck
                    };
                    existing.RecalculateTotalStat();
                    dbContent.UpdateStat = false; // reset flag
                    Debug.Log($"Force updated base stat for {dbContent.CharacterInformation.ID}");
                }
                else if (dbContent.UpdateInformation)
                {
                    existing.CharacterInformation = new CharacterInformation
                    {
                        ID = dbContent.CharacterInformation.ID,
                        Name = dbContent.CharacterInformation.Name,
                        Avatar = dbContent.CharacterInformation.Avatar,
                        Prefab = dbContent.CharacterInformation.Prefab,
                        Description = dbContent.CharacterInformation.Description
                    };
                    existing.RecalculateTotalStat();
                    dbContent.UpdateInformation = false; // reset flag
                    Debug.Log($"Force updated Information for {dbContent.CharacterInformation.ID}");
                }
            }
        }
    }

    private Contents CreateNewContentFromDatabase(Content dbContent)
    {
        var content = new Contents
        {
            CharacterInformation = new CharacterInformation
            {
                ID = dbContent.CharacterInformation.ID,
                Name = dbContent.CharacterInformation.Name,
                Avatar = dbContent.CharacterInformation.Avatar,
                Prefab = dbContent.CharacterInformation.Prefab,
                Description = dbContent.CharacterInformation.Description
            },
            BaseStat = new CharacterStat
            {
                MaxHP = dbContent.Stat.MaxHP,
                Damage = dbContent.Stat.Damage,
                Speed = dbContent.Stat.Speed,
                Luck = dbContent.Stat.Luck
            },
            BonusStat = new CharacterStat()
        };

        content.RecalculateTotalStat();
        return content;
    }
}
