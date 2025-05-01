public class ArtillerySettings
  {
    public float ReloadTime { get; }
    public float DamageRadius { get; }

    public ArtillerySettings(float reloadTime, float damageRadius)
    {
      ReloadTime     = reloadTime;
      DamageRadius   = damageRadius;
    }
  }