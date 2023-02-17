public class NormalBlockClearCondition : ClearConditionBase
{
    public int BlockValue;
    public int ClearCount;

    private int currCount = 0;

    public NormalBlockClearCondition(int blockValue, int clearCount)
    {
        BlockValue = blockValue;
        ClearCount = clearCount;
    }

    public override void CheckBlockPang(Block block)
    {
        if (block.value == BlockValue)
        {
            currCount++;
            if (ClearCount <= currCount)
            {
                IsCleared = true;
                
                OnCleared?.Invoke();
            }
        }
    }
}