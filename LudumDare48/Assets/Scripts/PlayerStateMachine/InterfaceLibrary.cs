public interface IDamageable {
    void TakeDamage(float damage);
    void TakeDamage(float damage, bool hitFromRight);
}

public interface IPooledObject {
    void OnObjectSpawn();
}

public interface IIgnoreObject {
    bool IgnoreMe();
}

public enum Item {
    GEM,
    HEALTH,
    OXYGEN,
    DEFAULT
}
public interface ICollector {
    bool OnCollect(Item item);
}