using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourBooker.Logic
{
    public static class ChangeUndoer
    {
        public static void Undo(LinkedList<Country> itinerary, ItineraryChange changeToUndo)
        {
            switch (changeToUndo.ChangeType)
            {
                case ChangeType.Append:
                    // change was to append final country - we need to remove it
                    itinerary.RemoveLast();
                    break;
                case ChangeType.Insert:
                    // change was to insert a country before item x.  Remove the country before x
                    LinkedListNode<Country> insertion = itinerary.GetNthNode(changeToUndo.Index);
                    itinerary.Remove(insertion);
                    break;
                case ChangeType.Remove:
                    // change was to remove a country before node x.  Need o insert it back 
                    // Scenario 1.  Removed country was the last one on the list
                    if (changeToUndo.Index == itinerary.Count)
                    {
                        itinerary.AddLast(changeToUndo.Value);
                    }
                    else
                    {
                        LinkedListNode<Country> removedBefore = itinerary.GetNthNode(changeToUndo.Index);
                        itinerary.AddBefore(removedBefore, changeToUndo.Value);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
