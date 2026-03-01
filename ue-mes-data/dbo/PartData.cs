using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;
using ue_mes_entities.Enums;

namespace ue_mes_data.dbo
{
    public class PartData : DbContextBase, IPartData
    {
        #region AutoMapper Config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.Part, Part>()
                .ForMember(partEntity => partEntity.PartType, opt => opt.MapFrom(partData => (int)partData.PartTypeId))
                .ReverseMap()
                    .ForMember(partModel => partModel.PartTypeId, opt => opt.MapFrom(partEntity => (byte)partEntity.PartType))
                    .ForMember(partModel => partModel.UnitOfMeasureId, opt => opt.MapFrom(partEntity => partEntity.UnitOfMeasure.UnitOfMeasureId))
                    .ForMember(partModel => partModel.PartType, opt => opt.Ignore())
                    .ForMember(partModel => partModel.UnitOfMeasure, opt => opt.Ignore())
                    .ForMember(partModel => partModel.PartItemAttributeTypes, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.UnitOfMeasure, UnitOfMeasure>();
            mapperConfig.CreateMap<Model.PartItemAttributeType, PartItemAttributeType>()
                .ForPath(partItemAttributeTypeEntity => partItemAttributeTypeEntity.Part.PartId, opt => opt.MapFrom(partItemAttributeTypeModel => partItemAttributeTypeModel.PartId));
            mapperConfig.CreateMap<Model.ItemAttributeType, ItemAttributeType>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor
        public PartData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return all parts and include inactive parts if <paramref name="includeInactive"/> is set to true
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public List<Part> GetAllParts(bool includeInactive = false)
        {
            List<Part> parts = new List<Part>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var partRecords = mesProductionContext.Parts
                    .Where(part => part.IsActive || (!part.IsActive && includeInactive))
                    .Include(part => part.UnitOfMeasure)
                    .Include(part => part.PartItemAttributeTypes)
                    .ThenInclude(partItemAttributeType => partItemAttributeType.ItemAttributeType)
                    .ToList();


                if (partRecords != null)
                {
                    parts = mapper.Map<List<Model.Part>, List<Part>>(partRecords);
                }
            }

            return parts;
        }

        /// <summary>
        /// Returns a list of <see cref="Part"/> entities with part child entities included.
        /// The starting process ID is used to find BOPs where the process sequence is 1 and matches the process.  This indicates an process where an order item unit can be started.
        /// </summary>
        /// <param name="processId">Unique Process ID for the <see cref="BillOfProcessProcess"/> to search</param>
        /// <returns></returns>
        public List<Part> GetPartsForBillOfProcessAtStartingEquipmentProcess(int processId)
        {
            List<Part> parts = new List<Part>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var partRecords = mesProductionContext.Parts
                    .Join(mesProductionContext.BillOfProcesses,
                        part => part.PartId,
                        partbillOfProcess => partbillOfProcess.PartId,
                        (partResult, partbillOfProcessResult) => new { partResult, partbillOfProcessResult })
                    .Join(mesProductionContext.BillOfProcessProcesses,
                        partbillOfProcess => partbillOfProcess.partbillOfProcessResult.BillOfProcessId,
                        partbillOfProcessProcesses => partbillOfProcessProcesses.BillOfProcessId,
                        (partbillOfProcessResult, partbillOfProcessProcessesResult) => new { partbillOfProcessResult, partbillOfProcessProcessesResult })
                    .Where(partbillOfProcessProcessesResult => partbillOfProcessProcessesResult.partbillOfProcessResult.partResult.IsActive
                        && partbillOfProcessProcessesResult.partbillOfProcessProcessesResult.ProcessId == processId
                        && partbillOfProcessProcessesResult.partbillOfProcessProcessesResult.IsActive
                        && partbillOfProcessProcessesResult.partbillOfProcessProcessesResult.Sequence == 1)
                    .Select(partbillOfProcessProcessesResult => partbillOfProcessProcessesResult.partbillOfProcessResult.partResult )
                    .Distinct()
                    .ToList();

                if (partRecords != null)
                {
                    parts = mapper.Map<List<Model.Part>, List<Part>>(partRecords);
                }
            }

            return parts;
        }

        /// <summary>
        /// Return all parts where the part type matches a type in the partTypes list.
        /// Also, include inactive parts if <paramref name="includeInactive"/> is set to true
        /// </summary>
        /// <param name="partTypes">List of <see cref="PartTypeEnum"/> to return</param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public List<Part> GetPartsByPartTypes(List<PartTypeEnum> partTypes, bool includeInactive = false)
        {
            List<Part> parts = new List<Part>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var partRecords = mesProductionContext.Parts
                    .Where(part => (part.IsActive || (!part.IsActive && includeInactive)) && partTypes.Contains((PartTypeEnum)part.PartTypeId))
                    .Include(part => part.UnitOfMeasure)
                    .Include(part => part.PartItemAttributeTypes)
                    .ThenInclude(partItemAttributeType => partItemAttributeType.ItemAttributeType)
                    .ToList();

                if (partRecords != null)
                {
                    parts = mapper.Map<List<Model.Part>, List<Part>>(partRecords);
                }
            }

            return parts;
        }

        /// <summary>
        /// Return a part by part number and revision
        /// </summary>
        /// <param name="partNumber"></param>
        /// <param name="partRevision"></param>
        /// <returns></returns>
        public Part GetPartByPartNumberAndRevision(string partNumber, string partRevision)
        {
            Part part = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var partRecord = mesProductionContext.Parts
                    .Where(part => part.PartNumber == partNumber && part.PartRevision == partRevision)
                    .Include(part => part.UnitOfMeasure)
                    .Include(part => part.PartItemAttributeTypes)
                    .ThenInclude(partItemAttributeType => partItemAttributeType.ItemAttributeType)
                    .FirstOrDefault();

                if (partRecord != null)
                {
                    part = mapper.Map<Part>(partRecord);
                }
            }

            return part;
        }

        /// <summary>
        /// Return a part entity by unique part ID
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        public Part GetPartEntityByPartId(int partId)
        {
            Part part = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var partRecord = mesProductionContext.Parts
                    .Where(part => part.PartId == partId)
                    .Include(part => part.UnitOfMeasure)
                    .Include(part => part.PartItemAttributeTypes)
                    .ThenInclude(partItemAttributeType => partItemAttributeType.ItemAttributeType)
                    .FirstOrDefault();

                if (partRecord != null)
                {
                    part = mapper.Map<Part>(partRecord);
                }
            }

            return part;
        }

        /// <summary>
        /// Add a new part
        /// </summary>
        /// <param name="part"></param>
        public void AddPart(Part part)
        {
            var partRecord = mapper.Map<Model.Part>(part);
            if (partRecord != null)
            {
                // Set the Last Modified By if present
                partRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(part.LastModifiedBy) ? part.LastModifiedBy : null;

                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.Parts.Add(partRecord);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Update an existing part.  
        /// </summary>
        /// <param name="part"></param>
        public void UpdatePart(Part part)
        {
            var existingPart = GetPartByPartId(part.PartId);

            if (existingPart == null)
                throw new Exception("Object does not exist: There was an attempt to update an object that does not exist in the database.");

            if (existingPart.PartNumber != part.PartNumber || existingPart.PartRevision != part.PartRevision)
                throw new Exception("Invalid object: The object's immutable properties do not match the existing database record.");

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                existingPart.PartTypeId = (byte)part.PartType;
                existingPart.Description = part.Description;
                existingPart.UnitOfMeasureId = part.UnitOfMeasure.UnitOfMeasureId;
                existingPart.IsActive = part.IsActive;
                existingPart.LastModifiedTime = DateTime.Now;
                existingPart.LastModifiedTimeUtc = DateTime.UtcNow;

                // Set the Last Modified By if present, otherwise, 
                existingPart.LastModifiedBy = !string.IsNullOrWhiteSpace(part.LastModifiedBy) ? part.LastModifiedBy : existingPart.LastModifiedBy;

                mesProductionContext.Entry(existingPart).State = EntityState.Modified;
                mesProductionContext.SaveChanges();
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// This method is used to get the existing record for modification or deletion.
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        private Model.Part GetPartByPartId(int partId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                return mesProductionContext.Parts
                    .Where(part => part.PartId == partId)
                    .FirstOrDefault();
            }
        }

        #endregion
    }
}
