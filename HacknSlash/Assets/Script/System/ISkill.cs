

using UnityEngine;

public interface ISkill
{
    public void Casting();
    public void ActtSkill(Vector3 _pos);

}


public class SkillA : ISkill
{
    public void ActtSkill(Vector3 _pos)
    {
        throw new System.NotImplementedException();
    }

    public void Casting()
    {

    }

}
public class SkillB : ISkill
{
    public void ActtSkill(Vector3 _pos)
    {
        throw new System.NotImplementedException();
    }

    public void Casting()
    {

    }

}