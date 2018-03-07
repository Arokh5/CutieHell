public interface IUsable
{
    // Called by Player
    bool CanUse();
    // Called by Player
    int GetUsageCost();
    // Called by Player
    bool Activate(Player player);
    // Called by Player
    void Deactivate();
}