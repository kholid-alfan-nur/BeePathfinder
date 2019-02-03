//Group 4 Final Assignment Bee Breeding
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BeePathfinder
{
    class VM
    {
        #region properties
        //define local variables
        private int firstHex;
        private int secondHex;
        private int outputDistance;

        //constructor of class Coordinates
        private Corrdinates coord = new Corrdinates();

        public int FirstHex
        {
            get { return firstHex; }
            set { firstHex = value; Calculate(); OnNotifyChanged(); }
        }

        public int SecondHex
        {
            get { return secondHex; }
            set { secondHex = value; Calculate(); OnNotifyChanged(); }
        }

        public int OutputDistance
        {
            get { return outputDistance; }
            set { outputDistance = value; OnNotifyChanged(); }
        }
        #endregion

        public int Calculate()
        {
            if (FirstHex != 0 && SecondHex != 0)
            {
                //call findDistance method of Coordinates Class to calculate the distance between two cells
                OutputDistance = coord.FindDistance(FirstHex, SecondHex);     
            }
            return OutputDistance;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnNotifyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
