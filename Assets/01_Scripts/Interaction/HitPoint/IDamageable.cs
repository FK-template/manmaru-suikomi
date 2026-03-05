using UnityEngine;

namespace Manmaru.Interaction
{
    /// <summary>
    /// ダメージを受けうるオブジェクトのインターフェース
    /// </summary>
    public interface IDamageable
    {
        void TakeDamage(float damageValue);
    }
}