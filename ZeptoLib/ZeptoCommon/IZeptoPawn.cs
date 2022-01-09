namespace ZeptoCommon;

  public interface IZeptoPawn
  {
    public int GetAttrib(string key);

    public void SetAttrib(string key, int amount);

    public void ModifyAttrib(string key, int amount);

  }
