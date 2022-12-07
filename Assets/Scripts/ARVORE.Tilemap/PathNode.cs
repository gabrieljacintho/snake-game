namespace ARVORE.Tilemap
{
    public class PathNode
    {
        public int x, y, gCost, hCost, fCost;
        public PathNode previousNode;


        public PathNode(int x, int y, int gCost = int.MaxValue, int hCost = int.MaxValue)
        {
            this.x = x;
            this.y = y;
            this.gCost = gCost;
            this.hCost = hCost;
            CalculateFCost();
            previousNode = null;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }

        public override string ToString()
        {
            return x + "," + y;
        }
    }
}
