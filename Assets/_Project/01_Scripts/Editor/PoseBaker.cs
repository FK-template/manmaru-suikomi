#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class PoseBaker
{
    // ヒエラルキーの右クリックメニューに新しいボタンを追加する
    [MenuItem("GameObject/Bake Pose (プレビュー状態を複製)", false, 0)]
    static void BakePose()
    {
        GameObject source = Selection.activeGameObject;
        if (source == null) return;

        // 1. オブジェクトを複製する
        GameObject clone = Object.Instantiate(source, source.transform.parent);
        clone.name = source.name + "_Baked";

        // 2. クローン側のAnimatorを削除する（勝手にポーズをリセットさせないため）
        Animator anim = clone.GetComponent<Animator>();
        if (anim != null) Object.DestroyImmediate(anim);

        // 3. プレビュー中のTransform（座標・回転・大きさ）を、クローンに再帰的に完全コピーする
        CopyTransformRecurse(source.transform, clone.transform);

        // 4. Ctrl+Zで戻せるように登録し、クローンを選択状態にする
        Undo.RegisterCreatedObjectUndo(clone, "Bake Pose");
        Selection.activeGameObject = clone;
    }

    // 親から子へ、末端まで全てのTransformをコピーし続ける関数
    static void CopyTransformRecurse(Transform src, Transform dst)
    {
        dst.localPosition = src.localPosition;
        dst.localRotation = src.localRotation;
        dst.localScale = src.localScale;

        for (int i = 0; i < src.childCount; i++)
        {
            // 複製元と複製先の子オブジェクトの構成が同じ前提で回す
            if (i < dst.childCount)
            {
                CopyTransformRecurse(src.GetChild(i), dst.GetChild(i));
            }
        }
    }
}
#endif