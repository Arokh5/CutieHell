public interface IRepairable
{
    // Called by Player
    bool HasFullHealth();
    // Called by Player
    void FullRepair();
    // Called by Player
    int GetRepairCost();
}