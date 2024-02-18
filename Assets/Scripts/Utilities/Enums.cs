using System;

public enum NPCState
{
    Patrol, Chase, Skill
}

[Serializable]
public enum SceneType
{
    Location, Menu
}

[Serializable]
public enum PersistentType
{
    ReadWrite, DoNotPersist
}
