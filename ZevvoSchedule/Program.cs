using System.Collections.Generic;
using System.IO;

namespace ZevvoSchedule
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 20;
            string fileLocation = @"C:\Temp\Zevvo.csv";


            List<AP> scheduleList = new List<AP>();

            for (int i = 0; i < count; i++)
            {
                scheduleList.Add(new AP(i.ToString()));
            }

            string[] firstBatch = new string[6];
            string[] secondBatch = new string[4];
            
            using (StreamWriter sw = new StreamWriter(fileLocation, true))
            {
                bool mainOddFound;
                bool backupOddFound;
                bool mainEvenFound;
                bool backupEvenFound;
                int weekOddExtras;
                int weekEvenExtras;

                // Each iteration is one set of two weeks
                for (int i = 0; i < 52; i++)
                {
                    // Commented part below works specifically for 18 people and no other situation. Weird.
                    /*
                    mainOddFound = false;
                    backupOddFound = false;
                    mainEvenFound = false;
                    backupEvenFound = false;
                    weekOddExtras = 0;
                    weekEvenExtras = 0;

                    RefreshList(scheduleList);
                    while (!mainOddFound || !backupOddFound || !mainEvenFound || !backupEvenFound || weekOddExtras < 4 || weekEvenExtras < 2)
                    {
                        foreach (AP person in scheduleList)
                        {
                            if (person.Available)
                            {
                                if (!mainOddFound && person.EligibleMain)
                                {
                                    mainOddFound = true;
                                    person.Available = false;
                                    person.EligibleMain = false;
                                    firstBatch[0] = person.Name;
                                }
                                else if (!backupOddFound && person.EligibleBackup)
                                {
                                    backupOddFound = true;
                                    person.Available = false;
                                    person.EligibleBackup = false;
                                    firstBatch[1] = person.Name;
                                }
                                else if (!mainEvenFound && person.EligibleMain)
                                {
                                    mainEvenFound = true;
                                    person.Available = false;
                                    person.EligibleMain = false;
                                    secondBatch[0] = person.Name;
                                }
                                else if (!backupEvenFound && person.EligibleBackup)
                                {
                                    backupEvenFound = true;
                                    person.Available = false;
                                    person.EligibleBackup = false;
                                    secondBatch[1] = person.Name;
                                }
                                else if (weekOddExtras < 4)
                                {
                                    person.Available = false;
                                    firstBatch[weekOddExtras + 2] = person.Name;
                                    weekOddExtras += 1;
                                }
                                else if (weekEvenExtras < 2)
                                {
                                    person.Available = false;
                                    secondBatch[weekEvenExtras + 2] = person.Name;
                                    weekEvenExtras += 1;
                                }
                            }
                        }
                        RefreshList(scheduleList);
                    }

                    sw.WriteLine(",a{0}b, a{1}b, a{2}b, a{3}b, a{4}b, a{5}b", firstBatch[0], firstBatch[1], firstBatch[2], firstBatch[3], firstBatch[4], firstBatch[5]);
                    sw.WriteLine(",a{0}b, a{1}b, a{2}b, a{3}b", secondBatch[0], secondBatch[1], secondBatch[2], secondBatch[3]);
                    */

                    // Part below for basic one-offset rotation
                    List<string> firstGroup = new List<string>();
                    List<string> secondGroup = new List<string>();
                    while (firstGroup.Count < 6 || secondGroup.Count < 4)
                    {
                        if (firstGroup.Count < 6)
                        {
                            firstGroup.Add(scheduleList[0].Name.ToString());
                            scheduleList.Add(scheduleList[0]);
                            scheduleList.Remove(scheduleList[0]);
                        }
                        else if (secondGroup.Count < 4)
                        {
                            secondGroup.Add(scheduleList[0].Name.ToString());
                            scheduleList.Add(scheduleList[0]);
                            scheduleList.Remove(scheduleList[0]);
                        }
                    }
                    // At the end of every two sets of weeks, offset one (for rotation)
                    if (((i + 1) % 2).Equals(0))
                    {
                        scheduleList.Add(scheduleList[0]);
                        scheduleList.Remove(scheduleList[0]);
                    }
                    sw.WriteLine(",a{0}b, a{1}b, a{2}b, a{3}b, a{4}b, a{5}b", firstGroup[0], firstGroup[1], firstGroup[2], firstGroup[3], firstGroup[4], firstGroup[5]);
                    sw.WriteLine(",a{0}b, a{1}b, a{2}b, a{3}b", secondGroup[0], secondGroup[1], secondGroup[2], secondGroup[3]);
                }
            }
        }

        public static List<AP> RefreshList(List<AP> scheduleList)
        {
            // Check if any are available
            int availableCount = 0;
            // Main reset check
            int mainCount = 0;

            foreach (AP person in scheduleList)
            {
                if (person.Available)
                {
                    availableCount += 1;
                }
                // We can do main reset check here too
                if (person.EligibleMain)
                {
                    mainCount += 1;
                }
            }

            // Reset list if nobody is available
            if (availableCount.Equals(0))
            {
                foreach (AP person in scheduleList)
                {
                    person.ResetAvailability();
                }
            }
            // Reset main if nobody is mainable
            if (mainCount.Equals(0))
            {
                foreach (AP person in scheduleList)
                {
                    person.ResetMain();
                }
            }

            return scheduleList;
        }

    }

}
