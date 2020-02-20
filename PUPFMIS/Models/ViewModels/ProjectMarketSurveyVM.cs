using System.Collections.Generic;

namespace PUPFMIS.Models
{
    public class ProjectMarketSurveyVM
    {
        public EndUserProject Project { get; set; }
        public List<MarketSurvey> MarketSurveyItemList { get; set; }
    }
}