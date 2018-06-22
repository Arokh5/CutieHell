public interface IUsable
{
    // Called by Player
    bool CanUse();
    // Called by Player
    float GetUsageCost();
    // Called by Player
    bool Activate(Player player);
    // Called by Player
    void Deactivate();
}