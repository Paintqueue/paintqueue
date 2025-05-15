namespace PainterQueueApi.Models
{
    /// <summary>
    /// Provides extension methods for converting between <see cref="ToRule"/> and <see cref="ToRuleDto"/> objects.
    /// </summary>
    /// <remarks>These methods simplify the process of mapping data between the <see cref="ToRule"/> domain
    /// model and the <see cref="ToRuleDto"/> data transfer object.</remarks>
    public static class RuleExtensions
    {
        /// <summary>
        /// Converts a <see cref="ToRule"/> object to a <see cref="ToRuleDto"/> object.
        /// </summary>
        /// <param name="rule">The <see cref="ToRule"/> instance to convert. Must not be <see langword="null"/>.</param>
        /// <returns>A new <see cref="ToRuleDto"/> instance containing the data from the specified <see cref="ToRule"/>.</returns>
        public static RuleDto ToRuleDto(this Rule rule)
        {
            return new RuleDto
            {
                Id = rule.Id,
                Name = rule.Name,
                InternalDescription = rule.InternalDescription,
                Description = rule.Description,
                Page = rule.Page,
                Created = rule.Created,
                Updated = rule.Updated
            };
        }

        /// <summary>
        /// Converts a <see cref="ToRuleDto"/> instance to a <see cref="ToRule"/> instance.
        /// </summary>
        /// <param name="ruleDto">The <see cref="ToRuleDto"/> instance to convert. Cannot be <see langword="null"/>.</param>
        /// <returns>A new <see cref="ToRule"/> instance populated with the data from the specified <see cref="ToRuleDto"/>.</returns>
        public static Rule ToRule(this RuleDto ruleDto)
        {
            return new Rule
            {
                Id = ruleDto.Id,
                Name = ruleDto.Name,
                InternalDescription = ruleDto.InternalDescription,
                Description = ruleDto.Description,
                Page = ruleDto.Page,
                Created = ruleDto.Created,
                Updated = ruleDto.Updated
            };
        }
    }
}
