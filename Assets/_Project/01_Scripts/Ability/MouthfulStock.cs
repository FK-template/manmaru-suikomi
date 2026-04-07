using Manmaru.Interaction;

public class MouthfulStock
{
    // 内部変数
    private int _capturedCount = 0;

    // プロパティ
    public int CapturedCount => _capturedCount;

    // コンストラクタ
    public MouthfulStock() { }

    /// <summary>
    /// すいこみ済みカウンターを増やすメソッド
    /// </summary>
    public void AddCapturedCount(ICapturable captureTarget)
    {
        _capturedCount += captureTarget.CaptureMass;
    }

    /// <summary>
    /// すいこみ済みカウンターをリセットするメソッド
    /// </summary>
    public void ResetCapturedCount()
    {
        _capturedCount = 0;
    }
}