using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Header("Prefabs")]
    public ArtilleryController artilleryPrefab;
    public TankController tankPrefab;
    public Projectile projectilePrefab;

    [Header("Settings")]
    public Transform[] tankSpawnPoints;
    public float tankSpawnInterval = 2f;
    public float tankSpeed = 3f;
    public float fireReloadTime = 3f;
    public float tankDamageRadius = 2f;

    [Header("Projectile Settings")]
    public float projectileSpeed = 20f;
    public float projectileLifetime = 5f;
    public override void InstallBindings()
    {
        // Inputs
        Container.Bind<IGameplayInput>().To<MobileInput>().FromComponentInHierarchy().AsSingle();

        // Artillery
        Container.Bind<ArtilleryController>()
                .FromComponentInHierarchy()
                .AsSingle();

        Container.BindFactory<ArtilleryController, ArtilleryController.Factory>()
                .FromComponentInNewPrefab(artilleryPrefab);

        // Projectile Pool
        Container.Bind<Projectile.ProjectileSettings>()
                 .FromInstance(new Projectile.ProjectileSettings {
                     Speed    = projectileSpeed,
                     Lifetime = projectileLifetime
                 })
                 .AsSingle();
        Container.BindMemoryPool<Projectile, Projectile.Pool>()
                 .WithInitialSize(10)
                 .FromComponentInNewPrefab(projectilePrefab)
                 .UnderTransformGroup("Projectiles");

        // Tanks
        Container.Bind<float>()
                .WithId("TankSpeed")
                .FromInstance(tankSpeed)
                .AsSingle();

        Container.BindFactory<TankController, TankController.Factory>()
                .FromComponentInNewPrefab(tankPrefab)
                .UnderTransformGroup("Tanks");

        Container.Bind<TankSpawner>()
                .AsSingle()
                .WithArguments(tankSpawnPoints, tankSpawnInterval)
                .NonLazy();


        //UI
        Container.Bind<UIManager>()
                .FromComponentInHierarchy()
                .AsSingle();

        // Game States
        Container.BindInterfacesAndSelfTo<PlayingState>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameOverState>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
    }
}
