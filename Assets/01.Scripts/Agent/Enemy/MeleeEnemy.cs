using UnityEngine; 

public class MeleeEnemy : Enemy
{
    private void Start()
    {

        EnemyTree<MeleeEnemy> tree = new EnemyTree<MeleeEnemy>(this);
        
    }
    public void A()
    {

    }
}