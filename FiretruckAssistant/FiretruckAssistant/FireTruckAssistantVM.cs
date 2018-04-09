using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FiretruckAssistant
{
    class FireTruckAssistantVM : INotifyPropertyChanged
    {
        const int MIN_LOCATION = 1;
        const int MAX_LOCATION = 20;
        const int FIRESTATION_LOCATION = 1;

        static FireTruckAssistantVM instance = null;

        Timer timerUpdate = new Timer(1000);

        ObservableCollection<Fire> fireList = new ObservableCollection<Fire>();
        Fire fireToAdd = new Fire();
        OpenStreet openStreetToAdd = new OpenStreet();
        ObservableCollection<OpenStreet> openStreetList = new ObservableCollection<OpenStreet>();
        FireDB fireDB = FireDB.GetInstance();

        bool isLocationNotValid = false;

        public ObservableCollection<Fire> FireList
        {
            get { return fireList; }
            set { fireList = value; _propertyChanged(); }
        }

        public Fire FireToAdd
        {
            get { return fireToAdd; }
            set { fireToAdd = value; _propertyChanged(); }
        }

        public OpenStreet OpenStreetToAdd
        {
            get { return openStreetToAdd; }
            set { openStreetToAdd = value; _propertyChanged(); }
        }

        public ObservableCollection<OpenStreet> OpenStreetList
        {
            get { return openStreetList; }
            set { openStreetList = value; _propertyChanged(); }
        }

        public bool IsLocationNotValid
        {
            get { return isLocationNotValid; }
            set { isLocationNotValid = value; _propertyChanged(); }
        }

        private FireTruckAssistantVM()
        {
            updateFireList();

            timerUpdate.AutoReset = true;
            timerUpdate.Elapsed += timerElapsed_updateFireList;
            timerUpdate.Start();
        }

        private void timerElapsed_updateFireList(object sender, ElapsedEventArgs e)
        {
            updateFireList();
        }

        public static FireTruckAssistantVM GetInstance()
        {
            if (instance == null)
                instance = new FireTruckAssistantVM();
            return instance;
        }

        public void AddStreet()
        {
            if (!CheckLocation(openStreetToAdd.FirstCorner) || !CheckLocation(openStreetToAdd.SecondCorner))
            {
                IsLocationNotValid = true;
                return;
            }

            IsLocationNotValid = false;
            OpenStreetList.Add(OpenStreetToAdd);
            OpenStreetToAdd = new OpenStreet();
        }

        public bool AddFire()
        {
            if (!CheckLocation(fireToAdd.Location))
            {
                IsLocationNotValid = true;
                return false;
            }

            IsLocationNotValid = false;

            FireToAdd.RouteList = getRouteList();

            FireToAdd.StatusID = (int) Fire.StatusList.URGENT;
            fireDB.Add(FireToAdd);
            updateFireList();

            return true;
        }

        public void ResetFire()
        {
            FireToAdd = new Fire();
            OpenStreetList = new ObservableCollection<OpenStreet>();
        }

        private List<string> getRouteList()
        {
            List<string> retList = new List<string>();
            List<List<int>> positionsList = new List<List<int>>();
            List<int> positions = new List<int>();
            List<int> openStreetsUsed = new List<int>();

            positions.Add(FIRESTATION_LOCATION);
            getNextPosition(FIRESTATION_LOCATION, 0);

            void getNextPosition(int lastPosition, int lastStreetUsed)
            {
                for (int i = lastStreetUsed; i < openStreetList.Count; i++)
                {
                    OpenStreet curStreet = openStreetList.ElementAt(i);

                    if (curStreet.FirstCorner == lastPosition && !positions.Contains(curStreet.SecondCorner))
                    {
                        if (!checkFireCorner(curStreet.SecondCorner, i))
                            return;
                    }
                    else if (curStreet.SecondCorner == lastPosition && !positions.Contains(curStreet.FirstCorner))
                    {
                        if (!checkFireCorner(curStreet.FirstCorner, i))
                            return;
                    }
                }
                
                positions.Remove(positions.Last());
                if (positions.Count == 0)
                    return;

                int lastStreet = openStreetsUsed.Last();
                openStreetsUsed.Remove(lastStreet);
                getNextPosition(positions.Last(), lastStreet + 1);
            }

            bool checkFireCorner(int corner, int index)
            {
                positions.Add(corner);

                if (corner != FireToAdd.Location)
                {
                    openStreetsUsed.Add(index);
                    getNextPosition(positions.Last(), 0);
                    return false;
                }
                else
                {
                    List<int> auxPos = new List<int>(positions);
                    positionsList.Add(auxPos);
                    positions.Remove(corner);
                    return true;
                }
            }

            // Convert int List to string
            foreach (List<int> posList in positionsList)
            {
                string positionString = "";

                foreach (int pos in posList)
                {
                    positionString += $"{pos} ";

                    if (pos != posList.Last())
                        positionString += " - ";
                }
                retList.Add(positionString);
            }

            return retList;
        }

        private void updateFireList()
        {
            FireList = new ObservableCollection<Fire>(fireDB.Get());
        }

        private bool CheckLocation(int location)
        {
            return (location >= MIN_LOCATION && location <= MAX_LOCATION);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void _propertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
