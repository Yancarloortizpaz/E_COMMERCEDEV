using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class Segments_DTOS
    {
        public int? SegmentId { get; set; }
        public string? SegmentName { get; set; }
        public string? SegmentDescription { get; set; }
        public int? SegmentCreatorId { get; set; }
        public DateTime? SegmentCreationDate { get; set; }
        public int? SegmentModificatorId { get; set; }
        public DateTime? SegmentModificationDate { get; set; }
        public bool? SegmentStatusId { get; set; }
    }
}