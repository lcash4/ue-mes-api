using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_logic.dbo
{
    public class BillOfProcessProcessWorkElementLogic : LogicBase
    {
        IBillOfProcessProcessWorkElementData BillOfProcessProcessWorkElementData { get; }

        #region Constructor

        public BillOfProcessProcessWorkElementLogic(IBillOfProcessProcessWorkElementData billOfProcessProcessWorkElementData)
        {
            BillOfProcessProcessWorkElementData = billOfProcessProcessWorkElementData ?? throw new ArgumentNullException(nameof(billOfProcessProcessWorkElementData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a full <see cref="BillOfProcessProcessWorkElement"/> entity with child entities included.
        /// </summary>
        /// <param name="billOfProcessProcessWorkElementId">Unique Bill of Process Process Work Element ID for the <see cref="BillOfProcessProcessWorkElement"/></param>
        /// <returns></returns>
        public BillOfProcessProcessWorkElement GetBillOfProcessProcessWorkElementById(int billOfProcessProcessWorkElementId)
        {
            return BillOfProcessProcessWorkElementData.GetBillOfProcessProcessWorkElementById(billOfProcessProcessWorkElementId);
        }

        /// <summary>
        /// Add a new bill of process process work element
        /// </summary>
        /// <param name="billOfProcessProcessWorkElement"></param>
        public void AddBillOfProcessProcessWorkElement(BillOfProcessProcessWorkElement billOfProcessProcessWorkElement)
        {
            BillOfProcessProcessWorkElementData.AddBillOfProcessProcessWorkElement(billOfProcessProcessWorkElement);
        }

        /// <summary>
        /// Update an existing bill of process process work element.  
        /// </summary>
        /// <param name="billOfProcessProcessWorkElement"></param>
        public void UpdateBillOfProcessProcessWorkElement(BillOfProcessProcessWorkElement billOfProcessProcessWorkElement)
        {
            BillOfProcessProcessWorkElementData.UpdateBillOfProcessProcessWorkElement(billOfProcessProcessWorkElement);
        }

        /// <summary>
        /// Replace all existing bill of process process work elements with a new set.
        /// </summary>
        /// <param name="billOfProcessProcessId">ID of the bill of process process to update</param>
        /// <param name="billOfProcessProcessWorkElements">The new list of processes to assign to this bill of process</param>
        public void UpdateBillOfProcessProcessWorkElementList(int billOfProcessProcessId, List<BillOfProcessProcessWorkElement> billOfProcessProcessWorkElements, string imagesFolder)
        {
            // This method will first delete all existing records for the given BOP process that do NOT have any production data.
            // This means all work element attributes as well, since there is a foreign key relation.  (EF will cascade the delete)
            // The client should warn the user before they submit the request.

            // If a work element does have data, then it will be updated to isActive = false.
            // This will allow this history to be maintained, but then remove the work element from future BOP usage.

            // Any new elements will be added when the full list is saved after any updates\deletes.

            // Finally, any work elements that use the Image type need to refresh the images after the new list is saved.  
            // The API forces the image name to <BOP ProcessId>_<WorkElementId>.<extension>.
            // Since all of the elements will be added new again, we need to first save them, and then update any images with their new names.
            // The logic will also remove any images that are no longer usable.  This would be any of the old IDs.

            // TODO: Put this into a transaction later
            using (var billOfProcessProcessWorkElementAttributeData = new BillOfProcessProcessWorkElementAttributeData())
            using (var orderItemUnitWorkElementHistoryData = new OrderItemUnitWorkElementHistoryData())
            {
                // First delete the old.  Before we delete, we grab a copy of the existing data so that we can purge any old images.
                var workElementsPriorToUpdate = BillOfProcessProcessWorkElementData.GetBillOfProcessProcessWorkElementsByBillOfProcessProcessId(billOfProcessProcessId);

                // We also need to handle any elements that already have production data.  If they do, they will simply be marked inactive.  If not, they can be deleted.
                List<int> billOfProcessWorkElementsToDelete = new List<int>();
                List<int> billOfProcessWorkElementsToDeactivate = new List<int>();
                workElementsPriorToUpdate.ForEach(workElement =>
                {
                    var orderItemUnitWorkElementHistoryForThisWorkElement = orderItemUnitWorkElementHistoryData.GetAnyOrderItemUnitWorkElementHistoryByWorkElementId(workElement.BillOfProcessProcessWorkElementId);

                    if (orderItemUnitWorkElementHistoryForThisWorkElement == null)
                    {
                        billOfProcessWorkElementsToDelete.Add(workElement.BillOfProcessProcessWorkElementId);
                    }
                    else
                    {
                        billOfProcessWorkElementsToDeactivate.Add(workElement.BillOfProcessProcessWorkElementId);
                    }
                });

                BillOfProcessProcessWorkElementData.DeleteBillOfProcessProcessWorkElementsById(billOfProcessWorkElementsToDelete);
                BillOfProcessProcessWorkElementData.DeactivateBillOfProcessProcessWorkElementsById(billOfProcessWorkElementsToDeactivate);

                // Then add the new
                BillOfProcessProcessWorkElementData.AddBillOfProcessProcessWorkElements(billOfProcessProcessWorkElements);

                // Now, pull back the fresh list of elements and their new IDs, so that the images can be updated.
                // The sequence of the element is a temporary unique ID for these purposes and will be used to identify old and new element IDs
                UpdateWorkElementImages(billOfProcessProcessId, billOfProcessProcessWorkElements, imagesFolder, workElementsPriorToUpdate);

                // Finally, if we had any "in progress" or "paused" order item unit work element history, then these can be updated to the new ID of the matching work element.
                UpdateWorkElementHistory(billOfProcessProcessId, billOfProcessProcessWorkElements);
            }
        }

        public void UpdateWorkElementImages(int billOfProcessProcessId, List<BillOfProcessProcessWorkElement> billOfProcessProcessWorkElementsFromCaller, string imagesFolder, List<BillOfProcessProcessWorkElement> workElementsPriorToUpdate)
        {
            using (var billOfProcessProcessWorkElementAttributeData = new BillOfProcessProcessWorkElementAttributeData())
            {
                var refreshWorkElementList = BillOfProcessProcessWorkElementData.GetBillOfProcessProcessWorkElementsByBillOfProcessProcessId(billOfProcessProcessId);

                // Loop through all image types and adjust file names accordingly
                // Newly added elements will need to rename the files.  Existing elements will need renamed to the new ID.  The logic will work for both
                refreshWorkElementList.Where(workElement => workElement.WorkElementType.Name == "Image").ToList().ForEach(workElement =>
                {
                    var oldWorkElement = billOfProcessProcessWorkElementsFromCaller.Where(oldWorkElement => oldWorkElement.Sequence == workElement.Sequence).FirstOrDefault();

                    if (oldWorkElement != null)
                    {
                        var oldWorkElementAttributeValue = oldWorkElement.BillOfProcessProcessWorkElementAttributes.Where(attribute => attribute.WorkElementTypeAttribute.Name == "Work Element Image").FirstOrDefault();
                        var newWorkElementAttributeValue = workElement.BillOfProcessProcessWorkElementAttributes.Where(attribute => attribute.WorkElementTypeAttribute.Name == "Work Element Image").FirstOrDefault();
                        if (oldWorkElementAttributeValue != null && newWorkElementAttributeValue != null)
                        {
                            // First move the file
                            var newFileName = string.Format(@"{1}_{2}.{3}", imagesFolder, billOfProcessProcessId, newWorkElementAttributeValue.BillOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId, newWorkElementAttributeValue.AttributeValue.Split('.').Last());
                            File.Move(string.Format(@"{0}\{1}", imagesFolder, oldWorkElementAttributeValue.AttributeValue), string.Format(@"{0}\{1}", imagesFolder, newFileName), true);

                            // Now, update the attribute to match the new file name
                            newWorkElementAttributeValue.AttributeValue = newFileName;
                            billOfProcessProcessWorkElementAttributeData.UpdateBillOfProcessProcessWorkElementAttribute(newWorkElementAttributeValue);
                        }
                    }
                });

                // Finally, delete any old images left over from the work elements prior to updating
                workElementsPriorToUpdate.Where(workElement => workElement.WorkElementType.Name == "Image").ToList().ForEach(workElement =>
                {
                    if (workElement != null)
                    {
                        var workElementAttributeValue = workElement.BillOfProcessProcessWorkElementAttributes.Where(attribute => attribute.WorkElementTypeAttribute.Name == "Work Element Image").FirstOrDefault();
                        if (workElementAttributeValue != null)
                        {
                            // Delete the file
                            File.Delete(string.Format(@"{0}\{1}", imagesFolder, workElementAttributeValue.AttributeValue));
                        }
                    }
                });
            }
        }

        public void UpdateWorkElementHistory(int billOfProcessProcessId, List<BillOfProcessProcessWorkElement> billOfProcessProcessWorkElementsFromCaller)
        {
            using (var orderItemUnitWorkElementHistoryData = new OrderItemUnitWorkElementHistoryData())
            {
                var refreshWorkElementList = BillOfProcessProcessWorkElementData.GetBillOfProcessProcessWorkElementsByBillOfProcessProcessId(billOfProcessProcessId);

                // Loop through all work elements and update any in progress or paused history to match the newly created ID
                refreshWorkElementList.ForEach(workElement =>
                {
                    var oldWorkElement = billOfProcessProcessWorkElementsFromCaller.Where(oldWorkElement => oldWorkElement.Sequence == workElement.Sequence).FirstOrDefault();

                    if (oldWorkElement != null)
                    {
                        // This data call will simply do nothing if there are no records.
                        orderItemUnitWorkElementHistoryData.UpdateOrderItemUnitWorkElementHistoryWorkElementIds(oldWorkElement.BillOfProcessProcessWorkElementId, workElement.BillOfProcessProcessWorkElementId);
                    }
                });
            }
        }

        #endregion
    }
}
