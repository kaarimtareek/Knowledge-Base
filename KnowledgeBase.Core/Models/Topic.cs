using System.ComponentModel.DataAnnotations;
using Common.Constants;
using KnowledgeBase.Core.Models.Base;

namespace KnowledgeBase.Core.Models;

public class Topic : BaseEntity
{
    [MaxLength(ModelConstants.MaxLength.Name)]
    public string Name { get; set; }
}